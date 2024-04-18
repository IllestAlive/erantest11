using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : Instancable<CharacterSelection>
{
    public List<GameObject> characters; //Pre-customized characters.
    public List<GameObject> inventories;
    public List<GameObject> skills;
    public int activeId;
    

    public void PreviousCharacter()
    {
        activeId--;
        if (activeId == -1)
            activeId = characters.Count - 1;

        for (int i = 0; i < characters.Count; i++)
        {
            if (i != activeId)
            {
                characters[i].SetActive(false);
                inventories[i].SetActive(false);
                skills[i].SetActive(false);
            }
            else
            {
                characters[i].SetActive(true);
                inventories[i].SetActive(true);
                skills[i].SetActive(true);
            }
        }
        characters[activeId].GetComponent<Animator>().SetTrigger("attacking");
    }

    public void NextCharacter()
    {
        activeId++;
        if (activeId == characters.Count)
            activeId = 0;

        for (int i = 0; i < characters.Count; i++)
        {
            if (i != activeId)
            {
                characters[i].SetActive(false);
                inventories[i].SetActive(false);
                skills[i].SetActive(false);
            }
            else
            {
                characters[i].SetActive(true);
                inventories[i].SetActive(true);
                skills[i].SetActive(true);
            }
        }
        characters[activeId].GetComponent<Animator>().SetTrigger("attacking");
    }

    private void OnDestroy()
    {
            PlayerPrefs.SetInt("CharId",activeId);
    }
}
