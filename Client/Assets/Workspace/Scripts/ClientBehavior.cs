using UnityEngine;

namespace Valk.Networking 
{
    class ClientBehavior : MonoBehaviour
    {
        private Rigidbody2D clientGoRb;

        public float MoveSpeed = 125;

        public float px { get; set; }
        public float py { get; set; }

        private void Start() 
        {
            clientGoRb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() 
        {
            if (!Client.InGame)
                return;

            if (clientGoRb == null)
                return;

            float inputHorz = Input.GetAxis("Horizontal") * MoveSpeed;
            float inputVert = Input.GetAxis("Vertical") * MoveSpeed;
            clientGoRb.AddForce(new Vector2(inputHorz, inputVert));
        }
    }
}