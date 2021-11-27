using MLAPI;
using MLAPI.NetworkVariable;
using UI;
using UnityEngine;

namespace Game_Systems {
    public class PlayerHealth : NetworkBehaviour
    {
        [SerializeField] NetworkVariableInt health = new NetworkVariableInt(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly }, 100);
        PlayerRespawn respawnPlayer;
        [SerializeField] HealthBar bar;
        NetworkObject shooter;
        ScoreSystem score;
        // Start is called before the first frame update

        // Update is called once per frame

        private void Start()
        {
            respawnPlayer = GetComponent<PlayerRespawn>();
            bar.SetInitialHealth();
        }
        void Update()
        {
            if (IsOwner) {
                bar.SetHealth(health.Value);
            }
            if(IsOwner && health.Value<=0)
            {
                health.Value = 100;
                //bar.SetInitialHealth(100);
                respawnPlayer.Respawn();
            }

        }
        public void takeDemage(int damage, ulong player)
        {
            Debug.Log("applyDemage");
            health.Value = health.Value-damage;
            if(health.Value<=0)
            {
                shooter=NetworkManager.Singleton.ConnectedClients[player].PlayerObject;
                score = shooter.GetComponent<ScoreSystem>();
                score.AddPoint();
            }
        }
    }
}
