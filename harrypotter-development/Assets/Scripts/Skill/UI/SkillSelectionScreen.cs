using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions.Skill.UI;
using Firebase;
using UnityEngine;

public class SkillSelectionScreen : Instancable<SkillSelectionScreen>
{
    [SerializeField] private GameObject panel;
    
    [SerializeField] private CharacterEquipments characterEquipments;
    
    [SerializeField] private SkillUIElement skillUIElementPrefab;

    [SerializeField] private RectTransform primarySkillsTransform;
    [SerializeField] private RectTransform skillsTransform;

    private RightHandSword _rightHandSword;
    private LeftHandSword _leftHandSword;

    public List<SkillUIElement> _skillUIElements = new List<SkillUIElement>();

    private SkillSlotUIElement _selectedSkillSlotUIElement;
    

    internal SkillSlotUIElement GetSelectedSlot()
    {
        return _selectedSkillSlotUIElement;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Open();
        }
    }

    public void Open()
    {
        panel.SetActive(true);
        
        _rightHandSword = characterEquipments.rightHandSword;
        _leftHandSword = characterEquipments.leftHandSword;


        //It can only be SWORD
        if (_rightHandSword != RightHandSword.empty)
        {
            if (_leftHandSword >= LeftHandSword.shieldA)
            {
                FillSkills(SkillType.SkillDataType.Shield, SkillType.SkillDataType.Sword);
            }
            else //No Shield, ONLY Sword
            {
                FillSkills(SkillType.SkillDataType.Sword);
            }
        }
        else if (_leftHandSword == LeftHandSword.bow)
        {
            FillSkills(SkillType.SkillDataType.Bow);
        }
    }

    //Magic skills will always be added.
    //Secondary skill types (Melee, Bow, Shield) depends on the weapons we use
    private void FillSkills(params SkillType.SkillDataType[] skillFillTypes)
    {
        foreach (var skillUIElement in _skillUIElements)
        {
            Destroy(skillUIElement.gameObject);
        }
        
        _skillUIElements.Clear();
        
        var skillDatas = SkillFetcher.GetSkillDatas().Where(x => skillFillTypes.Contains(x.SkillType) || x.SkillType == SkillType.SkillDataType.Magic);
        
        if (skillFillTypes.Length != 0)
        {
            foreach (var skillData in skillDatas)
            {
                SkillUIElement skillUIElement = null;
                
                skillUIElement = Instantiate(skillUIElementPrefab, skillData.IsPrimary ? primarySkillsTransform : skillsTransform);

                skillUIElement.Init(skillData);
                    
                _skillUIElements.Add(skillUIElement);
            }
        }
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void SetSelectedSlot(SkillSlotUIElement skillSlotUIElement)
    {
        _selectedSkillSlotUIElement = skillSlotUIElement;
        
    }
}
