using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TopSkills : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI name;
    public Image image;
    public SkillCard skillCard;
    public bool onIt;
    public Image selectedSkill;
    
    public enum SkillType{Melee,Ranged,Defensive,Ultimate}

    public SkillType skillType;
    public enum WhichTop
    {
        first,second,third,forth
    }

    public WhichTop whichTop;
    public void OnPointerEnter(PointerEventData eventData)
    {
        onIt = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForEndOfFrame();
            onIt = false;
        }
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_EDITOR_OSX || PLATFORM_STANDALONE_WIN
        if (Input.GetMouseButtonUp(0) && onIt && skillType.ToString() == OfflineUIManager.Instance.holdingCard.skillType.ToString())
        {
            skillCard = OfflineUIManager.Instance.holdingCard;
            name.text = OfflineUIManager.Instance.holdingCard.name;
            image.sprite = OfflineUIManager.Instance.holdingCard.picture;
            selectedSkill.sprite = OfflineUIManager.Instance.holdingCard.picture;
        }
#else
        if (onIt && Input.touchCount > 0 && skillType.ToString() == OfflineUIManager.Instance.holdingCard.skillType.ToString())
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                skillCard = OfflineUIManager.Instance.holdingCard;
                name.text = OfflineUIManager.Instance.holdingCard.name;
                image.sprite = OfflineUIManager.Instance.holdingCard.picture;
                selectedSkill.sprite = OfflineUIManager.Instance.holdingCard.picture;
            }
        }
#endif
    }

    private void OnDestroy()
    {
        if (whichTop == WhichTop.first)
        {
            PlayerPrefs.SetString("FirstTop",skillCard.skillType.ToString());
            PlayerPrefs.SetInt("TopFirst", skillCard.id);
        }

        if (whichTop == WhichTop.second)
        {
            PlayerPrefs.SetString("SecondTop",skillCard.skillType.ToString());
            PlayerPrefs.SetInt("TopSecond", skillCard.id);
        }

        if (whichTop == WhichTop.third)
        {
            PlayerPrefs.SetString("ThirdTop",skillCard.skillType.ToString());
            PlayerPrefs.SetInt("TopThird", skillCard.id);
        }

        if (whichTop == WhichTop.forth)
        {
            PlayerPrefs.SetString("ForthTop",skillCard.skillType.ToString());
            PlayerPrefs.SetInt("TopForth", skillCard.id);
        }
        
        print(skillCard.skillType.ToString());
        
    }
}
