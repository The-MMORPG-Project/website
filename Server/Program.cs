using System;
using System.IO;
using System.Linq;
using ENet;
using EFGetStarted;

namespace Server
{
    class Program
    {
        const ushort port = 7777;
        const int maxClients = 8;

        static void Main(string[] args)
        {
            Console.Clear();
            StartServer();
        }

        static void StartServer()
        {
            Library.Initialize();

            using (Host server = new Host())
            {
                var address = new Address();

                address.Port = port;
                server.Create(address, maxClients);

                Console.WriteLine($"Server listening on {port}");

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
                                break;

                            case EventType.Disconnect:
                                Console.WriteLine("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                                break;

                            case EventType.Timeout:
                                Console.WriteLine("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                                break;

                            case EventType.Receive:
                                //Console.WriteLine("Packet received from - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP + ", Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
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

        enum PacketID
        {
            HeartBeat = 0,
            CreateAccount = 1,
            LoginAccount = 2,
            PositionUpdate = 3
        }

        static void HandlePacket(ref Event netEvent)
        {
            try
            {
                var readBuffer = new byte[1024];
                var readStream = new MemoryStream(readBuffer);
                var reader = new BinaryReader(readStream);

                readStream.Position = 0;
                netEvent.Packet.CopyTo(readBuffer);
                var packetID = (PacketID)reader.ReadByte();

                if (packetID == PacketID.CreateAccount)
                {
                    var name = reader.ReadString();

                    Console.WriteLine(name);
                    using (var db = new UserContext())
                    {
                        User user = db.Users.ToList().Find(b => b.Name.Contains(name));
                        if (user != null) {
                            Console.WriteLine($"User '{name}' already exists.");
                            return;
                        }

                        Console.WriteLine($"Registering '{name}' to database");
                        db.Add(new User { Name = name });
                        db.SaveChanges();
                    }
                }
            }

            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Received packet but buffer was too long. {netEvent.Packet.Length}");
            }
        }
    }
}