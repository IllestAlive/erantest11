using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemEquipment", menuName = "Equipment/Item Equipment")]
public class ItemEquipment : ScriptableObject
{
    public string nameOfWeapon;
    public Sprite imageOfWeapon;
    public int damage;
    public int score;
    public List<string> attributes;
    public List<string> magic;
}