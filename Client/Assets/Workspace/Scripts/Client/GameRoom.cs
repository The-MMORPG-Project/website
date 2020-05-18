using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

using ENet;

namespace Valk.Networking
{
    public class GameRoom : MonoBehaviour
    {
        // Other Clients
        public static Dictionary<uint, GameObject> clients;
        public static GameObject oClientPrefab;
        public static GameObject oClientCanvasPrefab;

        // Client
        public static GameObject clientGo;
        public static ClientBehaviour clientGoScript;
        public static Transform clientGoT;

        private void Start() 
        {
            oClientPrefab = Resources.Load("Prefabs/Client") as GameObject;
            oClientCanvasPrefab = Resources.Load("Prefabs/Canvas") as GameObject;
            clients = new Dictionary<uint, GameObject>();

            SpawnClient();
        }

        public void SpawnClient()
        {
            clientGo = Instantiate(oClientPrefab, Vector3.zero, Quaternion.identity);
            clientGo.name = $"mClient (You)";
            clientGoScript = clientGo.AddComponent<ClientBehaviour>();
            clientGoT = clientGo.transform;
            var ui = Instantiate(oClientCanvasPrefab, Vector3.zero, Quaternion.identity);
            ui.GetComponent<Canvas>().worldCamera = Camera.main;
            ui.transform.SetParent(clientGo.transform);
            ui.transform.position = new Vector3(0, 0.35f, 0);
            ui.GetComponentInChildren<TMP_Text>().text = ENetClient.myName;

            var pos = clientGoT.position;
            ENetNetwork.Send(PacketType.ClientPositionUpdate, PacketFlags.Reliable, pos.x, pos.y);

            // We are in the game, we are ready to receive the initial positions of all the other clients
            ENetNetwork.Send(PacketType.ClientRequestPositions, PacketFlags.Reliable);

            ENetClient.InGame = true;
        }

        public static void CreateOtherClient(uint oID, float oX, float oY)
        {
            var oClient = Instantiate(oClientPrefab, Vector3.zero, Quaternion.identity);
            oClient.transform.position = new Vector3(oX, oY, 0);
            oClient.name = $"oClient {oID}";
            clients.Add(oID, oClient);

            var ui = Instantiate(oClientCanvasPrefab, oClient.transform);
            ui.GetComponent<Canvas>().worldCamera = Camera.main;
            ui.transform.SetParent(oClient.transform);
            ui.transform.localPosition = new Vector3(0, 0.35f, 0);
            ui.GetComponentInChildren<TMP_Text>().text = $"{oID}";

            Debug.Log($"Added new oClient '{oID}' at x: {oX}, y: {oY}");
        }
    }
}