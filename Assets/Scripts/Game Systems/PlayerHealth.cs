using MLAPI;
using MLAPI.NetworkVariable;
using UI;
using UnityEngine;

namespace Game_Systems {
    public class PlayerHealth : NetworkBehaviour {
        [SerializeField] NetworkVariableInt health = new NetworkVariableInt(
            new NetworkVariableSettings {WritePermission = NetworkVariablePermission.OwnerOnly},
            100
        );

        private PlayerRespawn respawnPlayer;

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
        public void takeDemage(int damage) {
            Debug.Log($"Apply {damage} Damage");
            health.Value -= damage;
        }
    }
}
