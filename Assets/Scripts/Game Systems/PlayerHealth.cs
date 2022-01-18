using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UI;
using UnityEngine;
using Utility;
using Visuals;
using Debug = UnityEngine.Debug;

namespace Game_Systems {
    public class PlayerHealth : NetworkBehaviour {
        private NetworkVariableInt health = new NetworkVariableInt(
            new NetworkVariableSettings {WritePermission = NetworkVariablePermission.OwnerOnly},
            100
        );

        public int Health{
            get{
                return health.Value;
            }
            set{
                health.Value = value;
            }
        }
        [SerializeField] private HealthBar bar;

        public bool inactive = false;
        private PlayerRespawn respawnPlayer;
        private PlayerGameManager playerGameManager;

        [SerializeField] private DamageOverlay damageOverlay;
        
        GameObject shooter;
        ScoreSystem score;


        private void Awake() {
            playerGameManager = GetComponentInParent<PlayerGameManager>();
        }

        private void Start() {
            respawnPlayer = GetComponent<PlayerRespawn>();
            bar.SetInitialHealth();
        }
        void Update() {
            if (IsOwner) {
                bar.SetHealth(health.Value);
            }
            if (IsServer && health.Value <= 0 && !inactive) {
                health.Value = 100;
                respawnPlayer.Respawn();
            }
        }
        public void TakeDamage(int damage, ulong? player = null)
        {
            if (inactive) {
                Debug.Log("Player is immune");
                return;
            }

            Debug.Log($"Apply {damage} Damage");

            health.Value -= damage; 
            TakeDamageClientRPC(damage, this.OwnerClientParams());

            if(health.Value<=0)
            {
                if(player.HasValue)
                    playerGameManager.AddPlayerKill(player.Value);
                
                playerGameManager.AddPlayerDeath(OwnerClientId);
            }
        }
        
        [ClientRpc]
        private void TakeDamageClientRPC(int damage, ClientRpcParams rpcParams = default) {
            if (!enabled)
                return;
            if (IsOwner) {
                if (damageOverlay != null) {
                    damageOverlay.Trigger();
                }
            }
        }
    }
}
