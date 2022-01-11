using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Network;

public class PlayerGameManager : MonoBehaviour
{
    private ulong clientId;
    private string playerName;
    private int teamId;
    private int playerKills;
    public int playerDeaths;
    private GameManager gameManager;
    private PlayerScore playerScore;
    private GameInfo gameInfo;
    private void Awake() {
        playerScore = GetComponent<PlayerScore>();
    }

    public void SetPlayerState(PlayerState playerState)
    {
        clientId = playerState.ClientId;
        playerName = playerState.PlayerName;
        teamId = playerState.TeamId;
        playerKills = playerState.PlayerKills;
        playerDeaths = playerState.PlayerDeaths;

    }
    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
        gameInfo = gameManager.GetGameInfo();
    }

    public int GetTeamId()
    {
        return teamId;
    }

    public void AddPlayerDeath(ulong playerId)
    {
        gameManager.PlayerDeathUpdate(playerId);
    }

    public void AddPlayerKill(ulong playerId)
    {
        gameManager.PlayerKillUpdate(playerId);
    }

    public void UpdatePlayerDeathCounter()
    {
        playerDeaths++;
        playerScore.SetDeathCounter(playerDeaths);
    }

    public GameMode GetGameMode()
    {
        return gameInfo.gameMode;
    }
}
