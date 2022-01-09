using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class GameInfoManager : MonoBehaviour
    {
        private GameInfo gameInfo;
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
        public void SetGameInfo(GameInfo gameInfo)
        {
            this.gameInfo = gameInfo;
        }

        public GameInfo GetGameInfo()
        {
            return gameInfo;
        }
    }
}
