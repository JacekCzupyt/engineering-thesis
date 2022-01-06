using System;
using MLAPI;
using MLAPI.NetworkVariable;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network {
    public class PlayerManager : NetworkBehaviour {
        [FormerlySerializedAs("playerCharacter")] [SerializeField] private GameObject playerCharacterPrefab;
        private GameManager gameManager;
        private GameStateManager gamestateManager;
        public NetworkVariable<GameObject> playerCharacter;
        private ulong clientId;
        public string playerName;
        public int teamId;
        private int playerKills;
        private int playerDeaths;

        public GameObject SpawnCharacter(Vector3 pos) {
            if (!IsServer)
                throw new InvalidOperationException("This method can only be run on the server");

            var character = GameObject.Instantiate(playerCharacterPrefab, pos, Quaternion.identity);
            character.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, null, true);
            playerCharacter.Value = character;

            //HUD Score System, not scoreboard
            character.GetComponent<ScoreSystem>().AssignPlayerManager(this);
            return character;
        }

        public void SetGameManager(GameManager manager)
        {
            gameManager = manager;
        }
        public void SetGameStateManager(GameStateManager manager)
        {
            gamestateManager = manager;
        }

        public void SetPlayerData(ulong clientId, string playerName, int teamId)
        {
            this.clientId = clientId;
            this.playerName = playerName;
            this.teamId = teamId;
            playerKills = 0;
            playerDeaths = 0;
        }

        public PlayerState ToPlayerState()
        {
            return new PlayerState(
                clientId,
                playerName,
                teamId,
                playerKills,
                playerDeaths
            );
        }

        public void AddPlayerKills(ulong clientId)
        {
            //playerKills += 1;
            //gameManager.PlayerKillUpdate(clientId);
            //gamestateManager.CheckPlayerScore();
        }

        public void AddPlayerDeaths(ulong clientId)
        {
            //playerDeaths += 1;
            //gameManager.PlayerDeathUpdate(clientId);
        }

        public ulong GetClientId()
        {
            return clientId;
        }

        public void DestoryPlayerManager()
        {
            Destroy(gameObject);
        }
    }
}
