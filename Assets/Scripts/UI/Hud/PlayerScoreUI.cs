using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;

namespace UI.Hud
{
    public class PlayerScoreUI : MonoBehaviour
    {
        [SerializeField] private GameObject playerKillCounterObject;
        [SerializeField] private GameObject playerDeathCounterObject;
        private Text killCounterText;
        private Text deathCounterText;

        private void Awake()
        {
            killCounterText = playerKillCounterObject.GetComponent<Text>();
            deathCounterText = playerDeathCounterObject.GetComponent<Text>();
        }

        public void UpdatePlayerScore(PlayerState playerState)
        {
            killCounterText.text = "" + playerState.PlayerKills.ToString();
            deathCounterText.text = "" + playerState.PlayerDeaths.ToString();
        }
    }
}
