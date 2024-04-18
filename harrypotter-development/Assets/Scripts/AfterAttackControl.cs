using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterAttackControl : MonoBehaviour
{
    public CharacterActionManager cam;
    public void CheckMoving()
    {
        if (true)
        {
            if (SceneManager.GetActiveScene().name.ToLower() == "offline")
            {
                return;
            }
            
            print("control moving");
            if (GetComponentInParent<CharacterMovement>().isMoving)
            {
                GetComponentInParent<Character>().animationManager.SetAnimationStateCMD(CharacterAnimationState.Run);
                GetComponentInParent<CharacterMovement>().isMovementAnimating = true;
            }

            //If we've stopped in this frame
            if (!GetComponentInParent<CharacterMovement>().isMoving)
            {
                GetComponentInParent<Character>().animationManager.SetAnimationStateCMD(CharacterAnimationState.Idle);
                GetComponentInParent<CharacterMovement>().isMovementAnimating = false;
            }
        }
    }

    public void Attack()
    {
        if (SceneManager.GetActiveScene().name.ToLower() == "offline")
        {
            return;
        }
        
        //cam.HitMeleeDamage();
    }

    public void RangedAttack()
    {
        if (SceneManager.GetActiveScene().name.ToLower() == "offline")
        {
            return;
        }
        //cam.RequestSpawnAxe();
    }
    
}
