using System.Collections.Generic;
using Extensions.TUTORIAL;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using VillageMode.Internal;

namespace Firebase
{
    public class FirebaseDataManager : Instancable<FirebaseDataManager>
    {
        private FirebaseFirestore _db;
        private CollectionReference _playerCollectionRef;
        private DocumentReference _playerRef;
        private DocumentSnapshot _playerSnapshot;
        
        private DocumentReference _enemyRef;
        private DocumentSnapshot _enemySnapshot;
        

        public PlayerData MyPlayerData { get; private set; }
        public PlayerData EnemyPlayerData { get; private set; }
        private string _myPlayerID => AuthManager.EMail;
        
        public bool initialized;

        private void Start()
        {
            InitializeData();
        }

        public async void InitializeData()
        {
            _db = FirebaseFirestore.DefaultInstance;
            
            _playerCollectionRef = _db.Collection("Players");
            
            await _playerCollectionRef.Document(_myPlayerID).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                _playerSnapshot = task.Result;
                if (_playerSnapshot.Exists)
                {
                    Debug.Log($"Session '{_myPlayerID}' exists. Initializing it...");
                    _playerRef = _playerCollectionRef.Document(_myPlayerID);
                    ImportPlayerFromDatabase();
                }
                else
                {
                    Debug.Log($"There is no session '{_myPlayerID}'. Creating a new one...");
                    CreateNewPlayer();
                }

                initialized = true;
            });
            
            TutorialManager.OnEndReadingPlayerData();
        }

        public async void InitializeEnemyData(string enemyId)
        {
            await _playerCollectionRef.Document(enemyId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                _enemySnapshot = task.Result;
                Debug.Log($"Session '{enemyId}' exists. Initializing it...");
                _enemyRef = _playerCollectionRef.Document(enemyId);
                ImportEnemyFromDatabase(enemyId);
            });
        }

        private async void CreateNewPlayer()
        {
            // PlayerData testData = new PlayerData()
            // {
            //     CommanderNFT = new CommanderNFTData() {CommanderID = 1},
            //     EquipmentSlot = new List<EquipmentSlotData>()
            //     {
            //         new() {EquipmentID = 1, SlotID = 0},
            //         new() {EquipmentID = 1, SlotID = 1},
            //         new() {EquipmentID = 1, SlotID = 2},
            //         new() {EquipmentID = 1, SlotID = 3},
            //         new() {EquipmentID = 1, SlotID = 4},
            //     }
            // };
            
            await _playerCollectionRef.Document(_myPlayerID).SetAsync(new PlayerData()).ContinueWithOnMainThread(task =>
            {
                Debug.Log($"Created a new Player! ID: {_myPlayerID}");
            });
            
            await _playerCollectionRef.Document(_myPlayerID).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                _playerSnapshot = task.Result;
            });
        }
        
        private void ImportPlayerFromDatabase()
        {
            MyPlayerData = new PlayerData();
            
            _playerRef = _playerCollectionRef.Document(_myPlayerID);
            MyPlayerData = _playerSnapshot.ConvertTo<PlayerData>();
        }

        private void ImportEnemyFromDatabase(string enemyId)
        {
            EnemyPlayerData = new PlayerData();
            
            _enemyRef = _playerCollectionRef.Document(enemyId);
            EnemyPlayerData = _enemySnapshot.ConvertTo<PlayerData>();
        }

        public async void WritePlayerData(PlayerData playerData)
        {
            MyPlayerData = playerData;
            
            await _playerCollectionRef.Document(_myPlayerID).UpdateAsync("CommanderNFT", MyPlayerData.CommanderNFT);
            await _playerCollectionRef.Document(_myPlayerID).UpdateAsync("EquipmentSlot", MyPlayerData.EquipmentSlot);
            await _playerCollectionRef.Document(_myPlayerID).UpdateAsync("SkillData", MyPlayerData.SkillData);
        }

        public async void UpdateMyPlayerDataExperience()
        {
            await _playerCollectionRef.Document(_myPlayerID).UpdateAsync("Experience", MyPlayerData.Experience);
        }
    }
}