using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using DG.Tweening;

public class Axe : NetworkBehaviour
{
    public NetworkIdentity owner;
    
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float throwSpeed;

    public float timePassed;
    private DateTime timeSpawned;
    private TimeSpan lifeSpan = TimeSpan.FromSeconds(1.3);

    private TimeSpan Age => DateTime.Now - timeSpawned;
    private bool ShouldDie => Age >= lifeSpan;

    [SyncVar] private Vector3 target;
    public float rotateSpeed;
    public bool shouldRotate;
    public GameObject theAttack;
    public bool zoneFire;
    private void Start()
    {
        SoundManager.Instance.DoSFX(SoundCategory.MagicProjectileSwoosh); //TODO: Differantiate between metal and magic
        
        if (!isServer)
        {
            Destroy(rb);
        }
        else
        {
            SetOwnerOnClient(owner);
        }

        timeSpawned = DateTime.Now;
    }
    

    private void Update()
    {
        //transform.Rotate(Vector3.right * rotateSpeed);
        
        if (isServer)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= 0.033333f)
            {
                timePassed = 0;
                UpdatePositionAndRotationOnClient(transform.position, transform.eulerAngles);
            }
            if (ShouldDie)
            {
                Destroy(gameObject);
            }
            print("Distance: " + Vector3.Distance(target, transform.position));
        }
    }

    [ClientRpc]
    public void SetOwnerOnClient(NetworkIdentity identity)
    {
        owner = identity;
        
        if (owner == Character.myCharacter.netIdentity)
        {
            CameraManager.Instance.ShakeCamera(ShakeIntensity.Low, ShakeTime.Instant, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isServer && other.CompareTag("CharacterGraphic") && other.GetComponentInParent<NetworkIdentity>() != owner)
        {
            var damageAmt = transform.GetChild(PlayerPrefs.GetInt("RangedId", 1)).gameObject.GetComponent<SkillType>().damageAmount;
            damageAmt = 10; //temporarily
            other.GetComponentInParent<Character>().TakeDamage(damageAmt, HealthChangeReason.MagicDamage);//TODO: Differantiate between magic and metal
            other.GetComponentInParent<Character>().RenderDamageVFX(damageAmt);

            if(isServer && theAttack.GetComponent<SkillType>().typeOfSkill != SkillType.TypeOfSkill.zone)
                Destroy(gameObject);
        }

        if (other.CompareTag("Obstacle") && transform.GetChild(Character.myCharacter.GetComponent<CharacterModelManager>().rangedId).GetComponent<SkillType>().typeOfSkill == SkillType.TypeOfSkill.throwable)
        {
            Destroy(gameObject);
        }
    }

    [Server]
    public void Fire(Vector3 _target)
    {
        // gameObject.AddComponent<BoxCollider>().
        // GetComponent<Rigidbody>().isKinematic = false;
        //GetComponent<Collider>().enabled = true;
        target = _target;
        rb.velocity = (_target - transform.position).normalized * throwSpeed;
        transform.LookAt(rb.velocity);
        UpdatePositionAndRotationOnClient(transform.position, transform.eulerAngles);
    }

    [Server]
    public void ZoneFire()
    {
        //GetComponent<Collider>().enabled = true;
        UpdatePositionAndRotationOnClient(transform.position, transform.eulerAngles);
        zoneFire = true;
    }

    [ClientRpc]
    private void UpdatePositionAndRotationOnClient(Vector3 _newPos, Vector3 _newRot)
    {
        transform.position = _newPos;
        transform.eulerAngles = _newRot;
    }

    [ClientRpc]
    public void OpenGraphic(int graphicIndex)
    {
        if (graphicIndex == 1)
            shouldRotate = true;
        
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        transform.GetChild(graphicIndex).gameObject.SetActive(true);

        StartCoroutine(OpenTrigger());
        IEnumerator OpenTrigger()
        {
            yield return new WaitForSeconds(0.1f);
            transform.GetChild(graphicIndex).GetComponent<SphereCollider>().isTrigger = true;
        }
    }

    [Server]
    public void OpenGraphicInServer(int graphicIndex)
    {
        
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        transform.GetChild(graphicIndex).gameObject.SetActive(true);

        StartCoroutine(OpenTrigger());
        IEnumerator OpenTrigger()
        {
            yield return new WaitForSeconds(0.1f);
            transform.GetChild(graphicIndex).GetComponent<SphereCollider>().isTrigger = true;
        }
    }
    
    private void OnDestroy()
    {
        if(isServer)
            return;
        if (owner == Character.myCharacter.netIdentity)
        {
            SoundManager.Instance.DoSFX(SoundCategory.MagicProjectileHit); //TODO: Differantiate between metal and magic
            CameraManager.Instance.ShakeCamera(ShakeIntensity.Mid, ShakeTime.Quick);
        }
    }
}
