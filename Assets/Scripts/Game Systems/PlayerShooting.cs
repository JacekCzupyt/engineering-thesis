using System.Linq;
using Input_Systems;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game_Systems {
    public class PlayerShooting : NetworkBehaviour

    {
        [SerializeField] private float tickRate = 20;

        [SerializeField] Camera cam;
        [SerializeField] ParticleSystem bulletSystem;
        private ParticleSystem.EmissionModule em;

        private float lastSendTime;
        
        private CharacterInputManager input;

        private bool firing = false;
        //NetworkVariable<ParticleSystem> par;
        private ClientRpcParams NonOwnerClientParams =>
            new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = NetworkManager.Singleton.ConnectedClientsList.Where(c => c.ClientId != OwnerClientId)
                        .Select(c => c.ClientId).ToArray()
                }
            };
        // Start is called before the first frame update
        void Start()
        {
            //  par = new NetworkVariable<ParticleSystem>(bulletSystem);
            em = bulletSystem.emission;
            input = CharacterInputManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            //bulletSystem = par.Value;
            if (IsOwner)
            {
                SendData();
            } 
        }
        void SendData()
        {       
            if (input.GetFireAction() == InputActionPhase.Started) {
                firing = true;
                ShootServerRPC(true);
            }
            else if(firing) {
                firing = false;
                ShootServerRPC(false);
            }
        }
        [ServerRpc]
        private void ShootServerRPC(bool isShoot,ServerRpcParams rpcParams = default)
        {
            if (!(NetworkManager.Singleton.NetworkTime - lastSendTime >= (1f / tickRate)) && isShoot)
                return;
            lastSendTime = NetworkManager.Singleton.NetworkTime;
            // Debug.Log("HI" + isShoot);
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = cam.transform.position;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var enemyHealth = hit.transform.GetComponentInChildren<PlayerHealth>();
                if (enemyHealth!=null)
                {
                    enemyHealth.takeDemage(1,GetComponent<NetworkObject>().OwnerClientId);
                }

            }
            ShootClientRPC(isShoot,NonOwnerClientParams);

        }

        [ClientRpc]
        private void ShootClientRPC(bool isShoot,ClientRpcParams rpcParams = default)
        {
            if (isShoot)
                em.rateOverTime = 10f;
            else
                em.rateOverTime = 0f;
        }
   
    }
}
