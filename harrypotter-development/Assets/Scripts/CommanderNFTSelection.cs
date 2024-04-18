using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using UnityEngine;

public class CommanderNFTSelection : Instancable<CommanderNFTSelection>
{
    public List<GameObject> commanderNftList; //Non-customized characters.
    public int activeId;
    public GameObject activeCommander => commanderNftList[activeId];

    private IEnumerator Start()
    {
        FirebaseDataManager FDM = FirebaseDataManager.Instance;
        yield return new WaitUntil(() => FDM.MyPlayerData != null);

        if (FDM.MyPlayerData.CommanderNFT == null)
            yield break;

        if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.WithoutCustomizedCharacters)
        {
            int fireBaseActiveCommanderId = FDM.MyPlayerData.CommanderNFT.CommanderID;
            SelectCommander(fireBaseActiveCommanderId);
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
        commanderNftList[activeId].GetComponent<CharacterEquipments>().Equip(EquipmentManager.Instance.openPanel,
            EquipmentManager.Instance.selectedEquipmentId);
    }
    
    private void OnDestroy()
    {
        PlayerPrefs.SetInt("CharIdCommander",activeId);
    }

    public void SelectCommander(int id)
    {
        for (int j = 0; j < commanderNftList.Count; j++)
        {
            commanderNftList[j].SetActive(false);
        }
        commanderNftList[id].SetActive(true);

        activeId = id;
        OfflineUIManager.Instance.randomizeButton.SetActive(false);
    }
}
