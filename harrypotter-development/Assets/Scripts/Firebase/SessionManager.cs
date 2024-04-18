using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

namespace Firebase
{
    public class SessionManager
    {
        public bool testSessionData;
        public DocumentSnapshot sessionSnapshot;
        
        private FirebaseFirestore _db;
        private CollectionReference _sessions;
        private DocumentReference _sessionRef;
        private string _sessionId;

        private bool _isClosingSession;
        
        public void SessionStartup()
        {
            var serverConfig = FirebaseConfiguration.Instance.GetServerConfig();
            
            serverConfig.sessionToCreate = serverConfig.sessionToCreate == serverConfig.input_SessionToCreate.text ? serverConfig.sessionToCreate : serverConfig.input_SessionToCreate.text;

            _sessionId = serverConfig.sessionToCreate;
            
            TryInitializeSession();
        }

        private async void TryInitializeSession()
        {
            _db = FirebaseFirestore.DefaultInstance;

            _sessions = _db.Collection("sessions");

            await _sessions.Document(_sessionId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                sessionSnapshot = task.Result;
                if (sessionSnapshot.Exists)
                {
                    Debug.Log($"Session '{_sessionId}' exists. Initializing it...");
                    _sessionRef = _sessions.Document(_sessionId);

                    if (testSessionData)
                    {
                        SetTestData();
                    }
                    else
                    {
                        InitializeSession();
                    }                    
                }
                else
                {
                    Debug.Log($"There is no session '{_sessionId}'. Creating a new one...");
                    CreateNewSession();
                }
            });
        }

        private async void SetTestData()
        {
            SessionData.CurrentSessionData = SessionData.GetTestSessionData();
            await _sessionRef.SetAsync(SessionData.CurrentSessionData);
            InitializeSession();
        }

        private async void CreateNewSession()
        {
            if (testSessionData)
            {
                SessionData.CurrentSessionData = SessionData.GetTestSessionData();
                await _sessions.AddAsync(SessionData.CurrentSessionData).ContinueWithOnMainThread(task =>
                {
                    _sessionRef = task.Result;
                    Debug.Log($"Created a new session! ID: {_sessionRef.Id}");
                });
                return;
            }
            
            var sessionToCreate = new Dictionary<string, object>
            {
                { "isActive", true }
            };
            await _sessions.AddAsync(sessionToCreate).ContinueWithOnMainThread(task =>
            {
                _sessionRef = task.Result;
                Debug.Log($"Created a new session! ID: {_sessionRef.Id}");
            });
        }

        private async void InitializeSession()
        {
            Dictionary<string, object> sessionToInit = new Dictionary<string, object>
            {
                { "isActive", true }
            };
            await _sessionRef.UpdateAsync(sessionToInit);
            SessionData.CurrentSessionData = sessionSnapshot.ConvertTo<SessionData>();

            if (SessionData.CurrentSessionData.Players != null)
            {
                foreach (var playerData in SessionData.CurrentSessionData.Players)
                {
                    SessionData.CurrentSessionData.PlayerDataDictionary.Add(playerData.ID, playerData);
                }
            }

            //TODO: Spawn Players
            
            Debug.Log("init success");
        }

        public async void UpdatePlayers()
        {
            await _sessionRef.UpdateAsync("Players", SessionData.CurrentSessionData.Players);
        }
        
        public async void UpdateIsSessionFull()
        {
            await _sessionRef.UpdateAsync("IsSessionFull", SessionData.CurrentSessionData.IsSessionFull);
        }

        private void OnDestroy()
        {
            CloseSession();
        }

        internal async void CloseSession()
        {
            if (_isClosingSession)
            {
                return;
            }
            _isClosingSession = true;
            var sessionToClose = new Dictionary<string, object>
            {
                { "isActive", false }
            };
            Debug.Log($"Closing session: {_sessionId}");
            await _sessionRef.UpdateAsync(sessionToClose);
        }
    }
}