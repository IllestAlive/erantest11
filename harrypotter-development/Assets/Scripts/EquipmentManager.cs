using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase;

public class EquipmentManager : Instancable<EquipmentManager>
{
    public List<Image> equipmentImages;
    public TextMeshProUGUI name;
    public TextMeshProUGUI level;
    public TextMeshProUGUI specialName;
    public TextMeshProUGUI rarity;
    public GameObject attackFill;
    public GameObject defenseFill;
    public GameObject dodgeFill;

    public List<GameObject> panels;

    public GameObject commanderNft;
    
    public EquipmentSlot openPanel;
    public int selectedEquipmentId;
    public List<GameObject> equipmentButtons;
    public List<GameObject> panelButtons;
    public GameObject activePanelButton;
    public Equipment jotnar;
    
    [Header("Original")]
    public List<GameObject> panelsOriginal;
    public List<GameObject> equipmentButtonsOriginal;
    public List<GameObject> panelButtonsOriginal;

    public Dictionary<EquipmentSlot, GameObject> panelDictionary = new();
    public List<Image> equipmentImagesOriginal;

    public TextMeshProUGUI nameOriginal;
    public TextMeshProUGUI levelOriginal;
    public TextMeshProUGUI specialNameOriginal;
    public TextMeshProUGUI rarityOriginal;
    public GameObject attackFillOriginal;
    public GameObject defenseFillOriginal;
    public GameObject dodgeFillOriginal;

    public List<GameObject> selectedVersionsOfTheEquipments = new List<GameObject>();

    private IEnumerator Start()
    {
        if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.WithoutCustomizedCharacters)
        {
            panelDictionary.Add(EquipmentSlot.rightHand, panelButtons[0]);
            panelDictionary.Add(EquipmentSlot.leftHand, panelButtons[1]);
            panelDictionary.Add(EquipmentSlot.helmet, panelButtons[2]);
            panelDictionary.Add(EquipmentSlot.rightShoulder, panelButtons[3]);
            panelDictionary.Add(EquipmentSlot.leftShoulder, panelButtons[4]);
        }

        if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.OriginalCharacters)
        {
            panelDictionary.Add(EquipmentSlot.rightHand, panelButtonsOriginal[0]);
            panelDictionary.Add(EquipmentSlot.leftHand, panelButtonsOriginal[1]);
            panelDictionary.Add(EquipmentSlot.helmet, panelButtonsOriginal[2]);
            panelDictionary.Add(EquipmentSlot.rightShoulder, panelButtonsOriginal[3]);
            panelDictionary.Add(EquipmentSlot.leftShoulder, panelButtonsOriginal[4]);

            yield return new WaitUntil(() => FirebaseDataManager.Instance.initialized);
            int experience = FirebaseDataManager.Instance.MyPlayerData.Experience;
            experience /= 100;
            OfflineUIManager.Instance.originalLevelText.text = "Level:" + experience;
            float e = experience;
            OfflineUIManager.Instance.originalLevelBar.localScale = new Vector3(e / 100f,1,1);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            FirebaseDataManager.Instance.MyPlayerData.Experience += 150;
            FirebaseDataManager.Instance.UpdateMyPlayerDataExperience();
        }
    }

    public void OpenPanel(int slotId)
    {
        // if (OfflineUIManager.Instance.randomizeButton.activeSelf)
        //     return;

        if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.WithoutCustomizedCharacters)
        {
            for (int j = 0; j < panels.Count; j++)
            {
                panels[j].SetActive(false);
            }

            switch ((EquipmentSlot)slotId)
            {
                case EquipmentSlot.leftHand:
                    panels[3].SetActive(true);
                    VisualizeJotnar();
                    break;
                case EquipmentSlot.rightHand:
                    panels[0].SetActive(true);
                    VisualizeJotnar();
                    break;
                case EquipmentSlot.leftShoulder:
                    panels[1].SetActive(true);
                    break;
                case EquipmentSlot.rightShoulder:
                    panels[1].SetActive(true);
                    break;
                case EquipmentSlot.helmet:
                    panels[2].SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slotId), slotId, null);
            }

            openPanel = (EquipmentSlot)slotId;

            for (int i = 0; i < panelButtons.Count; i++)
            {
                panelButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        
        if (PlaytypeManager.Instance.SelectedPlayType == PlaytypeManager.PlayType.OriginalCharacters)
        {
            // for (int j = 0; j < panelsOriginal.Count; j++)
            // {
            //     panelsOriginal[j].SetActive(false);
            // }
            //
            // switch ((EquipmentSlot)slotId)
            // {
            //     case EquipmentSlot.leftHand:
            //         panelsOriginal[4].SetActive(true);
            //         VisualizeJotnar();
            //         break;
            //     case EquipmentSlot.rightHand:
            //         panelsOriginal[0].SetActive(true);
            //         VisualizeJotnar();
            //         break;
            //     case EquipmentSlot.leftShoulder:
            //         panelsOriginal[2].SetActive(true);
            //         break;
            //     case EquipmentSlot.rightShoulder:
            //         panelsOriginal[1].SetActive(true);
            //         break;
            //     case EquipmentSlot.helmet:
            //         panelsOriginal[3].SetActive(true);
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException(nameof(slotId), slotId, null);
            // }

            openPanel = (EquipmentSlot)slotId;

            for (int i = 0; i < panelButtonsOriginal.Count; i++)
            {
                panelButtonsOriginal[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        
        
    }
    
    public void VisualizeJotnar()
    {
        if (OfflineUIManager.Instance.randomizeButton.activeSelf)
            return;
        
        for (int i = 0; i < equipmentImages.Count; i++)
        {
            equipmentImages[i].gameObject.SetActive(false);
        }
        equipmentImages[jotnar.visualizeWeapon].gameObject.SetActive(true);
        name.text = jotnar.name;
        level.text = "Level: " + jotnar.level;
        specialName.text = jotnar.specialName;
        rarity.text = jotnar.rarity;
        attackFill.transform.localScale = new Vector3(jotnar.attack / 10f, 1, 1);
        defenseFill.transform.localScale = new Vector3(jotnar.defense / 10f, 1, 1);
        dodgeFill.transform.localScale = new Vector3(jotnar.dodge / 10f, 1, 1);
        specialName.color = jotnar.color;
        rarity.color = jotnar.color;


        selectedEquipmentId = jotnar.visualizeWeapon;
    }

}
