using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : Instancable<LocationManager>
{
    public bool isFirstPlayerLocated;
    public Vector3 firstPlayerSpawnPoint;
    public Vector3 secondPlayerSpawnPoint;
}
