using System.Collections;
using System.Collections.Generic;
using Extensions.TUTORIAL;
using UnityEngine;
using DG.Tweening;

public class OfflineTutorialManager : Instancable<OfflineTutorialManager>
{
    [SerializeField] private RectTransform finger;
    [SerializeField] private List<GameObject> phases = new List<GameObject>();

    [SerializeField] private RectTransform phase0_fingerPos;
    [SerializeField] private RectTransform phase1_fingerPosEquipButton;
    [SerializeField] private RectTransform phase2_closeInventoryButton;
    
    [SerializeField] private RectTransform phase3_openSkillsButtonPos;
    [SerializeField] private RectTransform phase4_selectSkillPos;
    [SerializeField] private RectTransform phase5_primarySkillButtonPos;
    [SerializeField] private RectTransform phase6_equipSkillButtonPos;

    private int activePhase = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        if (TutorialManager.ShowTutorial)
        {
            if (TutorialManager.TutorialStep == 2)
            {
                OfflineUIManager.Instance.OpenOriginalInventory();
                AnimatePhase0();
            }
        }
    }

    //Select item
    void AnimatePhase0()
    {
        activePhase = 0;
        
        phases[0].SetActive(true);

        finger.position = phase0_fingerPos.position;
        
        finger.transform.LookAt(phase0_fingerPos.position);
        var rot = finger.localEulerAngles;
        rot.y = 0;
        rot.z = 135+rot.x;
        rot.x = 0;
        finger.localEulerAngles = rot;
        
        finger.DOJumpAnchorPos(finger.anchoredPosition + (Vector2)finger.forward * 5, 30, 1, 1.5f).SetLoops(-1);
    }

    public void OnSelectItem()
    {
        if (activePhase != 0)
        {
            return;
        }
        
        finger.DOKill();

        phases[0].SetActive(false);
        
        AnimatePhase1();
    }

    //Click equip
    void AnimatePhase1()
    {
        activePhase = 1;

        phases[1].SetActive(true);
        
        finger.position = phase1_fingerPosEquipButton.position;
        
        finger.transform.LookAt(phase1_fingerPosEquipButton.position);
        var rot = finger.localEulerAngles;
        rot.y = 0;
        rot.z = 135+rot.x;
        rot.x = 0;
        finger.localEulerAngles = rot;
        
        finger.DOJumpAnchorPos(finger.anchoredPosition + (Vector2)finger.forward * 5, 30, 1, 1.55f).SetLoops(-1);
    }

    public void OnClickEquip()
    {
        if (activePhase != 1)
        {
            return;
        }
        
        finger.DOKill();
        phases[1].SetActive(false);
        
        AnimatePhase2();
    }
    
    //Close inventory
    void AnimatePhase2()
    {
        activePhase = 2;

        phases[2].SetActive(true);

        finger.position = phase2_closeInventoryButton.position;
        
        finger.transform.LookAt(phase2_closeInventoryButton.position);
        var rot = finger.localEulerAngles;
        rot.y = 0;
        rot.z = 135+rot.x;
        rot.x = 0;
        finger.localEulerAngles = rot;
        
        finger.DOJumpAnchorPos(finger.anchoredPosition + (Vector2)finger.forward * 5, 30, 1, 1.5f).SetLoops(10);
    }

    public void OnClickCloseInventory()
    {
        if (activePhase != 2)
        {
            return;
        }
        
        finger.DOKill();

        phases[2].SetActive(false);
        AnimatePhase3();
    }

    void AnimatePhase3()
    {
        activePhase = 3;

        phases[3].SetActive(true);
        finger.position = phase3_openSkillsButtonPos.position;
        
        finger.transform.LookAt(phase3_openSkillsButtonPos.position);
        var rot = finger.localEulerAngles;
        rot.y = 0;
        rot.z = 135+rot.x;
        rot.x = 0;
        finger.localEulerAngles = rot;
        
        finger.DOJumpAnchorPos(finger.anchoredPosition + (Vector2)finger.forward * 5, 30, 1, 1.5f).SetLoops(10);
    }

    public void OnClickOpenSkills()
    {
        if (activePhase != 3)
        {
            return;
        }
        
        finger.DOKill();

        phases[3].SetActive(false);
        AnimatePhase4();
    }
    
    void AnimatePhase4()
    {
        activePhase = 4;

        phases[4].SetActive(true);
        finger.position = phase4_selectSkillPos.position;
        
        finger.transform.LookAt(phase3_openSkillsButtonPos.position);
        var rot = finger.localEulerAngles;
        rot.y = 0;
        rot.z = 135+rot.x;
        rot.x = 0;
        finger.localEulerAngles = rot;
        
        finger.DOJumpAnchorPos(finger.anchoredPosition + (Vector2)finger.forward * 5, 30, 1, 1.5f).SetLoops(10);
    }

    public void OnSelectPrimarySkill()
    {
        if (activePhase != 4)
        {
            return;
        }
        
        finger.DOKill();

        phases[4].SetActive(false);
        AnimatePhase5();
    }
    
    void AnimatePhase5()
    {
        activePhase = 5;

        phases[5].SetActive(true);
        finger.position = phase5_primarySkillButtonPos.position;
        
        finger.transform.LookAt(phase3_openSkillsButtonPos.position);
        var rot = finger.localEulerAngles;
        rot.y = 0;
        rot.z = 135+rot.x;
        rot.x = 0;
        finger.localEulerAngles = rot;
        
        finger.DOJumpAnchorPos(finger.anchoredPosition + (Vector2)finger.forward * 5, 30, 1, 1.5f).SetLoops(10);
    }

    public void OnTapPrimarySkillJoystick()
    {
        if (activePhase != 5)
        {
            return;
        }
        
        finger.DOKill();

        phases[5].SetActive(false);
        AnimatePhase6();
    }
    
    void AnimatePhase6()
    {
        activePhase = 6;

        phases[6].SetActive(true);
        finger.position = phase6_equipSkillButtonPos.position;
        
        finger.transform.LookAt(phase6_equipSkillButtonPos.position);
        var rot = finger.localEulerAngles;
        rot.y = 0;
        rot.z = 135+rot.x;
        rot.x = 0;
        finger.localEulerAngles = rot;
        
        finger.DOJumpAnchorPos(finger.anchoredPosition + (Vector2)finger.forward * 5, 30, 1, 1.5f).SetLoops(10);
    }
    
    public void OnEquipPrimarySkillJoystick()
    {
        if (activePhase != 6)
        {
            return;
        }
        
        finger.DOKill();

        finger.gameObject.SetActive(false);
        
        phases[6].SetActive(false);

        TutorialManager.TutorialStep = 3;
        
        Configuration.Instance.RegisterPlayButton();
    }
}
