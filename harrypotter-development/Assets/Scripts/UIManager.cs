using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Instancable<UIManager>
{
    public Joystick movementJoystick;
    public List<Joystick> rangedAttackJoysticks;
    public List<GameObject> defensiveButton;
    public List<GameObject> passiveButton;
    public GameObject ultimateButton;
    public List<string> Skills;
    public Slider opponentHp;
    public TextMeshProUGUI opponentHpText;
    public List<Image> actions;

    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI rttText;
    public float fps;
    private float _timePassed;
    public GameObject settings;
    

    public enum PlayType
    {
        nonCustomizedCharacters,
        customizedCharacters,
        OriginalCharacters
    };

    public PlayType SelectedPlayType;

    public List<SkillCard> allSkillCards;
    private void Start()
    {
        Application.targetFrameRate = 60;
        
        Skills.Add(PlayerPrefs.GetString("FirstTop", "Melee"));
        Skills.Add(PlayerPrefs.GetString("SecondTop", "Melee"));
        Skills.Add(PlayerPrefs.GetString("ThirdTop", "Melee"));
        Skills.Add(PlayerPrefs.GetString("ForthTop", "Melee"));

        
        
        // for (int i = 0; i < 4; i++)
        // {
        //     if (Skills[i] == "Melee") 
        //         rangedAttackJoysticks[i].gameObject.SetActive(true);
        //     if (Skills[i] == "Ranged") 
        //         rangedAttackJoysticks[i].gameObject.SetActive(true);
        //     if(Skills[i] == "Defensive")
        //         defensiveButton[i].gameObject.SetActive(true);
        //     if(Skills[i] == "Ultimate")
        //         ultimateButton.gameObject.SetActive(true);
        // }

        // int topFirst = PlayerPrefs.GetInt("TopFirst", 0);
        // rangedAttackJoysticks[0].gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite =
        //     allSkillCards[topFirst].picture;
        // defensiveButton[0].gameObject.transform.GetChild(0).GetComponent<Image>().sprite =
        //     allSkillCards[topFirst].picture;
        // ultimateButton.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite =
        //     allSkillCards[topFirst].picture;
        //
        // int topSecond = PlayerPrefs.GetInt("TopSecond", 0);
        // rangedAttackJoysticks[1].gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite =
        //     allSkillCards[topSecond].picture;
        // defensiveButton[1].gameObject.transform.GetChild(0).transform.GetComponent<Image>().sprite =
        //     allSkillCards[topSecond].picture;
        // ultimateButton.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite =
        //     allSkillCards[topSecond].picture;
        //
        // int topThird = PlayerPrefs.GetInt("TopThird", 0);
        // rangedAttackJoysticks[2].gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite =
        //     allSkillCards[topThird].picture;
        // defensiveButton[2].gameObject.transform.GetChild(0).GetComponent<Image>().sprite =
        //     allSkillCards[topThird].picture;
        // ultimateButton.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite =
        //     allSkillCards[topThird].picture;
        //
        // int topForth = PlayerPrefs.GetInt("TopForth", 0);
        // rangedAttackJoysticks[3].gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite =
        //     allSkillCards[topForth].picture;
        // defensiveButton[3].gameObject.transform.GetChild(0).GetComponent<Image>().sprite =
        //     allSkillCards[topForth].picture;
        // ultimateButton.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite =
        //     allSkillCards[topForth].picture;
    }
    
    private void Update()
    {
        _timePassed += Time.deltaTime;
        if (_timePassed > .1f)
        {
            fps = 1.0f / Time.deltaTime;
            _timePassed = 0;
            fpsText.text = Math.Round(fps, 0).ToString();
            rttText.text = $"{Math.Round(Mirror.NetworkTime.rtt * 1000)}";
        }
    }

    public void GoBackOffline()
    {
        
    }

    public void OpenCloseSettings()
    {
        settings.SetActive(!settings.activeSelf);
    }
}
