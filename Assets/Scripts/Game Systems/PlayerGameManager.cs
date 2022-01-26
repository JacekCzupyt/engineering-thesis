using UnityEngine;
using Network;

namespace Game_Systems
{
    public class PlayerGameManager : MonoBehaviour
    {
        private ulong clientId;
        private string playerName;
        private int teamId;
        private int playerKills;
        private int playerDeaths;
        private GameManager gameManager;
        private GameInfo gameInfo;

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
            gameObject.GetComponentInChildren<PlayerHealth>().SetGameManager(gameManager);
            gameInfo = gameManager.GetGameInfo();
            SetPlayerColor();
        }

        public int GetTeamId()
        {
            return teamId;
        }

        public GameMode GetGameMode()
        {
            return gameInfo.gameMode;
        }

        public GameManager GetGameManager()
        {
            return gameManager;
        }

        private void SetPlayerColor()
        {
            var playerRenderer = gameObject.GetComponent<MeshRenderer>();
            playerRenderer.material.SetColor("_Color", TeamColor.GetPlayerControllerColor(teamId));
        }
    }
}


