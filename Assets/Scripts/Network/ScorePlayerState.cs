using MLAPI.Serialization;

namespace Network {
    public struct ScorePlayerState : INetworkSerializable
    {
        public ulong ClientId;
        public string PlayerName;
        public int PlayerKills;
        public int PlayerDeaths;

        public ScorePlayerState(ulong clientId, string playerName, int playerKills, int playerDeaths)
        {
            ClientId = clientId;
            PlayerName = playerName;
            PlayerKills = playerKills;
            PlayerDeaths = playerDeaths;
        }

        public void NetworkSerialize(NetworkSerializer serializer)
        {
            serializer.Serialize(ref ClientId);
            serializer.Serialize(ref PlayerName);
            serializer.Serialize(ref PlayerKills);
            serializer.Serialize(ref PlayerDeaths); 
        }
    }
}
