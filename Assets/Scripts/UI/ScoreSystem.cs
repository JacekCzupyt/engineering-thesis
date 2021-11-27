using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.NetworkVariable;

public class ScoreSystem : NetworkBehaviour
{
    NetworkVariableInt userScore = new NetworkVariableInt(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.Everyone }, 0);
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
        userScore.Value = +1;
    }

}
