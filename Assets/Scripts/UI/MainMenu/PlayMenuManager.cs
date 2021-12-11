using UnityEngine;
using UnityEngine.UI;

public class PlayMenuManager : MonoBehaviour
{
    [SerializeField] private InputField playerNameInput;
    [SerializeField] private GameObject nameErrorText;

    public void HostGame(){
        if(NameValidation()) GameNetPortal.Instance.StartHost();
    }

    public void ClientConnect(){
        if(NameValidation()) ClientGameNetPortal.Instance.StartClient();
    }

    private bool NameValidation(){
        string playerName = playerNameInput.text;
        if(playerName.Length <= 0)
        {
            nameErrorText.SetActive(true);
            return false;
        }
        nameErrorText.SetActive(false);
        PlayerPrefs.SetString("PlayerName", playerName);
        return true;
    }
}