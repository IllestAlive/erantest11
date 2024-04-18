using System.Collections.Generic;
using UnityEngine;

namespace Firebase
{
    public class CommanderBridge : Instancable<CommanderBridge>
    {
        public List<EquipmentSlotData> equipmentSlots;
        public CommanderNFTData commanderData;
        public PlayerData playerData;
        public int experience;

        public void OnSubmitToDatabase()
        {
            if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.WithoutCustomizedCharacters)
            {
                equipmentSlots = CommanderNFTSelection.Instance.activeCommander.GetComponent<CharacterEquipments>()
                    .GetEquipmentInfo();
                commanderData = new CommanderNFTData() { CommanderID = CommanderNFTSelection.Instance.activeId };

                playerData = new PlayerData()
                {
                    EquipmentSlot = equipmentSlots,
                    CommanderNFT = commanderData
                };

                FirebaseDataManager.Instance.WritePlayerData(playerData);
            }

            if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.OriginalCharacters)
            {
                equipmentSlots = OriginalCharacterHolder.Instance.activeCommander.GetComponent<CharacterEquipments>()
                    .GetEquipmentInfo();

                var skui = SkillInfoUI.Instance;

                playerData = new PlayerData()
                {
                    EquipmentSlot = equipmentSlots,
                    SkillData = SkillInfoUI.selectedSkillsNumbers
                };

                FirebaseDataManager.Instance.WritePlayerData(playerData);
            }
        }

        
    }
}