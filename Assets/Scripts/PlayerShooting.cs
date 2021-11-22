using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System.Linq;

public class PlayerShooting : NetworkBehaviour

{
    [SerializeField] private float tickRate = 20;

    [SerializeField] Camera cam;
    [SerializeField] ParticleSystem bulletSystem;
    private ParticleSystem.EmissionModule em;
    private PlayerHealth enemyHealth;

    private float lastSendTime;
    //NetworkVariable<ParticleSystem> par;
    private ClientRpcParams NonOwnerClientParams =>
    new ClientRpcParams
    {
        Send = new ClientRpcSendParams
        {
            TargetClientIds = NetworkManager.Singleton.ConnectedClientsList.Where(c => c.ClientId != OwnerClientId)
                .Select(c => c.ClientId).ToArray()
        }
    };
    // Start is called before the first frame update
    void Start()
    {
      //  par = new NetworkVariable<ParticleSystem>(bulletSystem);
        em = bulletSystem.emission;
    }

    // Update is called once per frame
    void Update()
    {
        //bulletSystem = par.Value;
        if (IsOwner)
        {
            SendData();
        } 
    }
    void SendData()
    {       
        if (Input.GetMouseButton(0))
        {
            ShootServerRPC(true);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            ShootServerRPC(false);
        }
    }
    [ServerRpc]
    private void ShootServerRPC(bool isShoot,ServerRpcParams rpcParams = default)
    {
        if (!(NetworkManager.Singleton.NetworkTime - lastSendTime >= (1f / tickRate)) && isShoot)
            return;
        lastSendTime = NetworkManager.Singleton.NetworkTime;
        Debug.Log("HI" + isShoot);
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            enemyHealth = hit.transform.GetComponent<PlayerHealth>();
            if (enemyHealth!=null)
            {
                enemyHealth.takeDemage(1);
            }

        }
        ShootClientRPC(isShoot,NonOwnerClientParams);

    }

    [ClientRpc]
    private void ShootClientRPC(bool isShoot,ClientRpcParams rpcParams = default)
    {
        if (isShoot)
            em.rateOverTime = 10f;
        else
            em.rateOverTime = 0f;
    }
   
}
