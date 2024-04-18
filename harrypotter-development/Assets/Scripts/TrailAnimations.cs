using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TrailAnimations : MonoBehaviour
{
    
    void Start()
    {
        transform.DOLocalMoveX(1, 0.4f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.Linear);
    }

    
}
