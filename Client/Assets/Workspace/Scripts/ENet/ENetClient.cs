using UnityEngine;
using UnityEngine.SceneManagement;
using ENet;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using TMPro;

namespace Valk.Networking
{
    public class ENetClient : MonoBehaviour
    {
        public const byte CHANNEL_ID = 0;
        private const int MAX_FRAMES = 30;
        private const int TIMEOUT_SEND = 1000 * 5;
        private const int TIMEOUT_RECEIVE = 1000 * 30;
        private const int POSITION_UPDATE_DELAY = 100;
        private const ushort PORT = 7777;

        public static Peer Peer { get; set; }
        public static Host Host { get; set; }
        public static bool Running { get; set; }
        public static bool Connected { get; set; }
        public static bool Disconnecting { get; set; }
        public static bool IsQuitting { get; set; }
        public static bool InGame { get; set; }

        public static uint myID;
        public static string myName;

        private void Start()
        {
            Running = true;

#if !UNITY_WEBGL
            Application.targetFrameRate = MAX_FRAMES;
#endif
            Application.runInBackground = true;
            DontDestroyOnLoad(gameObject);

            Application.wantsToQuit += WantsToQuit;

            StartCoroutine(SendPositionUpdates());
        }

        public static void Connect(string ip)
        {
            ENet.Library.Initialize();

            Host = new Host();

            var address = new Address();
            address.SetHost(ip);
            address.Port = PORT;

            Host.Create();

            //Debug.Log("Connecting...");
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
                ENetNetwork.Send(PacketType.ClientDisconnect, PacketFlags.Reliable);
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
                    //Debug.Log("Client connected to server");
                    Connected = true;
                    break;

                case ENet.EventType.Disconnect:
                    Debug.Log("Client disconnected from server");
                    Connected = false;

                    GameRoom.clients.Remove(myID);

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

                    GameRoom.clients.Remove(myID);

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

                if (packetID == PacketType.ServerPositionUpdate)
                {
                    //Debug.Log("Received Server Position Update");
                    var oID = reader.ReadUInt32();
                    var oX = reader.ReadSingle();
                    var oY = reader.ReadSingle();
                    //Debug.Log($"ID: {oID}, X: {oX}, Y: {oY}");

                    if (GameRoom.clients.ContainsKey(oID))
                    {
                        if (GameRoom.clients[oID] == null)
                        {
                            Debug.LogWarning("oClient is null");
                            GameRoom.clients.Remove(oID);
                        }
                        else
                        {
                            //Debug.Log($"Updated position for other client ID '{oID}' x: {oX}, y: {oY}");
                            GameRoom.clients[oID].transform.position = new Vector2(oX, oY);

                            return;
                        }
                    }

                    if (oID == myID) // myID being the ID of THIS client
                    {
                        // This is just a safeguard, this was caused before because the server was using a Dict<> instead of a List<> causing the server position queue to not be in sync
                        Debug.LogWarning("Received position update for THIS client. Ignoring..");
                        return;
                    }

                    GameRoom.CreateOtherClient(oID, oX, oY);
                }

                if (packetID == PacketType.ServerClientDisconnected)
                {
                    var id = reader.ReadUInt32();

                    GameRoom.clients.Remove(id);
                    Destroy(GameObject.Find($"oClient {id}"));
                    Debug.Log($"Client {id} disconnected");
                }
            }

            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Received packet but buffer was too long. {netEvent.Packet.Length}");
            }
        }

        public IEnumerator SendPositionUpdates()
        {
            while (InGame)
            {
                var pos = GameRoom.clientGoT.position;

                if (pos.x != GameRoom.clientGoScript.px || pos.y != GameRoom.clientGoScript.py)
                {
                    ENetNetwork.Send(PacketType.ClientPositionUpdate, PacketFlags.None, pos.x, pos.y);
                }

                GameRoom.clientGoScript.px = pos.x;
                GameRoom.clientGoScript.py = pos.y;

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

            ENetNetwork.Send(PacketType.ClientDisconnect, PacketFlags.Reliable);

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
