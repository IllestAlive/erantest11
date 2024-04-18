using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class ActionCooldown
{
    public int index;

    public float maxValue;

    public float currentValue;
    public float CurrentValue
    {
        get => currentValue;
        set
        {
            currentValue = value;
            ActionUIManager.Instance.SetActionImageFillRatio(index, currentValue, maxValue);
        }
    }

}

public class CharacterActionManager : NetworkBehaviour
{
    
    [SyncVar(hook = nameof(OnReceiveIsBlocking))] internal bool isBlocking;

    public ParticleSystem hitParticle;

    public ArrowRendering arrowRendering;

    [SerializeField]
    private Character opponentInsideMe;

    public GameObject blockIndicator;
    public GameObject meleeIndicator;

    public ActionCooldown[] actionCooldowns;
    private Vector3 lookTarget;

    public int attackingThing = 1;

    public Vector3 axeHitPoint;

    private float reloadTime = 3f;
    private float rangedReloadTime = 7f;

    private float millisecondsBeforeManualAim = 200;
    
    public GameObject floatingTextPrefab;

    public WeaponParticleController weaponParticleController;

    private DateTime lastArrowOpenTime;

    private TimeSpan timeSinceLastArrowOpen => DateTime.Now - lastArrowOpenTime;

    private bool autoAimTime => timeSinceLastArrowOpen < TimeSpan.FromMilliseconds(millisecondsBeforeManualAim);

    public List<Renderer> clothRenderers;
    public Color startColorsOfCloths, redColorsOfCloths;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (hasAuthority)
        {
            for (int i = 0; i < actionCooldowns.Length; i++)
            {
                actionCooldowns[i].CurrentValue = actionCooldowns[i].currentValue;
                actionCooldowns[i].index = i;
            }

            ArrowRendering.OnArrowEnabled += SetLastArrowTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
        {
            return;
        }

        for (int i = 0; i < actionCooldowns.Length; i++)
        {
            if (actionCooldowns[i].CurrentValue > 0)
            {
                actionCooldowns[i].CurrentValue -= Time.deltaTime;
                actionCooldowns[i].CurrentValue = Mathf.Clamp(actionCooldowns[i].CurrentValue, 0, actionCooldowns[i].maxValue);
            }
        }

        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     UpgradeHealth();
        // }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Character>())
        {
            opponentInsideMe = other.GetComponentInParent<Character>();
        }

        if (other.CompareTag("Gungnir"))
        {
            TakeDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Character>())
        {
            opponentInsideMe = null;
        }
    }

    public void SetSkillGraphic(int id, bool enable)
    {
        switch (id)
        {
            case 0:
                if (enable)
                {
                    arrowRendering.EnableArrow();
                    break;
                }
                arrowRendering.DisableArrow();
                break;
            case 1:
                meleeIndicator.SetActive(enable);
                if (enable)
                {
                    arrowRendering.EnableArrow();
                    break;
                }
                arrowRendering.DisableArrow();
                break;
            case 2:
                break;
        }
    }

    public void CastSkill(int id)
    {
        SkillData skillData = SkillFetcher._chosenSkills[id];

        switch (skillData.SkillId)
        {
            case "aegishjalmur":

                var aegishjalmurData = skillData.GetSkillEffectConverted<AegishjalmurData>();
                CastAegishjalmur(aegishjalmurData);
                break;
        }
    }

    [Command(requiresAuthority = false)]
    private void CastAegishjalmur(AegishjalmurData aegishjalmurData)
    {
        //Create area
        
        //var area = Instantiate(NetworkManager.singleton.spawnPrefabs[], transform.parent);
        //NetworkServer.Spawn(area);
        
        //StartCoroutine(destroyRoutine);

        //IEnumerator destroyRoutine()
        //{
        //    yield return new WaitForSeconds(aegishjalmurData.Duration);
        //    Destroy(area.gameObject);
        //}
        
        //After this moment, we'll check if we're in the collider of this area (by using OnTriggerEnter/Exit or OnTriggerStay (first one is better I think) )
    }

