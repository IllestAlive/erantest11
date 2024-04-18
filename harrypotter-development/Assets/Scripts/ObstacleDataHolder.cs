using UnityEngine;

public class ObstacleDataHolder : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public Vector3 colliderCenter;
    public Vector3 colliderSize;

    public void InitializeObstacle()
    {
        position = transform.position;
        rotation = transform.eulerAngles;
        scale = transform.localScale;
        
        var col = GetComponent<BoxCollider>();

        colliderCenter = col.center;
        colliderSize = col.size;
    }
}