using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("References")]
    
    [SerializeField] private Button readyUpButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private LobbyManager lobbyManager;
    [SerializeField] private GameObject playerCountText;

    [SerializeField] private GameObject lobbyListView;

    public void OnLeaveButtonClicked()
    {
        Debug.Log("Leave");
    }

    public void OnStartButtonClicked()
    {
        Debug.Log("Start");
    }

    public void OnReadyUpButtonClicked()
    {
        Debug.Log("Ready");
    }

}
