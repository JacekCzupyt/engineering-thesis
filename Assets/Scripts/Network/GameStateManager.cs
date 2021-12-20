using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using UI.Game;
using UnityEngine.UI;

namespace Network
{
    public class GameStateManager : NetworkBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] GameObject sc;
        private ScoreboardManager score;
        [SerializeField]  int NumOfKillsToWin;
        [SerializeField] GameObject EndGameUIobject;

        private Canvas can;
        private Text winMessage;
        
        void Start()
        {
            score = sc.GetComponent<ScoreboardManager>();
            can = EndGameUIobject.GetComponentInChildren<Canvas>();
            winMessage = can.GetComponentInChildren<Text>();
            EndGameUIobject.SetActive(false);
        }
        public void CheckPlayerScore()
        {
            foreach (var player in score.scoreboardPlayers)
            {
                if (player.PlayerKills >= NumOfKillsToWin)
                {
                    winMessage.text = "Player " + player.PlayerName + " win a game";
                    break;
                }
            }
            EndGameUIobject.SetActive(true);

        }
    }
}
