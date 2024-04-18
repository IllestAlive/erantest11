using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum CharacterAnimationState { Idle, Run, MeleeAttack, RangedAttack, Shield, Death, GettingHit, BasicAttack, Skill, Gouge, Temblor, Uvard, Idhun, Stun, OutStun, Blitz}

public class CharacterAnimationManager : NetworkBehaviour
{
    public Animator charAnimator;

    public CharacterAnimationState currentState;

    [Command]
    public void SetAnimationStateCMD(CharacterAnimationState _state)
    {
        SetAnimationStateRPC((int)_state);
    }

    [Server]
    public void SetAnimationStateServer(CharacterAnimationState _state)
    {
        SetAnimationStateRPC((int)_state);
    }

    [ClientRpc]
    private void SetAnimationStateRPC(int _state)
    {
        currentState = (CharacterAnimationState)_state;
        if (hasAuthority)
        {
            Debug.Log("new state: " + currentState.ToString());
        }

        if (currentState == CharacterAnimationState.Idle)
        {
            charAnimator.SetBool("run",false);
        }

        if (currentState == CharacterAnimationState.Run)
        {
            charAnimator.SetBool("run", true);
        }
        
        if (currentState == CharacterAnimationState.MeleeAttack)
        {
            charAnimator.SetTrigger("attacking");
            if (UIManager.Instance.SelectedPlayType == UIManager.PlayType.OriginalCharacters)
            {
                // int rand = charAnimator.GetInteger("randattack");
                // rand += 1;
                // if (rand == 6)
                //     rand = 0;
                // charAnimator.SetInteger("randattack",rand);
            }
        }
        if (currentState == CharacterAnimationState.RangedAttack)
        {
            charAnimator.SetTrigger("rangedattacking");
            if (UIManager.Instance.SelectedPlayType == UIManager.PlayType.OriginalCharacters)
            {
                // int rand = charAnimator.GetInteger("rangedRandAttack");
                // rand += 1;
                // if (rand == 6)
                //     rand = 0;
                // charAnimator.SetInteger("rangedRandAttack",rand);
            }
        }
        if (currentState == CharacterAnimationState.Shield)
        {
            charAnimator.SetTrigger("shield");
        }
        if (currentState == CharacterAnimationState.Skill)
        {
            charAnimator.SetTrigger("skill");
        }
        if (currentState == CharacterAnimationState.Death)
        {
            charAnimator.SetTrigger("death");
        }
        if (currentState == CharacterAnimationState.GettingHit)
        {
            //charAnimator.SetTrigger("gettingHit");
        }
        if (currentState == CharacterAnimationState.BasicAttack)
        {
            charAnimator.SetTrigger("basicAttack");
        }
        if (currentState == CharacterAnimationState.Gouge)
        {
            charAnimator.SetTrigger("Gouge");
        }
        if (currentState == CharacterAnimationState.Temblor)
        {
            charAnimator.SetTrigger("Temblor");
        }

        if (currentState == CharacterAnimationState.Uvard)
        {
            charAnimator.SetTrigger("Uvard");
        }
        if (currentState == CharacterAnimationState.Idhun)
        {
            charAnimator.SetTrigger("Idhun");
        }

        if (currentState == CharacterAnimationState.Stun)
        {
            charAnimator.SetBool("Stun",true);
        }
        
        if (currentState == CharacterAnimationState.OutStun)
        {
            charAnimator.SetBool("Stun",false);
        }
        
        if (currentState == CharacterAnimationState.Blitz)
        {
            charAnimator.SetTrigger("Blitz");
        }
        
    }
}
