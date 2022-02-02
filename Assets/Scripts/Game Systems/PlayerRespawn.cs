using System.Collections;
using System.Collections.Generic;
using Input_Systems;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using Game_Systems.Utility;

namespace Game_Systems {
    public class PlayerRespawn : NetworkBehaviour
    {
        
        private PlayerController cc;
        private Renderer[] renderers;
        [SerializeField] Behaviour[] scripts;
        [SerializeField] private GameObject canvas;
        private PlayerHealth health;
        CapsuleCollider playerCollider;

        // Start is called before the first frame update
        void Start()
        {
            playerCollider = GetComponentInParent<CapsuleCollider>();
            cc = GetComponentInParent<PlayerController>();
            renderers = transform.parent.GetComponentsInChildren<Renderer>();
            health = GetComponent<PlayerHealth>();
        }

        public void Respawn() {
            StartCoroutine(RespawnProcedure());
        }

        private IEnumerator RespawnProcedure()
        {
            if (!IsServer) {
                Debug.LogError("Method may only be called by server");
                yield break;
            }
            
            if(!IsClient)
                HidePlayer();
            HidePlayerClientRpc();
            
            yield return new WaitForSeconds(5); 
            
            health.Health = 100;
            
            int numPlayers = NetworkManager.Singleton.ConnectedClients.Count;
            List<Vector3> li = RespawnPointGenerator.generatePoints(numPlayers, 70);
            int r = RespawnPointGenerator.rnd.Next(li.Count);
            
            if(!IsClient)
                RespawnPlayer(li[r]);
            RespawnPlayerClientRpc(li[r]);
        }

        [ClientRpc]
        private void HidePlayerClientRpc() {
            HidePlayer();
        }
        
        private void HidePlayer() {
            cc.enabled = false;
            playerCollider.enabled = false;
            PlayerState(false);
            if (IsOwner) {
                canvas.SetActive(false);
                CharacterInputManager.Instance.enabled = false;
            }
        }

        [ClientRpc]
        private void RespawnPlayerClientRpc(Vector3 position) {
            RespawnPlayer(position);
        }

        private void RespawnPlayer(Vector3 position) {
            transform.parent.position = position;
            cc.enabled = true;
            playerCollider.enabled = true;
            PlayerState(true);
            if (IsOwner) {
                canvas.SetActive(true);
                GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
                CharacterInputManager.Instance.enabled = true;
            }

            //TODO: reload guns
        }

        private void PlayerState(bool state)
        {
            foreach(var script in scripts)
            {
                script.enabled = state;
            }
            foreach(var render in renderers)
            {
                render.enabled = state;
            }
        }
        

    }
}
