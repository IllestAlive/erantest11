using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player : NetworkBehaviour
{
    [SyncVar] public int localId;
    
    public Character myCharacter;
    public Character opponentCharacter;
    
    private NetworkIdentity networkIdentity;

    public static UnityAction<int> onPlayerDisconnected;
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        onPlayerDisconnected += OnPlayerDisconnect;
    }

    [Server]
    public void SpawnCharacter()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
        if (!LocationManager.Instance.isFirstPlayerLocated)
        {
            myCharacter = Instantiate(NetworkManager.singleton.spawnPrefabs[0], LocationManager.Instance.firstPlayerSpawnPoint, Quaternion.Euler(Vector3.zero),transform).GetComponent<Character>();
           
            LocationManager.Instance.isFirstPlayerLocated = true;
        }
        else
        {
            myCharacter = Instantiate(NetworkManager.singleton.spawnPrefabs[0], LocationManager.Instance.secondPlayerSpawnPoint, Quaternion.Euler(Vector3.zero),transform).GetComponent<Character>();
            myCharacter.transform.GetChild(0).transform.rotation = Quaternion.Euler(0,180,0);
            LocationManager.Instance.isFirstPlayerLocated = false;
        }

        myCharacter.myPlayer = this;
        myCharacter.GetComponent<NetworkMatch>().matchId = GetComponent<NetworkMatch>().matchId;
        NetworkServer.Spawn(myCharacter.gameObject, gameObject);
    }

    private void OnDestroy()
    {
        if (isClient && !isLocalPlayer)
        {
            onPlayerDisconnected(localId);
        }
    }

    private void OnPlayerDisconnect(int _id)
    {
        if (_id != localId && GameOverUIManager.Instance != null)
        {
            GameOverUIManager.Instance.ShowGameOverScreen(true);
        }
    }
}