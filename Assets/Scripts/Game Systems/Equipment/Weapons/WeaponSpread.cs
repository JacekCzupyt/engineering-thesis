using System;
using UI.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Systems.Equipment.Weapons {
    [RequireComponent(typeof(HitscanWeapon))]
    public class WeaponSpread : MonoBehaviour
    {
        [SerializeField] private float minimumSpread = 0f;
        [SerializeField] private float spreadPerShot = 20f;
        [SerializeField] private float returnSpeed = 4f;

        [SerializeField] private Crosshair crosshair;

        public float currentSpread;

        private void Update() {
            currentSpread = Mathf.Lerp(currentSpread, minimumSpread, returnSpeed * Time.deltaTime);
            crosshair.SetSpreadFromAngle(currentSpread);
        }

        private void OnEnable() {
            currentSpread = minimumSpread;
        }

        //Technically not uniform distribution, but for small angles it's good enough
        public Quaternion ApplySpread() {
            var rand = Random.insideUnitCircle.normalized;
            var magnitude = rand.magnitude;
            var angle = Mathf.Rad2Deg * Mathf.Atan2(rand.y, rand.x);

            var res = Quaternion.AngleAxis(magnitude * currentSpread, Vector3.up);
            res = Quaternion.AngleAxis(angle, Vector3.forward) * res;

            currentSpread += spreadPerShot;

            return res;
        }
    }
}
