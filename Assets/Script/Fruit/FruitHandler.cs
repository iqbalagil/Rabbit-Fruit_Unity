using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FruitHandler : MonoBehaviour , IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Asset has been placed");
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
