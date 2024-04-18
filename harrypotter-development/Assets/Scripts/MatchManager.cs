using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MatchManager : NetworkBehaviour
{
    public List<GameObject> obstaclePrefab;
    
    [Command]
    public void SpawnObstacle(int index, Vector3 position, Vector3 rotation, Vector3 scale, Vector3 colCenter, Vector3 colSize)
    {
        
    }
}
