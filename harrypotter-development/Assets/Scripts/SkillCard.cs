using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Card", menuName = "SkillCards/SkillCard")]
public class SkillCard : ScriptableObject
{
    public int id;
    public string name;
    public Sprite picture;
    public enum SkillType {Melee, Ranged, Defensive, Ultimate}

    public SkillType skillType;
}
