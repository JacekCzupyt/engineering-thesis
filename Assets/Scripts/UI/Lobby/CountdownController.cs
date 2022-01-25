using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public class CountdownController : MonoBehaviour
    {
        [SerializeField] public GameObject countdownTimerObject;
        private Text countdownText;

        private void Start() {
            countdownText = countdownTimerObject.GetComponent<Text>();
        }

        public void StartTimer(int countdownTime){
            StartCoroutine(CountdownToStart(countdownTime));
        }

        private IEnumerator CountdownToStart(int countdownTime)
        {
            countdownTimerObject.SetActive(true);

            while(countdownTime > 0)
            {
                countdownText.text = "Game will start in " + countdownTime.ToString() + "...";

                yield return new WaitForSeconds(1f);

                countdownTime--;
            }

            countdownText.text = "Game will start now...";
        }
    }
}
