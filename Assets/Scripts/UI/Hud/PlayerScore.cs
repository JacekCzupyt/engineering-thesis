using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private GameObject playerKillCounterObject;
    [SerializeField] private GameObject playerDeathCounterObject;
    [SerializeField] private GameObject playerTeamIndicatorObject;
    private PlayerGameManager playerGameManager;
    private Text killCounterText;
    private Text deathCounterText;
    private Image teamIndicatorImage;

    private void Awake() {
        killCounterText = playerKillCounterObject.GetComponent<Text>();
        deathCounterText = playerDeathCounterObject.GetComponent<Text>();
        teamIndicatorImage = playerTeamIndicatorObject.GetComponent<Image>();
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

    public void SetTeamIndicatorColor(Color c)
    {
        teamIndicatorImage.color = c;
    }

    private void UpdatePlayerDeathCouner()
    {
        killCounterText.text = playerGameManager.playerDeaths.ToString();
    }

}
