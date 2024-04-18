using System.Collections;
using System.Collections.Generic;
using Extensions.Skill.UI;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUIElement : MonoBehaviour
{
    private SkillData _currentSelected;
    public bool isPrimary;
    public int index;
    public Image imageEquivalentOnEntrance;
    public GameObject shine;
    public SkillUIElement skillUIElement;
    public void SetSkill(SkillData newSelected)
    {
        _currentSelected = newSelected;
        Debug.Log($"Set skill: {_currentSelected.SkillId} for slot {index}");

        SkillFetcher._chosenSkills[index] = _currentSelected;
    }
    public void OnTap()
    {
        OfflineUIManager OUIM = OfflineUIManager.Instance;
        OUIM.selectedSkill = index;
        Debug.Log("tap slot");
        SkillSelectionScreen.Instance.SetSelectedSlot(this);
        ActionUIManager.Instance.tappedActionImage = gameObject;

        for (int i = 0; i < SkillManager.Instance.shines.Count; i++)
        {
            SkillManager.Instance.shines[i].SetActive(false);
        }
        shine.SetActive(true);

        if(skillUIElement != null)
            SkillInfoUI.Instance.Open(skillUIElement);

    }
}