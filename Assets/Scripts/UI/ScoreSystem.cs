using System;
using Game_Systems;
using MLAPI;
using MLAPI.NetworkVariable;
using Network;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ScoreSystem : NetworkBehaviour
    {
        public event Action PlayerKill;
        public NetworkVariableInt userScore = new NetworkVariableInt(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.Everyone }, 0);
        public NetworkVariableInt playerDeaths = new NetworkVariableInt(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.Everyone }, 0);
        [SerializeField] Text Score;
        [SerializeField] CheckGameState checkState;

        private PlayerManager playerManager;
        // Start is called before the first frame update

        void Start()
        {
            Score.text = userScore.Value.ToString();
        }
        // Update is called once per frame
        void Update()
        {
            Score.text = userScore.Value.ToString();
        }
        public void AddPoint()
        {
            userScore.Value += 1;
            checkState.checkUserScore(playerManager.playerName);
        
            OnPlayerKill();
        }

        public void AddDeathCount()
        {
            playerDeaths.Value += 1;
        }

        public int GetPlayerKill()
        {
            return userScore.Value;
        }

        public virtual void OnPlayerKill()
        {
            PlayerKill?.Invoke();
        }

        public void AssignPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
        }

    }
}
