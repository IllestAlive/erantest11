using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : Instancable<RoomManager>
{
    public Match myMatch;
    public bool isRoomOwner;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isRoomOwner && scene.name == "Game")
        {
            NetworkClient.Send(new ServerMessage { _match = myMatch, messageType = ServerMessageType.StartedMatch });
        }
    }
}
