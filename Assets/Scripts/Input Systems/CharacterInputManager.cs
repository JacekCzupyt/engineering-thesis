using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

namespace Input_Systems {
    public class CharacterInputManager : MonoBehaviour {
        private static CharacterInputManager _instance;
        public static CharacterInputManager Instance {
            get {
                return _instance;
            }
        }
        private PlayerControls controls;
        public PlayerControls.PlayerActions Controls => controls.Player;

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
                controls = new PlayerControls();
                SetupCallbacks();
                DontDestroyOnLoad(this.gameObject);
            }

        }

        private void SetupCallbacks() {
            controls.Player.EquipWeapon1.performed += context => {SwitchEquipment?.Invoke(0);};
            controls.Player.EquipWeapon2.performed += context => {SwitchEquipment?.Invoke(1);};
            controls.Player.EquipWeapon3.performed += context => {SwitchEquipment?.Invoke(2);};
            controls.Player.EquipWeapon4.performed += context => {SwitchEquipment?.Invoke(3);};
            controls.Player.EquipWeapon5.performed += context => {SwitchEquipment?.Invoke(4);};

            controls.Player.ZoomIn.performed += context => { ToggleZoomIn?.Invoke(); };
        }

        private void OnEnable() {
            controls?.Enable();
        }

        private void OnDisable() {
            controls?.Disable();
        }

        public event Action<int> SwitchEquipment;
        public event Action ToggleZoomIn;

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

        public InputActionPhase GetFireAction() {
            return controls.Player.Fire.phase;
        }

        public InputAction AbilityAction => controls.Player.Ability;

        public InputAction HoldZoomIn => controls.Player.ZoomIn;
    }
}
