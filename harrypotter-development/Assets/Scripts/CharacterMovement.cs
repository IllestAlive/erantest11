using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CharacterMovement : NetworkBehaviour
{
    [Header("Power Up Specifications")] 
    public float speedUpAmount;
    public int healthUpAmount;
    
    [Header("Mobile")]
    public bool isMobile;
    public Joystick movementJoystick;

    [Header("Common")]
    public bool forceToJoystick;

    public Vector3 lastReceivedPosition;

    private Vector2 inputToSend;

    [SerializeField] private Vector3 moveDirection;
    private Vector3 lookTarget;

    public GameObject model, customizedModel, nonCustomizedModel, originalModel;
    
    [SyncVar(hook = nameof(OnSpeedChange))]public float speed;

    public float defaultSpeed;

    public float timePassed;

    public bool isMoving, isMovementAnimating;

    public Rigidbody rb;
    [SyncVar]public bool cantMove;
    public bool skillOn;

    public override void OnStartClient()
    {
        base.OnStartClient();

        movementJoystick = UIManager.Instance.movementJoystick;

        lastReceivedPosition = transform.position;
        StartCoroutine(DirectionCalculator());

#if UNITY_STANDALONE || UNITY_STANDALONE_OSX || UNITY_EDITOR
        isMobile = forceToJoystick || false;
#else
        isMobile = true;
#endif

    }

    private void Start()
    {
        if (UIManager.Instance.SelectedPlayType == UIManager.PlayType.customizedCharacters)
            model = customizedModel;
        if (UIManager.Instance.SelectedPlayType == UIManager.PlayType.nonCustomizedCharacters)
            model = nonCustomizedModel;
        if (UIManager.Instance.SelectedPlayType == UIManager.PlayType.OriginalCharacters)
            model = originalModel;

    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            return;
        }

        timePassed += Time.deltaTime;
        if (timePassed >= 0.033333f)
        {
            timePassed = 0;
        }


        var lastPos = transform.position;

        transform.position = Vector3.Lerp(transform.position, lastReceivedPosition, Time.deltaTime * 10);

        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     SpeedUp();
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedCollect"))
        {
            SpeedUp(other.gameObject);
        }
        if (other.CompareTag("HealthCollect"))
        {
            HealthUp();
            DestroyHealthCollectible(other.gameObject);
        }
    }

    [Server]
    void HealthUp()
    {
        GetComponent<CharacterStats>().ChangeHealth(healthUpAmount, HealthChangeReason.Heal);
    }

    [Server]
    public void DestroyHealthCollectible(GameObject spawnedGameObject)
    {
        SpawnManager.Instance.spawnedObjects.Remove(spawnedGameObject);
        
        DestroyClientObject(spawnedGameObject.GetComponent<Collect>().index);
        //Destroy(spawnedGameObject);
        spawnedGameObject.SetActive(false);
        var sO = SpawnManager.Instance.spawnedObjects;
        for (int i = 0; i < sO.Count; i++)
        {
            sO[i].GetComponent<Collect>().index = i;
        }
    }

    [Server]
    public void SpeedBoost(float oldSpeed, float newSpeed, float waitTime)
    {
        speed = newSpeed;

        StartCoroutine(ChangeBackToNormal());
        IEnumerator ChangeBackToNormal()
        {
            yield return new WaitForSeconds(waitTime);
            speed = oldSpeed;
        }
    }
    
    [Command]
    public void SpeedBoostCmd(float oldSpeed, float newSpeed, float waitTime)
    {
        speed = newSpeed;

        StartCoroutine(ChangeBackToNormal());
        IEnumerator ChangeBackToNormal()
        {
            yield return new WaitForSeconds(waitTime);
            speed = oldSpeed;
        }
    }
    
    
    [Server]
    public void SpeedUp(GameObject spawnedGameObject)
    {
        
        float newSpeed = speed + (speed * speedUpAmount);
        SpeedBoost(defaultSpeed,newSpeed,3f);
        SpawnManager.Instance.spawnedObjects.Remove(spawnedGameObject);
        
        DestroyClientObject(spawnedGameObject.GetComponent<Collect>().index);
        //Destroy(spawnedGameObject);
        spawnedGameObject.SetActive(false);
        var sO = SpawnManager.Instance.spawnedObjects;
        for (int i = 0; i < sO.Count; i++)
        {
            sO[i].GetComponent<Collect>().index = i;
        }
    }

    [ClientRpc]
    public void DestroyClientObject(int index)
    {
        Debug.Log("index= " + index);
        GameObject spawned = SpawnManager.Instance.spawnedObjects[index];
        if(hasAuthority) SoundManager.Instance.DoSFX(spawned.GetComponent<Collect>().collectType == Collect.CollectType.Health 
            ? SoundCategory.HealthPowerUp 
            : SoundCategory.SpeedPowerUp);
        SpawnManager.Instance.spawnedObjects.Remove(spawned);
        //Destroy(spawned);
        spawned.SetActive(false);
        var sO = SpawnManager.Instance.spawnedObjects;
        for (int i = 0; i < sO.Count; i++)
        {
            sO[i].GetComponent<Collect>().index = i;
        }
    }

    private void FixedUpdate()
    {
        
        if (isServer)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (hasAuthority &&  Character.myCharacter.stats.health > 0)
        {
            var gameOverUIM = GameOverUIManager.Instance;
            if (!StartManagerInGame.Instance.canStart)
                return;

            if (!gameOverUIM.gameOver)
            {
                if (!skillOn)
                {
                    inputToSend = !isMobile
                        ? new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
                        : new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);
                    if (cantMove)
                        inputToSend = Vector2.zero;
                    SendInput(inputToSend);
                    isMoving = inputToSend.x != 0 || inputToSend.y != 0;
                }
            }
            else
            {
                isMoving = false;
            }

            

            //If we've started moving in this frame
            if (!isMovementAnimating && isMoving && !cantMove)
            {
                SoundManager.Instance.DoLongAudio(SoundCategory.Walking, loop: true);
                GetComponent<Character>().animationManager.SetAnimationStateCMD(CharacterAnimationState.Run);
                isMovementAnimating = true;
            }

            //If we've stopped in this frame
            if (isMovementAnimating && !isMoving && !cantMove)
            {
                SoundManager.Instance.KillLongAudio(SoundCategory.Walking);
                GetComponent<Character>().animationManager.SetAnimationStateCMD(CharacterAnimationState.Idle);
                isMovementAnimating = false;
            }
        }
    }

    

    private void OnGUI()
    {
        if (!hasAuthority)
        {
            return;
        }
        GUILayout.Label(GetComponent<Character>().animationManager.currentState.ToString());
    }

    [Command]
    private void SendInput(Vector2 input)
    {
        input.Normalize();

        Vector3 targetPos = transform.position;
        if (!cantMove)
            targetPos = transform.position + (input.x * Vector3.right + input.y * Vector3.forward) * Time.fixedDeltaTime * speed;

        // targetPos.x = Mathf.Clamp(targetPos.x, -9, 9);
        // targetPos.z = Mathf.Clamp(targetPos.z, -16f, 16);

        transform.position = targetPos;

        FixPosition(transform.position);
    }

    [Command]
    public void BeamUp(Vector3 newPosition)
    {
        transform.position = newPosition;
        FixPosition(transform.position);
    }

    [ClientRpc]
    private void FixPosition(Vector3 pos)
    {
        lastReceivedPosition = pos;
    }

    IEnumerator DirectionCalculator()
    {
        while (true)
        {
            //if (!GetComponent<Character>().skillOn)
            {
                var lastPos = transform.position;
                yield return new WaitForEndOfFrame();
                moveDirection = (transform.position - lastPos).normalized * 2;
                moveDirection.y = 0;

                lookTarget = Vector3.Lerp(lookTarget, transform.position + moveDirection, Time.deltaTime * 5);
                lookTarget.y = model.transform.position.y;

                if (moveDirection != Vector3.zero &&
                    GetComponent<Character>().animationManager.currentState != CharacterAnimationState.MeleeAttack &&
                    GetComponent<Character>().animationManager.currentState != CharacterAnimationState.RangedAttack)
                {
                    if (hasAuthority)
                    {
                        if ((isMobile || forceToJoystick) &&
                            (movementJoystick.Vertical != 0 || movementJoystick.Horizontal != 0) && !skillOn)
                        {
                            model.transform.LookAt(lookTarget);
                        }

                        if (!isMobile && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !skillOn)
                        {
                            model.transform.LookAt(lookTarget);
                        }
                    }
                    else
                    {
                        model.transform.LookAt(lookTarget);
                    }
                }
            }
        }

    }

    [Command(requiresAuthority = false)]
    public void LookAttackTargetCmd(Vector3 lookTarget)
    {
        lookTarget.y = model.transform.position.y;
        
        model.transform.LookAt(lookTarget);
        
        var rot = model.transform.localEulerAngles;
        rot.x = 0;

        model.transform.localEulerAngles = rot;
        
        LookAttackTargetRpc(lookTarget);
    }

    [ClientRpc]
    public void LookAttackTargetRpc(Vector3 lookTarget)
    {
        model.transform.LookAt(lookTarget);
        
        var rot = model.transform.localEulerAngles;
        rot.x = 0;

        model.transform.localEulerAngles = rot;
    }
    
    private void OnSpeedChange(float _old, float _new)
    {
        speed = _new;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + moveDirection, .2f);
    }
}

