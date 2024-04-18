using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Match
{
    public string matchName;
    public Guid matchID;
    public List<int> networkConnectionIds = new List<int>();

    public Match() { }

    public Match(Guid _matchID)
    {
        matchID = _matchID;
    }

    public bool IsFull { get => networkConnectionIds.Count == 2; }

    public void AddPlayer(NetworkConnectionToClient conn)
    {
        LobbyManager.Instance.playerMatchPairs[conn] = this;

        networkConnectionIds.Add(conn.connectionId);
    }

    public void RemovePlayer(NetworkConnectionToClient conn)
    {
        LobbyManager.Instance.playerMatchPairs[conn] = null;

        networkConnectionIds.Remove(conn.connectionId);
    }
}

public static class MatchExtensions
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hashBytes = provider.ComputeHash(inputBytes);

        return new Guid(hashBytes);
    }

    public static string GetRandomMatchID()
    {
        string _id = string.Empty;
        for (int i = 0; i < 5; i++)
        {
            int random = UnityEngine.Random.Range(0, 36);
            if (random < 26)
            {
                _id += (char)(random + 65);
            }
            else
            {
                _id += (random - 26).ToString();
            }
        }
        Debug.Log($"Random Match ID: {_id}");
        return _id;
    }

}

public enum ServerMessageType { JoinedPlayer, CreateRoom, RemoveRoom, JoinRoom, RequestStartMatch, StartedMatch, RoomList }
public struct ServerMessage : NetworkMessage
{
    public Match _match;
    public string matchName;
    public ServerMessageType messageType;
}

public enum ClientMessageType { AddRoom, RemoveRoom, JoinedRoom, UpdateMatch, StartedMatch }
public struct ClientMessage : NetworkMessage
{
    public Match _match;
    public bool isOwner;
    public ClientMessageType messageType;
}

public class LobbyManager : Instancable<LobbyManager>
{
    public RoomUI roomButtonPrefab;
    public Transform buttonsContainer;

    public TMP_InputField roomNameField;

    public Dictionary<Guid , Match> matches = new Dictionary<Guid, Match>();
    public Dictionary<Guid , RoomUI> matchButtons = new Dictionary<Guid, RoomUI>();
    public Dictionary<NetworkConnectionToClient, Match> playerMatchPairs = new Dictionary<NetworkConnectionToClient, Match>();
    public List<NetworkConnectionToClient> clientsInLobby = new List<NetworkConnectionToClient>();

    public void OnServerConnected()
    {
        NetworkServer.RegisterHandler<ServerMessage>(OnServerMessage);
    }

