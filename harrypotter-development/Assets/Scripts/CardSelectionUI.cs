using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectionUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SkillCard skillCard;
    public Vector3 originalPosition;
    
    private void Start()
    {
        StartCoroutine(GetPosition());
        IEnumerator GetPosition()
        {
            yield return new WaitForSeconds(0.1f);
            originalPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        OfflineUIManager.Instance.holdingCard = skillCard;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(Fit());

        IEnumerator Fit()
        {
            yield return new WaitForSeconds(0.01f);
            GetComponent<RectTransform>().anchoredPosition = originalPosition;
            OfflineUIManager.Instance.holdingCard = skillCard;
        }

    }
}