    public void UseAction(int id)
    {
        var GameOverUIM = GameOverUIManager.Instance;
        if(!StartManagerInGame.Instance.canStart || GameOverUIM.gameOver)
            return;
        
        switch (id)
        {
            case 0: //Ranged
                if(UIManager.Instance.actions[0].GetComponent<Image>().fillAmount < 1)
                    return;
                
                UIManager.Instance.actions[0].GetComponent<Image>().fillAmount = 0;
                float valueZero = 0;
                DOTween.To(() => valueZero, x => valueZero = x, 1, rangedReloadTime)
                    .OnUpdate(() => {
                        UIManager.Instance.actions[0].GetComponent<Image>().fillAmount = valueZero;
                    }).SetEase(Ease.Linear);
                
                GetComponent<Character>().animationManager.SetAnimationStateCMD(CharacterAnimationState.RangedAttack);
                SoundManager.Instance.DoSFX(SoundCategory.MagicProjectileSpawn); //TODO: Differentiate between metal and magic
                
                //hitPoint.y = 2;
                var distance = Vector3.Distance(GetComponentInChildren<ArrowRendering>().hitPoint, transform.position);
                if (autoAimTime || distance < 5f)
                {
                    axeHitPoint = Character.opponentCharacter.transform.position;
                }
                else
                {
                    axeHitPoint = GetComponentInChildren<ArrowRendering>().hitPoint;
                }
                
                StartCoroutine(ChangeDirArrow());
                IEnumerator ChangeDirArrow()
                {
                    yield return new WaitForEndOfFrame();
                    Vector3 arrowTransform = GetComponentInChildren<ArrowRendering>().hitPoint;
                    arrowTransform.y = GetComponent<CharacterMovement>().model.transform.position.y;
                    GetComponent<CharacterMovement>().model.transform.LookAt(arrowTransform);
                }
                break;
            case 1: //Melee
                if (actionCooldowns[1].CurrentValue == 0)
                {
                    if(UIManager.Instance.actions[1].GetComponent<Image>().fillAmount < 1)
                        return;
                
                    UIManager.Instance.actions[1].GetComponent<Image>().fillAmount = 0;
                    float valueOne = 0;
                    DOTween.To(() => valueOne, x => valueOne = x, 1, reloadTime)
                        .OnUpdate(() => {
                            UIManager.Instance.actions[1].GetComponent<Image>().fillAmount = valueOne;
                        }).SetEase(Ease.Linear);
                    
                    GetComponent<Character>().animationManager.SetAnimationStateCMD(CharacterAnimationState.MeleeAttack);
                    SoundManager.Instance.DoSFX(SoundCategory.EffortGroan); //TODO: Change depending on gender
                    SoundManager.Instance.DoSFX(SoundCategory.HeavyMeleeSwing); //TODO: Change depending on weapon
                    
                    if(opponentInsideMe!=null)
                        StartCoroutine(ChangeDir());
                    
                    IEnumerator ChangeDir()
                    {
                        yield return new WaitForEndOfFrame();
                        Vector3 arrowTransform = GetComponentInChildren<ArrowRendering>().hitPoint;
                        arrowTransform.y = GetComponent<CharacterMovement>().model.transform.position.y;
                        GetComponent<CharacterMovement>().model.transform.LookAt(arrowTransform);
                        // Vector3 opponentTransform = opponentInsideMe.transform.position;
                        // opponentTransform.y = GetComponent<CharacterMovement>().model.transform.position.y;
                        // GetComponent<CharacterMovement>().model.transform.LookAt(opponentTransform);
                    }

                    // transform.LookAt(opponentInsideMe.gameObject.transform);
                    actionCooldowns[1].CurrentValue = actionCooldowns[1].maxValue;
                    // HitMeleeDamage();
                }
                break;
            case 2: //Block
                if (actionCooldowns[2].CurrentValue == 0)
                {
                    if (UIManager.Instance.actions[2].GetComponent<Image>().fillAmount < 1)
                        return;

                    UIManager.Instance.actions[2].GetComponent<Image>().fillAmount = 0;
                    float valueOne = 0;
                    DOTween.To(() => valueOne, x => valueOne = x, 1, reloadTime)
                        .OnUpdate(() => { UIManager.Instance.actions[2].GetComponent<Image>().fillAmount = valueOne; })
                        .SetEase(Ease.Linear);


                    GetComponent<Character>().animationManager.SetAnimationStateCMD(CharacterAnimationState.Shield);

                    actionCooldowns[2].CurrentValue = actionCooldowns[2].maxValue;
                    UseBlock();
                }
                break;
            case 3: //Basic Attack
                if (actionCooldowns[3].CurrentValue == 0)
                {
                    if(UIManager.Instance.actions[3].GetComponent<Image>().fillAmount < 1)
                        return;
                
                    UIManager.Instance.actions[3].GetComponent<Image>().fillAmount = 0;
                    float valueOne = 0;
                    DOTween.To(() => valueOne, x => valueOne = x, 1, reloadTime/2f)
                        .OnUpdate(() => {
                            UIManager.Instance.actions[3].GetComponent<Image>().fillAmount = valueOne;
                        }).SetEase(Ease.Linear);
                    
                    GetComponent<Character>().animationManager.SetAnimationStateCMD(CharacterAnimationState.BasicAttack);
                    SoundManager.Instance.DoSFX(SoundCategory.EffortGroan);//TODO: Differentiate between genders
                    SoundManager.Instance.DoSFX(SoundCategory.LightMeleeSwing); //TODO: Differentiate between weapons
                    
                    // if(opponentInsideMe!=null)
                    //     StartCoroutine(ChangeDir());
                    //
                    // IEnumerator ChangeDir()
                    // {
                    //     yield return new WaitForEndOfFrame();
                    //     Vector3 arrowTransform = GetComponentInChildren<ArrowRendering>().hitPoint;
                    //     arrowTransform.y = GetComponent<CharacterMovement>().model.transform.position.y;
                    //     GetComponent<CharacterMovement>().model.transform.LookAt(arrowTransform);
                    //     // Vector3 opponentTransform = opponentInsideMe.transform.position;
                    //     // opponentTransform.y = GetComponent<CharacterMovement>().model.transform.position.y;
                    //     // GetComponent<CharacterMovement>().model.transform.LookAt(opponentTransform);
                    // }

                    // transform.LookAt(opponentInsideMe.gameObject.transform);
                    actionCooldowns[3].CurrentValue = actionCooldowns[3].maxValue;
                    // HitMeleeDamage();
                }
                break;
        }
    }
    
