using System;
using Game_Systems.Settings;
using Input_Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game_Systems.Equipment.Weapons {
    [RequireComponent(typeof(HitscanWeapon))]
    public class WeaponZoom : MonoBehaviour {
        private bool ToggleZoom => settings.data.aimToggle;
        
        [SerializeField] private CameraZoom cam;
        [SerializeField] private float zoomMultiplier;
        [SerializeField] private float zoomTime;
        [SerializeField] private float zoomTimeExponent = 2;

        private float currentZoomMod = 0f;
        private bool currentZoomInState = false;
        private CharacterInputManager input;

        private HitscanWeapon weapon;
        private SettingsManager settings;
        
        private void Start() {
            weapon = GetComponent<HitscanWeapon>();
            input = CharacterInputManager.Instance;
            input.ToggleZoomIn += ZoomInToggleCallback;
            settings = SettingsManager.Instance;
        }

        private void OnDestroy() {
            input.ToggleZoomIn -= ZoomInToggleCallback;
        }

        private void Update() {
            ZoomInHold();

            if (!weapon.enabled)
                currentZoomInState = false;
            
            ManageZoom();
        }

        private float ZoomCurve(float val) {
            return Mathf.Pow(val, zoomTimeExponent);
        }

        private void ManageZoom() {
            var newZoomMod = Mathf.Clamp(currentZoomMod + (currentZoomInState ? 1 : -1) * Time.deltaTime / zoomTime, 0, 1);

            var zoomDelta = ZoomCurve(newZoomMod) - ZoomCurve(currentZoomMod);
            
            cam.currentMultiplier += zoomDelta * (zoomMultiplier-1);
            currentZoomMod = newZoomMod;
        }

        private void ZoomInHold() {
            if (ToggleZoom)
                return;

            currentZoomInState = input.HoldZoomIn.phase == InputActionPhase.Started;
        }

        private void ZoomInToggleCallback() {
            if (!ToggleZoom)
                return;

            currentZoomInState = !currentZoomInState;
        }
    }
}
