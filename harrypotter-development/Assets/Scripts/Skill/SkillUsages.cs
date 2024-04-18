using System;
using System.Collections;
using UnityEngine;
using Mirror;
public class SkillUsages : InstancableNB<SkillUsages>
{
    //PracticeSkills on Practice Screen
    
    public void CastZarn(ZarnData zarnData)
    {
        float firstSpeed = Character.myCharacter.GetComponent<CharacterMovement>().speed;
        float additionToSpeed = firstSpeed * zarnData.MovementSpeedBonus / 100f;
        
        Character.myCharacter.GetComponent<CharacterMovement>().SpeedBoostCmd(firstSpeed, firstSpeed + additionToSpeed,zarnData.Duration);
    }

    public void CastCigonid(CigonidData cigonidData)
    {
        Character.myCharacter.GetComponent<CharacterStats>().HealUpCmd(cigonidData.Heal);
        Character.myCharacter.GetComponent<CharacterStats>().ChangeHealUpMultiplier(cigonidData.HealMultiplierFromPotions,cigonidData.Duration);
    }

    
    public void SetGouge(GougeData gougeData, Joystick usingJoystick)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        JoystickRotation.Instance.hitArea = CSH.swordPrimaries;
        JoystickRotation.Instance.joystick = usingJoystick;
        CSH.swordPrimaries.SetActive(true);
        CSH.gougeImage.SetActive(true);
        //CSH.swordPrimaries.transform.localScale = Vector3.one * gougeData.Range / 9f; // when size gets bigger the area should move further.
    }

    public void CastGouge(GougeData gougeData)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CircleInsideCircle CIC = CircleInsideCircle.Instance;

        //Character.myCharacter.GetComponent<CharacterMovement>().model.transform.localRotation = CSH.swordPrimaries.transform.localRotation;
        var character = Character.myCharacter.GetComponent<CharacterMovement>();
        character.skillOn = true;
        character.LookAttackTargetCmd(CSH.swordTarget.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(1f);
            character.skillOn = false;
        }
        //PM.practiceCharacter.GetComponent<PracticeAnimations>().Gouge(); //Animation
        CastGougeCmd(Character.myCharacter.netIdentity,CSH.swordPrimaries.transform.localRotation);
        CSH.gougeImage.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(StopAndCloseGouge());
        IEnumerator StopAndCloseGouge()
        {
            CSH.swordPrimaries.SetActive(false);
            CSH.gougeImage.SetActive(false);
            yield return new WaitForSeconds(2);
            CSH.gougeImage.GetComponent<BoxCollider>().enabled = false;
        }
    }

    [Command(requiresAuthority = false)]
    public void CastGougeCmd(NetworkIdentity CSH, Quaternion rotation)
    {
        CSH.GetComponent<CharacterSkillHolder>().swordPrimaries.SetActive(true);
        CSH.GetComponent<CharacterSkillHolder>().gougeImage.SetActive(true);
        CSH.GetComponent<CharacterSkillHolder>().swordPrimaries.transform.localRotation = rotation;
        CSH.GetComponent<CharacterSkillHolder>().gougeImage.GetComponent<UseTheSkill>().usingCharacter = CSH.GetComponent<Character>().forCollision;
        StartCoroutine(StopAndCloseGouge());

        IEnumerator StopAndCloseGouge()
        {
            yield return new WaitForSeconds(0.5f);
            CSH.GetComponent<CharacterSkillHolder>().gougeImage.GetComponent<BoxCollider>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            CSH.GetComponent<CharacterSkillHolder>().swordPrimaries.SetActive(false);
            CSH.GetComponent<CharacterSkillHolder>().gougeImage.SetActive(false);
            CSH.GetComponent<CharacterSkillHolder>().gougeImage.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void SetTemblor(TemblorData temblorData, Joystick usingJoystick)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        JoystickRotation.Instance.hitArea = CSH.swordPrimaries;
        JoystickRotation.Instance.joystick = usingJoystick;
        CSH.swordPrimaries.SetActive(true);
        CSH.temblorImage.SetActive(true);
        //CSH.swordPrimaries.transform.localScale = Vector3.one * temblorData.Range / 9f; // when size gets bigger the area should move further.
    }

    public void CastTemblor(TemblorData temblorData)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CircleInsideCircle CIC = CircleInsideCircle.Instance;

        
        //Character.myCharacter.GetComponent<CharacterMovement>().model.transform.localRotation = CSH.swordPrimaries.transform.localRotation;
        // PM.practiceCharacter.GetComponent<PracticeAnimations>().Temblor();
        var character = Character.myCharacter.GetComponent<CharacterMovement>();
        character.skillOn = true;
        character.LookAttackTargetCmd(CSH.swordTarget.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(1f);
            character.skillOn = false;
        }
        CastTemblorCmd(Character.myCharacter.netIdentity,CSH.swordPrimaries.transform.localRotation);

        StartCoroutine(StopAndCloseTemblor());
        IEnumerator StopAndCloseTemblor()
        {
            CSH.temblorImage.SetActive(false);
            yield return new WaitForSeconds(2);
            CSH.swordPrimaries.SetActive(false);

        }
    }

    [Command(requiresAuthority = false)]
    public void CastTemblorCmd(NetworkIdentity CSH, Quaternion rotation)
    {
        CSH.GetComponent<CharacterSkillHolder>().swordPrimaries.SetActive(true);
        CSH.GetComponent<CharacterSkillHolder>().temblorImage.SetActive(true);
        CSH.GetComponent<CharacterSkillHolder>().swordPrimaries.transform.localRotation = rotation;
        StartCoroutine(StopAndCloseTemblor());

        IEnumerator StopAndCloseTemblor()
        {
            yield return new WaitForSeconds(0.5f);
            CSH.GetComponent<CharacterSkillHolder>().temblorImage.GetComponent<BoxCollider>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            CSH.GetComponent<CharacterSkillHolder>().swordPrimaries.SetActive(false);
            CSH.GetComponent<CharacterSkillHolder>().temblorImage.SetActive(false);
            CSH.GetComponent<CharacterSkillHolder>().temblorImage.GetComponent<UseTheSkill>().usingCharacter = CSH.GetComponent<Character>().forCollision;
            CSH.GetComponent<CharacterSkillHolder>().temblorImage.GetComponent<BoxCollider>().enabled = false;
        }
    }
    public void SetGungnir(GungnirData gungnirData)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CircleInsideCircle CIC = CircleInsideCircle.Instance;
        CIC.bigCircle = CSH.gungnirBigCircle.transform;
        CIC.littleCircle = CSH.gungnirSmallCircle.transform;
        CSH.gungnirBigCircle.SetActive(true);
        int bigCircleSize = gungnirData.Range;
        float smallCircleSize = gungnirData.Area;
        float bigCircleFloat = (float)bigCircleSize; 
        float smallCircleFloat = (float)smallCircleSize;
        uint affectedBigCircleSize = Convert.ToUInt16(bigCircleFloat);
        uint affectedSmallCircleSize = Convert.ToUInt16(smallCircleFloat);
        CSH.gungnirBigCircle.transform.localScale = Vector3.one * bigCircleFloat / 24f; // normally /8 but the UI's are 3x
        CSH.gungnirSmallCircle.transform.localScale = Vector3.one * (smallCircleFloat/bigCircleFloat);
        CIC.multiplier = 32 - (((smallCircleFloat / bigCircleFloat) * 36)-4); //32 is when the ratio is 0.1 . When the number goues up the multiplier should goes down. Every 0.1 equals to minus 3.6~4
    }
    public void CastGungnir(GungnirData gungnirData)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CircleInsideCircle CIC = CircleInsideCircle.Instance;

        //After 1 second a big ice piece will emerge in the area

        var character = Character.myCharacter.GetComponent<CharacterMovement>();
        character.skillOn = true;
        character.LookAttackTargetCmd(CIC.littleCircle.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.5f);
            character.skillOn = false;
        }
        Vector3 spawnPos = CIC.littleCircle.transform.position;
        CSH.gungnirBigCircle.SetActive(false);
        StartCoroutine(CastAfterDelay());
        IEnumerator CastAfterDelay()
        {
            yield return new WaitForSeconds(gungnirData.Delay);
            CastGungnirCMD(gungnirData,spawnPos,Character.myCharacter.netIdentity);
        }

        // StartCoroutine(PlayGungnir());
        // IEnumerator PlayGungnir()
        // {
        //     yield return new WaitForSeconds(gungnirData.Delay);
        //     CSH.gungnirParticle.Play();
        //     //deal damage and slow down opponent
        // }
        //
        // StartCoroutine(StopAndCloseGungnir());
        // IEnumerator StopAndCloseGungnir()
        // {
        //     yield return new WaitForSeconds(gungnirData.Delay + gungnirData.RootToPlaceTime);
        //     CSH.gungnirParticle.Stop();
        //     CSH.gungnirBigCircle.SetActive(false);
        //     CIC.skillOn = false;
        //     //PM.skillCasted = false;
        // }
    }

    [Command(requiresAuthority = false)]
    private void CastGungnirCMD(GungnirData gungnirData, Vector3 spawnPos, NetworkIdentity character)
    {
        CastGungnirRpc(gungnirData, spawnPos);
        var gungnir = Instantiate(SkillVisibles.Instance.gungnirServer, spawnPos, Quaternion.identity, transform);
        gungnir.GetComponent<UseTheSkill>().usingCharacter = character.GetComponent<Character>().forCollision;
        gungnir.GetComponent<UseTheSkill>().myCollision = character.GetComponent<Character>().forCollision;

        StartCoroutine(DestroyTheGungnir());
        IEnumerator DestroyTheGungnir()
        {
            yield return new WaitForFixedUpdate();
            gungnir.GetComponent<BoxCollider>().enabled = true;

            yield return new WaitForSeconds(0.5f);
            gungnir.GetComponent<BoxCollider>().enabled = false;
        }
    }
    
    [ClientRpc]
    private void CastGungnirRpc(GungnirData gungnirData, Vector3 spawnPos)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CircleInsideCircle CIC = CircleInsideCircle.Instance;
        print("Client RPC");
        var gungnir = Instantiate(SkillVisibles.Instance.gungnir, spawnPos, Quaternion.identity);
        gungnir.GetComponent<ParticleSystem>().Play();
        //CSH.gungnirBigCircle.SetActive(true);

        StartCoroutine(PlayGungnir());
        IEnumerator PlayGungnir()
        {
            yield return new WaitForSeconds(gungnirData.Delay);
            CSH.gungnirParticle.Play();
            //deal damage and slow down opponent
        }

        StartCoroutine(StopAndCloseGungnir());
        IEnumerator StopAndCloseGungnir()
        {
            yield return new WaitForSeconds(gungnirData.Delay + gungnirData.RootToPlaceTime);
            CSH.gungnirParticle.Stop();
            CSH.gungnirBigCircle.SetActive(false);
            CIC.skillOn = false;
            //PM.skillCasted = false;
        }
    }
    
    public void SetMjollnir(MjollnirData mjollnirData)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CircleInsideCircle CIC = CircleInsideCircle.Instance;
        CIC.bigCircle = CSH.mjollnirBigCircle.transform;
        CIC.littleCircle = CSH.mjollnirSmallCircle.transform;
        CSH.mjollnirBigCircle.SetActive(true);
        int bigCircleSize = mjollnirData.Range;
        float smallCircleSize = mjollnirData.Area;
        float bigCircleFloat = (float)bigCircleSize; 
        float smallCircleFloat = (float)smallCircleSize;
        uint affectedBigCircleSize = Convert.ToUInt16(bigCircleFloat);
        uint affectedSmallCircleSize = Convert.ToUInt16(smallCircleFloat);
        CSH.mjollnirBigCircle.transform.localScale = Vector3.one * bigCircleFloat / 24f; // normally /8 but the UI's are 3x
        CSH.mjollnirSmallCircle.transform.localScale = Vector3.one * (smallCircleFloat/bigCircleFloat);
        CIC.multiplier = 32 - (((smallCircleFloat / bigCircleFloat) * 36)-4); //32 is when the ratio is 0.1 . When the number goues up the multiplier should goes down. Every 0.1 equals to minus 3.6~4
    }
    public void CastMjollnir(MjollnirData mjollnirData)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CircleInsideCircle CIC = CircleInsideCircle.Instance;

        var character = Character.myCharacter.GetComponent<CharacterMovement>();
        character.skillOn = true;
        character.LookAttackTargetCmd(CIC.littleCircle.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.5f);
            character.skillOn = false;
        }
        CSH.mjollnirBigCircle.SetActive(false);
        Vector3 spawnPos = CIC.littleCircle.transform.position;
        StartCoroutine(CastAfterDelay());
        IEnumerator CastAfterDelay()
        {
            yield return new WaitForSeconds(mjollnirData.Delay);
            CastMjollnirCmd(mjollnirData, spawnPos,Character.myCharacter.netIdentity);
        }
        // StartCoroutine(PlayMjollnirFirst());
        // IEnumerator PlayMjollnirFirst()
        // {
        //     yield return new WaitForSeconds(mjollnirData.Delay);
        //     CSH.mjollnirParticleOne.Play();
        //     //DMGAmount1 + StunDuration1
        //     StartCoroutine(PlayMjollnirSecond());
        // }
        //
        // IEnumerator PlayMjollnirSecond()
        // {
        //     yield return new WaitForSeconds(mjollnirData.TimeApart);
        //     CSH.mjollnirParticleTwo.Play();
        //     //DMGAmount2 + StunDuration2
        // }
        //
        // StartCoroutine(StopAndCloseMjollnir());
        // IEnumerator StopAndCloseMjollnir()
        // {
        //     yield return new WaitForSeconds(mjollnirData.Delay + mjollnirData.TimeApart + 2);
        //     CSH.mjollnirBigCircle.SetActive(false);
        //     //PM.skillCasted = false;
        // }
    }

    [Command(requiresAuthority = false)]
    private void CastMjollnirCmd(MjollnirData mjollnirData, Vector3 spawnPos, NetworkIdentity character)
    {
        CastMjollnirRpc(mjollnirData, spawnPos);
        var mjollnir = Instantiate(SkillVisibles.Instance.mjollnirServer, spawnPos, Quaternion.identity, transform);
        mjollnir.transform.GetChild(0).transform.GetChild(0).GetComponent<UseTheSkill>().myCollision = character.GetComponent<Character>().forCollision;
        mjollnir.transform.GetChild(0).transform.GetChild(1).GetComponent<UseTheSkill>().myCollision = character.GetComponent<Character>().forCollision;
        StartCoroutine(PlayMjollnirFirst());
        IEnumerator PlayMjollnirFirst()
        {
            yield return new WaitForSeconds(0+0.7f+0.5f);
            mjollnir.transform.GetChild(0).transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
            //DMGAmount1 + StunDuration1
            StartCoroutine(PlayMjollnirSecond());
        }

        IEnumerator PlayMjollnirSecond()
        {
            yield return new WaitForSeconds(mjollnirData.TimeApart+0.7f+0.5f);
            mjollnir.transform.GetChild(0).transform.GetChild(0).GetComponent<SphereCollider>().enabled = false;
            mjollnir.transform.GetChild(0).transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
            //DMGAmount2 + StunDuration2
            StartCoroutine(DestroyTheMjollnir());
        }

        IEnumerator DestroyTheMjollnir()
        {
            yield return new WaitForSeconds(0.5f);
            mjollnir.transform.GetChild(0).transform.GetChild(0).GetComponent<SphereCollider>().enabled = false;
            mjollnir.transform.GetChild(0).transform.GetChild(0).GetComponent<SphereCollider>().enabled = false;
        }
        //Destroy(mjollnir, mjollnirData.Delay + mjollnirData.TimeApart + 1f);
    }

    [ClientRpc]
    private void CastMjollnirRpc(MjollnirData mjollnirData, Vector3 spawnPos)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CircleInsideCircle CIC = CircleInsideCircle.Instance;
        var mjollnir = Instantiate(SkillVisibles.Instance.mjollnir, spawnPos, Quaternion.identity);
        StartCoroutine(PlayMjollnirFirst());
        IEnumerator PlayMjollnirFirst()
        {
            yield return new WaitForSeconds(0+0.7f);
            mjollnir.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            //CSH.mjollnirParticleOne.Play();
            //DMGAmount1 + StunDuration1
            StartCoroutine(PlayMjollnirSecond());
        }

        IEnumerator PlayMjollnirSecond()
        {
            yield return new WaitForSeconds(mjollnirData.TimeApart+0.7f);
            mjollnir.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            //CSH.mjollnirParticleTwo.Play();
            //DMGAmount2 + StunDuration2
        }

        StartCoroutine(StopAndCloseMjollnir());
        IEnumerator StopAndCloseMjollnir()
        {
            yield return new WaitForSeconds(mjollnirData.Delay + mjollnirData.TimeApart + 2);
            CSH.mjollnirBigCircle.SetActive(false);
            CIC.skillOn = false;
            //PM.skillCasted = false;
        }
    }

    public void SetUvard()
    { 
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CircleInsideCircle CIC = CircleInsideCircle.Instance;
        CSH.bowPrimaries.SetActive(true);
        CSH.uvardImage.SetActive(true);
        CIC.bowPrimaries = CSH.bowPrimaries.transform;
    }
    public void CastUvard()
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CastUvardCmd(Character.myCharacter.netIdentity,Character.opponentCharacter.netIdentity,Character.myCharacter.GetComponent<CharacterSkillHolder>().uvardTarget.transform.position);
        CSH.bowPrimaries.SetActive(false);
        CSH.uvardImage.SetActive(false);
        
        var character = Character.myCharacter.GetComponent<CharacterMovement>();
        character.skillOn = true;
        character.LookAttackTargetCmd(Character.myCharacter.GetComponent<CharacterSkillHolder>().uvardTarget.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.5f);
            character.skillOn = false;
        }
    }

    [Command(requiresAuthority = false)]
    public void CastUvardCmd(NetworkIdentity networkIdentity, NetworkIdentity opponentNetworkIdentity, Vector3 targetPos)
    {
        CircleInsideCircle CIC = CircleInsideCircle.Instance;
        var character = networkIdentity.GetComponent<Character>();
        var arrow = Instantiate(NetworkManager.singleton.spawnPrefabs[6], character.transform.position, Quaternion.identity, transform);
        arrow.GetComponent<UseTheSkill>().myCollision = character.forCollision;
        arrow.GetComponent<BoxCollider>().enabled = true;
        arrow.GetComponent<NetworkMatch>().matchId = character.GetComponent<NetworkMatch>().matchId;
        NetworkServer.Spawn(arrow.gameObject, networkIdentity.connectionToServer);
        arrow.GetComponent<ArrowSkill>().Fire(targetPos);
        
        
        StartCoroutine(DestroyWhenReach());
        IEnumerator DestroyWhenReach()
        {
            yield return new WaitUntil(() => Vector3.Distance(arrow.transform.position,targetPos) < 0.2f);
            Destroy(arrow);
        }
    }
    public void SetIdhun()
    { 
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CircleInsideCircle CIC = CircleInsideCircle.Instance;
        CSH.bowPrimaries.SetActive(true);
        CSH.idhunImage.SetActive(true);
        CIC.bowPrimaries = CSH.bowPrimaries.transform;
    }
    public void CastIdhun()
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CastIdhunCmd(Character.myCharacter.netIdentity,Character.opponentCharacter.netIdentity,Character.myCharacter.GetComponent<CharacterSkillHolder>().idhunTarget.transform.position);
        CSH.bowPrimaries.SetActive(false);
        CSH.idhunImage.SetActive(false);
        
        var character = Character.myCharacter.GetComponent<CharacterMovement>();
        character.skillOn = true;
        character.LookAttackTargetCmd(Character.myCharacter.GetComponent<CharacterSkillHolder>().uvardTarget.transform.position);

        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.5f);
            character.skillOn = false;
        }
    }

    [Command(requiresAuthority = false)]
    public void CastIdhunCmd(NetworkIdentity networkIdentity, NetworkIdentity opponentNetworkIdentity, Vector3 targetPos)
    {
        var character = networkIdentity.GetComponent<Character>();
        var arrow = Instantiate(NetworkManager.singleton.spawnPrefabs[7], character.transform.position, Quaternion.identity, transform);
        arrow.GetComponent<UseTheSkill>().myCollision = character.forCollision;
        arrow.GetComponent<BoxCollider>().enabled = true;
        arrow.GetComponent<NetworkMatch>().matchId = character.GetComponent<NetworkMatch>().matchId;
        NetworkServer.Spawn(arrow.gameObject, networkIdentity.connectionToServer);
        arrow.GetComponent<ArrowSkill>().Fire(targetPos);

        StartCoroutine(DestroyWhenReach());
        IEnumerator DestroyWhenReach()
        {
            yield return new WaitUntil(() => Vector3.Distance(arrow.transform.position,targetPos) < 0.2f);
            Destroy(arrow);
        }
    }

    public void SetBlitz(BlitzData blitzData, Joystick usingJoystick)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CircleInsideCircle CIC = CircleInsideCircle.Instance;
        JoystickRotation.Instance.hitArea = CSH.blitzImage;
        JoystickRotation.Instance.joystick = usingJoystick;
        CSH.physicalPrimaries.SetActive(true);
        CSH.blitzImage.SetActive(true);
    }

    public void CastBlitz(BlitzData blitzData)
    {
        CharacterSkillHolder CSH = Character.myCharacter.GetComponent<CharacterSkillHolder>();
        CSH.physicalPrimaries.SetActive(false);
        CSH.blitzImage.SetActive(false);
        var character = Character.myCharacter.GetComponent<CharacterMovement>();
        Vector3 blitzTargetPos = new Vector3(
            Character.myCharacter.GetComponent<CharacterSkillHolder>().blitzTarget.transform.position.x,
            character.transform.position.y,
            Character.myCharacter.GetComponent<CharacterSkillHolder>().blitzTarget.transform.position.z);
        CastBlitzCmd(character.transform.position, blitzTargetPos);
        
        //Destroy(blitz,2);
        character.skillOn = true;
        character.LookAttackTargetCmd(blitzTargetPos);
        Vector3 BeamPosition =
            Character.myCharacter.GetComponent<CharacterSkillHolder>().blitzTarget.transform.position;
        BeamPosition = new Vector3(BeamPosition.x, character.transform.position.y, BeamPosition.z);
        print("Beam Position: " + BeamPosition);
        character.BeamUp(BeamPosition);
        
        
        StartCoroutine(CloseSkillOn());
        IEnumerator CloseSkillOn()
        {
            yield return new WaitForSeconds(0.5f);
            character.skillOn = false;
        }
    }

    [Command(requiresAuthority = false)]
    public void CastBlitzCmd(Vector3 charPos, Vector3 targetPos)
    {
        CastBlitzRpc(charPos, targetPos);
    }

    [ClientRpc]
    public void CastBlitzRpc(Vector3 charPos, Vector3 targetPos)
    {
        var blitz = Instantiate(SkillVisibles.Instance.blitz, charPos, Quaternion.identity);
        blitz.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        blitz.transform.LookAt(targetPos);
    }
}
