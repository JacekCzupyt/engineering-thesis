using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance => instance;
    private static PlayerDataManager instance;

    private Dictionary<ulong, PlayerManager> PlayerManagers;

    private void Awake() {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        } 

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        PlayerManagers = new Dictionary<ulong, PlayerManager>();
    }

    public void AddPlayerManager(ulong clientId, PlayerManager playerManager){
        PlayerManagers[clientId] = playerManager;
    }

    public void PrintPlayerManagerData(){
        foreach(var mgr in PlayerManagers){
            Debug.Log(mgr.Key);
            mgr.Value.PrintData();
        }
    }

    public void SpawnPlayers(){
        foreach(var playerManager in PlayerManagers){
            playerManager.Value.SpawnCharacter();
        }
    }
}
