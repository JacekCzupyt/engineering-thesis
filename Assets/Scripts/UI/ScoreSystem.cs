using System;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class ScoreSystem : NetworkBehaviour
{
    public event Action PlayerKill;
    public NetworkVariableInt userScore = new NetworkVariableInt(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.Everyone }, 0);
    [SerializeField] Text Score;
    [SerializeField] CheckGameState checkState;
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
        if(IsOwner)
            checkState.checkUserScoreServerRPC();
        
        OnPlayerKill();
    }

    public int GetPlayerScore()
    {
        return userScore.Value;
    }

    public virtual void OnPlayerKill()
    {
        PlayerKill?.Invoke();
    }
}
