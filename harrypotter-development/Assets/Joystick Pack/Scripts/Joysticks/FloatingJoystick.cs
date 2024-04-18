using System.Collections;
using System.Collections.Generic;
using Extensions.TUTORIAL;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FloatingJoystick : Joystick
{
    private bool _isPracticeScene;
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name.Equals("Practice"))
        {
            _isPracticeScene = true;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (TutorialManager.ShowTutorial && _isPracticeScene)
        {
            if (isMovement)
            {
                PracticeTutorialManager.Instance.OnTapMovementJoystick();
            }
        }
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}