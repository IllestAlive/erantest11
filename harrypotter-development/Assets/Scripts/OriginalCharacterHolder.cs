using System;
using System.Collections;
using System.Collections.Generic;
using Extensions.TUTORIAL;
using UnityEngine;
using Firebase;

public class OriginalCharacterHolder : Instancable<OriginalCharacterHolder>
{
    public GameObject originalCharacter;
    public GameObject activeCommander => originalCharacter;

    private IEnumerator Start()
    {
        FirebaseDataManager FDM = FirebaseDataManager.Instance;
        yield return new WaitUntil(() => FDM.MyPlayerData != null);

        // if (FDM.MyPlayerData.CommanderNFT == null)
        //     yield break;

        if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.OriginalCharacters)
        {
            var equipments = activeCommander.GetComponent<CharacterEquipments>();

            foreach (var data in FDM.MyPlayerData.EquipmentSlot)
            {
                if (data.EquipmentID == -1)
                    continue;
                equipments.Equip((EquipmentSlot)data.SlotID, data.EquipmentID);

            }
        }
    }

    public void Equip()
    {
        originalCharacter.GetComponent<CharacterEquipments>().Equip(EquipmentManager.Instance.openPanel,
            EquipmentManager.Instance.selectedEquipmentId);

        if (TutorialManager.ShowTutorial)
        {
            OfflineTutorialManager.Instance.OnClickEquip();
        }
    }
}
