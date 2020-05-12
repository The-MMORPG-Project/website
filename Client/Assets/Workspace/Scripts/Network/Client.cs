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
        private const int MAX_FRAMES = 30;
        private const int TIMEOUT_SEND = 1000 * 5;
        private const int TIMEOUT_RECEIVE = 1000 * 30;
        public const byte CHANNEL_ID = 0;
        private const int POSITION_UPDATE_DELAY = 100;

        private const ushort PORT = 7777;

        public static Peer Peer;
        public static Host Host;

        private Dictionary<uint, GameObject> clients;

        private static GameObject oClientPrefab;
        private GameObject clientGo;
        private Transform clientGoT;
        
        public static bool InGame = false;

        private void Start()
        {
#if !UNITY_WEBGL
            Application.targetFrameRate = MAX_FRAMES;
#endif
            Application.runInBackground = true;
            DontDestroyOnLoad(gameObject);

            oClientPrefab = Resources.Load("Prefabs/Client") as GameObject;
            clients = new Dictionary<uint, GameObject>();
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
            if (Host == null)
                return;

            UpdateENet();
        }

        private void UpdateENet()
        {
            ENet.Event netEvent;

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
                    break;

                case ENet.EventType.Disconnect:
                    Debug.Log("Client disconnected from server");
                    break;

                case ENet.EventType.Timeout:
                    Debug.Log("Client connection timeout");
                    Destroy(gameObject);
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
                        GameObject oClient = Instantiate(oClientPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        clients.Add(id, oClient);
                    }
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
            clientGo.AddComponent<ClientBehavior>();
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
                Vector3 pos = clientGoT.position;
                Network.Send(PacketType.ClientPositionUpdate, PacketFlags.None, (int) pos.x, (int) pos.y);

                yield return new WaitForSeconds(POSITION_UPDATE_DELAY / 1000);
            }
        }

        public static bool IsConnected()
        {
            return Peer.State == ENet.PeerState.Connected;
        }

        private void OnApplicationQuit()
        {
            if (Host == null) // Might press player then stop in Unity editor before client is created
                return;

            Debug.Log("Disposing client");
            Host.Dispose();
            ENet.Library.Deinitialize();
        }
    }
}
