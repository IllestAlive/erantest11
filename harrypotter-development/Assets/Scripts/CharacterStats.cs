using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using DG.Tweening;
using Extensions;
using Random = System.Random;

public class CharacterStats : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnReceiveHealthFromServer))] public int health;
    public TextMeshProUGUI healthText;
    public int healUpMultiplier = 1;
    public int dodge = 0;
    

    public void OnReceiveHealthFromServer(int _old, int _new)
    {
        if (hasAuthority)
        {
            if(_new < _old)
                VFXManager.Instance.DamageVFX(_old - _new, Character.myCharacter);
            
            HealthUIManager.Instance.playerHB.value = _new / 100.0f;
            // healthText.text = _new.ToString();
        }
        else
        {
            if(_new < _old)
                VFXManager.Instance.DamageVFX(_old - _new, Character.opponentCharacter);
            
            HealthUIManager.Instance.opponentHB.value = _new / 100.0f; 
            // healthText.text = _new.ToString(); 
            UIManager.Instance.opponentHp.value = _new / 100.0f; 
            UIManager.Instance.opponentHpText.text = _new.ToString() + "/100";
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        health = 100;
    }

    [Command]
    public void HealUpCmd(int healAmount)
    {
        ChangeHealth(healAmount,HealthChangeReason.Heal);
    }

    [Command]
    public void ChangeHealUpMultiplier(int multiplyAmount, int changeBackToNormal)
    {
        healUpMultiplier = multiplyAmount;

        StartCoroutine(ChangeBackToNormal());
        IEnumerator ChangeBackToNormal()
        {
            yield return new WaitForSeconds(changeBackToNormal);
            healUpMultiplier = 1;
        }
    }
    
    [Server]
    public void ChangeHealth(int _amount, HealthChangeReason changeReason)
    {
        if (health <= 0 && _amount >= 0)
        {
            return;
        }
        if(health > 99.5 && _amount >= 0)
        {
            if (changeReason == HealthChangeReason.Regenerate)
            {
                StopRegenFX();
            }
            return;
        }

        if (_amount > 0 && healUpMultiplier > 1)
        {
            _amount *= healUpMultiplier;
        }
        
        health += _amount;
        
        health = Mathf.Clamp(health, 0, 100);
        
        if (health <= 0)
        {
            Death();
            StartCoroutine(OpenGameOverUI());
            ChangeGameStatus();
            IEnumerator OpenGameOverUI()
            {
                yield return new WaitForSeconds(2f);
                GetComponent<Character>().GameOver();
            }
        }

        if (_amount < 0)
        {
            HealthRegenerateTimer.Instance.ResetTimer(GetComponent<Character>().myPlayer.localId);
            DamageFX(changeReason);
        }
        else
        {
            HealFX(changeReason);
        }
    }

    [ClientRpc]
    public void ChangeGameStatus()
    {
        GameOverUIManager.Instance.gameOver = true;
    }
    [ClientRpc]
    private void StopRegenFX()
    {
        if(hasAuthority)
            SoundManager.Instance.KillLongAudio(SoundCategory.Regenerate);
    }

    [ClientRpc]
    private void DamageFX(HealthChangeReason changeReason)
    {
        if (!hasAuthority)
        {
            switch (changeReason)
            {
                case HealthChangeReason.LightDamage:
                    SoundManager.Instance.DoSFX(SoundCategory.LightMeleeHit);
                    break;
                case HealthChangeReason.HeavyDamage:
                    SoundManager.Instance.DoSFX(SoundCategory.HeavyMeleeHit);
                    break;
                case HealthChangeReason.BluntDamage:
                    SoundManager.Instance.DoSFX(SoundCategory.BluntMeleeHit);
                    break;
                case HealthChangeReason.MagicDamage:
                    SoundManager.Instance.DoSFX(SoundCategory.MagicProjectileHit);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(changeReason), changeReason, null);
            }
        }
        else
        {
            //TODO: Differantiate between flesh and armor damage
            SoundManager.Instance.DoSFX(SoundManager.PickRandomCategory(SoundCategory.FleshHitReceive, SoundCategory.ArmorHitReceive));
            SoundManager.Instance.DelayedSFX(SoundCategory.PainGroan, 0.1f);
        }
    }

    [ClientRpc]
    public void HealFX(HealthChangeReason healthChangeReason)
    {
        if (hasAuthority)
        {
            switch (healthChangeReason)
            {
                case HealthChangeReason.Heal:
                    SoundManager.Instance.DoSFX(SoundCategory.Heal);
                    break;
                case HealthChangeReason.Regenerate:
                    SoundManager.Instance.DoLongAudio(SoundCategory.Regenerate, loop: true, volume: 0.5f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(healthChangeReason), healthChangeReason, null);
            }
        }
    }

    [ClientRpc]
    public void Death()
    {
        SoundManager.Instance.DoSFX(SoundCategory.Death);
        SoundManager.Instance.KillLongAudio(SoundCategory.Walking);
        SoundManager.Instance.KillLongAudio(SoundCategory.Regenerate);
        GetComponent<Character>().animationManager.SetAnimationStateCMD(CharacterAnimationState.Death);
    }
}

public enum HealthChangeReason
{
    LightDamage,
    HeavyDamage,
    BluntDamage,
    MagicDamage,
    Heal,
    Regenerate
}
