using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTheSkill : MonoBehaviour
{
    public int skillNumber;
    public GameObject myCollision;
    public bool mjollnirFirst, mjollnirSecond;
    public GameObject usingCharacter;
    private void OnTriggerEnter(Collider other)
    {
        if (myCollision == null)
        {
            Debug.Log("My Collision is NULL");
            return;
        }
        
        if (other.gameObject.name == "ForCollision")
        {
            SkillData skillData = SkillFetcher._skillDatas[skillNumber];
            Character character = other.gameObject.transform.parent.GetComponentInParent<Character>();
            switch (skillNumber)
            {
                case 8: //Gouge
                    if (other.gameObject != myCollision)
                    {
                        var gougeData = skillData.GetSkillEffectConverted<GougeData>();
                        TakeDamage(character, gougeData.DmgAmount);
                    }

                    break;
                case 10: //Idhun
                    if (other.gameObject != myCollision)
                    {
                        var idhunData = skillData.GetSkillEffectConverted<IdhunData>();
                        TakeDamage(character, idhunData.DmgAmount);
                    }

                    break;
                case 15: //Mjollnir
                    if (other.gameObject != myCollision)
                    {
                        var mjollnirData = skillData.GetSkillEffectConverted<MjollnirData>();
                        if (mjollnirFirst)
                        {
                            TakeDamage(character,mjollnirData.DmgAmount1);
                            Root(other.gameObject.transform.parent.GetComponentInParent<CharacterMovement>(),mjollnirData.StunDuration1);
                        }

                        if (mjollnirSecond)
                        {
                            TakeDamage(character,mjollnirData.DmgAmount2);
                            Root(other.gameObject.transform.parent.GetComponentInParent<CharacterMovement>(),mjollnirData.StunDuration2);
                        }
                    }

                    break;
                        
                case 18: //Temblor
                    if (other.gameObject != myCollision)
                    {
                        var temblorData = skillData.GetSkillEffectConverted<TemblorData>();
                        TakeDamage(character, temblorData.DmgAmount);
                    }

                    break;
                case 19: // Uvard
                    if (other.gameObject != myCollision)
                    {
                        var uvardData = skillData.GetSkillEffectConverted<UvardData>();
                        TakeDamage(character, uvardData.DmgAmount);
                    }

                    break;
                
                case 22: //Gungnir
                    if (other.gameObject != myCollision)
                    {
                        var gungnirData = skillData.GetSkillEffectConverted<GungnirData>();
                        //dealing X damage
                        TakeDamage(character,gungnirData.DmgAmount);
                        Root(other.gameObject.transform.parent.GetComponentInParent<CharacterMovement>(),gungnirData.RootToPlaceTime);
                    }
                    
                    break;
                
            }
        }
    }
    
    public void TakeDamage(Character character,int damageAmount)
    {
        if(character.GetComponent<CharacterStats>().health > 0)
            character.TakeDamage(damageAmount,HealthChangeReason.MagicDamage);
        print("damageTaken");
    }

    public void Root(CharacterMovement characterMovement, float rootToPlaceTime)
    {
        characterMovement.cantMove = true;
        Character character = characterMovement.GetComponent<Character>();
        if(characterMovement.GetComponent<CharacterStats>().health > 0)
            character.animationManager.SetAnimationStateServer(CharacterAnimationState.Stun);
        StartCoroutine(Root());
        IEnumerator Root()
        {
            yield return new WaitForSeconds(rootToPlaceTime);
            if (characterMovement.GetComponent<CharacterStats>().health > 0)
            {
                characterMovement.cantMove = false;
                character.animationManager.SetAnimationStateServer(CharacterAnimationState.OutStun);
            }

            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }
        
    }
}
