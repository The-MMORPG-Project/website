using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using ENet;
using EF;

class Program
{
    private static List<Client> clients;
    private static List<Player> players;

    private const ushort port = 7777;
    private const int maxClients = 8;
    public const int CHANNEL_ID = 0;
    private static Network network;
    private static int packetCounter = 0;

    public static void Main(string[] args)
    {
        clients = new List<Client>();
        players = new List<Player>();

        Console.Clear();

        Thread server = new Thread(StartServer);
        Thread inputs = new Thread(HandleInput);
        server.Start();
        inputs.Start();
    }

    private static void HandleInput() 
    {
        Thread.Sleep(500); // Temporary until I can find a better solution
        string input;
        while (true)
        {
            //Console.Write(": ");
            input = Console.ReadLine();
            HandleCommands(input);
        }
    }

    private static void HandleCommands(string input)
    {
        string cmd = input.ToLower();

        if (cmd.Equals("help"))
        {
            Console.WriteLine("Useful command");
        }
    }

    private static void StartServer()
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

    private static void HandlePacket(ref Event netEvent)
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

                        Network.Send(ref netEvent, Packet.Type.ServerCreateAccountDenied, "Account name already registered");

                        return;
                    }

                    // Account is unique, creating...
                    Console.WriteLine($"Registering '{name}' to database");
                    db.Add(new User { Name = name, Pass = pass });
                    db.SaveChanges();

                    Network.Send(ref netEvent, Packet.Type.ServerCreateAccountAccepted);
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

                        Network.Send(ref netEvent, Packet.Type.ServerLoginDenied, "User login does not exist");

                        return;
                    }

                    // User login exists
                    if (!user.Pass.Equals(pass))
                    {
                        // Logged in with wrong password
                        Network.Send(ref netEvent, Packet.Type.ServerLoginDenied, "Wrong password");
                        Console.WriteLine($"User '{user}' failed to login");
                        return;
                    }

                    // Logged in with correct password
                    Console.WriteLine($"User '{user}' successfully logged in");
                    players.Add(new Player(netEvent.Peer));
                    Network.Send(ref netEvent, Packet.Type.ServerLoginAccepted);
                }
            }

            if (packetID == Packet.Type.ClientPositionUpdate)
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                Console.WriteLine($"Recieved x {x}, y {y}");
            }
        }

        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Received packet but buffer was too long. {netEvent.Packet.Length}");
        }
    }
}