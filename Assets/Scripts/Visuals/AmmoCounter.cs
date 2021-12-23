using Game_Systems.Equipment.Weapons;
using TMPro;
using UnityEngine;

namespace Visuals {
    public class AmmoCounter : MonoBehaviour {
        [SerializeField] private HitscanWeapon weaponScript;

        [SerializeField] private Gradient colorGradient;

        private TextMeshPro text;
        private Light textLight;

        private void Start() {
            text = GetComponent<TextMeshPro>();
            textLight = GetComponent<Light>();
        }

        private void Update() {
            var ammo = weaponScript.currentAmmoCount;
            var maxAmmo = weaponScript.maxAmmoCount;

            text.text = ammo.ToString();
            var color = colorGradient.Evaluate(1f-(float)ammo / maxAmmo);
            text.color = color;
            textLight.color = color;
        }
    }
}
