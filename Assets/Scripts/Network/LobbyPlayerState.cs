using MLAPI.Serialization;

namespace Network {
    public struct LobbyPlayerState : INetworkSerializable
    {
        public ulong ClientId;
        public string PlayerName;
        public int TeamId;
        public bool IsReady;

        public LobbyPlayerState(ulong clientId, string playerName, int teamId, bool isReady)
        {
            ClientId = clientId;
            PlayerName = playerName;
            TeamId = teamId;
            IsReady = isReady;
        }

        public void NetworkSerialize(NetworkSerializer serializer)
        {
            serializer.Serialize(ref ClientId);
            serializer.Serialize(ref PlayerName);
            serializer.Serialize(ref TeamId);
            serializer.Serialize(ref IsReady);
        }
    }
}
