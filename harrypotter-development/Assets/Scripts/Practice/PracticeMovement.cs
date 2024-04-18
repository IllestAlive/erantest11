using System;
using System.Collections;
using System.Collections.Generic;
using Extensions.TUTORIAL;
using UnityEngine;

public class PracticeMovement : MonoBehaviour
{
    public GameObject model;
    
    public Joystick movementJoystick;
    
    private Vector2 inputToSend;
    private Vector3 lookTarget;
    [SerializeField] private Vector3 moveDirection;
    
    public float speed;
    
    public bool isMobile;
    public bool isMoving;
    public bool forceToJoystick;
    public bool skillOn;

    private void Start()
    {
#if UNITY_STANDALONE || UNITY_STANDALONE_OSX || UNITY_EDITOR
        isMobile = forceToJoystick || false;
#else
        isMobile = true;
#endif
        
        StartCoroutine(DirectionCalculator());
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthCollect"))
        {
            GetComponent<PracticeCharacter>().Heal(15);
            Destroy(other.gameObject);

            if (TutorialManager.ShowTutorial)
            {
                TutorialManager.CollectedHealthCount++;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!skillOn)
        {
            PracticeManager PM = PracticeManager.Instance;
            speed = PM.speedCharacter;
            inputToSend = !isMobile
                ? new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
                : new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);
            MoveTheCharacter(inputToSend);
            isMoving = inputToSend.x != 0 || inputToSend.y != 0;
            PracticeAnimations practiceAnimations = GetComponent<PracticeAnimations>();
            if (isMoving && PM.speedCharacter > 0)
                practiceAnimations.Move();
            else
                practiceAnimations.Idle();
        }

    }

    private void MoveTheCharacter(Vector2 input) //SendInput on online.
    {
        input.Normalize();
        var targetPos = transform.position +
                        (input.x * Vector3.right + input.y * Vector3.forward) * Time.fixedDeltaTime * speed;
        transform.position = targetPos;
    }
    
    
    IEnumerator DirectionCalculator()
    {
        //if (!skillOn)
        {
            PracticeManager PM = PracticeManager.Instance;
            while (true)
            {
                var lastPos = transform.position;
                yield return new WaitForEndOfFrame();
                moveDirection = (transform.position - lastPos).normalized * 2;
                moveDirection.y = 0;

                lookTarget = Vector3.Lerp(lookTarget, transform.position + moveDirection, Time.deltaTime * 5);
                lookTarget.y = model.transform.position.y;

                if (moveDirection != Vector3.zero && !PM.primarySkillCasted && !skillOn)
                {
                    model.transform.LookAt(lookTarget);
                }
            }
        }
    }
    
    public void LookAttackTarget(Vector3 lookTarget)
    {
        lookTarget.y = model.transform.position.y;
        
        model.transform.LookAt(lookTarget);
        
        var rot = model.transform.localEulerAngles;
        rot.x = 0;

        model.transform.localEulerAngles = rot;
    }
    
}
