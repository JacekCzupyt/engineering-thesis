using UnityEngine;

namespace Input_Systems {
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] public float mouseSensitivity = 5f;
        public Transform playerBody;

        private CharacterInputManager input;
        private void Start(){
            Cursor.lockState = CursorLockMode.Locked;
            input = CharacterInputManager.Instance;
        }
        private void Update() {
            var rotation = input.GetMouseDelta() * mouseSensitivity;

            if (input.GetRollMod()) {
                //roll
                playerBody.Rotate(Vector3.back, rotation.x);
            }
            else {
                //yaw
                playerBody.Rotate(Vector3.up, rotation.x);
            }

            //pitch
            playerBody.Rotate(Vector3.left, rotation.y);
        }
    }
}

