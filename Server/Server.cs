using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Collections.Generic;

using EF;
using ENet;

namespace Valk.Networking
{
    class Server
    {
        public Host server;
        public Timer positionUpdatePump;

        private const int POSITION_UPDATE_DELAY = 100;

        public static byte channelID = 0;

        private ushort port;
        private int maxClients;

        private List<Client> clients;

        private bool serverRunning;

        public Server(ushort port, int maxClients)
        {
            this.port = port;
            this.maxClients = maxClients;

            clients = new List<Client>();

            positionUpdatePump = new Timer(POSITION_UPDATE_DELAY, PositionUpdates);
            positionUpdatePump.Start();
        }

        public void Start()
        {
            Library.Initialize();

            server = new Host();

            var address = new Address();
            address.Port = port;
            server.Create(address, maxClients);
            serverRunning = true;

            Logger.Log($"Server listening on {port}");

            //int packetCounter = 0;
            //int i = 0;
            Event netEvent;

            while (serverRunning)
            {
                bool polled = false;

                while (!polled)
                {
                    if (!server.IsSet)
                        return;

                    if (server.CheckEvents(out netEvent) <= 0)
                    {
                        if (server.Service(15, out netEvent) <= 0)
                            break;

                        polled = true;
                    }

                    switch (netEvent.Type)
                    {
                        case EventType.None:
                            break;

                        case EventType.Connect:
                            Logger.Log("Client connected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                            clients.Add(new Client(netEvent.Peer));
                            break;

                        case EventType.Disconnect:
                            Logger.Log("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                            break;

                        case EventType.Timeout:
                            Logger.Log("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                            clients.Remove(clients.Find(x => x.ID.Equals(netEvent.Peer.ID)));
                            break;

                        case EventType.Receive:
                            //Logger.Log($"{packetCounter++} Packet received from - ID: {netEvent.Peer.ID}, IP: {netEvent.Peer.IP}, Channel ID: {netEvent.ChannelID}, Data length: {netEvent.Packet.Length}");
                            HandlePacket(netEvent);
                            netEvent.Packet.Dispose();
                            break;
                    }
                }

                server.Flush();
            }

            CleanUp();
        }

        private void PositionUpdates(Object source, ElapsedEventArgs e)
        {
            var peersInGame = clients.FindAll(x => x.Status == ClientStatus.InGame).Select(x => x.Peer).ToArray();

            if (peersInGame.Length <= 0)
                return;

            var data = new List<object>();

            int length = 0;

            foreach (Client client in clients)
            {
                if (client.Status == ClientStatus.InGame)
                {
                    if (client.x == client.px || client.y == client.py)
                        continue;

                    data.Add(client.ID);
                    data.Add(client.x);
                    data.Add(client.y);

                    client.px = client.x;
                    client.py = client.y;

                    length++;
                }
            }
            data.Insert(0, length);

            Network.Broadcast(server, Packet.Create(PacketType.ServerPositionUpdate, PacketFlags.None, data.ToArray()), peersInGame.ToArray());
        }

        private void HandlePacket(Event netEvent)
        {
            try
            {
                var readBuffer = new byte[1024];
                var readStream = new MemoryStream(readBuffer);
                var reader = new BinaryReader(readStream);

                readStream.Position = 0;
                netEvent.Packet.CopyTo(readBuffer);
                var packetID = (PacketType)reader.ReadByte();

                if (packetID == PacketType.ClientCreateAccount)
                {
                    var name = reader.ReadString();
                    var pass = reader.ReadString();

                    using (var db = new UserContext())
                    {
                        User user = db.Users.ToList().Find(x => x.Name.Equals(name));
                        if (user != null) // Account already exists in database
                        {
                            Logger.Log($"Client '{netEvent.Peer.ID}' tried to make an account '{name}' but its already registered");

                            Network.Send(ref netEvent, Packet.Create(PacketType.ServerCreateAccountDenied, PacketFlags.Reliable, "Account name already registered"));

                            return;
                        }

                        // Account is unique, creating...
                        Logger.Log($"Client '{netEvent.Peer.ID}' successfully created a new account '{name}'");
                        Logger.Log($"Registering account '{name}' to database");
                        db.Add(new User { Name = name, Pass = pass });
                        db.SaveChanges();

                        Network.Send(ref netEvent, Packet.Create(PacketType.ServerCreateAccountAccepted, PacketFlags.Reliable));
                    }
                }

                if (packetID == PacketType.ClientLoginAccount)
                {
                    var name = reader.ReadString();
                    var pass = reader.ReadString();

                    using (var db = new UserContext())
                    {
                        User user = db.Users.ToList().Find(x => x.Name.Equals(name));

                        if (user == null) // User login does not exist
                        {
                            Network.Send(ref netEvent, Packet.Create(PacketType.ServerLoginDenied, PacketFlags.Reliable, "User login does not exist"));
                            Logger.Log($"Client '{netEvent.Peer.ID}' tried to login to a non-existant account called '{name}'");
                            return;
                        }

                        // User login exists
                        if (!user.Pass.Equals(pass))
                        {
                            // Logged in with wrong password
                            Network.Send(ref netEvent, Packet.Create(PacketType.ServerLoginDenied, PacketFlags.Reliable, "Wrong password"));
                            Logger.Log($"Client '{netEvent.Peer.ID}' tried to log into account '{name}' but typed in the wrong password");
                            return;
                        }

                        // Logged in with correct password
                        Network.Send(ref netEvent, Packet.Create(PacketType.ServerLoginAccepted, PacketFlags.Reliable));
                        Logger.Log($"Client '{netEvent.Peer.ID}' successfully logged into account '{name}'");
                        clients.Find(x => x.ID.Equals(netEvent.Peer.ID)).Status = ClientStatus.InGame;
                    }
                }

                if (packetID == PacketType.ClientPositionUpdate)
                {
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    //Logger.Log($"Recieved x {x}, y {y}");

                    uint id = netEvent.Peer.ID;
                    Client client = clients.Find(x => x.ID.Equals(id));
                    client.x = x;
                    client.y = y;
                    //Logger.Log(client);
                }

                readStream.Dispose();
                reader.Dispose();
            }

            catch (ArgumentOutOfRangeException)
            {
                Logger.Log($"Received packet from client '{netEvent.Peer.ID}' but buffer was too long. {netEvent.Packet.Length}");
            }
        }

        public void Stop()
        {
            serverRunning = false;
        }

        private void CleanUp()
        {
            positionUpdatePump.Dispose();
            server.Dispose();
            Library.Deinitialize();
        }
    }
}
