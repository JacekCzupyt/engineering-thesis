using System;
using Game_Systems.Movement;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud {
    public class FuelBar : MonoBehaviour {
        [SerializeField] private Jetpack jetpack;

        private Slider slider;

        private void Start() {
            slider = GetComponent<Slider>();
            slider.maxValue = jetpack.maxFuel;
        }
        private void Update() {
            slider.value = jetpack.currentFuel;
        }
    }
}
