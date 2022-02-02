using Input_Systems;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace Game_Systems {
    public class PlayerShooting : NetworkBehaviour

    {
        private AudioSource myAudio;
        [SerializeField] private float tickRate = 20;

        [SerializeField] Camera cam;
        [SerializeField] ParticleSystem bulletSystem;
        private ParticleSystem.EmissionModule em;

        private float lastSendTime;
        
        private CharacterInputManager input;

        private bool firing = false;

        void Start()
        {
            myAudio = GetComponent<AudioSource>();
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
            if (input.GetFireAction() == InputActionPhase.Started)
            {
                if(!firing)
                    myAudio.Play();
                firing = true;
            }
            else
            {
                firing = false;
                myAudio.Stop();
            }
                
        }
        [ServerRpc]
        private void ShootServerRPC(ServerRpcParams rpcParams = default)
        {

            if (!(NetworkManager.Singleton.NetworkTime - lastSendTime >= (1f / tickRate)))
                return;
            lastSendTime = NetworkManager.Singleton.NetworkTime;
            // Debug.Log("HI" + isShoot);
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = cam.transform.position;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var hitObject = hit.transform.GetComponent<PlayerGameManager>();
                if (hitObject)
                {
                    if(hitObject.GetTeamId() == GetComponent<PlayerGameManager>().GetTeamId())
                    {
                        Debug.Log("Friendly Fire");
                    }else
                    {
                        hitObject.GetComponent<PlayerHealth>().TakeDamage(1, OwnerClientId);
                    }
                }
            }
            ShootClientRPC(this.NonOwnerClientParams());

        }

        [ClientRpc(Delivery = RpcDelivery.Unreliable)]
        private void ShootClientRPC(ClientRpcParams rpcParams = default)
        {
            //var p=Instantiate(bulletSystem, transform.position, transform.rotation);
            var sound = Instantiate(myAudio, transform.position, transform.rotation);

            //p.Play();
            //p.Stop();
        }
   
    }
}
