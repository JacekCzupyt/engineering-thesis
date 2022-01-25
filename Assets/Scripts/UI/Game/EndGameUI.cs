using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject endGameUIObject;
    [SerializeField] private Text endGameText;

    private void Awake() {
        endGameText = endGameUIObject.GetComponentInChildren<Text>();
    }

    public void UpdateEndGameText(bool winStatus)
    {
        if(winStatus)
        {
            endGameText.text = "Victory";
            endGameText.color = Color.blue;
        }else
        {
            endGameText.text = "Defeat";
            endGameText.color = Color.red;
        }
    }

    public void SetGameobjectActive(bool active)
    {
        endGameUIObject.SetActive(active);
    }
}
