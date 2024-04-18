using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Firebase;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class Character : NetworkBehaviour
{
    public Player myPlayer;

    public static Character myCharacter;
    public static Character opponentCharacter;

    public CharacterStats stats;

    public CharacterActionManager actionManager;
    public CharacterAnimationManager animationManager;
    public CharacterModelManager modelManager;
    public CharacterMovement charMovement;

    public Character reachableMyCharacter, reachableOpponentCharacter;
    public HealthBarSelect hbs;

    public Transform canvas;
    public GameObject forCollision;
    
    public SyncList<int> selectedSkillImages = new SyncList<int>();

    public override void OnStartClient()
    {
        base.OnStartClient();
        Destroy(GetComponent<Rigidbody>());
        if (hasAuthority)
        {
            myCharacter = this;
            hbs.InitializeHealthBar(true);
            reachableMyCharacter = this;
            ClientManager.Instance.myCharacter = gameObject;
            //Camera.main.GetComponent<CameraFollow>().character = gameObject;
            CameraManager.Instance.SetFollowObject(transform);
            
            HealthUIManager.Instance.playerHB = myCharacter.gameObject.transform.Find("Canvas").gameObject.transform
                .Find("MyHealth").GetComponent<Slider>();

            
            
                // Vector3 holdPos = Camera.main.transform.localPosition;
            // Camera.main.transform.localPosition = new Vector3(0, holdPos.y, holdPos.z);
        }
        else
        {
            opponentCharacter = this;
            hbs.InitializeHealthBar(false);
            ClientManager.Instance.opponentCharacter = gameObject;
            reachableOpponentCharacter = this;
            HealthUIManager.Instance.opponentHB = opponentCharacter.gameObject.transform.Find("Canvas").gameObject.transform
                .Find("MyHealth").GetComponent<Slider>();
        }
    }

    [Command(requiresAuthority = false)]
    internal void SetSkillImageIndexes(List<int> skillImageIndexes)
    {
        foreach (var skillImageIndex in skillImageIndexes)
        {
            selectedSkillImages.Add(skillImageIndex);
        }
    }
    
    

    [Server]
    public void TakeDamage(int _dmgAmount, HealthChangeReason changeReason)
    {
        if (actionManager.isBlocking)
        {
            BlockingFX();
            return;
        }
        
        
        actionManager.DamageIndicators();

        stats.ChangeHealth(-_dmgAmount, changeReason);
        actionManager.OnTakeDamage(_dmgAmount);
    }

    [ClientRpc]
    void BlockingFX()
    {
        SoundManager.Instance.DoSFX(
            SoundManager.PickRandomCategory(
                SoundCategory.ShieldBlock, SoundCategory.WeaponBlock)); //TODO: Differentiate between Shield vs Weapon
    }

    [ClientRpc]
    public void RenderDamageVFX(int damage)
    {
        var CMM = GetComponent<CharacterModelManager>();
        CMM.vikings[CMM.modelId].transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMaterial.color = Color.red;
        CMM.vikings[CMM.modelId].transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMaterial.DOColor(Color.white, 0.5f);

        if (hasAuthority)
        {
            VFXManager.Instance.DamageVFX(damage, myCharacter);
        }
        else
        {
            VFXManager.Instance.OpponentDamageVFX(damage, opponentCharacter);
        }
    }
    

    [Server]
    public void GameOver()
    {
        GameOverRPC(false);
        myPlayer.opponentCharacter.GameOverRPC(true);
    }
    
    [ClientRpc]
    private void GameOverRPC(bool isWin)
    {
        if (!hasAuthority)
        {
            return;
        }
        GameOverUIManager.Instance.ShowGameOverScreen(isWin);
        FirebaseDataManager.Instance.MyPlayerData.Experience += isWin ? 100 : 50;
        FirebaseDataManager.Instance.UpdateMyPlayerDataExperience();
    }
}
