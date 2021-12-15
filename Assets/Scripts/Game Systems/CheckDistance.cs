using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

namespace Game_Systems
{
    public class CheckDistance : NetworkBehaviour
    {
        [SerializeField] GameObject warning_UI;
        // Start is called before the first frame update
        private float lastShootTime = float.NegativeInfinity;
        PlayerHealth heal;
        void Start()
        {
            heal = GetComponent<PlayerHealth>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(transform.position, new Vector3(0, 0, 0)) >= 100)
            {
                warning_UI.SetActive(true);
                if (Time.time - lastShootTime >= 1)
                {
                    lastShootTime = Time.time;
                    heal.takeDemage(10);
                }
            }
            else
            {
                warning_UI.SetActive(false);
            }
        }
    }
}
