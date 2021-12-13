using System.Collections;
using System.Collections.Generic;
using Input_Systems;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using Game_Systems.Utility;

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
            
            int numPlayers = NetworkManager.Singleton.ConnectedClients.Count;
            List<Vector3> li = RespawnPointGenerator.generatePoints(numPlayers);
            int r = RespawnPointGenerator.rnd.Next(li.Count);
            RespawnClientRpc(li[r]);
        }
        [ClientRpc]
        private void RespawnClientRpc(Vector3 vec)
        {
            StartCoroutine(WaitForRespawn(vec));     
        }
        private Vector3 RandomPos()
        {
            return new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            /*
            if (num == 1)
                return new Vector3(0, 0, 0);
            List<Vector3> li= RespawnPointGenerator.generatePoints(num);
            int r = RespawnPointGenerator.rnd.Next(li.Count);            
            return new Vector3(li[r].x, li[r].y, li[r].z);
            */
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
