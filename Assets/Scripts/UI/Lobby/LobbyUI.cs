using Network;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

namespace UI.Lobby {
    public class LobbyUI : MonoBehaviour
    {
        [Header("Lobby Logic References")]
        [SerializeField] private LobbyManager lobbyManager;
        [SerializeField] private GameObject lobbyListViewObject;

        [Header("Lobby Panel References")]
        [SerializeField] private GameObject changeGameModePanel;
        [SerializeField] private GameObject gameOptionsPanel;

        [Header("Button References")]
        [SerializeField] private Button readyUpButton;
        [SerializeField] public Button startGameButton;
        [SerializeField] public Button changeModeButton;
        [SerializeField] public Button gameOptionsButton;

        [Header("Input Field References")]
        [SerializeField] private InputField killsToWinInput;
        [SerializeField] private InputField teamsInGameInput;

        [Header("Text References")]
        [SerializeField] private GameObject playerCountText;
        [SerializeField] private GameObject gameModeText;

        private ListView.ListView lobbyListView;
        private bool IsPlayerReadyUI;

        private void Awake() {
            lobbyListView = lobbyListViewObject.GetComponent<ListView.ListView>();
            IsPlayerReadyUI = false;
        }

        public void OnLeaveButtonClicked()
        {
            lobbyManager.LeaveGame();
        }

        public void OnStartButtonClicked()
        {
            lobbyManager.StartGame();
        }

        public void OnReadyUpButtonClicked()
        {
            lobbyManager.ReadyUp();
            ToggleReadyUpButtonText();
        }

        public void ToggleGameOptionsClicked()
        {
            gameOptionsPanel.SetActive(!gameOptionsPanel.activeSelf);
        }

        private void ToggleReadyUpButtonText()
        {
            if(IsPlayerReadyUI)
            {
                readyUpButton.GetComponentInChildren<Text>().text =  "Ready Up!";
                IsPlayerReadyUI = false;
            }
            else
            {
                readyUpButton.GetComponentInChildren<Text>().text =  "Cancel";
                IsPlayerReadyUI = true;
            }
        }

        public void OnChangeModeClicked()
        {
            changeGameModePanel.SetActive(true);
        }

        public void ChooseGameModeClicked(int mode){

            lobbyManager.SetGameMode((GameMode) mode);
            changeGameModePanel.SetActive(false);

            if(mode == 0)
            {
                SetTeamsInGameInteractions(false);
            }else
            {
                SetTeamsInGameInteractions(true);
            }
        }

        public void CreateListItem(LobbyPlayerState state, float position)
        {
            GameObject obj = Instantiate(lobbyListView.prefab, new Vector3(0, position, 0), 
                Quaternion.identity) as GameObject;
            LobbyListItem item = new LobbyListItem(obj, state);
            lobbyListView.AddItem(item);
        }

        public void DestroyCards()
        {
            lobbyListView.ClearList();
        }

        public void UpdatePlayerCount(int count)
        {
            playerCountText.GetComponent<Text>().text = "Players("+count+"/10)";
        }

        public void UpdateGameMode(GameMode mode){
            switch((int) mode){
                case 0: gameModeText.gameObject.GetComponent<Text>().text = "GAME MODE: Free For All";
                break;
                case 1: gameModeText.gameObject.GetComponent<Text>().text = "GAME MODE: Team Death Match";
                break;
                default: break;
            }
        }

        public int GetNumberOfKillsToWin()
        {
            return Convert.ToInt16(killsToWinInput.text);
        }

        public void ValidateKillsToWinInput()
        {
            if(killsToWinInput.text.Length <= 0)
            {
                killsToWinInput.text = "5";
            }else if(Convert.ToInt16(killsToWinInput.text) <= 0)
            {
                killsToWinInput.text = "1";
            }
        }

        public void ValidateTeamsInGameInput()
        {
            if(teamsInGameInput.text.Length <= 0)
            {
                teamsInGameInput.text = "2";
            }else if(Convert.ToInt16(teamsInGameInput.text) > 4)
            {
                //Error
                teamsInGameInput.text = "4";
            }else if(Convert.ToInt16(teamsInGameInput.text) < 1)
            {
                teamsInGameInput.text = "2";
            }
        }

        public void SetTeamsInGameInteractions(bool interactable)
        {
            teamsInGameInput.interactable = interactable;
        }

        public void StartGameDeactivation()
        {
            foreach(var button in this.GetComponentsInChildren<Button>())
            {
                button.interactable = false;
            }

            gameOptionsPanel.SetActive(false);
        }
    }
}
