using MLAPI;
using UnityEditor;
using UnityEngine;
using Utility;

namespace Input_Systems {
    public class PlayerController : NetworkBehaviour {

        [SerializeField] Canvas barHealth;

        private void Start() {
            if (!IsOwner) {
                foreach(Transform child in transform) {
                    if (child.CompareTag("ClientSide")) {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
