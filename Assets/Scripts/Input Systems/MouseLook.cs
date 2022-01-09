using Game_Systems.Settings;
using UnityEngine;
using Utility;

namespace Input_Systems {
    public class MouseLook : MonoBehaviour {
        [SerializeField] private Camera cam;
        public Transform playerBody;
        private SettingsManager settings;

        private CharacterInputManager input;
        private void Start(){
            Cursor.lockState = CursorLockMode.Locked;
            input = CharacterInputManager.Instance;
            settings = SettingsManager.Instance;
        }
        private void Update() {
            var sensitivity = (settings.data.mouseSensitivity * cam.GetHorizontalFov() / settings.data.fov);

            if (input.GetRollMod()) {
                //roll
                playerBody.Rotate(Vector3.back, (input.GetMouseDelta() * settings.data.mouseRollSensitivity).x);
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

