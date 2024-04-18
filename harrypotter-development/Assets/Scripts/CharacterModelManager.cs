using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Mirror;
using UnityEngine;
using VillageMode.Internal;

public class CharacterModelManager : NetworkBehaviour
{
    public List<GameObject> vikings = new List<GameObject>();
    public List<GameObject> nonCustomizedVikings = new List<GameObject>();
    public List<GameObject> originalVikings = new List<GameObject>();
    public GameObject originalCharacter;
    [SyncVar(hook = nameof(ReceiveModelIDFromServer))] public int modelId;
    [SyncVar(hook = nameof(ReceiveRangedIDFromServer))] public int rangedId;
    [SyncVar(hook = nameof(ReceiveEMailFromServer))] public string email;

    public GameObject redModel;
    public GameObject greenModel;
    private IEnumerator Start()
    {
        if (hasAuthority)
        {
            greenModel.SetActive(true);
            redModel.SetActive(false);
            if(UIManager.Instance.SelectedPlayType == UIManager.PlayType.customizedCharacters)
                modelId = PlayerPrefs.GetInt("CharId", 0);

            if (UIManager.Instance.SelectedPlayType == UIManager.PlayType.nonCustomizedCharacters)
                modelId = FirebaseDataManager.Instance.MyPlayerData.CommanderNFT.CommanderID;

            if (UIManager.Instance.SelectedPlayType == UIManager.PlayType.OriginalCharacters)
                modelId = 0;

            SetModelIDOnServer(modelId);
            
            if(UIManager.Instance.SelectedPlayType == UIManager.PlayType.customizedCharacters)
            {
                vikings[modelId].SetActive(true);
                GetComponent<CharacterAnimationManager>().charAnimator = vikings[modelId].GetComponent<Animator>();
            }
            
            if(UIManager.Instance.SelectedPlayType == UIManager.PlayType.nonCustomizedCharacters)
            {
                nonCustomizedVikings[modelId].SetActive(true);
                GetComponent<CharacterAnimationManager>().charAnimator = nonCustomizedVikings[modelId].GetComponent<Animator>();
                // GetComponent<CharacterAnimationManager>().charAnimator = originalCharacter.GetComponent<Animator>();
            }

            if (UIManager.Instance.SelectedPlayType == UIManager.PlayType.OriginalCharacters)
            {
                originalVikings[modelId].SetActive(true);
                GetComponent<CharacterAnimationManager>().charAnimator =
                    originalVikings[modelId].GetComponent<Animator>();
            }
            
            rangedId = PlayerPrefs.GetInt("RangedId", 1);
            SetRangedIDOnServer(rangedId);
            GetComponent<CharacterActionManager>().attackingThing = rangedId;

            email = AuthManager.EMail;
            
            FirebaseDataManager FDM = FirebaseDataManager.Instance;
            SetEMailOnServer(email);

            yield return new WaitUntil(() =>
                !String.IsNullOrEmpty(ClientManager.Instance.opponentCharacter.GetComponent<CharacterModelManager>()
                    .email));
            string opponentEmail = ClientManager.Instance.opponentCharacter.GetComponent<CharacterModelManager>().email;
            print("OPPONENT EMAIL: " + opponentEmail);
            FDM.InitializeEnemyData(opponentEmail);

            if (UIManager.Instance.SelectedPlayType == UIManager.PlayType.nonCustomizedCharacters)
            {
                foreach (var data in FDM.MyPlayerData.EquipmentSlot)
                {
                    if (data.EquipmentID == -1)
                        continue;
                    nonCustomizedVikings[modelId].GetComponent<CharacterEquipments>()
                        .EquipCharacter((EquipmentSlot)data.SlotID, data.EquipmentID);
                }

                yield return new WaitUntil(() => FDM.EnemyPlayerData != null);
                foreach (var data in FDM.EnemyPlayerData.EquipmentSlot)
                {
                    if (data.EquipmentID == -1)
                        continue;
                    ClientManager.Instance.opponentCharacter.GetComponent<CharacterModelManager>()
                        .nonCustomizedVikings[FDM.EnemyPlayerData.CommanderNFT.CommanderID]
                        .GetComponent<CharacterEquipments>()
                        .EquipCharacter((EquipmentSlot)data.SlotID, data.EquipmentID);

                }
            }

            if (UIManager.Instance.SelectedPlayType == UIManager.PlayType.OriginalCharacters)
            {
                foreach (var data in FDM.MyPlayerData.EquipmentSlot)
                {
                    if (data.EquipmentID == -1)
                        continue;
                    originalVikings[modelId].GetComponent<CharacterEquipments>()
                        .EquipCharacter((EquipmentSlot)data.SlotID, data.EquipmentID);
                }

                yield return new WaitUntil(() => FDM.EnemyPlayerData != null);
                foreach (var data in FDM.EnemyPlayerData.EquipmentSlot)
                {
                    if (data.EquipmentID == -1)
                        continue;
                    ClientManager.Instance.opponentCharacter.GetComponent<CharacterModelManager>()
                        .originalVikings[modelId]
                        .GetComponent<CharacterEquipments>()
                        .EquipCharacter((EquipmentSlot)data.SlotID, data.EquipmentID);

                }
            }


        }

        else
        {
            redModel.SetActive(true);
            greenModel.SetActive(false);
        }
    }


    [Command]
    public void SetModelIDOnServer(int _modelId)
    {
        modelId = _modelId;
    }

    [Command]
    public void SetRangedIDOnServer(int _rangedId)
    {
        rangedId = _rangedId;
    }

    [Command]
    public void SetEMailOnServer(string _email)
    {
        email = _email;
    }

    
    public void ReceiveModelIDFromServer(int _old, int _new)
    {
        if (!hasAuthority)
        {
            if(UIManager.Instance.SelectedPlayType == UIManager.PlayType.customizedCharacters)
            {
                vikings[modelId].SetActive(true);
                GetComponent<CharacterAnimationManager>().charAnimator = vikings[modelId].GetComponent<Animator>();
            }
            
            if(UIManager.Instance.SelectedPlayType == UIManager.PlayType.nonCustomizedCharacters)
            {
                nonCustomizedVikings[modelId].SetActive(true);
                GetComponent<CharacterAnimationManager>().charAnimator = nonCustomizedVikings[modelId].GetComponent<Animator>();
            }

            if (UIManager.Instance.SelectedPlayType == UIManager.PlayType.OriginalCharacters)
            {
                originalVikings[modelId].SetActive(true);
                GetComponent<CharacterAnimationManager>().charAnimator = originalVikings[modelId].GetComponent<Animator>();
            }

        }
    }
    public void ReceiveRangedIDFromServer(int _old, int _new)
    {
        if (!hasAuthority)
        {
            rangedId = GetComponent<CharacterActionManager>().attackingThing;
        }
    }

    public void ReceiveEMailFromServer(string _old, string _new)
    {
        // if (!hasAuthority)
        // {
        //     email = _new;
        // }
    }
    
    
}
