using System;
using System.Collections;
using System.Collections.Generic;
using Extensions.Skill.UI;
using Firebase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoUI : Instancable<SkillInfoUI>
{
    [SerializeField] private GameObject graphicContainer;
    
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI cooldown;
    [SerializeField] private TextMeshProUGUI description;
    public Image icon;
    public GameObject selectASkill;

    private SkillUIElement _skillUIElement;

    public static List<int> selectedSkillsNumbers = new List<int>();

    public int removingTickNumber;

    private void OnEnable()
    {
        
    }

    public void Open(SkillUIElement skillUIElement)
    {
        _skillUIElement = skillUIElement;
        
        var skillData = skillUIElement.SkillData;

        title.text = skillData.SkillName;
        cooldown.text = "Cooldown: " + skillData.Cooldown.ToString();
        description.text = skillData.Description;
        icon.sprite = SkillManager.Instance.skillImagesCircular[skillData.SkillNumber];
        
        graphicContainer.SetActive(true);
    }

    public void OnTapEquip()
    {
        var selectedSlot = SkillSelectionScreen.Instance.GetSelectedSlot();
        
        selectedSlot.skillUIElement = _skillUIElement;
        
        var skillData = _skillUIElement.SkillData;

        if (selectedSlot == null) return;

        for (int i = 0; i < selectedSkillsNumbers.Count; i++)
        {
            if(skillData.SkillNumber == selectedSkillsNumbers[i])
                return;
        }

        OfflineUIManager OUIM = OfflineUIManager.Instance;

        if (!(skillData.IsPrimary ^ selectedSlot.isPrimary))
        {
            selectedSlot.SetSkill(skillData);
            
            removingTickNumber = selectedSkillsNumbers[OUIM.selectedSkill];
            
            selectedSkillsNumbers[OUIM.selectedSkill] = skillData.SkillNumber;

            _skillUIElement.OnSelect();
            ActionUIManager.Instance.tappedActionImage.transform.GetChild(1).GetComponent<Image>().sprite =
                SkillManager.Instance.skillImagesCircular[skillData.SkillNumber];
            ActionUIManager.Instance.tappedActionImage.GetComponent<SkillSlotUIElement>().imageEquivalentOnEntrance.sprite = 
                SkillManager.Instance.skillImagesCircular[skillData.SkillNumber];

            for (int i = 0; i < SkillSelectionScreen.Instance._skillUIElements.Count; i++)
            {
                if (SkillSelectionScreen.Instance._skillUIElements[i].SkillData.SkillNumber == removingTickNumber)
                    SkillSelectionScreen.Instance._skillUIElements[i].tick.gameObject.SetActive(false);
            }
            
            
            
        }
    }

    public void Close()
    {
        graphicContainer.SetActive(false);
    }
}
