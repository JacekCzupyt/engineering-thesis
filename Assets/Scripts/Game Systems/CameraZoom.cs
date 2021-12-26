using System;
using UnityEngine;

namespace Game_Systems {
    public class CameraZoom : MonoBehaviour {
        private Camera cam;
        [SerializeField] public float defaultFov = 60f;

        public float currentMultiplier = 1f;

        private void Start() {
            cam = GetComponent<Camera>();
        }

        private void Update() {
            cam.fieldOfView = defaultFov / currentMultiplier;
        }
    }
}
