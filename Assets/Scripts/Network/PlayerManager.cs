using System;
using System.Collections.Generic;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network {
    public class PlayerManager : NetworkBehaviour {
        [FormerlySerializedAs("playerCharacter")] [SerializeField] private GameObject playerCharacterPrefab;
        public NetworkVariable<GameObject> playerCharacter;
        private string playerName;
        private ulong clientId;
        private int playerKills;
        private int playerDeaths;

        public GameObject SpawnCharacter(Vector3 pos) {
            if (!IsServer)
                throw new InvalidOperationException("This method can only be run on the server");

            var character = GameObject.Instantiate(playerCharacterPrefab, pos, Quaternion.identity);
            character.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, null, true);
            playerCharacter.Value = character;
            return character;
        }

        public void SetPlayerData(ulong clientId, string playerName)
        {
            this.clientId = clientId;
            this.playerName = playerName;
            playerKills = 0;
            playerDeaths = 0;
        }

        public ScorePlayerState ToPlayerScoreState()
        {
            return new ScorePlayerState(
                clientId,
                playerName,
                playerKills,
                playerDeaths
            );
        }

    }
}
