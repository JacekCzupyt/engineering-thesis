using System.Collections;
using Input_Systems;
using MLAPI;
using MLAPI.Messaging;
using NetPortals;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Systems {
    public class CheckGameState : NetworkBehaviour {

        private PlayerController cc;
        private Renderer[] renderers;
        [SerializeField] Behaviour[] scripts;
        [SerializeField] private GameObject canvas;
        CapsuleCollider playerCollider;

        [SerializeField] ScoreSystem score;

        // Start is called before the first frame update
        // Update is called once per frame

        void Start() {
            playerCollider = GetComponentInParent<CapsuleCollider>();
            cc = GetComponentInParent<PlayerController>();
            renderers = transform.parent.GetComponentsInChildren<Renderer>();
        }

        public void checkUserScore(string player_name) {
            if (score.userScore.Value >= 2) {
                endGameClientRPC(player_name);
            }
        }
        [ClientRpc]
        private void endGameClientRPC(string name) {
            StartCoroutine(WaitForGameEnd(name));
        }
        IEnumerator WaitForGameEnd(string winner) {          
            cc.enabled = false;
            if (IsOwner)
                canvas.SetActive(false);
            //playerCollider.enabled = false;
            PlayerState(false);
            GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
            yield return new WaitForSeconds(5);
            Cursor.lockState = CursorLockMode.None;
            GameNetPortal.Instance.RequestDisconnect();
        }
        private void PlayerState(bool state) {
            foreach(var script in scripts) {
                script.enabled = state;
            }
            foreach(var render in renderers) {
                render.enabled = state;
            }
        }
    }
}
                
