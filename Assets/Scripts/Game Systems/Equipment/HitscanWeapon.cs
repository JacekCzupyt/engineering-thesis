using System.Linq;
using Input_Systems;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game_Systems.Equipment {
    public class HitscanWeapon : NetworkBehaviour {
        //TODO: recoil, bloom, damage falloff?, physics recoil?, ammo
        
        //TODO: sound, particle effects

        [SerializeField] private Camera cam;

        [SerializeField] private bool fullAuto;
        [SerializeField] private float fireRate;

        [SerializeField] private int damage;
        
        private CharacterInputManager input;
        private bool firing = false;
        private float lastShotTime = float.NegativeInfinity;
        
        private ClientRpcParams NonOwnerClientParams =>
            new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = NetworkManager.Singleton.ConnectedClientsList.Where(c => c.ClientId != OwnerClientId)
                        .Select(c => c.ClientId).ToArray()
                }
            };

        private void Start() {
            input = CharacterInputManager.Instance;
        }

        // Update is called once per frame
        private void Update()
        {
            if (IsOwner) {
                CheckFiringState();
            }
        }

        private void CheckFiringState() {
            var fireAction = input.GetFireAction() == InputActionPhase.Started;
            if (fireAction && (!firing || fullAuto)) {
                firing = true;
                if (Time.time - lastShotTime >= 1f / fireRate) {
                    FireWeapon();
                }
            }
            firing = fireAction;
        }

        private void FireWeapon() {
            lastShotTime = Time.time;
            
            var ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = cam.transform.position;
            GameObject hitPlayer = null;
            var hitPlayerId = ulong.MaxValue;
            
            if (Physics.Raycast(ray,out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Player", "Terrain")))
            {
                if (hit.collider.gameObject.CompareTag("Player")) {
                    hitPlayer = hit.collider.gameObject;
                    hitPlayerId = hitPlayer.GetComponent<NetworkObject>().NetworkObjectId;
                }
            }

            ShotServerRPC(ray,  hitPlayerId);
        }

        [ServerRpc]
        private void ShotServerRPC(Ray shot, ulong playerHitId, ServerRpcParams rpcParams = default) {
            // if (!enabled)
            //     return;

            if (playerHitId != ulong.MaxValue) {
                var playerHit = NetworkSpawnManager.SpawnedObjects[playerHitId];

                //TODO: validate hit
            
                var enemyHealth = playerHit.transform.GetComponentInChildren<PlayerHealth>();
                if (enemyHealth!=null)
                {
                    enemyHealth.takeDemage(damage, OwnerClientId);
                }
            }

            ShootClientRPC(shot);
        }
        
        [ClientRpc]
        private void ShootClientRPC(Ray shot, ClientRpcParams rpcParams = default) {
            if (!enabled || IsOwner)
                return;

            //TODO: implement visuals
        }
    }
}
