using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private GameObject playerKillCounterObject;
    [SerializeField] private GameObject playerDeathCounterObject;
    private PlayerGameManager playerGameManager;
    private Text killCounterText;
    private Text deathCounterText;

    private void Awake() {
        killCounterText = playerKillCounterObject.GetComponent<Text>();
        deathCounterText = playerDeathCounterObject.GetComponent<Text>();
        playerGameManager = GetComponent<PlayerGameManager>();
    }

    public void SetKillCounter(int value)
    {
        killCounterText.text = value.ToString();
    }

    public void SetDeathCounter(int value)
    {
        deathCounterText.text = "" + value.ToString();
    }

    private void UpdatePlayerDeathCouner()
    {
        killCounterText.text = playerGameManager.playerDeaths.ToString();
    }

}
