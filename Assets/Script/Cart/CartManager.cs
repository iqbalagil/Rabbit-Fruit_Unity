using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CartManager : MonoBehaviour
{
    public GameObject loc1, loc2, loc3;
    private Vector3 _originalParentPosition, _originalParentPosition1, _originalParentPosition2;
    public float delayDuration;
    private void Start()
    {
        _originalParentPosition =  loc1.transform.position;
        _originalParentPosition1 = loc2.transform.position;
        _originalParentPosition2 = loc3.transform.position;
    }

    public void BackToPosition()
    {
        StartCoroutine(BackToOriginalPosition(delayDuration));
    }

    public IEnumerator BackToOriginalPosition(float delay)
    {
        yield return new WaitForSeconds(delay);
        loc1.transform.position = _originalParentPosition;
        loc2.transform.position = _originalParentPosition1;
        loc3.transform.position = _originalParentPosition2;
    }
}
