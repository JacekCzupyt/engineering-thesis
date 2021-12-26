using UnityEngine;

namespace Input_Systems {
    public class MouseLook : MonoBehaviour {
        [SerializeField] private Camera cam;
        [SerializeField] public float mouseSensitivity = 5f;
        [SerializeField] public float rollSensitivity = 0.5f;
        public Transform playerBody;
        private const float DefaultCamFov = 60f;

        private CharacterInputManager input;
        private void Start(){
            Cursor.lockState = CursorLockMode.Locked;
            input = CharacterInputManager.Instance;
        }
        private void Update() {
            var sensitivity = (mouseSensitivity * cam.fieldOfView / DefaultCamFov);

            if (input.GetRollMod()) {
                //roll
                playerBody.Rotate(Vector3.back, (input.GetMouseDelta() * rollSensitivity).x);
            }
            else {
                //yaw
                playerBody.Rotate(Vector3.up, (input.GetMouseDelta() * sensitivity).x);
            }

            //pitch
            playerBody.Rotate(Vector3.left, (input.GetMouseDelta() * sensitivity).y);
        }
    }
}

