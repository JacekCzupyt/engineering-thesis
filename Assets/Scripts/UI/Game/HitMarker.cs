using System;
using UnityEngine;

namespace UI.Game {
    public class HitMarker : MonoBehaviour {
        private CanvasGroup canvasGroup;
        [SerializeField] private float hitMarkerDuration = 0.2f;

        private void Start() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update() {
            canvasGroup.alpha = Math.Max(0, canvasGroup.alpha - Time.deltaTime / hitMarkerDuration);
        }

        public void Trigger() {
            canvasGroup.alpha = 1;
        }
    }
}
