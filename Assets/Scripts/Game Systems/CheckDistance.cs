using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

namespace Game_Systems
{
    public class CheckDistance : MonoBehaviour
    {
        [SerializeField] public GameObject warningUI;
        [SerializeField] private float maxDistance = 150;
        private float lastDamageInstanceTime = float.NegativeInfinity;
        PlayerHealth health;
        private void Start()
        {
            health = GetComponent<PlayerHealth>();
        }

        // Update is called once per frame
        public void Update()
        {
            if (Vector3.Distance(transform.position, Vector3.zero) >= maxDistance)
            {
                warningUI.SetActive(true);
                if (Time.time - lastDamageInstanceTime >= 1)
                {
                    lastDamageInstanceTime = Time.time;
                    health?.takeDemage(10);
                }
            }
            else
            {
                warningUI.SetActive(false);
            }
        }
    }
}
