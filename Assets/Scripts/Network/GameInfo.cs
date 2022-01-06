using UnityEngine;

namespace Network
{
    public class GameInfo : MonoBehaviour
    {
        private GameMode gameMode;
        private int TeamCount;
        private int PlayerCount;

        private void Awake() {
            gameMode = GameMode.Undefined;
            TeamCount = 0;
            PlayerCount = 0;
            DontDestroyOnLoad(gameObject);
        }

        public void SetGameInfo(GameMode mode, int teamCount, int playerCount)
        {
            gameMode = mode;
            TeamCount = teamCount;
            PlayerCount = playerCount;
        }

        public void DestroyGameInfo()
        {
            Destroy(gameObject);
        }
    }
}

