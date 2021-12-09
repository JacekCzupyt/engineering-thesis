using System;
using System.Linq;
using Audio;
using Input_Systems;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

namespace Game_Systems.Equipment {
    public class HitscanWeapon : NetworkBehaviour {
        //TODO: recoil, bloom, damage falloff?, physics recoil?, ammo
        
        //TODO: variable pitch depending on ammo

        [SerializeField] private Camera cam;
        [SerializeField] private GameObject gunModel;

        [SerializeField] private bool fullAuto;
        [SerializeField] private float fireRate;
        [SerializeField] private int maxAmmoCount;
        [SerializeField] private int currentAmmoCount;

        [SerializeField] private int damage;

        [SerializeField] private SoundPlayer fireAudio;
        [SerializeField] private SoundPlayer misfireAudio;
        
        private CharacterInputManager input;
        private ParticleSystem particles;
        private bool firedDuringAction = false;
        private bool misfiredDuringAction = false;
        private float lastShotTime = float.NegativeInfinity;

        private void OnEnable() {
            gunModel.SetActive(true);
        }

        private void OnDisable() {
            gunModel.SetActive(false);
        }

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
            currentAmmoCount = maxAmmoCount;
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
            if (fireAction && !misfiredDuringAction && (!firedDuringAction || fullAuto)) {
                if (Time.time - lastShotTime >= 1f / fireRate) {
                    AttemptFireWeapon();
                }
            }
            if (!fireAction) {
                firedDuringAction = false;
                misfiredDuringAction = false;
            }

        }

        private void AttemptFireWeapon() {
            firedDuringAction = true;
            if(currentAmmoCount > 0)
                FireWeapon();
            else {
                misfiredDuringAction = true;
                misfireAudio.Play();
            }
        }

        private void FireWeapon() {
            lastShotTime = Time.time;
            currentAmmoCount--;
            
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
