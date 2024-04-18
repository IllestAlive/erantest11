using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Equipment/New Equipment")]
public class Equipment : ScriptableObject
{
    public int visualizeWeapon;
    public string name;
    public int level;
    public string specialName;
    public string rarity;
    public int attack;
    public int defense;
    public int dodge;
    public Color color;
}
