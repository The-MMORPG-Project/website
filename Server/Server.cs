using System.Diagnostics;
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

                    string ip = netEvent.Peer.IP;
                    uint id = netEvent.Peer.ID;

                    switch (netEvent.Type)
                    {
                        case EventType.None:
                            break;

                        case EventType.Connect:
                            Logger.Log($"Client connected - ID: {id}, IP: {ip}");
                            clients.Add(new Client(netEvent.Peer));
                            break;

                        case EventType.Disconnect:
                            Logger.Log($"Client disconnected - ID: {id}, IP: {ip}");
                            break;

                        case EventType.Timeout:
                            Logger.Log($"Client timeout - ID: {id}, IP: {ip}");
                            clients.Remove(clients.Find(x => x.ID.Equals(netEvent.Peer.ID)));
                            break;

                        case EventType.Receive:
                            //Logger.Log($"{packetCounter++} Packet received from - ID: {id}, IP: {ip}, Channel ID: {netEvent.ChannelID}, Data length: {netEvent.Packet.Length}");
                            HandlePacket(netEvent);
                            netEvent.Packet.Dispose();
                            break;
                    }
                }

                server.Flush();
            }

            CleanUp();
        }

        // Send a position update to all peers in the game every x ms
        private void PositionUpdates(Object source, ElapsedEventArgs e)
        {
            SendPositionUpdate(PacketFlags.None, false, GetPeersInGame());
        }

        private void SendPositionUpdate(PacketFlags flags, bool ignorePositionChecks, params Peer[] peers)
        {
            if (GetPeersInGame().Length == 0)
                return;

            var data = new List<object>();

            int playerDataLength = 0;

            foreach (Client client in clients)
            {
                if (client.Status == ClientStatus.InGame)
                {
                    if (!ignorePositionChecks && (client.x == client.px && client.y == client.py))
                        continue;

                    data.Add(client.ID);
                    data.Add(client.x);
                    data.Add(client.y);

                    client.px = client.x;
                    client.py = client.y;

                    playerDataLength++;
                }
            }
            data.Insert(0, playerDataLength);

            if (playerDataLength == 0)
                return;

            Network.Broadcast(server, Packet.Create(PacketType.ServerPositionUpdate, flags, data.ToArray()), peers);
        }

        private Peer[] GetPeersInGame()
        {
            return clients.FindAll(x => x.Status == ClientStatus.InGame).Select(x => x.Peer).ToArray();
        }

        private Peer[] GetPeersInGame(uint excludedPeer)
        {
            return clients.FindAll(x => x.Status == ClientStatus.InGame && x.ID != excludedPeer).Select(x => x.Peer).ToArray();
        }

        private void HandlePacket(Event netEvent)
        {
            uint id = netEvent.Peer.ID;

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
                            Logger.Log($"Client '{id}' tried to make an account '{name}' but its already registered");

                            Network.Send(ref netEvent, Packet.Create(PacketType.ServerCreateAccountDenied, PacketFlags.Reliable, "Account name already registered"));

                            return;
                        }

                        // Account is unique, creating...
                        Logger.Log($"Client '{id}' successfully created a new account '{name}'");
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
                            Logger.Log($"Client '{id}' tried to login to a non-existant account called '{name}'");
                            return;
                        }

                        // User login exists
                        if (!user.Pass.Equals(pass))
                        {
                            // Logged in with wrong password
                            Network.Send(ref netEvent, Packet.Create(PacketType.ServerLoginDenied, PacketFlags.Reliable, "Wrong password"));
                            Logger.Log($"Client '{id}' tried to log into account '{name}' but typed in the wrong password");
                            return;
                        }

                        // Logged in with correct password
                        Network.Send(ref netEvent, Packet.Create(PacketType.ServerLoginAccepted, PacketFlags.Reliable));
                        Logger.Log($"Client '{id}' successfully logged into account '{name}'");
                        clients.Find(x => x.ID.Equals(id)).Status = ClientStatus.InGame;
                        Logger.Log($"Client '{id}' joined game room.");
                    }
                }

                if (packetID == PacketType.ClientRequestPositions)
                {
                    var peers = GetPeersInGame(id);
                    if (peers.Length == 0)
                        return;

                    SendPositionUpdate(PacketFlags.Reliable, true, peers);
                }

                if (packetID == PacketType.ClientPositionUpdate)
                {
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    //Logger.Log($"Recieved x {x}, y {y}");

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
                Logger.Log($"Received packet from client '{id}' but buffer was too long. {netEvent.Packet.Length}");
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
