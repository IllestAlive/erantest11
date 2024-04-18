using TMPro;
using UnityEngine;

namespace Firebase
{
    public class FirebaseConfiguration : Instancable<FirebaseConfiguration>
    {
        [System.Serializable]
        public class ClientConfiguration
        {
            public string sessionToJoin;
            public string playerId;
        }

        [System.Serializable]
        public class ServerConfiguration
        {
            public TMP_InputField input_SessionToCreate;
            public string sessionToCreate;
        }
    
        [SerializeField] internal bool isServer;

        [SerializeField] private ClientConfiguration clientConfig;
        [SerializeField] private ServerConfiguration serverConfig;

        public ClientConfiguration GetClientConfig()
        {
            return clientConfig;
        }

        public ServerConfiguration GetServerConfig()
        {
            return serverConfig;
        }
    }
}