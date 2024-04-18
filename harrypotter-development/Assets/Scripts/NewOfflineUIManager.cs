using System.Collections;
using System.Collections.Generic;
using Firebase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewOfflineUIManager : Instancable<NewOfflineUIManager>
{
    public List<GameObject> skillsOnPanel = new List<GameObject>();
    public List<GameObject> skillsEquivalentOnEntrance = new List<GameObject>();
    public TextMeshProUGUI nameOfWeapon, damage, score, attributes, magic;
    public Image weaponOfImage;
    public GameObject infoEquipment;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => FirebaseDataManager.Instance.MyPlayerData != null);
        
        var playerSkillData = FirebaseDataManager.Instance.MyPlayerData.SkillData;
        
        if (playerSkillData == null || playerSkillData.Count < 4)
        {
            playerSkillData = new List<int>(new int[4]);
        }
        
        int selectedZero = playerSkillData[0];
        int selectedOne = playerSkillData[1];
        int selectedTwo = playerSkillData[2];
        int selectedThree = playerSkillData[3];

        SkillInfoUI.selectedSkillsNumbers = new List<int>(new int[4]);
        for (int i = 0; i < 4; i++)
        {
            SkillInfoUI.selectedSkillsNumbers[i] = playerSkillData[i];
        }
        
        SkillManager SM = SkillManager.Instance;

        skillsOnPanel[0].GetComponent<Image>().sprite =
            SM.skillImagesCircular[selectedZero];
        skillsOnPanel[1].GetComponent<Image>().sprite =
            SM.skillImagesCircular[selectedOne];
        skillsOnPanel[2].GetComponent<Image>().sprite =
            SM.skillImagesCircular[selectedTwo];
        if(selectedThree < 1000)
            skillsOnPanel[3].GetComponent<Image>().sprite =
                SM.skillImagesCircular[selectedThree];

        skillsEquivalentOnEntrance[0].GetComponent<Image>().sprite =
            SM.skillImagesCircular[selectedZero];
        skillsEquivalentOnEntrance[1].GetComponent<Image>().sprite =
            SM.skillImagesCircular[selectedOne];
        skillsEquivalentOnEntrance[2].GetComponent<Image>().sprite =
            SM.skillImagesCircular[selectedTwo];
        if(selectedThree < 1000)
            skillsEquivalentOnEntrance[3].GetComponent<Image>().sprite =
                SM.skillImagesCircular[selectedThree];

        
        
    }
}
