using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


/// <summary>
/// Skill casting logic will be here
/// </summary>
public class CharacterSkillManager : NetworkBehaviour
{
    private List<SkillData> _skillDatas;

    internal void GetSkillsFromSkillFetcher()
    {
        _skillDatas = SkillFetcher.GetSkillDatas();
    }
}
