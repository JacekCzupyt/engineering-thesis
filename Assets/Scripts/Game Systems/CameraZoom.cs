using System;
using Game_Systems.Settings;
using UnityEngine;
using Utility;

namespace Game_Systems {
    public class CameraZoom : MonoBehaviour {
        private Camera cam;
        private SettingsManager settings;
        [SerializeField] public float defaultFov = 60f;

        public float currentMultiplier = 1f;

        private void Start() {
            cam = GetComponent<Camera>();
            settings = SettingsManager.Instance;
        }

        private void Update() {
            cam.SetHorizontalFov(settings.data.fov / currentMultiplier);;
        }
    }
}
