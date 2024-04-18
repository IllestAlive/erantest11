using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager : Instancable<ActionUIManager>
{
    [SerializeField] private Image[] actionImages;
    private int indexOfSelected = -1;

    [SerializeField] private float scaleMultipler = 1.2f;

    public GameObject tappedActionImage;
    

    public void SetActionImageFillRatio(int index, float val, float maxVal)
    {
        actionImages[index].fillAmount = (maxVal - val) / maxVal;
    }

    public void SelectSkill(int index)
    {
        if (indexOfSelected != index)
        {
            if (indexOfSelected != -1)
            {
                actionImages[indexOfSelected].transform.localScale = Vector3.one;
            }

            actionImages[index].transform.localScale = Vector3.one * scaleMultipler;

            indexOfSelected = index;
        }
    }

    public void SelectOrDeselectIndependentSkill(int index, bool setEnable)
    {
        actionImages[index].transform.localScale = Vector3.one * (setEnable ? scaleMultipler : 1);
    }

    public void OnPointerDownSkill(int id)
    {
        Character.myCharacter.actionManager.SetSkillGraphic(id, true);
    }

    public void OnPointerExitFromSkill(int id)
    {
        Character.myCharacter.actionManager.SetSkillGraphic(id, false);
    }

    public void OnPointerUpFromSkill(int id)
    {
        Character.myCharacter.actionManager.CastSkill(id);
        //Character.myCharacter.actionManager.UseAction(id);
        Character.myCharacter.actionManager.SetSkillGraphic(id, false);
    }
}