    public void OnClientConnected()
    {
        NetworkClient.RegisterHandler<ClientMessage>(OnClientMessage);
        NetworkClient.connection.Send(new ServerMessage { messageType = ServerMessageType.RoomList });
        NetworkClient.connection.Send(new ServerMessage { messageType = ServerMessageType.JoinedPlayer });
        print("I am connected");

    }
    private void OnServerMessage(NetworkConnectionToClient conn, ServerMessage msg)
    {
        switch (msg.messageType)
        {
            case ServerMessageType.JoinedPlayer:
                Debug.Log("Player #" + conn.connectionId + " has joined");

                Match matchToJoin = null;

                foreach (var item in matches)
                {
                    if (!item.Value.IsFull)
                    {
                        matchToJoin = item.Value;
                        break;
                    }
                }

                clientsInLobby.Add(conn);

                if (matchToJoin != null)
                {
                    ServerJoinRoom(conn, matchToJoin.matchID);
                    break;
                }
                else
                {
                    ServerCreateRoom(conn, "Room " + matches.Count.ToString());
                    break;
                }
            case ServerMessageType.CreateRoom:
                ServerCreateRoom(conn, msg.matchName);
                break;
            case ServerMessageType.RemoveRoom:
                matches.Remove(msg._match.matchID);

                //Remove room from list for all clients
                foreach (var clientInLobby in clientsInLobby)
                {
                    clientInLobby.Send(new ClientMessage { messageType = ClientMessageType.RemoveRoom, _match = msg._match });
                }
                break;
            case ServerMessageType.JoinRoom:
                ServerJoinRoom(conn, msg._match.matchID);
                break;
            case ServerMessageType.RequestStartMatch:
                foreach (var item in matches[msg._match.matchID].networkConnectionIds)
                {
                    NetworkServer.connections[item].Send(new ClientMessage { _match = matches[msg._match.matchID], messageType = ClientMessageType.StartedMatch });
                }
                break;
            case ServerMessageType.StartedMatch:

                Match matchToStart = null;

                if (!matches.TryGetValue(msg._match.matchID, out matchToStart))
                {
                    return;
                }

                print($"starting match: {msg._match.matchID}");
                //Start match logic

                var game = new GameObject($"Game #{msg._match.matchID}").AddComponent<Game>();

                game.SetGame(msg._match.matchID);
                game.SetManagers();

                ServerGameManager.Instance.games.Add(msg._match.matchID, game);
                
                //Spawn players
                int i = 0;
                foreach (var connId in matchToStart.networkConnectionIds)
                {
                    clientsInLobby.Remove(NetworkServer.connections[connId]);

                    var player = Instantiate(NetworkManager.singleton.playerPrefab, game.transform);
                    player.GetComponent<NetworkMatch>().matchId = msg._match.matchID;
                    game.connectionPlayerDictionary.Add(NetworkServer.connections[connId], player.GetComponent<Player>());
                    game.idPlayerDictionary.Add(i, player.GetComponent<Player>());
                    player.GetComponent<Player>().localId = i;
                    NetworkServer.Spawn(player);
                    NetworkServer.AddPlayerForConnection(NetworkServer.connections[connId], player);
                    player.GetComponent<Player>().SpawnCharacter();

                    i++;
                }

                var healthRegenTimer = Instantiate(NetworkManager.singleton.spawnPrefabs[2], game.transform);
                healthRegenTimer.GetComponent<NetworkMatch>().matchId = msg._match.matchID;
                NetworkServer.Spawn(healthRegenTimer);
                
                var spawnManager = Instantiate(NetworkManager.singleton.spawnPrefabs[3], game.transform);
                spawnManager.GetComponent<NetworkMatch>().matchId = msg._match.matchID;
                NetworkServer.Spawn(spawnManager);
                
                var skillUsages = Instantiate(NetworkManager.singleton.spawnPrefabs[4], game.transform);
                skillUsages.GetComponent<NetworkMatch>().matchId = msg._match.matchID;
                NetworkServer.Spawn(skillUsages);
                
                
                
                
                
                game.idPlayerDictionary[0].opponentCharacter = game.idPlayerDictionary[1].myCharacter;
                game.idPlayerDictionary[1].opponentCharacter = game.idPlayerDictionary[0].myCharacter;
                
                break;
            case ServerMessageType.RoomList:
                foreach (var item in matches)
                {
                    conn.Send(new ClientMessage { messageType = ClientMessageType.AddRoom, _match = item.Value });
                }
                break;
            default:
                break;
        }
    }

    void ServerCreateRoom(NetworkConnectionToClient conn, string _matchName)
    {
        var matchID = MatchExtensions.GetRandomMatchID().ToGuid();

        Match match = new Match(matchID);

        match.matchName = _matchName;
        match.AddPlayer(conn);

        //Add it to the match dictionary with it's id
        matches.Add(matchID, match);

        //Add room to list for all clients
        foreach (var clientInLobby in clientsInLobby)
        {
            clientInLobby.Send(new ClientMessage { messageType = ClientMessageType.AddRoom, _match = match });
        }
        //Let the client know his match
        conn.Send(new ClientMessage() { messageType = ClientMessageType.JoinedRoom, _match = match, isOwner = true });

        Debug.Log("Created room!");
    }

    void ServerJoinRoom(NetworkConnectionToClient conn, Guid _matchId)
    {
        var _match = matches[_matchId];

        if (_match.networkConnectionIds.Count == 2)
        {
            return;
        }

        _match.AddPlayer(conn);

        //Let the client know his match
        conn.Send(new ClientMessage() { messageType = ClientMessageType.JoinedRoom, _match = _match });

        //Update room information for all clients (They want to know if it's full or not)
        foreach (var clientInLobby in clientsInLobby)
        {
            clientInLobby.Send(new ClientMessage { messageType = ClientMessageType.UpdateMatch, _match = _match });
        }
    }

