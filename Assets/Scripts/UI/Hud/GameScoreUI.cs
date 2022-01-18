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
        [SerializeField] private GameObject scoresPanelObject;
        [SerializeField] private GameObject TeamScorePrefab;
        [SerializeField] private GameObject PlayerTextPrefab;
        private List<GameObject> scoreTexts = new List<GameObject>();


        public void UpdateGameMode(GameMode gamemode)
        {
            gameModeScoreImagePanelObject.transform.GetChild(0).GetComponent<Image>().sprite = GameModeImageSprite(gamemode);
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

            var teamScore = Instantiate(TeamScorePrefab, 
            new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;

            teamScore.GetComponent<Image>().color = TeamColor.GetTeamColor(teamId + 1, 0.4f);
            teamScore.GetComponentInChildren<Text>().text = "" + teamScoreValue.ToString();

            teamScore.transform.SetParent(scoresPanelObject.transform, false);
            teamScore.SetActive(true);
            scoreTexts.Add(teamScore);
        }

        public void AddPlayerScores(PlayerState playerState, int pos, bool highlight)
        {
            float textHeight = 90;
            float posY = textHeight/2 + (pos * textHeight);
            var playerScore = Instantiate(PlayerTextPrefab,
            new Vector3(0, posY, 0), Quaternion.identity) as GameObject;

            playerScore.GetComponent<Text>().text = playerState.PlayerName + " " + playerState.PlayerKills.ToString();
            if(highlight) playerScore.GetComponent<Text>().color = new Color(255, 150, 0, 255);

            playerScore.transform.SetParent(scoresPanelObject.transform, false);
            playerScore.SetActive(true);
            scoreTexts.Add(playerScore);
        }

        public void DeleteScores()
        {
            foreach(var teamScore in scoreTexts)
            {
                Destroy(teamScore);
            }
            scoreTexts.Clear();
        }
    }
}
