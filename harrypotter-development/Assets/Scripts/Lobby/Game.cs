using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Game : MonoBehaviour
{
    public Guid gameId;
    public Dictionary<NetworkConnectionToClient, Player> connectionPlayerDictionary = new Dictionary<NetworkConnectionToClient, Player>();
    public Dictionary<int, Player> idPlayerDictionary = new Dictionary<int, Player>();

    #region Managers

    

    #endregion
    
    public void SetGame(Guid gameid)
    {
        this.gameId = gameid;
    }
    
    public void SetManagers()
    {
        
    }
}
