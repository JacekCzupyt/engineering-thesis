using System;
using System.Collections.Generic;
using Input_Systems;
using UnityEngine;

namespace Game_Systems.Equipment {
    public class SwitchEquipment : MonoBehaviour {
        [SerializeField] private List<GameObject> equipment;
        private int currentlyEquipped = 0;

        private void Start() {
            var input = CharacterInputManager.Instance;
            input.SwitchEquipment += SwitchEquipmentCallback;
        }

        private void SwitchEquipmentCallback(int index) {
            if (index == currentlyEquipped)
                return;
            if (index > equipment.Count) {
                Debug.LogWarning($"No equipment with index {index}");
                return;
            }
            equipment[currentlyEquipped].SetActive(false);
            equipment[index].SetActive(true);
            currentlyEquipped = index;
        }
    }
}
