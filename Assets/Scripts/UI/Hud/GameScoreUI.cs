using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;

namespace UI.Hud
{
    public class GameScoreUI : MonoBehaviour
    {
        [Header("Sprite Image References")]
        [SerializeField] private Sprite freeForAllSprite;
        [SerializeField] private Sprite teamDeathMatchSprite;

        [Header("UI References")]
        [SerializeField] private GameObject gameModeScoreImagePanelObject;
        [SerializeField] private GameObject teamScoresPanelObject;
        [SerializeField] private GameObject playerScoresPanelObject;
        [SerializeField] private GameObject teamScorePrefab;
        [SerializeField] private GameObject topPlayerTextObject;
        [SerializeField] private GameObject currentPlayerTextObject;
        private List<GameObject> teamObjects = new List<GameObject>();


        public void UpdateGameMode(GameMode gamemode)
        {
            gameModeScoreImagePanelObject.transform.GetChild(0).GetComponent<Image>().sprite = GameModeImageSprite(gamemode);
            if(gamemode == GameMode.FreeForAll)
            {
                playerScoresPanelObject.SetActive(true);
                teamScoresPanelObject.SetActive(false);
            }else if(gamemode == GameMode.TeamDeathmatch)
            {
                playerScoresPanelObject.SetActive(false);
                teamScoresPanelObject.SetActive(true);
            }
        }

        public Sprite GameModeImageSprite(GameMode mode)
        {
            if(mode == GameMode.FreeForAll)
            {
                return freeForAllSprite;
            }else if(mode == GameMode.TeamDeathmatch)
            {
                return teamDeathMatchSprite;
            }
            return null;
        }

        
        public void AddTeamScores(int teamId, int teamScoreValue)
        {
            float margin = 10f, scoreHeight = 120, scoreWidth = 100;

            float posX =  (scoreWidth/2 + margin) + (teamId*margin) + (teamId*scoreWidth);
            float posY = -(scoreHeight/2 + margin);

            var teamScore = Instantiate(teamScorePrefab, 
            new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;

            teamScore.GetComponent<Image>().color = TeamColor.GetTeamColor(teamId + 1, 0.4f);
            teamScore.GetComponentInChildren<Text>().text = "" + teamScoreValue.ToString();

            teamScore.transform.SetParent(teamScoresPanelObject.transform, false);
            teamScore.SetActive(true);
            teamObjects.Add(teamScore);
        }

        public void SetPlayerScore(PlayerState playerState, int pos)
        {
            Text scoreText = null;
            if(pos == 0)
            {
                scoreText = topPlayerTextObject.GetComponent<Text>();
            }else if(pos == 1)
            {
                scoreText = currentPlayerTextObject.GetComponent<Text>();
            }
            scoreText.text = playerState.PlayerName + " " + playerState.PlayerKills.ToString();
        }

        public void DeleteScores()
        {
            foreach(var teamScore in teamObjects)
            {
                Destroy(teamScore);
            }
            teamObjects.Clear();
        }
    }
}
