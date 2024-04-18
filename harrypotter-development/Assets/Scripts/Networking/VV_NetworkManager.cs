using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;
using PlayFab;
using PlayFab.MultiplayerAgent.Model;

public class VV_NetworkManager : NetworkManager
{
    bool clientRegistered;

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitUntil(() => LobbyManager.Instance != null);
            LobbyManager.Instance.OnServerConnected();
        }

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitUntil(() => LobbyManager.Instance != null);
            LobbyManager.Instance.OnClientConnected();
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        
        LobbyManager.Instance.HandlePlayerDisconnection(conn);
        if (Configuration.Instance.buildType != BuildType.LOCAL_SERVER && NetworkServer.connections.Count == 0)
        {
            //ServerManagerBehaviour.Instance.OnServerShutDown();
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (RoomManager.Instance)
        {
            Destroy(RoomManager.Instance.gameObject);
        }
    }

    public override void OnClientDisconnect()
    {
        return;
        
        Destroy(NetworkManager.singleton.gameObject);
        Destroy(RoomManager.Instance.gameObject);
        Destroy(GameObject.Find("PLAYFAB"));

        NetworkClient.Disconnect();
        SceneManager.LoadScene("Offline");
    }
}
