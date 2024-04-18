    using System;
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using UnityEngine.SceneManagement;

    public class VillageBuilding : MonoBehaviour
    {
        public GameObject buildingPanel;

        private void OnMouseUp()
        {
            buildingPanel.SetActive(true);
        }

        public void OnTapPlay()
        {
            SceneManager.LoadScene("Offline");
        }
    }
