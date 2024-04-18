using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public enum CollectType { Speed,Health };

    public CollectType collectType;
    public int index;
}
