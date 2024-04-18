using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillType : MonoBehaviour
{
    public enum TypeOfSkill{throwable,zone}
    public enum SkillDataType
    {
        Magic, //0
        Bow, //1
        Sword, //2
        Shield, //3
        NotUsing //4
    }

    public TypeOfSkill typeOfSkill;
    public TypeOfSkill skillDataType;
    public string nameOfSkill;
    public int damageAmount;
}
