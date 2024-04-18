using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum EquipmentSlot
{
    leftHand, rightHand, leftShoulder, rightShoulder, helmet, _helmet
}

public enum RightHandSword
{
    empty, sword, maceSmall, axeDouble, axeOneSide, maceBig
}

public enum LeftHandSword
{
    empty, bow, shieldA,shieldB,shieldC
}
public class CharacterEquipments : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftShoulder;
    public GameObject rightShoulder;
    public GameObject helmet;
    public GameObject _helmet;

    public GameObject rightHandButton, leftHandButton;

    public Dictionary<EquipmentSlot, GameObject> slotDictionary = new();
    public RightHandSword rightHandSword;
    public LeftHandSword leftHandSword;

    private void Awake()
    {
        slotDictionary.Add(EquipmentSlot.leftHand, leftHand);
        slotDictionary.Add(EquipmentSlot.rightHand, rightHand);
        slotDictionary.Add(EquipmentSlot.leftShoulder, leftShoulder);
        slotDictionary.Add(EquipmentSlot.rightShoulder, rightShoulder);
        slotDictionary.Add(EquipmentSlot.helmet, helmet);
        slotDictionary.Add(EquipmentSlot._helmet, _helmet);
    }

    public void Equip(EquipmentSlot equipmentSlot, int equipmentId)
    {
        EquipToSlot(slotDictionary[equipmentSlot], equipmentId);
        EquipToPanel(equipmentSlot, equipmentId);
        ChangeAnimAndEnum(equipmentId);
    }

    public void ChangeAnimAndEnum(int equipmentId)
    {
        if (!(equipmentId >= 0 && equipmentId <= 8))
            return;
        for (int i = 1; i <= 8; i++)
            GetComponent<Animator>().SetLayerWeight(i,0);
        
        
        switch (equipmentId)
        {
            case 4: // Sword
                if (leftHandSword != LeftHandSword.empty)
                {
                    UnEquip(leftHand,leftHandButton);
                    leftHandSword = LeftHandSword.empty;
                }
                rightHandSword = RightHandSword.sword;
                GetComponent<Animator>().SetLayerWeight(4,100);
                GetComponent<Animator>().SetLayerWeight(8,100);
                break;
            case 5: // Mace Small
                if (leftHandSword == LeftHandSword.bow)
                {
                    UnEquip(leftHand,leftHandButton);
                    leftHandSword = LeftHandSword.empty;
                }
                rightHandSword = RightHandSword.maceSmall;
                GetComponent<Animator>().SetLayerWeight(1,100);
                GetComponent<Animator>().SetLayerWeight(5,100);
                break;
            case 6: // Axe Double
                if (leftHandSword == LeftHandSword.bow)
                {
                    UnEquip(leftHand,leftHandButton);
                    leftHandSword = LeftHandSword.empty;
                }
                rightHandSword = RightHandSword.axeDouble;
                GetComponent<Animator>().SetLayerWeight(1,100);
                GetComponent<Animator>().SetLayerWeight(5,100);
                break;
            case 7: // Axe One Side
                if (leftHandSword == LeftHandSword.bow)
                {
                    UnEquip(leftHand,leftHandButton);
                    leftHandSword = LeftHandSword.empty;

                }
                rightHandSword = RightHandSword.axeOneSide;
                GetComponent<Animator>().SetLayerWeight(1,100);
                GetComponent<Animator>().SetLayerWeight(5,100);
                break;
            case 8: // Mace Big
                if (leftHandSword == LeftHandSword.bow)
                {
                    UnEquip(leftHand,leftHandButton);
                    rightHandSword = RightHandSword.empty;
                }
                rightHandSword = RightHandSword.maceBig;
                GetComponent<Animator>().SetLayerWeight(1,100);
                GetComponent<Animator>().SetLayerWeight(5,100);
                break;
            
            case 0: // Bow
                rightHandSword = RightHandSword.empty;
                leftHandSword = LeftHandSword.bow;
                UnEquip(rightHand,rightHandButton);
                GetComponent<Animator>().SetLayerWeight(3,100);
                GetComponent<Animator>().SetLayerWeight(7,100);
                break;
                
            case 1: // Shield A
                if (rightHandSword == RightHandSword.sword)
                {
                    UnEquip(rightHand,rightHandButton);
                    rightHandSword = RightHandSword.empty;
                }

                leftHandSword = LeftHandSword.shieldA;
                GetComponent<Animator>().SetLayerWeight(1,100);
                GetComponent<Animator>().SetLayerWeight(5,100);
                break;
            
            case 2: // Shield B
                if (rightHandSword == RightHandSword.sword)
                {
                    UnEquip(rightHand,rightHandButton);
                    rightHandSword = RightHandSword.empty;
                }

                leftHandSword = LeftHandSword.shieldB;
                GetComponent<Animator>().SetLayerWeight(1,100);
                GetComponent<Animator>().SetLayerWeight(5,100);
                break;
            
            case 3: // Shield C
                if (rightHandSword == RightHandSword.sword)
                {
                    UnEquip(rightHand,rightHandButton);
                    rightHandSword = RightHandSword.empty;
                }

                leftHandSword = LeftHandSword.shieldC;
                GetComponent<Animator>().SetLayerWeight(1,100);
                GetComponent<Animator>().SetLayerWeight(5,100);
                break;
            
            
        }

        
    }

    public void UnEquip(GameObject parentGameObject, GameObject parentButtonGameObject)
    {
        for (int i = 0; i < parentGameObject.transform.childCount; i++)
        {
            parentGameObject.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "Offline")
        {
            for (int i = 0; i < parentButtonGameObject.transform.childCount; i++)
            {
                parentButtonGameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void EquipCharacter(EquipmentSlot equipmentSlot, int equipmentId)
    {
        EquipToSlot(slotDictionary[equipmentSlot], equipmentId);
        ChangeAnimAndEnum(equipmentId);
    }

    private void EquipToSlot(GameObject slotParent, int equipmentId)
    {
        foreach (var equipment in slotParent.GetComponentsInChildren<EquipmentId>(true))
        {
            equipment.CheckId(equipmentId);

            if (slotParent == helmet)
            {
                foreach (var _equipment in _helmet.GetComponentsInChildren<EquipmentId>(true))
                {
                    _equipment.CheckId(equipmentId);
                }
            }
        }
    }

    private void EquipToPanel(EquipmentSlot equipmentSlot, int equipmentId)
    {
        foreach (var panel in EquipmentManager.Instance.panelDictionary[equipmentSlot].GetComponentsInChildren<EquipmentId>(true))
        {
            panel.CheckIdForPanel(equipmentId);
        }
    }

    public List<EquipmentSlotData> GetEquipmentInfo()
    {
        var equipmentList = new List<EquipmentSlotData>();

        foreach (var slot in slotDictionary.Keys)
        {
            equipmentList.Add(new EquipmentSlotData()
            {
                EquipmentID = GetSlotData(slotDictionary[slot]),
                SlotID = (int) slot
            });
        }

        return equipmentList;
    }

    private int GetSlotData(GameObject slot)
    {
        var data = slot.GetComponentInChildren<EquipmentId>(false);
        return data == null ? -1 : data.id;
    }
}
