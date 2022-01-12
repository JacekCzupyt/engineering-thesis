using MLAPI;
using MLAPI.NetworkVariable;
using Network;
using UI;
using UnityEngine;

namespace Game_Systems {
    public class PlayerHealth : NetworkBehaviour {
        [SerializeField] public NetworkVariableInt health = new NetworkVariableInt(
            new NetworkVariableSettings {WritePermission = NetworkVariablePermission.OwnerOnly},
            100
        );

        private PlayerRespawn respawnPlayer;
        private PlayerScore playerScore;
        private PlayerGameManager playerGameManager;
        
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
        public void TakeDamage(int damage, ulong player)
        {
            Debug.Log($"Apply {damage} Damage");

            health.Value -= damage; 

            if(health.Value<=0)
            {
                playerGameManager.AddPlayerKill(player);
                playerGameManager.AddPlayerDeath(OwnerClientId);  

                playerScore.SetDeathCounter(1);
            }
        }
        public void takeDemage(int damage)
        {
            Debug.Log($"Apply {damage} Damage");
            health.Value -= damage;
        }
    }
}
