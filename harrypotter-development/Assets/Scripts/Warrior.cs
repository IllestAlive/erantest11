using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Warrior", menuName = "Warriors/Warrior")]
public class Warrior : ScriptableObject
{
    public int id;
    public string nickname;
    public string name;
    public int level;
    public string description;
}
