using System;
using UnityEngine;

namespace Game_Systems.Movement {
    public class Jetpack : MonoBehaviour {
        /// When jetpack is being used, fuel is drained at fuelConsumptionRate.
        /// When jetpack stops being used, fuel starts regenerating at fuelReplenishRate after fuelUseCooldown passes
        /// If jetpack fuel reached 0, it can't used until it reaches fuelResetThreshold
        [Header("Config")]
        [SerializeField] public float jetpackForce = 5f;


        [SerializeField] public float maxFuel = 100;
        [SerializeField] public float fuelResetThreshold = 20;
        [SerializeField] public float fuelConsumptionRate = 20;
        [SerializeField] public float fuelUseCooldown = 1;
        [SerializeField] public float fuelReplenishRate = 20;

        [Header("Active Values")]
        [SerializeField] public float currentFuel;
        [SerializeField] public float lastUseTime = Mathf.NegativeInfinity;
        [SerializeField] public bool isResetting;

        private Rigidbody rb;

        private void Start() {
            currentFuel = maxFuel;
            lastUseTime = Mathf.NegativeInfinity;
            isResetting = false;
            rb = GetComponentInParent<Rigidbody>();
        }

        private void FixedUpdate() {
            if (Time.time - lastUseTime > Mathf.Max(fuelUseCooldown, 2 * Time.fixedDeltaTime))
                currentFuel = Mathf.Min(currentFuel + fuelReplenishRate * Time.fixedDeltaTime, maxFuel);
            if (isResetting && currentFuel > fuelResetThreshold)
                isResetting = false;
        }

        /// <summary>
        /// Pushes player in a given direction using jetpack movement. Should be used during fixed update;
        /// </summary>
        /// <param name="direction">Local direction of jetpack use</param>
        /// <returns>Applied force in world space</returns>
        public Vector3 UseJetpack(Vector3 direction) {
            if (direction == Vector3.zero)
                return Vector3.zero;
            
            if (currentFuel <= 0 || isResetting) {
                isResetting = true;
                return Vector3.zero;
            }
            
            var dir = transform.rotation * direction;
            var force = dir * jetpackForce;
            rb.AddForce(force, ForceMode.Force);

            lastUseTime = Time.time;
            currentFuel = Mathf.Max(0, currentFuel - fuelConsumptionRate * Time.fixedDeltaTime);
            
            return force;
        }
    }
}
