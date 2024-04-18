using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParticleController : MonoBehaviour
{
    public GameObject activeParticle;

    public IEnumerator ActivateForSeconds(float waitTime, float time)
    {
        yield return new WaitForSeconds(waitTime);
        if (activeParticle == null)
        {
            yield break;
        }
        
        activeParticle.SetActive(true);
        StartCoroutine(ActivateRoutine());
        
        IEnumerator ActivateRoutine()
        {
            yield return new WaitForSeconds(time);
            activeParticle.SetActive(false);
        }
    }
}
