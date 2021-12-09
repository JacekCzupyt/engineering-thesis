using System;
using Game_Systems.Equipment;
using TMPro;
using UnityEngine;

namespace Visuals {
    public class AmmoCounter : MonoBehaviour {
        [SerializeField] private HitscanWeapon weaponScript;

        [SerializeField] private Gradient colorGradient;

        private TextMeshPro text;
        private Light light;

        private void Start() {
            text = GetComponent<TextMeshPro>();
            light = GetComponent<Light>();
        }

        private void Update() {
            var ammo = weaponScript.currentAmmoCount;
            var maxAmmo = weaponScript.maxAmmoCount;

            text.text = ammo.ToString();
            var color = colorGradient.Evaluate(1f-(float)ammo / maxAmmo);
            text.color = color;
            light.color = color;
        }
    }
}
