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
        
        GameObject shooter;
        ScoreSystem score;

        [SerializeField] private HealthBar bar;

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
        public void takeDemage(int damage, ulong player)
        {
            Debug.Log($"Apply {damage} Damage");
            health.Value -= damage; 
            if(health.Value<=0)
            {
                var shooterPlayerManager = NetworkManager.Singleton.ConnectedClients[player].PlayerObject
                    .GetComponent<PlayerManager>();
                shooter = shooterPlayerManager.playerCharacter.Value;
                score = shooter.GetComponentInChildren<ScoreSystem>();
                score.AddPoint();

                var receiverPlayerManager = NetworkManager.Singleton.ConnectedClients[OwnerClientId]
                    .PlayerObject.GetComponent<PlayerManager>();


                receiverPlayerManager.AddPlayerDeaths(OwnerClientId);
                shooterPlayerManager.AddPlayerKills(player);  
            }
        }
        public void takeDemage(int damage)
        {
            Debug.Log($"Apply {damage} Damage");
            health.Value -= damage;
        }
    }
}
