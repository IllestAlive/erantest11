using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PracticeSkills : Instancable<PracticeSkills>
{
    private ParticleSystem gung;
    private Vector3 spawnMjollnir;
    public GameObject characterGameObject;
    public void CastZarn(ZarnData zarnData)
    {
        PracticeManager PM = PracticeManager.Instance;
        float firstSpeed = PM.speedCharacter;
        float additionToSpeed = PM.speedCharacter * zarnData.MovementSpeedBonus / 100f;
        PM.speedCharacter += additionToSpeed;
        var character =  characterGameObject.GetComponent<PracticeMovement>();

        character.skillOn = true;
        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.1f);
            character.skillOn = false;
        }
        
        PM.practiceCharacter.GetComponent<PracticeAnimations>().Skill();
        StartCoroutine(StopZarnEffect());
        IEnumerator StopZarnEffect()
        {
            yield return new WaitForSeconds(zarnData.Duration);
            PM.speedCharacter = firstSpeed;
        }
    }

    public void SetGungnir(GungnirData gungnirData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        PracticeCircleInsideCircle PCIC = PracticeCircleInsideCircle.Instance;
        PCIC.littleCircle = PSH.gungnirSmallCircle.transform;
        PSH.gungnirBigCircle.SetActive(true);
        int bigCircleSize = gungnirData.Range;
        float smallCircleSize = gungnirData.Area;
        float bigCircleFloat = (float)bigCircleSize; 
        float smallCircleFloat = (float)smallCircleSize;
        uint affectedBigCircleSize = Convert.ToUInt16(bigCircleFloat);
        uint affectedSmallCircleSize = Convert.ToUInt16(smallCircleFloat);
        PSH.gungnirBigCircle.transform.localScale = Vector3.one * bigCircleFloat / 24f; // normally /8 but the UI's are 3x
        PSH.gungnirSmallCircle.transform.localScale = Vector3.one * (smallCircleFloat/bigCircleFloat);
        PCIC.multiplier = 32 - (((smallCircleFloat / bigCircleFloat) * 36)-4); //32 is when the ratio is 0.1 . When the number goues up the multiplier should goes down. Every 0.1 equals to minus 3.6~4
    }
    public void CastGungnir(GungnirData gungnirData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        PracticeCircleInsideCircle PCIC = PracticeCircleInsideCircle.Instance;

        PSH.gungnirBigCircle.SetActive(false);
        StartCoroutine(PlayGungnir());
        gung = Instantiate(PSH.gungnirParticle, PSH.gungnirParticle.transform.position,
            PSH.gungnirParticle.transform.rotation);
        gung.transform.localScale = Vector3.one;
        var character =  characterGameObject.GetComponent<PracticeMovement>();
        character.skillOn = true;
        character.LookAttackTarget(PCIC.littleCircle.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.6f);
            character.skillOn = false;
        }
        IEnumerator PlayGungnir()
        {
            yield return new WaitForSeconds(gungnirData.Delay);
            gung.Play();
            //deal damage and slow down opponent
        }

        StartCoroutine(StopAndCloseGungnir());
        IEnumerator StopAndCloseGungnir()
        {
            yield return new WaitForSeconds(gungnirData.Delay + gungnirData.RootToPlaceTime);
            gung.Stop();
            PM.skillCasted = false;
        }
    }
    
    public void SetMjollnir(MjollnirData mjollnirData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        PracticeCircleInsideCircle PCIC = PracticeCircleInsideCircle.Instance;
        PCIC.littleCircle = PSH.mjollnirSmallCircle.transform;
        PSH.mjollnirBigCircle.SetActive(true);
        int bigCircleSize = mjollnirData.Range;
        float smallCircleSize = mjollnirData.Area;
        float bigCircleFloat = (float)bigCircleSize; 
        float smallCircleFloat = (float)smallCircleSize;
        uint affectedBigCircleSize = Convert.ToUInt16(bigCircleFloat);
        uint affectedSmallCircleSize = Convert.ToUInt16(smallCircleFloat);
        PSH.mjollnirBigCircle.transform.localScale = Vector3.one * bigCircleFloat / 24f; // normally /8 but the UI's are 3x
        PSH.mjollnirSmallCircle.transform.localScale = Vector3.one * (smallCircleFloat/bigCircleFloat);
        PCIC.multiplier = 32 - (((smallCircleFloat / bigCircleFloat) * 36)-4); //32 is when the ratio is 0.1 . When the number goues up the multiplier should goes down. Every 0.1 equals to minus 3.6~4
    }
    public void CastMjollnir(MjollnirData mjollnirData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        PracticeCircleInsideCircle PCIC = PracticeCircleInsideCircle.Instance;
        var character =  characterGameObject.GetComponent<PracticeMovement>();
        character.skillOn = true;
        character.LookAttackTarget(PCIC.littleCircle.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.6f);
            character.skillOn = false;
        }
        PSH.mjollnirBigCircle.SetActive(false);
        StartCoroutine(PlayMjollnirFirst());
        spawnMjollnir = PSH.mjollnirParticleOne.transform.position;
        IEnumerator PlayMjollnirFirst()
        {
            yield return new WaitForSeconds(mjollnirData.Delay);
            var parOne = Instantiate(PSH.mjollnirParticleOne, spawnMjollnir,
                Quaternion.identity);
            parOne.transform.rotation = PSH.mjollnirParticleOne.transform.rotation;
            parOne.transform.localScale = Vector3.one;
            parOne.Play();
            //PSH.mjollnirParticleOne.Play();
            //DMGAmount1 + StunDuration1
            StartCoroutine(PlayMjollnirSecond());
        }

        IEnumerator PlayMjollnirSecond()
        {
            yield return new WaitForSeconds(mjollnirData.TimeApart);
            var parTwo = Instantiate(PSH.mjollnirParticleTwo, spawnMjollnir,
                Quaternion.identity);
            parTwo.transform.rotation = PSH.mjollnirParticleTwo.transform.rotation;
            parTwo.transform.localScale = Vector3.one;
            parTwo.Play();
            //PSH.mjollnirParticleTwo.Play();
            //DMGAmount2 + StunDuration2
        }

        StartCoroutine(StopAndCloseMjollnir());
        IEnumerator StopAndCloseMjollnir()
        {
            yield return new WaitForSeconds(mjollnirData.Delay + mjollnirData.TimeApart + 2);
            
            PM.skillCasted = false;
        }
    }

    public void SetGouge(GougeData gougeData, Joystick usingJoystick)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        
        //PracticeJoystickRotation.Instance.hitArea = PSH.temblorImage;
        PracticeJoystickRotation.Instance.joystick = usingJoystick;
        
        PSH.areaAttacks.SetActive(true);
        PSH.gougeImage.SetActive(true);
        //PSH.areaAttacks.transform.localScale = Vector3.one * gougeData.Range / 9f; // when size gets bigger the area should move further.
    }

    public void CastGouge(GougeData gougeData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        PracticeCircleInsideCircle PCIC = PracticeCircleInsideCircle.Instance;

        PM.model.transform.localRotation = PSH.areaAttacks.transform.localRotation;
        PM.practiceCharacter.GetComponent<PracticeAnimations>().Gouge();
        var character =  characterGameObject.GetComponent<PracticeMovement>();
        character.skillOn = true;
        character.LookAttackTarget(PSH.swordTarget.transform.position);
        
        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.6f);
            character.skillOn = false;
        }

        PSH.areaAttacks.SetActive(false);
        PSH.gougeImage.SetActive(false);

        
    }

    public void SetTemblor(TemblorData temblorData, Joystick usingJoystick)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        //PracticeJoystickRotation.Instance.hitArea = PSH.temblorImage;
        PracticeJoystickRotation.Instance.joystick = usingJoystick;
        PSH.areaAttacks.SetActive(true);
        PSH.temblorImage.SetActive(true);
        //PSH.areaAttacks.transform.localScale = Vector3.one * temblorData.Range / 9f; // when size gets bigger the area should move further.
    }

    public void CastTemblor(TemblorData temblorData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();

        PM.model.transform.localRotation = PSH.areaAttacks.transform.localRotation;
        PM.practiceCharacter.GetComponent<PracticeAnimations>().Temblor();
        var character =  characterGameObject.GetComponent<PracticeMovement>();
        character.skillOn = true;
        character.LookAttackTarget(PSH.swordTarget.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.6f);
            character.skillOn = false;
        }
        
        PSH.areaAttacks.SetActive(false);
        PSH.temblorImage.SetActive(false);

    
    }

    public void SetIdhun(IdhunData idhunData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeCharacter PC = PM.practiceCharacter.GetComponent<PracticeCharacter>();
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        PracticeAnimations PA = PM.practiceCharacter.GetComponent<PracticeAnimations>();
        PM.primarySkillCasted = true;
        PM.savedSpeed = PM.speedCharacter;
        PM.speedCharacter = PM.speedCharacter * (idhunData.MovementSpeedReduction / 100f);
        PSH.bowPrimaries.SetActive(true);
        PSH.idhunImage.SetActive(true);
        PA.BowWalk();
    }

    public void CastIdhun(IdhunData idhunData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeCharacter PC = PM.practiceCharacter.GetComponent<PracticeCharacter>();
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        PracticeAnimations PA = PM.practiceCharacter.GetComponent<PracticeAnimations>();
        PM.speedCharacter = PM.savedSpeed;
        PM.model.transform.localRotation = PSH.areaAttacks.transform.localRotation;
        PA.StopBowWalk();
        PA.Idhun();
        var character =  characterGameObject.GetComponent<PracticeMovement>();
        character.skillOn = true;
        character.LookAttackTarget(PSH.bowTarget.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.6f);
            character.skillOn = false;
        }
        GameObject idhun = Instantiate(PSH.idhunPrefab, PSH.idhunHolderForPos.transform.position, PSH.idhunHolderForPos.transform.rotation);
        PM.primarySkillCasted = false;
        float waitTime = 0;
        DOTween.To(() => waitTime, x => waitTime = x, 360, idhunData.Range)
            .OnUpdate(() => {
                idhun.transform.position += idhun.transform.forward * idhunData.ArrowSpeed * Time.deltaTime;
            }).OnComplete(() =>
            {
                Destroy(idhun);
            });
        
        PSH.bowPrimaries.SetActive(false);
        PSH.idhunImage.SetActive(false);
    }
    
 public void SetUvard(UvardData uvardData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeCharacter PC = PM.practiceCharacter.GetComponent<PracticeCharacter>();
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        PracticeAnimations PA = PM.practiceCharacter.GetComponent<PracticeAnimations>();
        PM.primarySkillCasted = true;
        PM.savedSpeed = PM.speedCharacter;
        PM.speedCharacter = 0;
        PSH.bowPrimaries.SetActive(true);
        PSH.uvardImage.SetActive(true);
        PA.Idle();
    }

    public void CastUvard(UvardData uvardData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeCharacter PC = PM.practiceCharacter.GetComponent<PracticeCharacter>();
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        PracticeAnimations PA = PM.practiceCharacter.GetComponent<PracticeAnimations>();
        PM.speedCharacter = PM.savedSpeed;
        PM.model.transform.localRotation = PSH.areaAttacks.transform.localRotation;
        PA.Uvard();
        var character =  characterGameObject.GetComponent<PracticeMovement>();
        character.skillOn = true;
        character.LookAttackTarget(PSH.bowTarget.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.6f);
            character.skillOn = false;
        }
        GameObject uvard = Instantiate(PSH.idhunPrefab, PSH.idhunHolderForPos.transform.position, PSH.idhunHolderForPos.transform.rotation);
        PM.primarySkillCasted = false;
        float waitTime = 0;
        DOTween.To(() => waitTime, x => waitTime = x, 360, uvardData.Range)
            .OnUpdate(() => {
                uvard.transform.position += uvard.transform.forward * uvardData.ArrowSpeed * Time.deltaTime;
            }).OnComplete(() =>
            {
                Destroy(uvard);
            });
       
        PSH.bowPrimaries.SetActive(false);
        PSH.uvardImage.SetActive(false);
    }
    

    public void CastIgowuc(IgowucData igowucData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeCharacter PC = PM.practiceCharacter.GetComponent<PracticeCharacter>();
        int firstDodgeChance = PC.dodgeChance;
        PC.dodgeChance = igowucData.DodgeChance;

        StartCoroutine(StopIgowuc());

        IEnumerator StopIgowuc()
        {
            yield return new WaitForSeconds(5f);
            PC.dodgeChance = firstDodgeChance;
        }
    }

    public void CastCigonid(CigonidData cigonidData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeCharacter PC = PM.practiceCharacter.GetComponent<PracticeCharacter>();
        PC.Heal(cigonidData.Heal);
        PC.healthMultiplier = cigonidData.HealMultiplierFromPotions;
        var character =  characterGameObject.GetComponent<PracticeMovement>();
        character.skillOn = true;
        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.1f);
            character.skillOn = false;
        }
        PM.practiceCharacter.GetComponent<PracticeAnimations>().Skill();


        StartCoroutine(StopCigonid());
        IEnumerator StopCigonid()
        {
            yield return new WaitForSeconds(cigonidData.Duration);
            PC.healthMultiplier = 1;
        }
    }
    
    public void SetBlitz(BlitzData blitzData, Joystick usingJoystick)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeCharacter PC = PM.practiceCharacter.GetComponent<PracticeCharacter>();
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        PracticeAnimations PA = PM.practiceCharacter.GetComponent<PracticeAnimations>();
        PracticeCircleInsideCircle PCIC = PracticeCircleInsideCircle.Instance;
        PCIC.joystick = usingJoystick;
        
        
        //PracticeJoystickRotation.Instance.hitArea = PSH.blitzImage;
        //PracticeJoystickRotation.Instance.joystick = usingJoystick;
        PSH.physicalPrimaries.SetActive(true);
        PSH.blitzImage.SetActive(true);
    }
    
    public void CastBlitz(BlitzData blitzData)
    {
        PracticeManager PM = PracticeManager.Instance;
        PracticeCharacter PC = PM.practiceCharacter.GetComponent<PracticeCharacter>();
        PracticeSkillHolder PSH = PM.practiceCharacter.GetComponent<PracticeSkillHolder>();
        PracticeAnimations PA = PM.practiceCharacter.GetComponent<PracticeAnimations>();

        PM.practiceCharacter.GetComponent<PracticeAnimations>().Blitz();

        
        PSH.physicalPrimaries.SetActive(false);
        PSH.blitzImage.SetActive(false);
        var character =  characterGameObject.GetComponent<PracticeMovement>();
        character.skillOn = true;
        character.LookAttackTarget(PSH.blitzTarget.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.6f);
            character.skillOn = false;
        }
        
        Vector3 blitzTargetPos = new Vector3(
            PSH.blitzTarget.transform.position.x,
            character.transform.position.y,
            PSH.blitzTarget.transform.position.z);

        //Destroy(blitz,2);
        
        // character.skillOn = true;
        // character.LookAttackTargetCmd(blitzTargetPos);
        Vector3 BeamPosition =
            PSH.blitzTarget.transform.position;
        BeamPosition = new Vector3(BeamPosition.x, character.transform.position.y, BeamPosition.z);
        characterGameObject.transform.position = BeamPosition;
        
        
        // StartCoroutine(CloseSkillOn());
        // IEnumerator CloseSkillOn()
        // {
        //     yield return new WaitForSeconds(0.5f);
        //     character.skillOn = false;
        // }
    }
}
