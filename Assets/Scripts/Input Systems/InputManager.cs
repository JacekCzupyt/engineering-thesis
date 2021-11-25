using UnityEngine;
using UnityEngine.InputSystem;

namespace Input_Systems {
    public class InputManager : MonoBehaviour {
        private static InputManager _instance;
        public static InputManager Instance {
            get {
                return _instance;
            }
        }
        PlayerControls controls;

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
            }
            controls = new PlayerControls();
        }

        private void OnEnable() {
            controls.Enable();
        }

        private void OnDisable() {
            controls.Disable();
        }

        public Vector3 GetPlayerMovement() {
            var vertical = controls.Player.VerticalMovement.ReadValue<float>();
            var horizontal = controls.Player.HorizontalMovement.ReadValue<Vector2>();
            return new Vector3(horizontal.x, vertical, horizontal.y).normalized;
        }

        public Vector2 GetMouseDelta() {
            return controls.Player.CameraMovement.ReadValue<Vector2>();
        }

        public bool GetRollMod() {
            return controls.Player.CameraRollMod.ReadValue<float>() == 1f;
        }

        public InputActionPhase GetGripAction() {
            return controls.Player.Grip.phase;
        }

        public InputActionPhase GetJumpAction() {
            return controls.Player.Jump.phase;
        }
    }
}
