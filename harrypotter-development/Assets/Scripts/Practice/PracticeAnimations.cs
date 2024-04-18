using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeAnimations : MonoBehaviour
{
    public GameObject model;
    public Animator animator;
    public void Move()
    {
        animator.SetBool("run",true);
    }

    public void Idle()
    {
        animator.SetBool("run",false);
    }
    
    public void Blitz()
    {
        animator.SetTrigger("Blitz");
    }

    public void Attack()
    {
        int rand = Random.Range(0, 3);
        animator.SetInteger("randAttack",rand);
        animator.SetTrigger("attacking");
    }

    public void Skill()
    {
        animator.SetTrigger("skill");
    }

    public void Gouge()
    {
        animator.SetTrigger("Gouge");
    }

    public void Temblor()
    {
        animator.SetTrigger("Temblor");
    }

    public void BowWalk()
    {
        animator.SetBool("BowWalk",true);
    }

    public void StopBowWalk()
    {
        animator.SetBool("BowWalk",false);
    }

    public void Idhun()
    {
        animator.SetTrigger("Idhun");
    }

    public void Uvard()
    {
        animator.SetTrigger("Uvard");
    }
}
