using System;
using System.Linq;
using Audio;
using Input_Systems;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using Unity.Plastic.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Game_Systems.Equipment {
    public class HitscanWeapon : NetworkBehaviour {
        //TODO: recoil, bloom, damage falloff?, physics recoil?

        //TODO: variable pitch depending on ammo

        [SerializeField] private GameObject player;
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


        [Header("Recoil")] [SerializeField] private bool simulateRecoil;
        [SerializeField] private Vector2 standardRecoil;
        [SerializeField] private float minRecoilDeviation;
        [SerializeField] private float maxRecoilDeviation;
        [SerializeField] private float deviationIncreasePerShot;
        [SerializeField] private float deviationDecreasePerSecond;

        private float currentRecoilDeviation;
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
        }

        private void OnDisable() {
            if (reloading)
                CancelReload();
            gunModel.SetActive(false);
        }

        private ClientRpcParams NonOwnerClientParams =>
            new ClientRpcParams {
                Send = new ClientRpcSendParams {
                    TargetClientIds = NetworkManager.Singleton.ConnectedClientsList.Where(c => c.ClientId != OwnerClientId)
                        .Select(c => c.ClientId).ToArray()
                }
            };

        private void Start() {
            input = CharacterInputManager.Instance;
            particles = GetComponentInChildren<ParticleSystem>();
            currentAmmoCount = maxAmmoCount;
            input.Controls.Reload.performed += Reload;
            playerRb = player.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update() {
            if (IsOwner) {
                ReduceRecoilDeviation();
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

            FireWeaponPresentation();
            KnockBackPlayer();

            var ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = cam.transform.position;
            var hitPlayerId = ulong.MaxValue;

            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Player", "Terrain"))) {
                if (hit.collider.gameObject.CompareTag("Player")) {
                    var hitPlayer = hit.collider.gameObject;
                    hitPlayerId = hitPlayer.GetComponent<NetworkObject>().NetworkObjectId;
                }
            }

            ShotServerRPC(ray, hitPlayerId);

            SimulateRecoil();
        }

        private void SimulateRecoil() {
            if (!simulateRecoil)
                return;

            var deviation = Mathf.Lerp(minRecoilDeviation, maxRecoilDeviation, currentRecoilDeviation);
            var currentRecoil = standardRecoil + Random.insideUnitCircle * deviation;
            player.transform.Rotate(new Vector3(-currentRecoil.y, currentRecoil.x, 0));

            currentRecoilDeviation = Mathf.Min(currentRecoilDeviation + deviationIncreasePerShot, 1f);
        }

        private void ReduceRecoilDeviation() {
            currentRecoilDeviation = Mathf.Max(currentRecoilDeviation - deviationDecreasePerSecond * Time.deltaTime, 0f);
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

        private void FireWeaponPresentation() {
            fireAudio.Play();
            particles.Emit(new ParticleSystem.EmitParams(), 1);
        }

        [ServerRpc]
        private void ShotServerRPC(Ray shot, ulong playerHitId, ServerRpcParams rpcParams = default) {
            if (!enabled)
                return;

            if (playerHitId != ulong.MaxValue) {
                var playerHit = NetworkSpawnManager.SpawnedObjects[playerHitId];

                //TODO: validate hit

                var enemyHealth = playerHit.transform.GetComponentInChildren<PlayerHealth>();
                if (enemyHealth != null) {
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
