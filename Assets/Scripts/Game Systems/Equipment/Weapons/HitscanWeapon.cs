using System;
using System.Diagnostics;
using System.Linq;
using Audio;
using Input_Systems;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using UI.Game;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Game_Systems.Equipment.Weapons {
    public class HitscanWeapon : NetworkBehaviour {
        //TODO: damage falloff?

        //TODO: variable pitch depending on ammo

        [SerializeField] public GameObject player;
        [SerializeField] private Camera cam;
        [SerializeField] private GameObject gunModel;

        [SerializeField] private bool fullAuto;
        [SerializeField] private float fireRate;

        [SerializeField] private int damage;

        [Header("Ammo")] [SerializeField] public int maxAmmoCount;
        [SerializeField] public int currentAmmoCount;
        [SerializeField] private float reloadTime;

        [Header("Audio")] [SerializeField] private SoundPlayer fireAudio;
        [SerializeField] private SoundPlayer misfireAudio;
        [SerializeField] private SoundPlayer reloadAudio;

        [Header("Knock-back")] [SerializeField]
        private float knockBackForce;
        

        [SerializeField] private bool simulateRecoil;
        [SerializeField] private bool weaponSpread;
        [SerializeField] private HitMarker hitMarker;

        // private float currentRecoilDeviation;
        private WeaponRecoil recoilManager;
        public WeaponRecoil RecoilManager {
            get {
                if (!recoilManager)
                    recoilManager = GetComponent<WeaponRecoil>();
                if (!recoilManager)
                    throw new MissingComponentException("No weapon recoil component");
                return recoilManager;
            }
        }

        private WeaponSpread spreadManager;
        public WeaponSpread SpreadManager {
            get {
                if (!spreadManager)
                    spreadManager = GetComponent<WeaponSpread>();
                if (!spreadManager)
                    throw new MissingComponentException("No weapon spread component");
                return spreadManager;
            }
        }

        private Rigidbody playerRb;

        private CharacterInputManager input;
        private ParticleSystem particles;
        private bool firedDuringAction = false;
        private bool misfiredDuringAction = false;
        private float lastShotTime = float.NegativeInfinity;

        private bool reloading = false;
        private float reloadStartTime;

        private void OnEnable() {
            gunModel.SetActive(true);
            if (weaponSpread)
                SpreadManager.enabled = true;
        }

        private void OnDisable() {
            if (reloading)
                CancelReload();
            gunModel.SetActive(false);
            if(weaponSpread)
                SpreadManager.enabled = false;
        }


        private void Start() {
            if (!IsOwner)
                return;
            
            Debug.LogWarning("Script started");
            
            input = CharacterInputManager.Instance;
            particles = GetComponentInChildren<ParticleSystem>();
            currentAmmoCount = maxAmmoCount;
            input.Reload.performed += Reload;
            playerRb = player.GetComponent<Rigidbody>();
        }

        private void OnDestroy() {
            if (!IsOwner)
                return;
            
            if(input != null)
                input.Reload.performed -= Reload;
        }

        private void Update() {
            if (IsOwner) {
                CheckFiringState();
            }
        }

        private void CheckFiringState() {
            if (reloading && Time.time - reloadStartTime >= reloadTime) {
                CompleteReload();
            }
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
            if (currentAmmoCount > 0)
                FireWeapon();
            else if (!reloading) {
                misfiredDuringAction = true;
                misfireAudio.Play();
            }
        }

        private void FireWeapon() {
            lastShotTime = Time.time;
            currentAmmoCount--;

            if (reloading)
                CancelReload();

            var shotDirection = cam.transform.rotation;
            
            if (weaponSpread)
                shotDirection *= SpreadManager.ApplySpread();

            var ray = new Ray(cam.transform.position, shotDirection * Vector3.forward);
            var hitPlayerId = ulong.MaxValue;

            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Player", "Terrain"))) {
                if (hit.collider.gameObject.CompareTag("Player")) {
                    var hitPlayer = hit.collider.gameObject;
                    hitPlayerId = hitPlayer.GetComponent<NetworkObject>().NetworkObjectId;
                }
            }

            ShotServerRPC(ray, hitPlayerId);
            
            KnockBackPlayer();
            FireWeaponPresentation(ray);

            if (simulateRecoil)
                RecoilManager.AddRecoil();
        }

        private void KnockBackPlayer() {
            playerRb.AddForce(-player.transform.forward * knockBackForce, ForceMode.VelocityChange);
        }

        private void Reload(InputAction.CallbackContext context) {
            if (currentAmmoCount == maxAmmoCount || reloading || !enabled)
                return;

            reloading = true;
            reloadStartTime = Time.time;
            reloadAudio.Play();
        }

        private void CancelReload() {
            reloading = false;
            reloadAudio.Stop();
        }

        private void CompleteReload() {
            reloading = false;
            currentAmmoCount = maxAmmoCount;
        }

        private void FireWeaponPresentation(Ray shot) {
            fireAudio.Play();
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.velocity = shot.direction * particles.main.startSpeedMultiplier;
            particles.Emit(emitParams, 1);
        }

        [ServerRpc]
        private void ShotServerRPC(Ray shot, ulong playerHitId, ServerRpcParams rpcParams = default) {
            if (!enabled)
                return;

            if (playerHitId != ulong.MaxValue) {
                var playerHit = NetworkSpawnManager.SpawnedObjects[playerHitId];

                //TODO: validate hit

                var hitPlayerManager = playerHit.GetComponent<PlayerGameManager>();
                var playerManager = player.GetComponent<PlayerGameManager>();

                var enemyHealth = playerHit.transform.GetComponentInChildren<PlayerHealth>();

                if(enemyHealth)
                {
                    if(playerManager.GetGameMode() == Network.GameMode.TeamDeathmatch &&
                    hitPlayerManager.GetTeamId() == playerManager.GetTeamId())
                    {
                            Debug.Log("Friendly fire");
                    }else{
                        enemyHealth.TakeDamage(damage, OwnerClientId);
                    }
                }
            }

            ShootClientRPC(shot, playerHitId != ulong.MaxValue);
        }

        [ClientRpc]
        private void ShootClientRPC(Ray shot, bool hit, ClientRpcParams rpcParams = default) {
            if (!enabled)
                return;
            if (IsOwner) {
                if(hit)
                    hitMarker.Trigger();
                return;
            }

            FireWeaponPresentation(shot);
        }
    }
}
