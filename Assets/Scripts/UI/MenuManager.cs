using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject initialMenu;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private InputField playerNameInput;
    [SerializeField] private InputField passwordInput;
    [SerializeField] private GameObject nameErrorText;

    public void Play(){
        initialMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    public void Exit(){
        Application.Quit();
    }

    public void Settings(){

    }

    public void BackToInitialMenu(){
        playMenu.SetActive(false);
        initialMenu.SetActive(true);
    }

    public void HostGame(){
        if(NameValidation()) GameNetPortal.Instance.StartHost();
    }

    public void ClientConnect(){
        if(NameValidation()) ClientGameNetPortal.Instance.StartClient();
    }

    public bool NameValidation(){
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
