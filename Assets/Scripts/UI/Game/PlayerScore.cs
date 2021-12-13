using MLAPI.Serialization;

namespace UI.Game {
    public struct PlayerScore : INetworkSerializable
    {
        public ulong ClientId;
        public string PlayerName;
        public int KillScore;
        public int DeathScore;

        public PlayerScore(ulong clientId, string playerName, int killScore, int deathScore)
        {
            ClientId = clientId;
            PlayerName = playerName;
            KillScore = killScore;
            DeathScore = deathScore;
        }

        public void NetworkSerialize(NetworkSerializer serializer)
        {
            serializer.Serialize(ref ClientId);
            serializer.Serialize(ref PlayerName);
            serializer.Serialize(ref KillScore);
            serializer.Serialize(ref DeathScore);
        }
    }
}
