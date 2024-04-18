using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VillageMode.Internal
{
    public class NonDestroyable : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
