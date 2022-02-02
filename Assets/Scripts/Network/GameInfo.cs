using MLAPI.Serialization;

namespace Network
{
    public struct GameInfo : INetworkSerializable
    {
        public GameMode gameMode;
        public int teamCount;
        public int playerCount;
        public int maxPlayersPerTeam;
        public int numberOfKillsToWin;

        public GameInfo(GameMode gameMode, int teamCount, int playerCount, int maxPlayersPerTeam, int numberOfKillsToWin)
        {
            this.gameMode = gameMode;
            this.teamCount = teamCount;
            this.playerCount = playerCount;
            this.maxPlayersPerTeam = maxPlayersPerTeam;
            this.numberOfKillsToWin = numberOfKillsToWin;
        }

        public void NetworkSerialize(NetworkSerializer serializer)
        {
            serializer.Serialize(ref gameMode);
            serializer.Serialize(ref teamCount);
            serializer.Serialize(ref playerCount);
            serializer.Serialize(ref maxPlayersPerTeam);
            serializer.Serialize(ref numberOfKillsToWin);
        }
    }
}

