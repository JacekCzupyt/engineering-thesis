using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ScoreSystem : NetworkBehaviour
    {
        public NetworkVariableInt userScore = new NetworkVariableInt(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.Everyone }, 0);
        [SerializeField] Text Score;
        // Start is called before the first frame update

        void Start()
        {
            Score.text = userScore.Value.ToString();
        }
        // Update is called once per frame
        void Update()
        {
            Score.text = userScore.Value.ToString();
        }
        public void AddPoint()
        {
            userScore.Value += 1;
        }

    }
}
