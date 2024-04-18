using System.Collections.Generic;
using Firebase.Firestore;

namespace Firebase
{
    [FirestoreData]
    public class SessionData
    {
        public static SessionData CurrentSessionData;

        private static SessionData _testSessionData;

        private bool _initialized;
        
        [FirestoreProperty] public bool IsSessionFull { get; set; }
        
        [FirestoreProperty] public List<PlayerData> Players { get; set; }

        public Dictionary<string, PlayerData> PlayerDataDictionary = new();

        public static SessionData GetTestSessionData()
        {
            if(_testSessionData == null) CreateNewTestSession();

            return _testSessionData;
        }

        private static void CreateNewTestSession()
        {   
            _testSessionData = new SessionData();

            _testSessionData.Players = new List<PlayerData>() {
                new()
                {
                    CommanderNFT = new CommanderNFTData(){CommanderID = 1},
                    EquipmentSlot = new List<EquipmentSlotData>()
                    {
                        new(){EquipmentID = 1, SlotID = 0},
                        new(){EquipmentID = 1, SlotID = 1},
                        new(){EquipmentID = 1, SlotID = 2},
                        new(){EquipmentID = 1, SlotID = 3},
                        new(){EquipmentID = 1, SlotID = 4},
                    }
                },
                new()
                {
                    CommanderNFT = new CommanderNFTData(){CommanderID = 2},
                    EquipmentSlot = new List<EquipmentSlotData>()
                    {
                        new(){EquipmentID = 2, SlotID = 0},
                        new(){EquipmentID = 2, SlotID = 1},
                        new(){EquipmentID = 2, SlotID = 2},
                        new(){EquipmentID = 2, SlotID = 3},
                        new(){EquipmentID = 2, SlotID = 4},
                    }
                }
            };
        }
    }
}