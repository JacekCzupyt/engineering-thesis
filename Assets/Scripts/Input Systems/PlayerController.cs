using MLAPI;
using UnityEditor;
using UnityEngine;
using Utility;

namespace Input_Systems {
    public class PlayerController : NetworkBehaviour {
        
        public float snapDistance = 3;
        public float networkPullModifier = 1;
        private int Itemindex;
        private int? tickDelta = null;

        [SerializeField] Canvas barHealth;

        private void Start() {
            if (!IsOwner) {
                foreach(Transform child in transform) {
                    if (child.CompareTag("ClientSide")) {
                        child.gameObject.SetActive(false);
                    }
                }
            }
            // EquipItem(0);

            // em = bulletSystem.emission;
        }
    }
}
