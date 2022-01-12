using System.Collections.Generic;
using System;
using Network;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game {
    public class PlayerScoreUI : MonoBehaviour
    {
        [Header("Sprite Image References")]
        [SerializeField] private Sprite freeForAllSprite;
        [SerializeField] private Sprite teamDeathMatchSprite;
        [Header("UI References")]
        [SerializeField] private GameObject scoreListViewObject;
        [SerializeField] private GameObject gameModeTitle;
        [SerializeField] private GameObject gameModeScoreImagePanelObject;
        [SerializeField] private GameObject gameScorePanelObject;
        [SerializeField] private GameObject TextPrefab;
        private List<GameObject> scoreTexts = new List<GameObject>();
        private ListView.ListView scoreListView;

        private void Awake()
        {
            scoreListView = scoreListViewObject.GetComponent<ListView.ListView>();
        }

        private void InitializeScoreListView()
        {
            scoreListView = scoreListViewObject.GetComponent<ListView.ListView>();
        }

        public void CreateListItem(PlayerState state, float position, bool IsPlayerId)
        {
            GameObject obj = Instantiate(scoreListView.prefab, new Vector3(0, position, 0), 
                Quaternion.identity) as GameObject;

            ScoreListItem item = new ScoreListItem(obj, state);
            if(IsPlayerId) item.HighlightPlayerName();
            scoreListView.AddItem(item);
            
        }

        public void DestroyCards()
        {
            scoreListView.ClearList();
        }

        public void UpdateGameMode(GameMode gamemode)
        {
            gameModeTitle.GetComponent<Text>().text = gamemode.ToString();
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
            int column = (int)Math.Floor((double)(teamId-1)/2);
            int row = (teamId-1)%2;
            float posX = 125 + column*250;
            float posY = -(50 + row*50);
            var teamScore = Instantiate(TextPrefab, 
            new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;

            teamScore.GetComponent<Text>().text = "Team " + teamId +": " + teamScoreValue;

            teamScore.transform.SetParent(gameScorePanelObject.transform, false);
            teamScore.SetActive(true);
            scoreTexts.Add(teamScore);
        }

        public void AddPlayerScores(string playerName, int killScore, int pos)
        {
            float posY = -(pos*50);
            var playerScore = Instantiate(TextPrefab,
            new Vector3(0, posY, 0), Quaternion.identity) as GameObject;

            playerScore.GetComponent<Text>().text = playerName + " " + killScore;

            playerScore.transform.SetParent(gameScorePanelObject.transform, false);
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
