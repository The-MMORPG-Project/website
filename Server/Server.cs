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

            int packetCounter = 0;

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
                            Logger.Log($"{packetCounter++} Packet received from - ID: {netEvent.Peer.ID}, IP: {netEvent.Peer.IP}, Channel ID: {netEvent.ChannelID}, Data length: {netEvent.Packet.Length}");
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
                            Logger.Log($"User account '{name}' already exists.");

                            Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerCreateAccountDenied, "Account name already registered"));

                            return;
                        }

                        // Account is unique, creating...
                        Logger.Log($"Registering '{name}' to database");
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
                            Logger.Log($"User login for name '{name}' does not exist.");

                            Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerLoginDenied, "User login does not exist"));

                            return;
                        }

                        // User login exists
                        if (!user.Pass.Equals(pass))
                        {
                            // Logged in with wrong password
                            Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerLoginDenied, "Wrong password"));
                            Logger.Log($"User '{user}' failed to login");
                            return;
                        }

                        // Logged in with correct password
                        Logger.Log($"User '{user}' successfully logged in");
                        players.Add(new Player(netEvent.Peer));
                        Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerLoginAccepted));
                    }
                }

                if (packetID == Packet.Type.ClientPositionUpdate)
                {
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    Logger.Log($"Recieved x {x}, y {y}");
                }

                readStream.Dispose();
                reader.Dispose();
            }

            catch (ArgumentOutOfRangeException)
            {
                Logger.Log($"Received packet but buffer was too long. {netEvent.Packet.Length}");
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
