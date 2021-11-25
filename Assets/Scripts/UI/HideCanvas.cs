using UnityEngine;

namespace UI {
    public class HideCanvas : MonoBehaviour
    {
        [SerializeField] GameObject can;

        [SerializeField] GameObject healthBar;
        // Start is called before the first frame update
        public void hideCanvas()
        {
            can.SetActive(false);
            healthBar.SetActive(false);
        }
        public void showCanvas()
        {
            can.SetActive(true);
            healthBar.SetActive(true);
        }
    }
}
