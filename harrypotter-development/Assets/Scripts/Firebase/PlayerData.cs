using System.Collections.Generic;
using Firebase.Firestore;

namespace Firebase
{
    [FirestoreData]
    public class PlayerData
    {
        [FirestoreProperty]
        public string ID { get; set; }
        
        [FirestoreProperty]
        public CommanderNFTData CommanderNFT { get; set; }
        
        [FirestoreProperty]
        public List<EquipmentSlotData> EquipmentSlot { get; set; }
        
        [FirestoreProperty]
        public int Experience { get; set; }
        
        [FirestoreProperty]
        public List<int> SkillData { get; set; }
    }

    [FirestoreData]
    public class EquipmentSlotData
    {
        [FirestoreProperty] 
        public int SlotID { get; set; }
        
        [FirestoreProperty] 
        public int EquipmentID { get; set; }
    }
    
    [FirestoreData]
    public class CommanderNFTData
    {
        [FirestoreProperty] 
        public int CommanderID { get; set; }
    }
}