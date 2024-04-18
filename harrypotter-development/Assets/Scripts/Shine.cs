using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shine : MonoBehaviour
{
    public void Bright()
    {
        if (OfflineUIManager.Instance.randomizeButton.activeSelf)
            return;
        transform.GetChild(0).gameObject.SetActive(true);
        EquipmentManager.Instance.activePanelButton = gameObject;
    }
}
