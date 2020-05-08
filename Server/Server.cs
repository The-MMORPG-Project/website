using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using EF;
using ENet;

namespace Valk.Networking
{
    class Server
    {
        public static byte channelID = 0;

        private List<Client> clients;
        private List<Player> players;

        private ushort port;
        private int maxClients;

        public Server(ushort port, int maxClients) 
        {
            this.port = port;
            this.maxClients = maxClients;

            clients = new List<Client>();
            players = new List<Player>();
        }

        public void Run()
        {
            Library.Initialize();

            using (Host server = new Host())
            {
                var address = new Address();
                address.Port = port;
                server.Create(address, maxClients);

                Console.WriteLine($"Server listening on {port}");

                int packetCounter = 0;
                Event netEvent;

                while (!Console.KeyAvailable)
                {
                    bool polled = false;

                    while (!polled)
                    {
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
                                Console.WriteLine("Client connected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                                clients.Add(new Client(netEvent.Peer));
                                break;

                            case EventType.Disconnect:
                                Console.WriteLine("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                                break;

                            case EventType.Timeout:
                                Console.WriteLine("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                                clients.Remove(clients.Find(x => x.ID.Equals(netEvent.Peer.ID)));
                                players.Remove(players.Find(x => x.ID.Equals(netEvent.Peer.ID)));
                                break;

                            case EventType.Receive:
                                Console.WriteLine($"{packetCounter++} Packet received from - ID: {netEvent.Peer.ID}, IP: {netEvent.Peer.IP}, Channel ID: {netEvent.ChannelID}, Data length: {netEvent.Packet.Length}");
                                HandlePacket(ref netEvent);
                                netEvent.Packet.Dispose();
                                break;
                        }
                    }

                    server.Flush();
                }

                Library.Deinitialize();
            }
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
                            Console.WriteLine($"User account '{name}' already exists.");

                            Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerCreateAccountDenied, "Account name already registered"));

                            return;
                        }

                        // Account is unique, creating...
                        Console.WriteLine($"Registering '{name}' to database");
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
                            Console.WriteLine($"User login for '{user}' does not exist.");

                            Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerLoginDenied, "User login does not exist"));

                            return;
                        }

                        // User login exists
                        if (!user.Pass.Equals(pass))
                        {
                            // Logged in with wrong password
                            Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerLoginDenied, "Wrong password"));
                            Console.WriteLine($"User '{user}' failed to login");
                            return;
                        }

                        // Logged in with correct password
                        Console.WriteLine($"User '{user}' successfully logged in");
                        players.Add(new Player(netEvent.Peer));
                        Network.Send(ref netEvent, Packet.Create(Packet.Type.ServerLoginAccepted));
                    }
                }

                if (packetID == Packet.Type.ClientPositionUpdate)
                {
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    Console.WriteLine($"Recieved x {x}, y {y}");
                }

                readStream.Dispose();
                reader.Dispose();
            }

            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Received packet but buffer was too long. {netEvent.Packet.Length}");
            }
        }
    }
}
