using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : InstancableNB<SpawnManager>
{
    public Vector3 spawnPoint = new Vector3(-5,0,4);
    public List<GameObject> spawnableObjects;
    public List<GameObject> spawnedObjects;
    

    private void Start()
    {
        StartCoroutine(SpawnTheObject());
    }

    IEnumerator SpawnTheObject()
    {
        yield return new WaitForSeconds(30f);
        int randomNumber = Random.Range(0, 2);
        
        Vector3 randV = new Vector3(Random.Range(-7f, 7f), 0.25f, Random.Range(-13f, 13f));
        SpawnObject(Random.Range(1,2),randV);
        StartCoroutine(SpawnTheObject());
    }


    [Server]
    public void SpawnObject(int spawnIndex, Vector3 randV)
    {
        GameObject spawnedObject = Instantiate(spawnableObjects[spawnIndex], randV, Quaternion.Euler(new Vector3(0,Random.Range(0f,180f),0)), transform);
        SpawnOnClient(spawnIndex,randV);
        spawnedObjects.Add(spawnedObject);
        spawnedObject.GetComponent<Collect>().index = spawnedObjects.Count - 1;
    }

    [ClientRpc]
    public void SpawnOnClient(int spawnIndex,Vector3 randV)
    {
        GameObject spawnedObject = Instantiate(spawnableObjects[spawnIndex], randV, Quaternion.Euler(new Vector3(0,Random.Range(0f,180f),0)));
        spawnedObjects.Add(spawnedObject);
        //spawnedObject.GetComponent<MeshRenderer>().enabled = true;
        spawnedObject.GetComponent<Collect>().index = spawnedObjects.Count - 1;
    }
}
