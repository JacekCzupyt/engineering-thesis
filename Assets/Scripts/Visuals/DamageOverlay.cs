using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Visuals {
    public class DamageOverlay : MonoBehaviour {
        [SerializeField] private float rate = 2;

        private float alpha = 0;
        private Image overlay;

        private void Start() {
            overlay = GetComponent<Image>();
        }

        void Update() {
            alpha = Mathf.Lerp(0, alpha, 1 - (Time.deltaTime * rate));
            overlay.color = new Color(1, 0, 0, alpha);
        }

        public void Trigger() {
            alpha = 1;
        }
    }
}
