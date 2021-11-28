using UnityEngine;

namespace Game_Systems {
    public class SceneManager : MonoBehaviour {
        [SerializeField] private GameObject projectileContainer;
        
        public GameObject ProjectileContainer => projectileContainer;

        public static SceneManager Instance { get; private set; }

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(this.gameObject);
            }
            else {
                Instance = this;
            }
        }
    }
}
