using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerGameManager : Instancable<ServerGameManager>
{
    public Dictionary<Guid, Game> games = new Dictionary<Guid, Game>();

    public void DestroyGame(Guid gameId)
    {
        Game gameToDestroy = null;

        if (games.TryGetValue(gameId, out gameToDestroy))
        {
            games.Remove(gameId);
            Destroy(gameToDestroy.gameObject);
        }
        
    }
}