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

        [Header("Button References")]
        [SerializeField] private Button readyUpButton;
        [SerializeField] public Button startGameButton;
        [SerializeField] public Button changeModeButton;
        [SerializeField] public Button playTabButton;
        [SerializeField] public Button weaponsTabButton;
        [SerializeField] public Button settingsButton;

        [Header("Text References")]
        [SerializeField] private GameObject playerCountText;
        [SerializeField] private GameObject gameModeText;

        private ListView.ListView lobbyListView;
        private bool IsPlayerReadyUI;
        private List<GameObject> lobbyPanels;

        private void Awake() {
            lobbyListView = lobbyListViewObject.GetComponent<ListView.ListView>();
            IsPlayerReadyUI = false;
            lobbyPanels = new List<GameObject>(GameObject.FindGameObjectsWithTag("LobbyPanel"));
            ChangeLobbyPanels(0);
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

        public void ChangeLobbyPanels(int index)
        {
            DeactivateLobbyPanels();
            switch(index){
                case 0: lobbyPanels.Find(x => x.gameObject.name == "PlayLobby").SetActive(true);
                break;
                case 1: lobbyPanels.Find(x => x.gameObject.name == "WeaponsLobbby").SetActive(true);
                break;
                case 2: lobbyPanels.Find(x => x.gameObject.name == "SettingsLobby").SetActive(true);
                break;
                default: break;
            }
        }

        public void DeactivateLobbyPanels(){
            foreach(var panel in GameObject.FindGameObjectsWithTag("LobbyPanel")){
                panel.gameObject.SetActive(false);
            }
        }

        public void OnChangeModeClicked()
        {
            changeGameModePanel.SetActive(true);
        }

        public void ChooseGameModeClicked(int mode){
            lobbyManager.SetGameMode((GameMode) mode);
            changeGameModePanel.SetActive(false);
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

        public void StartGameDeactivation()
        {
            foreach(var button in this.GetComponentsInChildren<Button>())
            {
                button.interactable = false;
            }
        }
    }
}
