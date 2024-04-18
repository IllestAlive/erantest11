using System;
using Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommanderSpin : MonoBehaviour
{
    public float fullSpinDistance;
    public bool useMouse;
    
    private float lastSpinAngle;
    private Vector2 touchStartPosition;
    public bool touchBegan;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            useMouse = false;
    }

    void Update()
    {
        //if (!EventSystem.current.IsPointerOverGameObject())
        {
            //if (useMouse)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    touchStartPosition = Input.mousePosition;
                    RayShoot();
                }

                if (Input.GetMouseButton(0))
                {
                    if(touchBegan)
                        RotateCommander();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    touchBegan = false;
                    lastSpinAngle = GetDegrees();
                }
            }
            // else if (Input.touchCount > 0)
            // {
            //     var touch = Input.GetTouch(0);
            //
            //     switch (touch.phase)
            //     {
            //         case TouchPhase.Began:
            //             touchStartPosition = touch.position;
            //             break;
            //         case TouchPhase.Moved:
            //             RotateCommander();
            //             break;
            //         case TouchPhase.Stationary:
            //             break;
            //         case TouchPhase.Ended:
            //             lastSpinAngle = GetDegrees();
            //             break;
            //         case TouchPhase.Canceled:
            //             break;
            //         default:
            //             throw new ArgumentOutOfRangeException();
            //     }
            // }
        }
    }

    public void RayShoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.name == "OriginalCharacterHolder")
                {
                    touchBegan = true;
                }
            }
        }
    }

    public float NormalizedInputMovement()
    {
        var originalMovementAmount = touchStartPosition.x - (useMouse ? Input.mousePosition.x : Input.GetTouch(0).position.x);
        return originalMovementAmount / Screen.width;
    }

    public float GetDegrees()
    {
        var touchDistance = NormalizedInputMovement();
        var newRotation = (touchDistance / fullSpinDistance) * 360f;
        return lastSpinAngle + newRotation;
    }

    public void RotateCommander()
    {
        var eulers = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulers.ModifyYValue(GetDegrees()));
    }
}
