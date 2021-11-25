using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject initialMenu;
    [SerializeField] GameObject playMenu;

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
}
