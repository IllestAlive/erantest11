using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class OfflineUIManager : Instancable<OfflineUIManager>
{
    public TextMeshProUGUI nickname;
    public TextMeshProUGUI name;
    public TextMeshProUGUI level;
    public TextMeshProUGUI description;
    public GameObject levelBar;
    public GameObject selectionUI, generalUI, customizedUI, nonCustomizedUI;
    public GameObject skillUI;
    public int rangedIndex;
    public GameObject loadingFill;
    public GameObject loadingScreen;

    public SkillCard holdingCard;
    public GameObject animationSelectScreen;

    public List<GameObject> skillAnimations;
    public int selectedSkillID;
    public TextMeshProUGUI nameOfSkill;

    public GameObject inventoryUI;
    public GameObject commanderNfts;
    public GameObject generalInventory;
    public GameObject randomizeButton;
    private float nextActionTime = 1f;
    public float period = 0.5f;

    [Header("Original")] 
    public TextMeshProUGUI originalLevelText;
    public Transform originalLevelBar;
    public GameObject originalInventoryScreen;
    public GameObject originalEntranceScreen;
    public GameObject originalCharacterHolder;
    public GameObject originalCharacterGround;
    public GameObject originalPlayButton;
    public float charAndGroundInvXPos = -2.25f;
    public float playYPosOnInventory = 104f;
    public float playYPosOnEntrance = 40.91f;
    public List<Image> skillImagesOnEntrance = new List<Image>();
    public int selectedSkill;

    private void Start()
    {
        PlayerPrefs.SetInt("RangedId",16);
        loadingFill.transform.DOScale(1,3).OnComplete(() =>
        {
            loadingScreen.SetActive(false);
        });
    }

    public void OpenSelectionUI()
    {
        selectionUI.SetActive(true);
        generalUI.SetActive(false);
        CharacterSelection.Instance.characters[CharacterSelection.Instance.activeId].GetComponent<Animator>().SetTrigger("attacking");
    }

    public void CloseSelectionUI()
    {
        selectionUI.SetActive(false);
        generalUI.SetActive(true);
    }

    public void OpenCloseSkillUI(int a)
    {
        if(a==0)
            skillUI.SetActive(true);
        if(a==1)
            skillUI.SetActive(false);
    }

    public void SelectRanged(int _rangedIndex)
    {
        // rangedIndex = _rangedIndex;
        // PlayerPrefs.SetInt("RangedId", rangedIndex);
    }

    public void OpenCloseAnimationSelectionUI(int a)
    {
        if(a == 0)
            animationSelectScreen.SetActive(true);
        if(a == 1)
            animationSelectScreen.SetActive(false);
    }

    public void ChangeSkillAnimation(bool next)
    {
        if (next)
        {
            for (int i = 0; i < skillAnimations.Count; i++)
            {
                skillAnimations[i].SetActive(false);
            }

            selectedSkillID++;
            if (selectedSkillID == skillAnimations.Count)
                selectedSkillID = 0;

            skillAnimations[selectedSkillID].SetActive(true);
        }
        else
        {
            for (int i = 0; i < skillAnimations.Count; i++)
            {
                skillAnimations[i].SetActive(false);
            }

            selectedSkillID--;
            if (selectedSkillID == -1)
                selectedSkillID = skillAnimations.Count - 1;
            
            skillAnimations[selectedSkillID].SetActive(true);
        }
        
        PlayerPrefs.SetInt("RangedId",selectedSkillID+1); //Because of the cube inside of Axe.
        nameOfSkill.text = skillAnimations[selectedSkillID].GetComponent<SkillType>().nameOfSkill;
    }

    public void RandomiseCommanderNFT()
    {
        float time = 0;
        DOTween.To(() => time, x => time = x, 1, 3)
            .OnUpdate(() =>
            {
                if (Time.time > nextActionTime ) {
                    nextActionTime += period;
                    int random = Random.Range(0, CommanderNFTSelection.Instance.commanderNftList.Count);
                    CommanderNFTSelection.Instance.SelectCommander(random);
                }
                
            });
    }

    public void OpenNewInventoryUI()
    {
        inventoryUI.SetActive(true);
        generalInventory.SetActive(false);
        commanderNfts.transform.DOMoveX(1.74f, 1f);
    }

    public void OpenOriginalInventory()
    {
        originalCharacterHolder.transform.DOMoveX(charAndGroundInvXPos, 1f);
        // originalPlayButton.transform.DOScale(0.3f, 1f);
        // originalPlayButton.transform.DOMoveY(playYPosOnInventory, 1f);
        originalCharacterGround.transform.DOMoveX(charAndGroundInvXPos, 1f).OnComplete(() =>
        {
            originalEntranceScreen.SetActive(false);
            originalInventoryScreen.SetActive(true);
        });
    }

    public void OpenOriginalEntrance()
    {
        originalCharacterHolder.transform.DOMoveX(0, 1f);
        // originalPlayButton.transform.DOScale(1f, 1f);
        // originalPlayButton.transform.DOMoveY(playYPosOnEntrance, 1f);
        originalCharacterGround.transform.DOMoveX(0, 1f).OnComplete(() =>
        {
            originalEntranceScreen.SetActive(true);
            originalInventoryScreen.SetActive(false);
        });
    }

    public void OpenPracticeScene()
    {
        SceneManager.LoadScene("Practice");
    }

    private void OnDestroy()
    {
        if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.WithoutCustomizedCharacters)
        {
            PlayerPrefs.SetInt("RangedId",13);
        }
        if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.OriginalCharacters)
        {
            PlayerPrefs.SetInt("RangedId",16);
        }
        
    }
}
