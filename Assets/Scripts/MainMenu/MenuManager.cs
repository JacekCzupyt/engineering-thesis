using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject initialMenu;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private InputField playerNameInput;
    [SerializeField] private InputField passwordInput;

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

    public void ClientConnect(){
        if(playerNameInput.text.Length > 0){
            Debug.Log(playerNameInput.text);
        }
    }
}
