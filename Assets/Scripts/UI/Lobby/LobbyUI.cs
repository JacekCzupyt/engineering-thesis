using Network;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UI.Lobby {
    public class LobbyUI : MonoBehaviour
    {
        [Header("Lobby Logic References")]
        [SerializeField] private LobbyManager lobbyManager;
        [SerializeField] private GameObject lobbyListViewObject;

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
        private GameObject[] lobbyPanels;

        private void Awake() {
            lobbyListView = lobbyListViewObject.GetComponent<ListView.ListView>();
            IsPlayerReadyUI = false;
            lobbyPanels = GameObject.FindGameObjectsWithTag("LobbyPanel");
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

        public void ChangeLobbyPanels(int index){
            for(int i = 0; i < lobbyPanels.Length; i++){
                if(i == index) lobbyPanels[i].SetActive(true);
                else lobbyPanels[i].SetActive(false);
            }
        }

        public void OnChangeModeClicked(){
            
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
    }
}
