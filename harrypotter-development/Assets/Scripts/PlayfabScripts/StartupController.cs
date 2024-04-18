using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupController : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Configuration.Instance.IsServer)
        {
            gameObject.AddComponent<ServerManagerBehaviour>();
        }
        else
        {
            gameObject.AddComponent<ClientManagerBehaviour>();
        }
    }
}
