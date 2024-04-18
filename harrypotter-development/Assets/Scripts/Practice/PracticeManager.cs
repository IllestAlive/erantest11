using System;
using System.Collections;
using System.Collections.Generic;
using Extensions.TUTORIAL;
using UnityEngine;
using Firebase;
using TMPro;
using UnityEngine.UI;

public class PracticeManager : Instancable<PracticeManager>
{
    public GameObject model;
    public GameObject practiceCharacter;
    public float speedCharacter;
    public float savedSpeed;
    public Joystick skillJoystick;
    public bool skillCasted;
    public bool primarySkillCasted;

    public GameObject tutorialHealthCollectibles;
    
    private IEnumerator Start()
    {
        if (TutorialManager.ShowTutorial)
        {
            ShowHealthCollectibles();
        }
        
        FirebaseDataManager FDM = FirebaseDataManager.Instance;
        yield return new WaitUntil(() => FDM.MyPlayerData != null);
        var equipments = model.GetComponent<CharacterEquipments>();
        foreach (var data in FDM.MyPlayerData.EquipmentSlot)
        {
            if (data.EquipmentID == -1)
                continue;
            equipments.EquipCharacter((EquipmentSlot)data.SlotID, data.EquipmentID);
        }
    }

    void ShowHealthCollectibles()
    {
        tutorialHealthCollectibles.SetActive(true);
    }
    
}
