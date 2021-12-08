using Input_Systems;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game_Systems.Equipment {
    public class HitscanWeapon : NetworkBehaviour {
        //TODO: recoil, bloom, damage falloff?, physics recoil?, ammo
        
        //TODO: sound, particle effects
        [SerializeField] private bool fullAuto;
        [SerializeField] private float fireRate;
        
        private CharacterInputManager input;
        private bool firing = false;
        private float lastShotTime = float.NegativeInfinity;

        private void Start() {
            input = CharacterInputManager.Instance;
        }

        // Update is called once per frame
        private void Update()
        {
            if (IsOwner) {
                CheckFiringState();
            }
        }

        private void CheckFiringState() {
            var fireAction = input.GetFireAction() == InputActionPhase.Started;
            if (fireAction && (!firing || fullAuto)) {
                firing = true;
                if (Time.time - lastShotTime >= 1f / fireRate) {
                    FireWeapon();
                }
            }
            firing = fireAction;
        }

        private void FireWeapon() {
            lastShotTime = Time.time;
            Debug.Log("Fire weapon");
        }
        
    }
}
