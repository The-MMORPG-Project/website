using UnityEngine;
using UnityEngine.SceneManagement;
using ENet;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Valk.Networking
{
    class Client : MonoBehaviour
    {
        public const byte CHANNEL_ID = 0;
        private const int MAX_FRAMES = 30;
        private const int TIMEOUT_SEND = 1000 * 5;
        private const int TIMEOUT_RECEIVE = 1000 * 30;
        private const int POSITION_UPDATE_DELAY = 100;
        private const ushort PORT = 7777;

        public static Peer Peer { get; set; }
        public static Host Host { get; set; }
        public static bool InGame { get; set; }
        public static bool Running { get; set; }
        public static bool Connected { get; set; }
        public static bool Disconnecting { get; set; }
        public static bool IsQuitting { get; set; }

        private Dictionary<uint, GameObject> clients;
        private static GameObject oClientPrefab;
        private GameObject clientGo;
        private ClientBehavior clientGoScript;
        private Transform clientGoT;

        private void Start()
        {
            Running = true;

#if !UNITY_WEBGL
            Application.targetFrameRate = MAX_FRAMES;
#endif
            Application.runInBackground = true;
            DontDestroyOnLoad(gameObject);

            oClientPrefab = Resources.Load("Prefabs/Client") as GameObject;
            clients = new Dictionary<uint, GameObject>();
            Application.wantsToQuit += WantsToQuit;
        }

        public static void Connect(string ip)
        {
            ENet.Library.Initialize();

            Host = new Host();

            var address = new Address();
            address.SetHost(ip);
            address.Port = PORT;

            Host.Create();

            Debug.Log("Connecting...");
            Peer = Host.Connect(address);
            Peer.Timeout(0, TIMEOUT_RECEIVE, TIMEOUT_SEND);
        }

        private void FixedUpdate()
        {
            UpdateENet();
        }

        private void UpdateENet()
        {
            if (Host == null)
                return;

            ENet.Event netEvent;

            if (!Running && !Disconnecting)
            {
                Disconnecting = true;
                Network.Send(PacketType.ClientDisconnect, PacketFlags.Reliable);
                return;
            }

            if (Host.CheckEvents(out netEvent) <= 0)
            {
                if (Host.Service(15, out netEvent) <= 0)
                    return;
            }

            switch (netEvent.Type)
            {
                case ENet.EventType.None:
                    break;

                case ENet.EventType.Connect:
                    Debug.Log("Client connected to server");
                    Connected = true;
                    break;

                case ENet.EventType.Disconnect:
                    Debug.Log("Client disconnected from server");
                    Connected = false;

                    if (!IsQuitting)
                    {
                        Destroy(gameObject);
                        CleanUp();
                        SceneManager.LoadScene("Main Menu");
                        Disconnecting = false;
                    }

                    break;

                case ENet.EventType.Timeout:
                    Debug.Log("Client connection timeout");
                    Destroy(gameObject);
                    CleanUp();
                    SceneManager.LoadScene("Main Menu");
                    break;

                case ENet.EventType.Receive:
                    //Debug.Log("Packet received from server - Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
                    HandlePacket(ref netEvent);
                    netEvent.Packet.Dispose();
                    break;
            }
        }

        private void HandlePacket(ref ENet.Event netEvent)
        {
            try
            {
                var readBuffer = new byte[1024];
                var readStream = new MemoryStream(readBuffer);
                var reader = new BinaryReader(readStream);

                readStream.Position = 0;
                netEvent.Packet.CopyTo(readBuffer);
                var packetID = (PacketType)reader.ReadByte();

                if (packetID == PacketType.ServerCreateAccountDenied)
                {
                    Debug.Log("Account denied");
                    var reason = reader.ReadString();
                    Debug.Log("Reason: " + reason);
                    UIAccountManagement.UpdateText($"Account Denied: {reason}");
                }

                if (packetID == PacketType.ServerCreateAccountAccepted)
                {
                    Debug.Log("Account created");
                    UIAccountManagement.UpdateText($"Account Created");
                }

                if (packetID == PacketType.ServerLoginAccepted)
                {
                    Debug.Log("Login accepted");
                    UIAccountManagement.UpdateText($"Logging in...");
                    StartCoroutine(ASyncLoadGame());
                }

                if (packetID == PacketType.ServerLoginDenied)
                {
                    Debug.Log("Login denied");
                    var reason = reader.ReadString();
                    Debug.Log("Reason: " + reason);
                    UIAccountManagement.UpdateText($"Login Denied: {reason}");
                }

                if (packetID == PacketType.ServerPositionUpdate)
                {
                    //Debug.Log("Received Server Position Update");
                    var id = reader.ReadUInt32();
                    var x = reader.ReadSingle();
                    var y = reader.ReadSingle();
                    //Debug.Log($"ID: {id}, X: {x}, Y: {y}");
                    if (clients.ContainsKey(id))
                    {
                        Debug.Log("Updated position of oClient");

                        if (clients[id] == null)
                        {
                            clients.Remove(id);
                        }
                        else
                        {
                            clients[id].transform.position = new Vector2(x, y);
                        }
                    }
                    else
                    {
                        Debug.Log($"Added new oClient '{id}'");
                        var oClient = Instantiate(oClientPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        oClient.name = $"oClient {id}";
                        clients.Add(id, oClient);
                    }
                }

                if (packetID == PacketType.ServerClientDisconnected)
                {
                    var id = reader.ReadUInt32();

                    clients.Remove(id);
                    Debug.Log($"oClient {id}");
                    Destroy(GameObject.Find($"oClient {id}"));
                    Debug.Log($"Client {id} disconnected");
                }
            }

            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Received packet but buffer was too long. {netEvent.Packet.Length}");
            }
        }

        private IEnumerator ASyncLoadGame()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Spawn();
        }

        private void Spawn()
        {
            clientGo = Instantiate(oClientPrefab, Vector3.zero, Quaternion.identity);
            clientGoScript = clientGo.AddComponent<ClientBehavior>();
            clientGoT = clientGo.transform;

            InGame = true;

            // We are in the game, we are ready to receive the initial positions of all the other clients
            Network.Send(PacketType.ClientRequestPositions, PacketFlags.Reliable);

            StartCoroutine(SendPositionUpdates());
        }

        private IEnumerator SendPositionUpdates()
        {
            while (InGame)
            {
                var pos = clientGoT.position;

                if (pos.x != clientGoScript.px || pos.y != clientGoScript.py)
                {
                    Network.Send(PacketType.ClientPositionUpdate, PacketFlags.None, (int)pos.x, (int)pos.y);
                }

                clientGoScript.px = pos.x;
                clientGoScript.py = pos.y;

                yield return new WaitForSeconds(POSITION_UPDATE_DELAY / 1000);
            }
        }

        public static bool IsConnected()
        {
            return Peer.State == ENet.PeerState.Connected;
        }

        private void OnApplicationQuit()
        {

        }

        private bool WantsToQuit()
        {
            IsQuitting = true;

            if (Host == null) // Might press player then stop in Unity editor before client is created
                return false;

            Network.Send(PacketType.ClientDisconnect, PacketFlags.Reliable);

            if (Connected)
            {
                StartCoroutine(Quitting());
                return false;
            }

            return true;
        }

        private IEnumerator Quitting()
        {
            while (Connected)
            {
                yield return new WaitForSeconds(0.05f);
            }

            Application.Quit();
        }

        private void CleanUp()
        {
            Debug.Log("Disposing client");
            Host.Dispose();
            ENet.Library.Deinitialize();
        }
    }
}
