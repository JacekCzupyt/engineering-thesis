using System;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

namespace Network {
    public class PlayerManager : NetworkBehaviour {
        [SerializeField] private GameObject playerCharacter;

        public GameObject SpawnCharacter() {
            if (!IsServer)
                throw new InvalidOperationException("This method can only be run on the server");

            var character = GameObject.Instantiate(playerCharacter, new Vector3(OwnerClientId, 0, 0), Quaternion.identity);
            character.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, null, true);
            return character;
        }
    }
}