    public void RequestSpawnAxe()
    {
        if (actionCooldowns[0].CurrentValue > 0)
        {
            return;
        }

        actionCooldowns[0].CurrentValue = actionCooldowns[0].maxValue;
        SpawnAxe(axeHitPoint);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (hasAuthority)
        {
            ArrowRendering.OnArrowEnabled -= SetLastArrowTime;
        }
    }

    [Command]
    private void SpawnAxe(Vector3 hitPoint)
    {
        var axe = Instantiate(NetworkManager.singleton.spawnPrefabs[1], transform.position, Quaternion.identity, transform.parent);
        axe.GetComponent<NetworkMatch>().matchId = GetComponent<NetworkMatch>().matchId;
        NetworkServer.Spawn(axe.gameObject, netIdentity.connectionToServer);
        axe.GetComponent<Axe>().owner = netIdentity;
        ServerGameManager SGM = ServerGameManager.Instance;
        axe.GetComponent<Axe>().OpenGraphicInServer(GetComponent<CharacterModelManager>().rangedId);
        axe.GetComponent<Axe>().OpenGraphic(GetComponent<CharacterModelManager>().rangedId);
        axe.GetComponent<Axe>().theAttack = axe.transform.GetChild(GetComponent<CharacterModelManager>().rangedId).gameObject;
        
        if(axe.transform.GetChild(GetComponent<CharacterModelManager>().rangedId).GetComponent<SkillType>().typeOfSkill == SkillType.TypeOfSkill.throwable)
            axe.GetComponent<Axe>().Fire(hitPoint);

        if (axe.transform.GetChild(GetComponent<CharacterModelManager>().rangedId).GetComponent<SkillType>()
                .typeOfSkill == SkillType.TypeOfSkill.zone)
        {
            axe.GetComponent<Axe>().zoneFire = true;
            axe.GetComponent<Axe>().ZoneFire();
            StartCoroutine(DestroyZoneFire());
        }


        IEnumerator DestroyZoneFire()
        {
            yield return new WaitForSeconds(2.5f);
            Destroy(axe);
        }
    }

