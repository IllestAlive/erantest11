using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ArrowSkill : NetworkBehaviour
{
    private float timePassed;
    public bool uvard, idhun;
    public int speed;
    private void Update()
    {
        
        if (isServer)
        {            
            SkillData skillDataUvard = SkillFetcher._skillDatas[19];
            var uvardData = skillDataUvard.GetSkillEffectConverted<UvardData>();
            SkillData skillDataIdhun = SkillFetcher._skillDatas[10];
            var idhunData = skillDataUvard.GetSkillEffectConverted<IdhunData>();
            if (uvard)
                speed = uvardData.ArrowSpeed;
            if (idhun)
                speed = idhunData.ArrowSpeed;
            transform.position += transform.forward * Time.deltaTime * uvardData.ArrowSpeed;
            timePassed += Time.deltaTime;
            if (timePassed >= 0.033333f)
            {
                timePassed = 0;
                UpdatePositionAndRotationOnClient(transform.position, transform.eulerAngles);
            }
        }
    }

    [ClientRpc]
    void UpdatePositionAndRotationOnClient(Vector3 pos, Vector3 rot) 
    {
        transform.position = pos;
        transform.eulerAngles = rot;
    }

    [Server]
    public void Fire(Vector3 target)
    {
        transform.LookAt(target);
        UpdatePositionAndRotationOnClient(transform.position,transform.eulerAngles); 
    }
    
}
