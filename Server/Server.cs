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

        public static byte channelID = 0;

        private List<Client> clients;
        private List<Player> players;

        private ushort port;
        private int maxClients;

        private bool serverRunning;

        public Server(ushort port, int maxClients)
        {
            this.port = port;
            this.maxClients = maxClients;

            clients = new List<Client>();
            players = new List<Player>();

            positionUpdatePump = new Timer(1000, PositionUpdates);
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
                            players.Remove(players.Find(x => x.ID.Equals(netEvent.Peer.ID)));
                            break;

                        case EventType.Receive:
                            //Logger.Log($"{packetCounter++} Packet received from - ID: {netEvent.Peer.ID}, IP: {netEvent.Peer.IP}, Channel ID: {netEvent.ChannelID}, Data length: {netEvent.Packet.Length}");
                            HandlePacket(ref netEvent);
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
            if (players.Count <= 0)
                return;

            int playerPropCount = 3;

            object[] values = new object[players.Count * playerPropCount];
            for (int i = 0; i < players.Count; i++)
            {
                values[0 + (i * playerPropCount)] = players[i].ID;
                values[1 + (i * playerPropCount)] = players[i].x;
                values[2 + (i * playerPropCount)] = players[i].y;
            }

            Network.Broadcast(server, Packet.Create(Packet.Type.ServerPositionUpdate, values));
        }

        private void HandlePacket(ref Event netEvent)
        {
            try
            {
                var readBuffer = new byte[1024];
                var readStream = new MemoryStream(readBuffer);
                var reader = new BinaryReader(readStream);

                readStream.Position = 0;
                netEvent.Packet.CopyTo(readBuffer);
                var packetID = (Packet.Type)reader.ReadByte();

                if (packetID == Packet.Type.ClientCreateAccount)
                {
                    var name = reader.ReadString();
                    var pass = reader.ReadString();

                    using (var db = new UserContext())
                    {
                        User user = db.Users.ToList().Find(x => x.Name.Equals(name));
                        if (user != null) // Account already exists in database
                        {
                            Logger.Log($"Client '{netEvent.Peer.ID}' tried to make an account '{name}' but its already registered");

                            Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerCreateAccountDenied, "Account name already registered"));

                            return;
                        }

                        // Account is unique, creating...
                        Logger.Log($"Client '{netEvent.Peer.ID}' successfully created a new account '{name}'");
                        Logger.Log($"Registering account '{name}' to database");
                        db.Add(new User { Name = name, Pass = pass });
                        db.SaveChanges();

                        Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerCreateAccountAccepted));
                    }
                }

                if (packetID == Packet.Type.ClientLoginAccount)
                {
                    var name = reader.ReadString();
                    var pass = reader.ReadString();

                    using (var db = new UserContext())
                    {
                        User user = db.Users.ToList().Find(x => x.Name.Equals(name));

                        if (user == null) // User login does not exist
                        {
                            Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerLoginDenied, "User login does not exist"));
                            Logger.Log($"Client '{netEvent.Peer.ID}' tried to login to a non-existant account called '{name}'");
                            return;
                        }

                        // User login exists
                        if (!user.Pass.Equals(pass))
                        {
                            // Logged in with wrong password
                            Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerLoginDenied, "Wrong password"));
                            Logger.Log($"Client '{netEvent.Peer.ID}' tried to log into account '{name}' but typed in the wrong password");
                            return;
                        }

                        // Logged in with correct password
                        Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerLoginAccepted));
                        Logger.Log($"Client '{netEvent.Peer.ID}' successfully logged into account '{name}'");
                        players.Add(new Player(netEvent.Peer));
                    }
                }

                if (packetID == Packet.Type.ClientPositionUpdate)
                {
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    //Logger.Log($"Recieved x {x}, y {y}");

                    uint id = netEvent.Peer.ID;
                    Player player = players.Find(x => x.ID.Equals(id));
                    player.x = x;
                    player.y = y;
                    Logger.Log(player);
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
