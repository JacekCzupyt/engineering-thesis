using System.Linq;
using Audio;
using Input_Systems;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game_Systems.Equipment {
    public class HitscanWeapon : NetworkBehaviour {
        //TODO: recoil, bloom, damage falloff?, physics recoil?, ammo
        
        //TODO: variable pitch depending on ammo

        [SerializeField] private Camera cam;

        [SerializeField] private bool fullAuto;
        [SerializeField] private float fireRate;

        [SerializeField] private int damage;

        [SerializeField] private VariableSound fireAudio;
        
        private CharacterInputManager input;
        private ParticleSystem particles;
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
            particles = GetComponentInChildren<ParticleSystem>();
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
            
            FireWeaponPresentation();
            
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

        private void FireWeaponPresentation() {
            fireAudio.Play();
            particles.Emit(new ParticleSystem.EmitParams(), 1);
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

            FireWeaponPresentation();
        }
    }
}
