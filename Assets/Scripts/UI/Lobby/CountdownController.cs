using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public class CountdownController : MonoBehaviour
    {
        [SerializeField] public int countdownTime = 5;
        [SerializeField] public GameObject countdownDisplay;

        public void StartTimer(){
            StartCoroutine(CountdownToStart());
        }

        IEnumerator CountdownToStart()
        {
            Text countdownText = countdownDisplay.gameObject.GetComponent<Text>();

            while(countdownTime > 0)
            {
                countdownText.text = "Game will start in " + countdownTime.ToString() + "...";

                yield return new WaitForSeconds(1f);

                countdownTime--;
            }

            countdownText.text = "Game will start now...";

            countdownDisplay.gameObject.SetActive(false);
        }
    }
}
