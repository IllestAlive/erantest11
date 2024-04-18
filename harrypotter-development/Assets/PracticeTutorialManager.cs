using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Extensions.TUTORIAL;
using TMPro;
using UnityEngine.UI;

public class PracticeTutorialManager : Instancable<PracticeTutorialManager>
{
    [SerializeField] private GameObject blackPanel;
    [SerializeField] private RectTransform finger;
    [SerializeField] private RectTransform joystickRect;

    [SerializeField] private Transform[] joystickFingerMovementPathTransforms;

    [SerializeField] private TextMeshProUGUI objectiveTextPoint;
    
    [SerializeField] private RectTransform skillsFingerTargetRext;
    [SerializeField] private Image completedImage;

    [SerializeField] private List<GameObject> phases = new List<GameObject>();

    private int activePhase = -1;

    private Vector3[] _joystickFingerMovementPath;

    // Start is called before the first frame update
    void Start()
    {
        _joystickFingerMovementPath = new Vector3[joystickFingerMovementPathTransforms.Length];

        for (int i = 0; i < _joystickFingerMovementPath.Length; i++)
        {
            _joystickFingerMovementPath[i] = joystickFingerMovementPathTransforms[i].position;
        }
        
        AnimatePhase0();
    }

    void AnimatePhase0()
    {
        activePhase = 0;
        
        phases[0].SetActive(true);

        finger.position = _joystickFingerMovementPath[0];
        
        finger.transform.LookAt(joystickRect.transform.position);
        var rot = finger.localEulerAngles;
        rot.y = 0;
        rot.z = 180+rot.x;
        rot.x = 0;
        finger.localEulerAngles = rot;
        
        finger.DOPath(_joystickFingerMovementPath, 2f).SetEase(Ease.Linear).SetLoops(-1);
    }

    public void OnTapMovementJoystick()
    {
        if (activePhase != 0)
        {
            return;
        }
        
        finger.DOKill();
        finger.gameObject.SetActive(false);
        phases[0].SetActive(false);
        blackPanel.SetActive(false);
        
        StartCoroutine(AnimatePhase1());
    }

    // Collect healths
    IEnumerator AnimatePhase1()
    {
        activePhase = 1;
        
        phases[1].SetActive(true);

        objectiveTextPoint.text = "use joystick to move around and collect hp";
        
        yield return null;
    }

    public void OnCollectedEveryHealth()
    {
        if (activePhase != 1)
        {
            return;
        }
        
        phases[1].SetActive(false);

        StartCoroutine(AnimatePhase2());
    }

    IEnumerator AnimatePhase2()
    {
        activePhase = 2;
        
        phases[2].SetActive(true);
        finger.gameObject.SetActive(true);

        finger.position = skillsFingerTargetRext.position;
        
        finger.transform.LookAt(skillsFingerTargetRext.transform.position);
        var rot = finger.localEulerAngles;
        rot.y = 0;
        rot.z = rot.x;
        rot.x = 0;
        finger.localEulerAngles = rot;
        
        finger.DOJumpAnchorPos(finger.anchoredPosition + (Vector2)finger.forward * 5, 30, 1, 1.5f).SetLoops(5);
        
        objectiveTextPoint.text = "press and drag to aim your skills";

        yield return null;
    }
    
    public void OnTapSkillJoystick()
    {
        if (activePhase != 2)
        {
            return;
        }
        
        finger.DOKill();
        finger.gameObject.SetActive(false);
        phases[2].SetActive(false);
        
        StartCoroutine(AnimatePhase3());
    }

    IEnumerator AnimatePhase3()
    {
        activePhase = 3;

        yield return new WaitForSeconds(1.5f);
        
        objectiveTextPoint.text = "";
        
        phases[3].SetActive(true);

        objectiveTextPoint.text = "You have completed the basic tutorial, now go and test your skills on the road to Valhalla!";

        completedImage.rectTransform.DOScale(Vector3.one, .5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnTapEndTutorial()
    {
        TutorialManager.TutorialStep = 2;
    }
}
