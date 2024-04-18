using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offSet;
    public GameObject character;
    public bool isCameraWithCharacter;
    private Vector3 targetPos;
    public bool shouldLerp;

    private void LateUpdate()
    {
        if (character != null)
        {
            if(!isCameraWithCharacter)
                targetPos = new Vector3(0,
                    character.transform.position.y + offSet.y,
                    character.transform.position.z + offSet.z);
            if(isCameraWithCharacter)
                targetPos = new Vector3(character.transform.position.x,
                    character.transform.position.y + offSet.y,
                    character.transform.position.z + offSet.z);
            
    
            if (shouldLerp)
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.smoothDeltaTime * 5);
            if (!shouldLerp)
                transform.position = targetPos;
        }
    }
}