    private void OnClientMessage(ClientMessage msg)
    {
        switch (msg.messageType)
        {
            case ClientMessageType.AddRoom:
                if (matches.ContainsKey(msg._match.matchID))
                {
                    return;
                }

                //Add it to the match dictionary with it's id
                matches.Add(msg._match.matchID, msg._match);
                CreateRoomButton(msg._match.matchID);
                break;
            case ClientMessageType.JoinedRoom:
                RoomManager.Instance.myMatch = matches[msg._match.matchID];

                RoomManager.Instance.isRoomOwner = msg.isOwner;

                print($"joined room:{RoomManager.Instance.myMatch}, isOwner: {RoomManager.Instance.isRoomOwner}");

                if (!RoomManager.Instance.isRoomOwner)
                {
                    //Request Start match
                    print("Sent start match request");
                    NetworkClient.connection.Send(new ServerMessage { _match = RoomManager.Instance.myMatch, messageType = ServerMessageType.RequestStartMatch });
                }
                break;
            case ClientMessageType.RemoveRoom:
                RemoveRoomButton(msg._match.matchID);
                matches.Remove(msg._match.matchID);
                break;
            case ClientMessageType.UpdateMatch:
                //Sync match with server
                if (matches.Count == 0)
                {
                    return;
                }
                var matchId = msg._match.matchID;
                matches[matchId] = msg._match;
                if (msg._match.IsFull)
                {
                    matchButtons[matchId].button.interactable = false;
                }
                else
                {
                    matchButtons[matchId].button.interactable = true;
                }
                break;
            case ClientMessageType.StartedMatch:
                NetworkClient.UnregisterHandler<ClientMessage>();
                SceneManager.LoadScene("Game");
                break;
            default:
                break;
        }
    }

    public void HandlePlayerDisconnection(NetworkConnectionToClient conn)
    {
        Debug.Log("Player #" + conn.connectionId + " has disconnected");

        Match playerMatch = null;
        
        if (playerMatchPairs.TryGetValue(conn, out playerMatch))
        {
            playerMatch.RemovePlayer(conn);


            //If there is no player left
            if (playerMatch.networkConnectionIds.Count == 0)
            {
                Debug.Log("Room #" + playerMatch.matchID + " is closing");

                ServerDeleteRoom(playerMatch.matchID);
            }

            //Send updated match information to every other player
            foreach (var clientInLobby in clientsInLobby)
            {
                clientInLobby.Send(new ClientMessage { messageType = ClientMessageType.UpdateMatch, _match = playerMatch });
            }

        }
    }

    void ServerDeleteRoom(Guid matchId)
    {
        ServerGameManager.Instance.DestroyGame(matchId);
        var matchToRemove = matches[matchId];

        //RemoveRoomButton(matchList.Values.ToList().IndexOf(matchToRemove));
        matches.Remove(matchToRemove.matchID);

        //Send updated room list to players in room
        foreach (var conn in clientsInLobby)
        {
            conn.Send(new ClientMessage { messageType = ClientMessageType.RemoveRoom, _match = matchToRemove });
        }
    }

    public void OnTapCreateRoom()
    {
        if (RoomManager.Instance.myMatch.matchID != Guid.Empty)
        {
            return;
        }

        string roomName = roomNameField.text;
        RoomManager.Instance.isRoomOwner = true;
        NetworkClient.connection.Send(new ServerMessage { messageType = ServerMessageType.CreateRoom, matchName = roomName });
    }

    public void CreateRoomButton(Guid matchID)
    {
        var match = matches[matchID];

        var roomButton = Instantiate(roomButtonPrefab, buttonsContainer);
        roomButton.Init(match);

        matchButtons.Add(matchID, roomButton);
    }

    public void RemoveRoomButton(Guid matchID)
    {
        var roomButtonToRemove = matchButtons[matchID];
        matchButtons.Remove(matchID);
        Destroy(roomButtonToRemove.gameObject);
    }

    public void OnTapRoomButton(Guid _matchId)
    {
        if (RoomManager.Instance.myMatch.matchID != Guid.Empty)
        {
            return;
        }

        NetworkClient.connection.Send(new ServerMessage { messageType = ServerMessageType.JoinRoom, _match = matches[_matchId] });
    }
}
