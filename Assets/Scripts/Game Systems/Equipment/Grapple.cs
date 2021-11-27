using Input_Systems;
using UnityEngine;

namespace Game_Systems.Equipment {
    public class Grapple : MonoBehaviour {
        [SerializeField] private GameObject player;
        private CharacterInputManager input;
    
        // Start is called before the first frame update
        void Start()
        {
            input = CharacterInputManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
