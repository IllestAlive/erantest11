using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectManager : Instancable<CollectManager>
{
    public List<GameObject> collects;

    public GameObject spawnManager;

    // private void Start()
    // {
    //     Instantiate(spawnManager, Vector3.zero, Quaternion.identity);
    // }
}
