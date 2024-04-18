using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloatingText : MonoBehaviour
{
    private float destroyTime = 3;

    private Vector3 offSet = new Vector3(0, 3, 0);
    private Vector3 randomizeIntensity = new Vector3(0.2f, 0, 0);

    private void Start()
    {
        Destroy(gameObject,destroyTime);
        transform.localPosition += offSet;
        transform.localPosition += new Vector3(Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
            Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
            Random.Range(-randomizeIntensity.z, randomizeIntensity.z));
    }
}
