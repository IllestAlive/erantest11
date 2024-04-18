using Mirror;
using System.Collections;
using System.Linq;
using UnityEngine;
using kcp2k;

public class ServerManagerBehaviour : Instancable<ServerManagerBehaviour>
{
    // This variable needs to be hardcoded in your MultiplayerSettings.json for localVM
    // In addition to also being set on your PlayFab server build settings.
    string _playFabPortName = "game_port";

    private void Awake()
    {
    }

    void Start()
    {
        Debug.Log("STARTING UP SERVER LOGIC");

        switch (Configuration.Instance.buildType)
        {
            case BuildType.LOCAL_SERVER:
                OpenServerOnIpAndPort(Configuration.Instance.ipAddress, Configuration.Instance.port);
                break;

            case BuildType.REMOTE_SERVER:
                PreparePlayFabServer();
                break;
        }
    }

    void PreparePlayFabServer()
    {
        PlayFab.PlayFabMultiplayerAgentAPI.Start();
        PlayFab.PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;
        PlayFab.PlayFabMultiplayerAgentAPI.OnShutDownCallback += OnServerShutDown;
        PlayFab.PlayFabMultiplayerAgentAPI.OnAgentErrorCallback += OnServerError;

        PlayFab.PlayFabMultiplayerAgentAPI.ReadyForPlayers();
    }

    private void OnServerError(string error)
    {
        Debug.LogError(error);
    }

    void OnServerActive()
    {
        var connectionInfo = PlayFab.PlayFabMultiplayerAgentAPI.GetGameServerConnectionInfo();
        var gamePortData = connectionInfo.GamePortsConfiguration.Single(x => x.Name == _playFabPortName);
        var playFabInternalIP = "localhost";

        OpenServerOnIpAndPort(playFabInternalIP, (ushort)gamePortData.ServerListeningPort);

        // StartCoroutine(ShutDownServerInMinutes(5));
    }

    public void OnServerShutDown()
    {
        Debug.Log("Server starting shutdown process");
        BeginShutDown();
    }

    void OpenServerOnIpAndPort(string serverIp, ushort serverPort)
    {
        var activeTransport = Transport.activeTransport as KcpTransport;
        activeTransport.Port = serverPort;
        NetworkManager.singleton.networkAddress = serverIp;

        NetworkManager.singleton.StartServer();

        Debug.Log($"Opening Server on: {NetworkManager.singleton.networkAddress}:{activeTransport.Port}");
    }


    IEnumerator ShutDownServerInMinutes(int minutes)
    {
        yield return new WaitForSeconds(60 * minutes);

        BeginShutDown();
    }

    void BeginShutDown()
    {
        Debug.Log("Closing application");
        Application.Quit();
    }
}