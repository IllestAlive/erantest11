using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PracticeActionUIElement : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
{
    public enum ActionUIElementType { Button, Joystick }
    public bool isPointerInside;
    public bool isSkill;
    public int skillId;
    public bool pressedButton;
    public Image cooldown;

    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isPointerInside)
            pressedButton = true;
        if (Input.GetMouseButtonUp(0))
            pressedButton = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerInside = true;
        PracticeCircleInsideCircle PCIC = PracticeCircleInsideCircle.Instance;
        PCIC.joystick = GetComponent<Joystick>();
        //PCIC.littleCircle = GetComponent<PracticeLittleCircleHolder>().littleCircle;
        SetSkill();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // if (!isPointerInside)
        //     return;

        PracticeManager PM = PracticeManager.Instance;
        
        if(!isSkill)
            PM.practiceCharacter.GetComponent<PracticeAnimations>().Attack();
        else
        {
            PM.skillCasted = true;
            //PM.practiceCharacter.GetComponent<PracticeAnimations>().Skill();
            CastSkill();
        }
        
        PracticeTutorialManager.Instance.OnTapSkillJoystick();
    }

    public void SetSkill()
    {
        SkillData skillData = SkillFetcher._skillDatas[skillId];
        switch (skillId)
        {
            case 22:
                var gungnirData = skillData.GetSkillEffectConverted<GungnirData>();
                PracticeSkills.Instance.SetGungnir(gungnirData);
                break;
            case 15:
                var mjollnirData = skillData.GetSkillEffectConverted<MjollnirData>();
                PracticeSkills.Instance.SetMjollnir(mjollnirData);
                break;
            case 8:
                var gougeData = skillData.GetSkillEffectConverted<GougeData>();
                PracticeSkills.Instance.SetGouge(gougeData,GetComponent<Joystick>());
                break;
            case 18:
                var temblorData = skillData.GetSkillEffectConverted<TemblorData>();
                PracticeSkills.Instance.SetTemblor(temblorData,GetComponent<Joystick>());
                break;
            case 10:
                var idhunData = skillData.GetSkillEffectConverted<IdhunData>();
                PracticeSkills.Instance.SetIdhun(idhunData);
                break;
            case 19:
                var uvardData = skillData.GetSkillEffectConverted<UvardData>();
                PracticeSkills.Instance.SetUvard(uvardData);
                break;
            case 3:
                var blitzData = skillData.GetSkillEffectConverted<BlitzData>();
                PracticeSkills.Instance.SetBlitz(blitzData,GetComponent<Joystick>());
                break;
        }
    }
    public void CastSkill()
    {
        SkillData skillData = SkillFetcher._skillDatas[skillId];
        PracticeManager PM = PracticeManager.Instance;
        switch (skillId)
        {
            case 22:
                PM.practiceCharacter.GetComponent<PracticeAnimations>().Skill();
                var gungnirData = skillData.GetSkillEffectConverted<GungnirData>();
                PracticeSkills.Instance.CastGungnir(gungnirData);
                ReloadSkill(gungnirData.Cooldown);
                break;
            case 15:
                PM.practiceCharacter.GetComponent<PracticeAnimations>().Skill();
                var mjollnirData = skillData.GetSkillEffectConverted<MjollnirData>();
                PracticeSkills.Instance.CastMjollnir(mjollnirData);
                ReloadSkill(mjollnirData.Cooldown);
                break;
            case 8:
                var gougeData = skillData.GetSkillEffectConverted<GougeData>();
                PracticeSkills.Instance.CastGouge(gougeData);
                ReloadSkill(gougeData.Cooldown);
                break;
            case 5:
                var cigonidData = skillData.GetSkillEffectConverted<CigonidData>();
                PracticeSkills.Instance.CastCigonid(cigonidData);
                ReloadSkill(cigonidData.Cooldown);
                break;
            case 18:
                var temblorData = skillData.GetSkillEffectConverted<TemblorData>();
                PracticeSkills.Instance.CastTemblor(temblorData);
                ReloadSkill(temblorData.Cooldown);
                break;
            case 10:
                var idhunData = skillData.GetSkillEffectConverted<IdhunData>();
                PracticeSkills.Instance.CastIdhun(idhunData);
                ReloadSkill(idhunData.Cooldown);
                break;
            case 19:
                var uvardData = skillData.GetSkillEffectConverted<UvardData>();
                PracticeSkills.Instance.CastUvard(uvardData);
                ReloadSkill(uvardData.Cooldown);
                break;
            case 21:
                var zarnData = skillData.GetSkillEffectConverted<ZarnData>();
                PracticeSkills.Instance.CastZarn(zarnData);
                ReloadSkill(zarnData.Cooldown);
                break;
            case 3:
                var blitzData = skillData.GetSkillEffectConverted<BlitzData>();
                PracticeSkills.Instance.CastBlitz(blitzData);
                ReloadSkill(blitzData.Cooldown);
                break;
        }
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!pressedButton)
            isPointerInside = false;
    }
    
    public void ReloadSkill(float time)
    {
        time /= 3f;
        float reload = 0;
        GetComponent<Joystick>().IsInteractable = false;
        DOTween.To(() => reload, x => reload = x, 1, time)
            .OnUpdate(() =>
            {
                cooldown.fillAmount = reload;
            }).OnComplete(() =>
            {
                var cooldownColor = cooldown.color;
                cooldownColor.a = 1;
                cooldown.color = cooldownColor;
                GetComponent<Joystick>().MakeItInteractableQueue = true;
            });
    }
}
