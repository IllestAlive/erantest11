using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Firebase;
using UnityEngine.SceneManagement;


public class NewUIManager : Instancable<NewUIManager>
{
    public List<Image> skillsImages = new List<Image>();
    public List<GameObject> skills = new List<GameObject>();
    public GameObject settings;
    public List<Image> underHealthBarSkillsImages = new List<Image>();

    public List<Image> loadingImages = new List<Image>();
    public List<Image> opponentLoadingImages = new List<Image>();

    public TextMeshProUGUI myLevel, opponentLevel;
    public TextMeshProUGUI opponentName;
    public List<string> possibleOpponentNames;
    public Image redLayer;
    public Color startColorRedLayer, finishColorRedLayer;
    

    private IEnumerator Start()
    {
        
        var playerSkillData = FirebaseDataManager.Instance.MyPlayerData.SkillData;

        SkillManager SM = SkillManager.Instance;

        int selectedZero = playerSkillData[0];
        int selectedOne = playerSkillData[1];
        int selectedTwo = playerSkillData[2];
        int selectedThree = playerSkillData[3];

        if (SceneManager.GetActiveScene().name != "Practice")
        {
            yield return new WaitUntil(() => Character.myCharacter != null);
            Character.myCharacter.SetSkillImageIndexes(new List<int>()
            {
                selectedZero, selectedOne, selectedTwo, selectedThree
            });
        }
        else
        {
            yield return new WaitForSeconds(0);
        }

        skillsImages[0].sprite =
            SM.skillImagesCircular[selectedZero];
        skillsImages[1].sprite =
            SM.skillImagesCircular[selectedOne];
        skillsImages[2].sprite =
            SM.skillImagesCircular[selectedTwo];
        skillsImages[3].sprite =
            SM.skillImagesCircular[selectedThree];

        if (SceneManager.GetActiveScene().name != "Practice")
        {
            loadingImages[0].sprite = SM.skillImagesCircular[selectedZero];
            loadingImages[1].sprite = SM.skillImagesCircular[selectedOne];
            loadingImages[2].sprite = SM.skillImagesCircular[selectedTwo];
            loadingImages[3].sprite = SM.skillImagesCircular[selectedThree];

            opponentName.text = possibleOpponentNames[Random.Range(0, 20)];

            int experience = FirebaseDataManager.Instance.MyPlayerData.Experience;
            experience /= 100;
            myLevel.text = experience.ToString();

            opponentLevel.text = Random.Range(1, 101).ToString();
        }

        if (SceneManager.GetActiveScene().name == "Practice")
        {
            skills[0].GetComponent<PracticeActionUIElement>().skillId = selectedZero;
            skills[1].GetComponent<PracticeActionUIElement>().skillId = selectedOne; 
            skills[2].GetComponent<PracticeActionUIElement>().skillId = selectedTwo; 
            skills[3].GetComponent<PracticeActionUIElement>().skillId = selectedThree;
        }
        else
        {
            skills[0].GetComponent<ActionUIElement>().skillId = selectedZero;
            skills[1].GetComponent<ActionUIElement>().skillId = selectedOne; 
            skills[2].GetComponent<ActionUIElement>().skillId = selectedTwo; 
            skills[3].GetComponent<ActionUIElement>().skillId = selectedThree;
        }

        if(SceneManager.GetActiveScene().name == "Practice")
            yield break;
        
        StartCoroutine(SetOpponentInfo());

    }

    public IEnumerator SetOpponentInfo()
    {
        SkillManager SM = SkillManager.Instance;

        yield return new WaitUntil(() =>
            Character.opponentCharacter != null && Character.opponentCharacter.selectedSkillImages.Count >= 4);

        var skillImages = Character.opponentCharacter.selectedSkillImages;
        
        underHealthBarSkillsImages[0].sprite = SM.skillImagesCircular[skillImages[0]];
        underHealthBarSkillsImages[1].sprite = SM.skillImagesCircular[skillImages[1]];
        underHealthBarSkillsImages[2].sprite = SM.skillImagesCircular[skillImages[2]];
        underHealthBarSkillsImages[3].sprite = SM.skillImagesCircular[skillImages[3]];
        
        opponentLoadingImages[0].sprite = SM.skillImagesCircular[skillImages[0]];
        opponentLoadingImages[1].sprite = SM.skillImagesCircular[skillImages[1]];
        opponentLoadingImages[2].sprite = SM.skillImagesCircular[skillImages[2]];
        opponentLoadingImages[3].sprite = SM.skillImagesCircular[skillImages[3]];
    }

    public void OpenCloseSettings()
    {
        settings.SetActive(!settings.activeSelf);
    }
}
