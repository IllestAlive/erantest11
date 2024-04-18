using System;
using System.Collections;
using Extensions.TUTORIAL;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Extensions.Skill.UI
{
    [RequireComponent(typeof(Button))]
    public class SkillUIElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        public Image tick;
        [SerializeField] private Image icon;
        public GameObject redBG;

        internal SkillData SkillData { get; private set; }

        private void OnEnable()
        {
            StartCoroutine(CheckUsing());
            IEnumerator CheckUsing()
            {
                yield return new WaitForSeconds(0.1f);
                for (int i = 0; i < SkillInfoUI.selectedSkillsNumbers.Count; i++)
                {
                    if (SkillData.SkillNumber == SkillInfoUI.selectedSkillsNumbers[i])
                        tick.gameObject.SetActive(true);
                }
            }
        }

        public void Init(SkillData skillData)
        {
            SkillData = skillData;
            title.text = skillData.SkillName;
            int skillNumber = skillData.SkillNumber;
            icon.sprite = SkillManager.Instance.skillImagesCircular[skillNumber];
        }

        public void OnTap()
        {
            SkillInfoUI.Instance.Open(this);
            SkillInfoUI.Instance.selectASkill.SetActive(false);
            for (int i = 0; i < SkillSelectionScreen.Instance._skillUIElements.Count; i++)
            {
                SkillSelectionScreen.Instance._skillUIElements[i].redBG.SetActive(false);
            }
            redBG.SetActive(true);

            if (TutorialManager.ShowTutorial)
            {
                if (SkillData.IsPrimary)
                {
                    OfflineTutorialManager.Instance.OnSelectPrimarySkill();
                }
            }
        }
        
        public void OnSelect()
        {
            tick.gameObject.SetActive(true);
        }

        public void OnDeselect()
        {
            tick.gameObject.SetActive(false);
        }
    }
}