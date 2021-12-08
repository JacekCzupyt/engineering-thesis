using System.Collections;
using Input_Systems;
using MLAPI;
using MLAPI.Messaging;
using UI;
using UnityEngine;

namespace Game_Systems {
    public class PlayerRespawn : NetworkBehaviour
    {
        private PlayerController cc;
        private Renderer[] renderers;
        [SerializeField] Behaviour[] scripts;
        [SerializeField] private GameObject canvas;
        CapsuleCollider playerCollider;
        // Start is called before the first frame update
        void Start()
        {
            playerCollider = GetComponentInParent<CapsuleCollider>();
            cc = GetComponentInParent<PlayerController>();
            renderers = transform.parent.GetComponentsInChildren<Renderer>();
        }
        // Update is called once per frame
        void Update()
        {
            if (IsOwner && Input.GetKeyDown(KeyCode.Y))
            {
                Respawn();
            }
        }
        public void Respawn()
        {
            RespawnServerRpc();
        }
        [ServerRpc]
        private void RespawnServerRpc()
        {
            RespawnClientRpc();
        }
        [ClientRpc]
        private void RespawnClientRpc()
        {
            StartCoroutine(WaitForRespawn(RandomPos()));     
        }
        private Vector3 RandomPos()
        {
            return new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        }

        IEnumerator WaitForRespawn(Vector3 randomPos)
        {
            cc.enabled = false;
            playerCollider.enabled = false;
            PlayerState(false);
            if(IsOwner)
                canvas.SetActive(false);
            yield return new WaitForSeconds(5);
            transform.parent.position = randomPos;
            cc.enabled = true;
            playerCollider.enabled = true;
            PlayerState(true);
            if(IsOwner)
                canvas.SetActive(true);
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
