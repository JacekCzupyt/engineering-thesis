using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Systems.Equipment.Weapons {
    [RequireComponent(typeof(HitscanWeapon))]
    public class WeaponRecoil : MonoBehaviour {
        [SerializeField] private Vector2 standardRecoil = new Vector2(-4, 0);
        [SerializeField] private Vector2 recoilDeviation = new Vector2(1, 2);
        [SerializeField] private float snapSpeed = 20f;
        [SerializeField] private float returnSpeed = 4f;
        

        private Transform player;
        private Quaternion localRotation;
        private Quaternion targetRotation;

        private void Start() {
            player = GetComponent<HitscanWeapon>().player.transform;
        }
        private void Update() {
            targetRotation = Quaternion.Lerp(targetRotation, Quaternion.identity, returnSpeed * Time.deltaTime);
            var newLocalRotation = Quaternion.Lerp(localRotation, targetRotation, snapSpeed * Time.deltaTime);
            var deltaRotation = newLocalRotation * Quaternion.Inverse(localRotation);
            player.localRotation *= deltaRotation;
            localRotation = newLocalRotation;
        }

        public void AddRecoil() {
            var recoil = standardRecoil + Vector2.Scale(Random.insideUnitCircle, recoilDeviation);
            targetRotation *= Quaternion.Euler(recoil);
        }
    }
}