    [Command]
    public void HitMeleeDamage()
    {
        if (opponentInsideMe)
        {
            DamageIndicators();
            opponentInsideMe.TakeDamage(15, HealthChangeReason.HeavyDamage);
            opponentInsideMe.RenderDamageVFX(15);
            Camera.main.transform.DOShakePosition(0.2f, 0.2f);
        }
    }

    [Server]
    public void TakeDamage()
    {
        //GetComponent<Character>().TakeDamage(15,HealthChangeReason.HeavyDamage);
        //print("damageTaken");
    }

    [ClientRpc]
    public void DamageIndicators()
    {
        if (hasAuthority) //The one who takes damage
        {
            var NUIM = NewUIManager.Instance;
            var redLayer = NewUIManager.Instance.redLayer;
            
            //Camera Shake
            CameraManager.Instance.ShakeCamera(ShakeIntensity.Mid, ShakeTime.Quick); 
            
            //Red Layer
            var spawnedRedLayer = Instantiate(redLayer, redLayer.transform.position,
                redLayer.rectTransform.rotation, redLayer.transform);
            spawnedRedLayer.color = NUIM.startColorRedLayer;
            spawnedRedLayer.DOColor(NUIM.finishColorRedLayer, 0.5f).OnComplete(() =>
            {
                Destroy(spawnedRedLayer.gameObject);
            });
        }
        else // The one who gives damage
        {
            CameraManager.Instance.ShakeCamera(ShakeIntensity.Low, ShakeTime.Quick);
            for (int i = 0; i < clothRenderers.Count; i++)
            {
                clothRenderers[i].material.color = redColorsOfCloths;
                StartCoroutine(ChangeBackToWhite(clothRenderers[i]));
            }

            IEnumerator ChangeBackToWhite(Renderer renderer)
            {
                yield return new WaitForSeconds(0.5f);
                renderer.material.color = Color.white;
            }
        }
    }
    [Command]
    private void UseBlock()
    {
        if (isBlocking)
        {
            return;
        }
        isBlocking = true;
        StartCoroutine(BlockCountdown());
        IEnumerator BlockCountdown()
        {
            yield return new WaitForSeconds(1);
            isBlocking = false;
        }
    }

    private void OnReceiveIsBlocking(bool _old, bool _new)
    {
        blockIndicator.SetActive(_new);
        ActionUIManager.Instance.SelectOrDeselectIndependentSkill(2, _new);
    }

    [ClientRpc]
    public void OnTakeDamage(int amount)
    {
        // if(floatingTextPrefab)
        //     ShowFloatingText(amount);
        
        hitParticle.Play();
        
        if (!hasAuthority)
        {
            return;
        }

        GetComponent<Character>().animationManager.SetAnimationStateCMD(CharacterAnimationState.GettingHit);
        Camera.main.transform.DOShakePosition(0.2f, 0.25f);
    }

    void ShowFloatingText(int amount)
    { 
        var gO = Instantiate(floatingTextPrefab, transform.position + new Vector3(-0.7f,-2f,0), Quaternion.identity, transform);
        gO.GetComponent<TextMesh>().text = amount.ToString();
    }

    void SetLastArrowTime()
    {
        lastArrowOpenTime = DateTime.Now;
    }
    
}