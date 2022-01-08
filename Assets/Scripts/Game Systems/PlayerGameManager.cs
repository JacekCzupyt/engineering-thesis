using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class PlayerGameManager : MonoBehaviour
{
    private ulong clientId;
    private string playerName;
    private int teamId;
    private int playerKills;
    private int playerDeaths;
    private GameManager gameManager;
    private GameInfo gameInfo;
    private void Awake() {

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

    public GameMode GetGameMode()
    {
        return gameInfo.GameMode;
    }
}
