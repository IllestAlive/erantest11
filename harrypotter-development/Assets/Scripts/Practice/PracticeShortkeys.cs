using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeShortkeys : MonoBehaviour
{
    private void Update()
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeCharacter PC = PM.practiceCharacter.GetComponent<PracticeCharacter>();
        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
        {
            PC.TakeDamage(10f);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            PC.Heal(10f);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            SkillData skillData = SkillFetcher._skillDatas[21]; //Zarn
            PM.practiceCharacter.GetComponent<PracticeAnimations>().Skill();
            var zarnData = skillData.GetSkillEffectConverted<ZarnData>();
            PracticeSkills.Instance.CastZarn(zarnData);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            SkillData skillData = SkillFetcher._skillDatas[11]; //Igowuc
            PM.practiceCharacter.GetComponent<PracticeAnimations>().Skill();
            var igowucData = skillData.GetSkillEffectConverted<IgowucData>();
            PracticeSkills.Instance.CastIgowuc(igowucData);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            SkillData skillData = SkillFetcher._skillDatas[5]; //Cigonid
            PM.practiceCharacter.GetComponent<PracticeAnimations>().Skill();
            var cigonidData = skillData.GetSkillEffectConverted<CigonidData>();
            PracticeSkills.Instance.CastCigonid(cigonidData);
        }
    }
}
