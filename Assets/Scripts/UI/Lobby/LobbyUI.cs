using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("References")]
    
    [SerializeField] private Button readyUpButton;
    [SerializeField] public Button startGameButton;
    [SerializeField] private LobbyManager lobbyManager;
    [SerializeField] private GameObject playerCountText;
    [SerializeField] private GameObject lobbyListViewObject;
    private ListView lobbyListView;
    private bool IsPlayerReadyUI;

    private void Awake() {
        lobbyListView = lobbyListViewObject.GetComponent<ListView>();
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
