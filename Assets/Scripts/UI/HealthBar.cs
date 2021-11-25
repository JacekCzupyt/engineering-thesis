using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update 
    [SerializeField] Slider healthBar;
    public void SetInitialHealth()
    {
        //healthBar.maxValue = life;
        healthBar.value = 100;
    }

    // Update is called once per frame
    public void SetHealth(int life)
    {
        healthBar.value = life;
    }
}
