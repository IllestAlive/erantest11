using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class HealthRegenerateTimer : InstancableNB<HealthRegenerateTimer>
{
    public float[] regenerateTimers = new float[2];
    public int healthRegenerateAmount = 20;
    public int tickDelay = 1;
    public float countDownTime = 4;

    public List<NetworkIdentity> characters = new();
    public override void OnStartServer()
    {
        base.OnStartServer();
        Array.Fill(regenerateTimers, countDownTime);
    }

    // private IEnumerator Start()
    // {
    //     yield return new WaitUntil(() => GetComponent<NetworkIdentity>() != null );
    //     for (int i = 0; i < regenerateTimers.Length; i++)
    //     {
    //         regenerateTimers[i] = 4;
    //     }
    // }

    public override void OnStartClient()
    {
        base.OnStartClient();
        StartCoroutine(WaitForMyCharacter());

        IEnumerator WaitForMyCharacter()
        {
            yield return new WaitUntil(() => Character.myCharacter != null );
            AddCharacter(Character.myCharacter.netIdentity);
        }
        SetFour();
    }

    // private void Update()
    // {
    //     if(isServer) Debug.LogError($"Regen[0] = {regenerateTimers[0]}");
    //     if(isServer) Debug.LogError($"Regen[1] = {regenerateTimers[1]}");
    // }

    [Command]
    public void SetFour()
    {
        regenerateTimers[0] = 4;
        regenerateTimers[1] = 4;
    }
    
    [Command(requiresAuthority = false)]
    public void AddCharacter(NetworkIdentity playerId)
    {
        characters.Add(playerId);
        if(characters.Count == 2) StartTimer();
    }

    public void StartTimer()
    {
        if (isServer)
        {
            StartCoroutine(Timer(tickDelay));
        }
    }
    
    IEnumerator Timer(float tickDelay)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(tickDelay);
            //Tick();
        }
    }

    void Tick()
    {
        for (int i = 0; i < 2; i++)
        {
            if (regenerateTimers[i] > 0)
            {
                regenerateTimers[i]--;
            }
            else
            {
                HealPlayer(i);
            }
        }
    }

    [Server]
    void HealPlayer(int index)
    {
        var character = characters[index];
        
        character.GetComponent<CharacterStats>().ChangeHealth(healthRegenerateAmount, HealthChangeReason.Regenerate);
    }

    [Server]
    public void ResetTimer(int index)
    {
        print("resetting timer to: " + countDownTime);
        regenerateTimers[index] = countDownTime;
        print($"new timer: {regenerateTimers[index]} || cdt: {countDownTime}");
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        ClientManager.CharactersInitialized -= StartTimer;
    }
}
