using System;
using System.Collections.Generic;
using DG.Tweening;
using Extensions;
using UnityEngine;

public class SoundManager : Instancable<SoundManager>
{
    [SerializeField] private AudioSource source;
    [SerializeField] private List<SFXItem> sfxItems;
    private Dictionary<SoundCategory, AudioSource> longAudioSources = new();
    
    public void DoSFX(SoundCategory category, float volume = 1)
    {
        return;
        PlayAudio(ParseSFX(category), volume);
    }
    
    public void DelayedSFX(SoundCategory category, float delay, float volume = 1)
    {
        return;
        DOVirtual.DelayedCall(delay, () => DoSFX(category, volume));
    }
    
    private void PlayAudio(AudioClip clip, float volume = 1)
    {
        return;
        if (clip != null)
        {
            source.PlayOneShot(clip, volume);
        }
    }
    
    public void DoLongAudio(SoundCategory category, bool restart = false, bool loop = false, float volume = 1)
    {
        return;
        if (longAudioSources.ContainsKey(category))
        {
            if (restart)
            {
                KillLongAudio(category);
            }
        }
        else
        {
            var newSource = gameObject.AddComponent<AudioSource>();
            longAudioSources.Add(category, newSource);
        }

        if (!restart && longAudioSources[category].isPlaying) return; 
        
        PlayLongAudio(ParseSFX(category), longAudioSources[category], loop, volume);
    }

    public void KillLongAudio(SoundCategory category)
    {
        return;
        if(longAudioSources.ContainsKey(category)) longAudioSources[category].Stop();
    }
    
    public void PlayLongAudio(AudioClip clip, AudioSource source, bool loop = false, float volume = 1)
    {
        return;
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
        source.Play();
    }

    private AudioClip ParseSFX(SoundCategory category)
    {
        foreach (var sfx in sfxItems)
        {
            if (sfx.soundCategory == category)
            {
                var clips = sfx.audioClips;

                return clips.PickRandom();
            }
        }
        return null;
    }

    public static SoundCategory PickRandomCategory(params SoundCategory[] categories)
    {
        return categories.PickRandom();
    }
}

public enum SoundCategory
{
    Death,
    Spawn,
    Heal,
    Regenerate,
    Walking,
    
    FleshHitReceive,
    ArmorHitReceive,
    
    PainGroan,
    PainGroanFemale,
    EffortGroan,
    EffortGroanFemale,
    
    BluntMeleeSwing,
    LightMeleeSwing,
    HeavyMeleeSwing,
    
    BluntMeleeHit,
    LightMeleeHit,
    HeavyMeleeHit,
    
    ShieldBlock,
    WeaponBlock,
    
    MetalProjectileSpawn,
    MetalProjectileSwoosh,
    MetalProjectileHit,
    
    MagicProjectileSpawn,
    MagicProjectileSwoosh,
    MagicProjectileHit,
    
    SpeedPowerUp,
    HealthPowerUp,
}

[Serializable]
public class SFXItem
{
    public SoundCategory soundCategory;
    public List<AudioClip> audioClips;
}