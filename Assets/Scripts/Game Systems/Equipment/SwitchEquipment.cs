using System;
using System.Collections.Generic;
using Input_Systems;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace Game_Systems.Equipment {
    public class SwitchEquipment : NetworkBehaviour {
        [SerializeField] private List<HitscanWeapon> equipment;
        
        [SerializeField] NetworkVariableInt currentlyEquipped = new NetworkVariableInt(
            new NetworkVariableSettings {WritePermission = NetworkVariablePermission.OwnerOnly},
            0
        );

        private void Start() {
            foreach(var item in equipment) {
                item.enabled = false;
            }
            if (IsOwner) {
                var input = CharacterInputManager.Instance;
                input.SwitchEquipment += SwitchEquipmentLocalCallback;
            }
            currentlyEquipped.OnValueChanged += SwitchEquipmentsCallback;
            equipment[currentlyEquipped.Value].enabled = true;
        }

        private void SwitchEquipmentLocalCallback(int index) {
            if (index > equipment.Count) {
                Debug.LogWarning($"No equipment with index {index}");
                return;
            }
            currentlyEquipped.Value = index;

        }

        private void SwitchEquipmentsCallback(int prev, int current) {
            equipment[prev].enabled = false;
            equipment[current].enabled = true;
        }
    }
}
