using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ArrowRendering : MonoBehaviour
{
    [SerializeField] private LineRenderer arrowLine;
    [SerializeField] private LayerMask hitMask;
    private bool isArrowEnabled => arrowLine.enabled;
    public List<Joystick> rangedAttackJoysticks;
    public static event Action OnArrowEnabled = delegate {  }; 

    internal Vector3 hitPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponentInParent<NetworkIdentity>().hasAuthority)
        {
            Destroy(gameObject);
        }
        else
        {
            for (int i = 0; i < UIManager.Instance.rangedAttackJoysticks.Count; i++)
            {
                rangedAttackJoysticks.Add(UIManager.Instance.rangedAttackJoysticks[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isArrowEnabled)
        {
            /*if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity,hitMask))
            {
                arrowLine.SetPosition(0, transform.position);
                var limitedOriginToTargetVector = Vector3.ClampMagnitude(hit.point + Vector3.up -  transform.position, 8);
                var limitedHitPoint = transform.position + limitedOriginToTargetVector;
                arrowLine.SetPosition(1, limitedHitPoint);
                
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponentInParent<Character>().animationManager
                        .SetAnimationStateCMD(CharacterAnimationState.RangedAttack);
                    var hitPoint = new Vector3(limitedHitPoint.x, 2, limitedHitPoint.z);
                    GetComponentInParent<CharacterActionManager>().RequestSpawnAxe(hitPoint);
                }
            }*/
            for (int i = 0; i < UIManager.Instance.rangedAttackJoysticks.Count; i++)
            {
                if (rangedAttackJoysticks[i].Horizontal != 0 || rangedAttackJoysticks[i].Vertical != 0)
                {
                    arrowLine.SetPosition(0, transform.position + Vector3.up * .01f);

                    Vector3 direction = new Vector3(rangedAttackJoysticks[i].Horizontal, 0, rangedAttackJoysticks[i].Vertical);

                    hitPoint = transform.position + Vector3.up * .01f + direction * 10;

                    arrowLine.SetPosition(1, transform.position + Vector3.up * .01f + direction * 10);
                }
            }
            
        }
    }

    public void EnableArrow()
    {
        OnArrowEnabled();
        arrowLine.enabled = true;
    }

    public void DisableArrow()
    {
        arrowLine.enabled = false;
    }
}
