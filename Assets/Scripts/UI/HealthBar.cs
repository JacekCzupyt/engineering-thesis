using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Slider healthBar;
        public void SetInitialHealth()
        {
            healthBar.value = 100;
        }
        public void SetHealth(int health)
        {
            healthBar.value = health;
        }
    }
}
