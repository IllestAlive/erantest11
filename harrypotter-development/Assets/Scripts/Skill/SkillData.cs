using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Mirror;
using Newtonsoft.Json;
using UnityEngine;

public enum SkillName
{
    Rualwish,
    Gungnir
}

public static class SkillNames
{
    public static string GetSkillID(this SkillName skillName)
    {
        return skillName.ToString().ToLower();
    }
}

[FirestoreData]
public class SkillData
{
    [FirestoreProperty] public SkillType.SkillDataType SkillType { get; set; }
    
    [FirestoreProperty] public bool IsPrimary { get; set; }

    [FirestoreProperty] public string SkillId { get; set; }

    [FirestoreProperty] public string SkillName { get; set; }
    
    [FirestoreProperty] public string Description { get; set; }

    [FirestoreProperty] public float Cooldown { get; set; }
    
    [FirestoreProperty] public object SkillEffect { get; set; }
    
    [FirestoreProperty] public int SkillNumber { get; set; }
    
    public T GetSkillEffectConverted<T>()
    {
        var serializedParticipant = JsonConvert.SerializeObject(SkillEffect);
        SkillEffect = JsonConvert.DeserializeObject<T>(serializedParticipant);
        return (T)SkillEffect;
    }
}

[FirestoreData]
public class AegishjalmurData
{
    [FirestoreProperty] public int DmgReduction { get; set; }
    [FirestoreProperty] public float Duration { get; set; }
    [FirestoreProperty] public int Range { get; set; }

}

[FirestoreData]
public class ZarnData
{
    [FirestoreProperty] public float Cooldown { get; set; }
    [FirestoreProperty] public float Duration { get; set; }
    [FirestoreProperty] public int MovementSpeedBonus { get; set; }
}

[FirestoreData]
public class RualwishData
{
    [FirestoreProperty] public int StunTime { get; set; }
    [FirestoreProperty] public int Range { get; set; }
}

[FirestoreData]
public class GungnirData
{
    [FirestoreProperty] public float Cooldown { get; set; }
    [FirestoreProperty] public float Area { get; set; }
    [FirestoreProperty] public float Delay { get; set; }
    [FirestoreProperty] public int DmgAmount { get; set; }
    [FirestoreProperty] public int Range { get; set; }
    [FirestoreProperty] public int RootToPlaceTime { get; set; }
    
}

[FirestoreData]
public class IgowucData
{
    public int DodgeChance;
}

[FirestoreData]
public class CigonidData
{
    [FirestoreProperty] public float Cooldown { get; set; }
    public int Duration;
    public int Heal;
    public int HealMultiplierFromPotions;
}

[FirestoreData]
public class MjollnirData
{
    [FirestoreProperty] public float Cooldown { get; set; }
    public float Area;
    public float Delay;
    public int DmgAmount1;
    public int DmgAmount2;
    public int Range;
    public float StunDuration1;
    public float StunDuration2;
    public float TimeApart;
}

[FirestoreData]
public class GougeData
{
    [FirestoreProperty] public float Cooldown { get; set; }
    public int DmgAmount;
    public int Range;
}

[FirestoreData]
public class TemblorData
{
    [FirestoreProperty] public float Cooldown { get; set; }
    public int DmgAmount;
    public int Range;
}

public class IdhunData
{
    [FirestoreProperty] public float Cooldown { get; set; }
    public int ArrowSpeed;
    public bool CanMove;
    public int DmgAmount;
    public int MovementSpeedReduction;
    public int Range;
}

public class UvardData
{
    [FirestoreProperty] public float Cooldown { get; set; }
    public int ArrowSpeed;
    public bool CanMove;
    public int DmgAmount;
    public int Range;
}

public class BlitzData
{
    [FirestoreProperty] public float Cooldown { get; set; }
}

public static class AegishjalmurDataSync
{
    public static void WriteTechnologyData(this NetworkWriter writer, AegishjalmurData techData)
    {
        writer.WriteFloat(techData.Duration);
        writer.WriteInt(techData.Range);
        writer.WriteInt(techData.DmgReduction);
    }

    public static AegishjalmurData ReadTechnologyData(this NetworkReader reader)
    {
        return new AegishjalmurData()
        {
            Duration = reader.ReadFloat(),
            Range = reader.ReadInt(),
            DmgReduction = reader.ReadInt(),
        };
    }
}
