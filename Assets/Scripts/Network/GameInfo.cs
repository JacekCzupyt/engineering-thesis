using UnityEngine;

namespace Network
{
    public class GameInfo : MonoBehaviour
    {
        private GameMode gameMode;
        public GameMode GameMode
        {
            get { return gameMode; }
        }
        private int teamCount;
        public int TeamCount 
        {
            get { return teamCount; }
        }
        private int playerCount;
        public int PlayerCount
        {
            get { return PlayerCount; }
        }

        private void Awake() {
            gameMode = GameMode.Undefined;
            teamCount = 0;
            playerCount = 0;
            DontDestroyOnLoad(gameObject);
        }

        public void SetGameInfo(GameMode gameMode, int teamCount, int playerCount)
        {
            this.gameMode = gameMode;
            this.teamCount = teamCount;
            this.playerCount = playerCount;
        }

        public void DestroyGameInfo()
        {
            Destroy(gameObject);
        }
    }
}

