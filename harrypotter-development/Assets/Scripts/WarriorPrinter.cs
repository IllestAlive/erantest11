using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarriorPrinter : MonoBehaviour
{
    public Warrior warrior;
    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name.ToLower() == "offline")
        {
            OfflineUIManager OUIM = OfflineUIManager.Instance;
            OUIM.name.text = warrior.name;
            OUIM.nickname.text = warrior.nickname;
            OUIM.level.text = "Level: " + warrior.level + "/10";
            OUIM.description.text = warrior.description;
            float level = warrior.level;
            OUIM.levelBar.transform.localScale = new Vector3(level / 10f, 1, 1);
        }
    }
}
