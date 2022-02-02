using System;
using MLAPI;
using MLAPI.NetworkVariable;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network {
    public class PlayerManager : NetworkBehaviour {
        [FormerlySerializedAs("playerCharacter")] [SerializeField] private GameObject playerCharacterPrefab;
        private PlayerState playerState;

        public GameObject SpawnCharacter(Vector3 pos) {
            if (!IsServer)
                throw new InvalidOperationException("This method can only be run on the server");

            var character = GameObject.Instantiate(playerCharacterPrefab, pos, Quaternion.identity);
            character.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, null, true);
            return character;
        }

        public void SetPlayerData(ulong clientId, string playerName, int teamId)
        {
            playerState = new PlayerState(
                clientId,
                playerName,
                teamId,
                0,
                0
            );
        }

        public PlayerState GetPlayerState()
        {
            return playerState;
        }

        public void DestoryPlayerManager()
        {
            Destroy(gameObject);
        }
    }
}
