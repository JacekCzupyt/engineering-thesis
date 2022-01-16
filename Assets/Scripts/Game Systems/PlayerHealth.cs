using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using Network;
using UI;
using UnityEngine;
using Utility;
using Visuals;

namespace Game_Systems {
    public class PlayerHealth : NetworkBehaviour {
        [SerializeField] public NetworkVariableInt health = new NetworkVariableInt(
            new NetworkVariableSettings {WritePermission = NetworkVariablePermission.OwnerOnly},
            100
        );

        private PlayerRespawn respawnPlayer;
        private PlayerScore playerScore;
        private PlayerGameManager playerGameManager;
        [SerializeField] private DamageOverlay damageOverlay;
        
        GameObject shooter;
        ScoreSystem score;

        [SerializeField] private HealthBar bar;

        private void Awake() {
            playerScore = GetComponentInParent<PlayerScore>();
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
            if (IsOwner && health.Value <= 0) {
                health.Value = 100;
                respawnPlayer.Respawn();
            }
        }
        public void TakeDamage(int damage, ulong? player = null)
        {
            Debug.Log($"Apply {damage} Damage");

            health.Value -= damage; 
            TakeDamageClientRPC(damage, this.OwnerClientParams());

            if(health.Value<=0)
            {
                if(player.HasValue)
                    playerGameManager.AddPlayerKill(player.Value);
                playerGameManager.AddPlayerDeath(OwnerClientId);  
                
                // playerScore.SetDeathCounter(1);
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
