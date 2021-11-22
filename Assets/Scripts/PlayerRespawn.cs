using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerRespawn : NetworkBehaviour
{
    private PlayerController cc;
    private Renderer[] renderers;
    [SerializeField] Behaviour[] scripts;
    HideCanvas hide;
    // Start is called before the first frame update
    void Start()
    {
        hide = GetComponent<HideCanvas>();
        cc = GetComponent<PlayerController>();
        renderers = GetComponentsInChildren<Renderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.Y))
        {
            Respawn();
        }
    }
    public void Respawn()
    {
        RespawnServerRpc();
    }
    [ServerRpc]
    private void RespawnServerRpc()
    {
        RespawnClientRpc();
    }
    [ClientRpc]
    private void RespawnClientRpc()
    {
        StartCoroutine(WaitForRespawn(RandomPos()));     
    }
    private Vector3 RandomPos()
    {
        return new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
    }

    IEnumerator WaitForRespawn(Vector3 randomPos)
    {
        cc.enabled = false;
        PlayerState(false);
        hide.hideCanvas();
        yield return new WaitForSeconds(5);
        transform.position = randomPos;
        cc.enabled = true;
        PlayerState(true);
        hide.showCanvas();
    }
    
    private void PlayerState(bool state)
    {
        foreach(var script in scripts)
        {
            script.enabled = state;
        }
        foreach(var render in renderers)
        {
            render.enabled = state;
        }
    }
        

}
