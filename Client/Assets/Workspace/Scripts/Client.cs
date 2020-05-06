using UnityEngine;
using UnityEngine.SceneManagement;
using ENet;
using Server;

class Client : MonoBehaviour
{
    private static Peer peer;
    private static Host client;
    private const int channelID = 0;

    void Start()
    {
        Application.targetFrameRate = 30;
        Application.runInBackground = true;
        DontDestroyOnLoad(gameObject);
    }

    public static void Connect()
    {
        const string ip = "127.0.0.1";
        const ushort port = 7777;

        ENet.Library.Initialize();

        client = new Host();

        var address = new Address();
        address.SetHost(ip);
        address.Port = port;

        client.Create();

        Debug.Log("Connecting...");
        peer = client.Connect(address);
    }

    void FixedUpdate() 
    {
        if (client == null)
            return;
        
        UpdateENet();
    }

    void UpdateENet()
    {
        ENet.Event netEvent;

        if (client.CheckEvents(out netEvent) <= 0)
        {
            if (client.Service(15, out netEvent) <= 0)
                return;
        }

        switch (netEvent.Type)
        {
            case ENet.EventType.None:
                break;

            case ENet.EventType.Connect:
                Debug.Log("Client connected to server");
                break;

            case ENet.EventType.Disconnect:
                Debug.Log("Client disconnected from server");
                break;

            case ENet.EventType.Timeout:
                Debug.Log("Client connection timeout");

                break;

            case ENet.EventType.Receive:
                Debug.Log("Packet received from server - Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
                netEvent.Packet.Dispose();
                break;
        }
    }

    public static void CreateAccount(string name) 
    {
        var protocol = new Protocol();
        var buffer = protocol.Serialize((byte) PacketID.CreateAccount, name);
        var packet = default(Packet);
        packet.Create(buffer);
        peer.Send(channelID, ref packet);
    }

    enum PacketID
    {
        HeartBeat = 0,
        CreateAccount = 1,
        LoginAccount = 2,
        PositionUpdate = 3
    }

    public static bool IsConnected()
    {
        return peer.State == ENet.PeerState.Connected;
    }

    void OnDestroy()
    {
        client.Dispose();
        ENet.Library.Deinitialize();
    }
}