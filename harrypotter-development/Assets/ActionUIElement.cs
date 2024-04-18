using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ActionUIElement : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
{
    // public enum ActionUIElementType { Button, Joystick }
    // public ActionUIElementType elementType;
    //
    // public int id;
    // public bool isPointerInside;
    //
    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     isPointerInside = true;
    //     ActionUIManager.Instance.OnPointerDownSkill(id);
    // }
    //
    // public void OnPointerUp(PointerEventData eventData)
    // {
    //     if (!isPointerInside && elementType != ActionUIElement.ActionUIElementType.Joystick)
    //     {
    //         return;
    //     }
    //     ActionUIManager.Instance.OnPointerUpFromSkill(id);
    // }
    //
    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     isPointerInside = false;
    //     if (elementType == ActionUIElement.ActionUIElementType.Joystick)
    //     {
    //         return;
    //     }
    //     ActionUIManager.Instance.OnPointerExitFromSkill(id);
    // }
    public enum ActionUIElementType { Button, Joystick }
    public bool isPointerInside;
    public bool isSkill;
    public int skillId;
    public bool pressedButton;
    public Image cooldown;
    
    private void Update()
    {
        //if (Input.GetMouseButtonDown(0) && isPointerInside)
            //pressedButton = true;
        //if (Input.GetMouseButtonUp(0))
            //pressedButton = false;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (cooldown.fillAmount < 1 || !GetComponent<Joystick>().IsInteractable)
        {
            return;
        }
        isPointerInside = true;
                pressedButton = true;

        CircleInsideCircle CIC = CircleInsideCircle.Instance;
        CIC.joystick = GetComponent<Joystick>();
        //CIC.littleCircle = GetComponent<PracticeLittleCircleHolder>().littleCircle;
        SetSkill();
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPointerInside || !GetComponent<Joystick>().IsInteractable || !pressedButton)
        {
            pressedButton = false;
            return;
        }
        pressedButton = false;

        // PracticeManager PM = PracticeManager.Instance;
        
        //if(!isSkill)
            //AttackAnim
        //else
        //{
          //  PM.skillCasted = true;
            //SkillAnim
        CastSkill();
        //}
    }
    
    public void SetSkill()
    {
        CircleInsideCircle CIC = CircleInsideCircle.Instance;
        SkillData skillData = SkillFetcher._skillDatas[skillId];
        if (cooldown.fillAmount < 1)
            return;
        
        switch (skillId)
        {
            case 3:
                var blitzData = skillData.GetSkillEffectConverted<BlitzData>();
                SkillUsages.Instance.SetBlitz(blitzData,GetComponent<Joystick>());
                break;
            case 8:
                var gougeData = skillData.GetSkillEffectConverted<GougeData>();
                SkillUsages.Instance.SetGouge(gougeData,GetComponent<Joystick>());
                break;
            case 10:
                var idhunData = skillData.GetSkillEffectConverted<IdhunData>();
                SkillUsages.Instance.SetIdhun();
                break;
            case 19:
                var uvardData = skillData.GetSkillEffectConverted<UvardData>();
                SkillUsages.Instance.SetUvard();
                break;
            case 22:
                var gungnirData = skillData.GetSkillEffectConverted<GungnirData>();
                SkillUsages.Instance.SetGungnir(gungnirData);
                CIC.skillUsing = true;
                break;
            case 15:
                var mjollnirData = skillData.GetSkillEffectConverted<MjollnirData>();
                SkillUsages.Instance.SetMjollnir(mjollnirData);
                CIC.skillUsing = true;
                break;
            case 18:
                var temblorData = skillData.GetSkillEffectConverted<TemblorData>();
                SkillUsages.Instance.SetTemblor(temblorData,GetComponent<Joystick>());
                break;
            //case 19:
                // var uvardData = skillData.GetSkillEffectConverted<UvardData>();
                // PracticeSkills.Instance.SetUvard(uvardData);
                //break;
        }
    }
    public void CastSkill()
    {
        SkillData skillData = SkillFetcher._skillDatas[skillId];
        PracticeManager PM = PracticeManager.Instance;
        CircleInsideCircle CIC = CircleInsideCircle.Instance;
        if (cooldown.fillAmount < 1)
            return;
        
        cooldown.fillAmount = 0;

        var cooldownColor = cooldown.color;
        cooldownColor.a = .7f;
        cooldown.color = cooldownColor;
        
        switch (skillId)
        {
            case 3:
                var blitzData = skillData.GetSkillEffectConverted<BlitzData>();
                SkillUsages.Instance.CastBlitz(blitzData);
                ReloadSkill(blitzData.Cooldown);
                Character.myCharacter.animationManager.SetAnimationStateCMD(CharacterAnimationState.Blitz);
                break;
            case 5:
                var cigonidData = skillData.GetSkillEffectConverted<CigonidData>();
                SkillUsages.Instance.CastCigonid(cigonidData);
                ReloadSkill(cigonidData.Cooldown);
                Character.myCharacter.animationManager.SetAnimationStateCMD(CharacterAnimationState.Skill);
                break;
            case 8:
                var gougeData = skillData.GetSkillEffectConverted<GougeData>();
                if(Character.myCharacter.GetComponent<CharacterMovement>().model.GetComponent<CharacterEquipments>().rightHandSword == RightHandSword.sword)
                    StartCoroutine(Character.myCharacter.actionManager.weaponParticleController.ActivateForSeconds(0.25f,1));
                else
                    StartCoroutine(Character.myCharacter.actionManager.weaponParticleController.ActivateForSeconds(0,1));
                SkillUsages.Instance.CastGouge(gougeData);
                ReloadSkill(gougeData.Cooldown);
                Character.myCharacter.animationManager.SetAnimationStateCMD(CharacterAnimationState.Gouge);
                break;
            case 10:
                var idhunData = skillData.GetSkillEffectConverted<IdhunData>();
                SkillUsages.Instance.CastIdhun();
                ReloadSkill(idhunData.Cooldown);
                Character.myCharacter.animationManager.SetAnimationStateCMD(CharacterAnimationState.Idhun);
                break;
            case 15:
                var mjollnirData = skillData.GetSkillEffectConverted<MjollnirData>();
                SkillUsages.Instance.CastMjollnir(mjollnirData);
                ReloadSkill(mjollnirData.Cooldown);
                Character.myCharacter.animationManager.SetAnimationStateCMD(CharacterAnimationState.Skill);
                CIC.skillOn = true;
                break;
            case 18:
                var temblorData = skillData.GetSkillEffectConverted<TemblorData>();
                if(Character.myCharacter.GetComponent<CharacterMovement>().model.GetComponent<CharacterEquipments>().rightHandSword == RightHandSword.sword)
                    StartCoroutine(Character.myCharacter.actionManager.weaponParticleController.ActivateForSeconds(0.25f,1));
                else
                    StartCoroutine(Character.myCharacter.actionManager.weaponParticleController.ActivateForSeconds(0,1));
                SkillUsages.Instance.CastTemblor(temblorData);
                ReloadSkill(temblorData.Cooldown);
                Character.myCharacter.animationManager.SetAnimationStateCMD(CharacterAnimationState.Temblor);
                break;
            case 19:
                var uvardData = skillData.GetSkillEffectConverted<UvardData>();
                SkillUsages.Instance.CastUvard();
                ReloadSkill(uvardData.Cooldown);
                Character.myCharacter.animationManager.SetAnimationStateCMD(CharacterAnimationState.Uvard);
                break;
            case 21:
                var zarnData = skillData.GetSkillEffectConverted<ZarnData>();
                SkillUsages.Instance.CastZarn(zarnData);
                ReloadSkill(zarnData.Cooldown);
                Character.myCharacter.animationManager.SetAnimationStateCMD(CharacterAnimationState.Skill);
                break;
            case 22:
                //PM.practiceCharacter.GetComponent<PracticeAnimations>().Skill();
                var gungnirData = skillData.GetSkillEffectConverted<GungnirData>();
                SkillUsages.Instance.CastGungnir(gungnirData);
                ReloadSkill(gungnirData.Cooldown);
                Character.myCharacter.animationManager.SetAnimationStateCMD(CharacterAnimationState.Skill);
                CIC.skillOn = true;
                break;
        }
        
    }

    public void ReloadSkill(float time)
    {
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

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
