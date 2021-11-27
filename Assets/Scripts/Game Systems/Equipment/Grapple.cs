using System.Diagnostics;
using Input_Systems;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

namespace Game_Systems.Equipment {
    public class Grapple : MonoBehaviour {
        [SerializeField] private GameObject player;
        private CharacterInputManager input;

        public enum GrappleState {
            Ready,
            InFlight,
            Attached,
            Retracting,
            Unavailable
        }

        public GrappleState State { get; set; }
        
        void Start()
        {
            input = CharacterInputManager.Instance;
            input.AbilityAction.started += Fire;
            input.AbilityAction.canceled += Cancel;
        }

        void Update()
        {
            Debug.Log(State);
        }

        void Fire(InputAction.CallbackContext context) {
            State = GrappleState.InFlight;
        }

        void Cancel(InputAction.CallbackContext context) {
            State = GrappleState.Ready;
        }
    }
}
