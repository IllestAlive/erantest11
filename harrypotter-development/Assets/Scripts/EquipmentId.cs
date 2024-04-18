using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipmentId : MonoBehaviour
{
    public int id;
    public GameObject particle;

    public void CheckId(int checkedId)
    {
        print("checked id:" + checkedId);
        print("id: " + id);
        gameObject.SetActive(checkedId == id);

        if (SceneManager.GetActiveScene().name == "Game" && checkedId == id && particle != null)
        {
            GetComponentInParent<WeaponParticleController>().activeParticle = particle;
            particle.SetActive(false);
        }
    }
    
    public void CheckIdForPanel(int checkedId)
    {
        gameObject.SetActive(checkedId == id);
    }
    
}
