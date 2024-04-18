using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PracticeCharacter : MonoBehaviour
{
    public int dodgeChance;
    public int healthMultiplier = 1;
    public Slider hp;
    public TextMeshProUGUI hpText;
    
    private void Update()
    {
        hp.value = Mathf.Clamp(hp.value,0f, 100f);
        int hpInt = Convert.ToInt16(hp.value * 100f);
        hpText.text = hpInt.ToString();
    }
    
    public void TakeDamage(float damageAmount)
    {
        hp.value -= damageAmount / 100f;

    }
    
    public void Heal(float healAmount)
    {
        hp.value += healAmount / 100f * healthMultiplier;
    }

    
}
