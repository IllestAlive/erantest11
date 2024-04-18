using Mirror;
using PlayFab;
using PlayFab.MultiplayerModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions.TUTORIAL;
using kcp2k;
using UnityEngine;
using UnityEngine.Serialization;

public class ClientManagerBehaviour : Instancable<ClientManagerBehaviour>
{
    // PlayFab Login Info
    PlayFab.ClientModels.LoginResult _loginData = null;

    public string ticketId;

    private string status;

    Coroutine c;

    public KcpTransport kcpTransport;

    private void Start()
    {
        if (TutorialManager.TutorialStep == 3)
        {
            TutorialManager.TutorialStep = 3;
        }
        
        if (TutorialManager.ShowTutorial)
            return;
        
        Configuration.Instance.RegisterPlayButton();
    }

    public void OnTapPlay()
    {
        if (Configuration.Instance.testWithoutPlayfab)
        {
            ConnectToServerWithIpAndPort(Configuration.Instance.ipAddress, Configuration.Instance.port);
            return;
        }

        PlayFab.ClientModels.LoginWithCustomIDRequest request = new PlayFab.ClientModels.LoginWithCustomIDRequest();
        request.CustomId = Configuration.Instance.userName;
        request.CreateAccount = true;

        PlayFabClientAPI.LoginWithCustomID(request, OnLogin, OnError);
    }

    private void OnError(PlayFab.PlayFabError obj)
    {
        Debug.LogError(obj.GenerateErrorReport());
    }

    private void OnLogin(PlayFab.ClientModels.LoginResult obj)
    {
        Debug.Log("Login was successful");
        _loginData = obj;

        if (Configuration.Instance.ipAddress != "" && Configuration.Instance.buildType == BuildType.REMOTE_CLIENT)
        {
            ConnectRemoteClient();
        }
        else
        {
            switch (Configuration.Instance.buildType)
            {
                case BuildType.LOCAL_CLIENT:
                    ConnectToServerWithIpAndPort(Configuration.Instance.ipAddress, Configuration.Instance.port);
                    break;

                case BuildType.REMOTE_CLIENT:
                    RequestServer();
                    break;

                case BuildType.PLAYFAB_LOCAL_CLIENT:
                    // 56100 is the NodePort on the MultiplayerSettings.json for the local VM
                    ConnectToServerWithIpAndPort("localhost", 56100);
                    break;
            }
        }
        
    }

    private void RequestServer()
    {
        RequestMultiplayerServerRequest request = new RequestMultiplayerServerRequest
        {
            BuildId = Configuration.Instance.buildId,
            SessionId = System.Guid.NewGuid().ToString(),
            PreferredRegions = new List<string>() { AzureRegion.FranceCentral.ToString() }
        };

        PlayFabMultiplayerAPI.RequestMultiplayerServer(request, OnRequestMPServer, OnError);
    }

    private void OnRequestMPServer(RequestMultiplayerServerResponse response)
    {
        Debug.Log(response.ToString());
        ConnectRemoteClient(response);
    }

    private void ConnectRemoteClient(RequestMultiplayerServerResponse response = null)
    {
        if (response == null)
        {
            //After requesting, cancel matchmaking tickets to create new one
            PlayFabMultiplayerAPI.CancelAllMatchmakingTicketsForPlayer(
                    new CancelAllMatchmakingTicketsForPlayerRequest
                    {
                        Entity = new EntityKey
                        {
                            Id = _loginData.EntityToken.Entity.Id,
                            Type = _loginData.EntityToken.Entity.Type
                        },
                        QueueName = "default"
                    },
                    this.OnCanceled,
                    this.OnMatchmakingError
                    );
        }
        else
        {
            Debug.Log("**** ADD THIS TO YOUR CONFIGURATION **** -- IP: " + response.IPV4Address + " Port: " + (ushort)response.Ports[0].Num);
            NetworkManager.singleton.networkAddress = response.IPV4Address;
            kcpTransport.Port = (ushort)response.Ports[0].Num;
        }
    }

    private void OnCanceled(CancelAllMatchmakingTicketsForPlayerResult obj)
    {
        //After canceling the tickets, create a new one

        Debug.Log("Canceled tickets");
        var ticket = new CreateMatchmakingTicketRequest
        {
            Creator = new MatchmakingPlayer
            {
                Entity = new EntityKey
                {
                    Id = _loginData.EntityToken.Entity.Id,
                    Type = _loginData.EntityToken.Entity.Type
                },
                // Here we specify the creator's attributes.
                Attributes = new MatchmakingPlayerAttributes
                {
                    DataObject = new
                    {
                        Skill = 24.4
                    },
                }
            },

            // Cancel matchmaking if a match is not found after 120 seconds.
            GiveUpAfterSeconds = 120,

            // The name of the queue to submit the ticket into.
            QueueName = "default",
        };

        PlayFabMultiplayerAPI.CreateMatchmakingTicket
        (
            ticket,
            // Callbacks for handling success and error.
            this.OnMatchmakingTicketCreated,
            this.OnMatchmakingError
        );
    }

    private void OnMatchmakingTicketCreated(CreateMatchmakingTicketResult obj)
    {
        ticketId = obj.TicketId;
        status = "Ticket created, searching...";

        ConnectToServerWithIpAndPort(
            Configuration.Instance.ipAddress,
            Configuration.Instance.port);

        c = StartCoroutine(PollTicket());
    }

    private IEnumerator PollTicket()
    {
        while (true)
        {
            PlayFabMultiplayerAPI.GetMatchmakingTicket(
                new GetMatchmakingTicketRequest
                {
                    TicketId = ticketId,
                    QueueName = "default",
                },
                this.OnGetMatchmakingTicket,
                this.OnMatchmakingError);

            yield return new WaitForSeconds(6);
            status = "Searching...";
        }
    }

    private void OnGetMatchmakingTicket(GetMatchmakingTicketResult obj)
    {
        switch (obj.Status)
        {
            case "Matched":
                StopCoroutine(c);
                status = "Found match: " + obj.MatchId;
                StartMatch(obj.MatchId);
                break;
        }
    }

    private void StartMatch(string matchId)
    {
        PlayFabMultiplayerAPI.GetMatch(
            new GetMatchRequest
            {
                MatchId = matchId,
                QueueName = "default"
            },
            OnGetMatch,
            OnMatchmakingError
        );
    }

    private void OnGetMatch(GetMatchResult obj)
    {
        status = "Joined match: " + obj.MatchId;
        return;
        ConnectToServerWithIpAndPort(
            Configuration.Instance.ipAddress,
            Configuration.Instance.port);
    }

    private void OnGUI()
    {
        GUILayout.Label(status);
    }

    private void OnMatchmakingError(PlayFabError obj)
    {
        Debug.Log("Matchmaking error: " + obj.ErrorDetails);
    }

    private void ConnectToServerWithIpAndPort(string ipAddress, ushort gamePort)
    {
        var transport = Transport.activeTransport as KcpTransport;
        transport.Port = gamePort;
        NetworkManager.singleton.networkAddress = ipAddress;

        Debug.Log($"Connecting to {NetworkManager.singleton.networkAddress}:{transport.Port}");

        NetworkManager.singleton.StartClient();
    }
}