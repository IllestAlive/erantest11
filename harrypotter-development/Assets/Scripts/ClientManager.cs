using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : Instancable<ClientManager>
{
    public GameObject myCharacter, opponentCharacter;
    public bool isCharacterInitialized => myCharacter && opponentCharacter;
    public static event Action CharactersInitialized = delegate {  };
    private bool initializingActionCalled;

    private Vector3 middlePointOfArena = Vector3.zero;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => isCharacterInitialized);
        if(myCharacter.transform.position.z > middlePointOfArena.z)
            myCharacter.transform.GetChild(0).transform.rotation = Quaternion.Euler(0,180,0);
        if (opponentCharacter.transform.position.z > middlePointOfArena.z)
            opponentCharacter.transform.GetChild(0).transform.rotation = Quaternion.Euler(0,180,0);
    }

    private void Update()
    {
        if (initializingActionCalled)
            return;
        if (isCharacterInitialized)
        {
            CharactersInitialized();
            initializingActionCalled = true;
        }
    }
}
