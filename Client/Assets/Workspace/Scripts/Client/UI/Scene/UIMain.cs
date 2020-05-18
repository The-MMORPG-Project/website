using UnityEngine;

namespace Valk.Networking
{
    public class UIMain : MonoBehaviour
    {
        public GameObject PanelGo;
        private bool visible;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                visible = !visible;
                PanelGo.SetActive(visible);
            }
        }

        public void Disconnect()
        {
            ENetClient.Running = false; // By setting Client.Running the client will eventually disconnect
        }
    }

}