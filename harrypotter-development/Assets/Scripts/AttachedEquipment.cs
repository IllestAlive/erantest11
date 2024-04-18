using System;
using System.Collections;
using System.Collections.Generic;
using Extensions.TUTORIAL;
using UnityEngine;
using UnityEngine.EventSystems;


public class AttachedEquipment : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Equipment equipment;
    public GameObject selectedVersion;
    public ItemEquipment itemEquipment;
    

    public void VisualizeEquipment()
    {
        // if (OfflineUIManager.Instance.randomizeButton.activeSelf)
        //     return;

        if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.WithoutCustomizedCharacters)
        {
            EquipmentManager EM = EquipmentManager.Instance;
            for (int i = 0; i < EM.equipmentImages.Count; i++)
            {
                EM.equipmentImages[i].gameObject.SetActive(false);
            }

            EM.equipmentImages[equipment.visualizeWeapon].gameObject.SetActive(true);
            EM.name.text = equipment.name;
            EM.level.text = "Level: " + equipment.level;
            EM.specialName.text = equipment.specialName;
            EM.rarity.text = equipment.rarity;
            EM.attackFill.transform.localScale = new Vector3(equipment.attack / 10f, 1, 1);
            EM.defenseFill.transform.localScale = new Vector3(equipment.defense / 10f, 1, 1);
            EM.dodgeFill.transform.localScale = new Vector3(equipment.dodge / 10f, 1, 1);
            EM.specialName.color = equipment.color;
            EM.rarity.color = equipment.color;


            EM.selectedEquipmentId = equipment.visualizeWeapon;

            Shine();

            // EquipmentManager.Instance.commanderNft.GetComponent<CharacterEquipments>().Equip(EquipmentManager.Instance.openPanel, equipment.visualizeWeapon);
        }

        if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.OriginalCharacters)
        {
            EquipmentManager EM = EquipmentManager.Instance;
            // for (int i = 0; i < EM.equipmentImagesOriginal.Count; i++)
            // {
            //     EM.equipmentImagesOriginal[i].gameObject.SetActive(false);
            // }
            //
            // EM.equipmentImagesOriginal[equipment.visualizeWeapon].gameObject.SetActive(true);
            // EM.nameOriginal.text = equipment.name;
            // EM.levelOriginal.text = "Level: " + equipment.level;
            // EM.specialNameOriginal.text = equipment.specialName;
            // EM.rarityOriginal.text = equipment.rarity;
            // EM.attackFillOriginal.transform.localScale = new Vector3(equipment.attack / 10f, 1, 1);
            // EM.defenseFillOriginal.transform.localScale = new Vector3(equipment.defense / 10f, 1, 1);
            // EM.dodgeFillOriginal.transform.localScale = new Vector3(equipment.dodge / 10f, 1, 1);
            // EM.specialNameOriginal.color = equipment.color;
            // EM.rarityOriginal.color = equipment.color;


            EM.selectedEquipmentId = equipment.visualizeWeapon;

            // Shine();
        }

        if (TutorialManager.ShowTutorial)
        {
            OfflineTutorialManager.Instance.OnSelectItem();
        }
    }

    

    public void Shine()
    {
        for (int i = 0; i < EquipmentManager.Instance.selectedVersionsOfTheEquipments.Count; i++)
        {
            EquipmentManager.Instance.selectedVersionsOfTheEquipments[i].SetActive(false);
        }
        selectedVersion.SetActive(true);
        // if (OfflineUIManager.Instance.randomizeButton.activeSelf)
        //     return;
        //
        // EquipmentManager EM = EquipmentManager.Instance;
        //
        // for (int i = 0; i < EM.equipmentButtons.Count; i++)
        // {
        //     EM.equipmentButtons[i].transform.GetChild(0).gameObject.SetActive(false);
        // }
        //
        // transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        NewOfflineUIManager NOUIM = NewOfflineUIManager.Instance;
        NOUIM.infoEquipment.SetActive(true);
        
        NOUIM.nameOfWeapon.text = itemEquipment.nameOfWeapon;
        NOUIM.damage.text = itemEquipment.damage.ToString();
        NOUIM.score.text = itemEquipment.score.ToString();
        NOUIM.weaponOfImage.sprite = itemEquipment.imageOfWeapon;

        NOUIM.attributes.text = String.Empty;
        NOUIM.magic.text = String.Empty;
        
        for (int i = 0; i < itemEquipment.attributes.Count; i++)
        {
            NOUIM.attributes.text += itemEquipment.attributes[i] + "\n";
        }

        for (int i = 0; i < itemEquipment.magic.Count; i++)
        {
            NOUIM.magic.text += itemEquipment.magic[i] + "\n";
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NewOfflineUIManager NOUIM = NewOfflineUIManager.Instance;
        NOUIM.infoEquipment.SetActive(false);
    }
}