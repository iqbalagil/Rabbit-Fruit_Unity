using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CartManager : MonoBehaviour
{
    public GameObject loc1, loc2, loc3;
    private Vector3 _originalParentPosition, _originalParentPosition1, _originalParentPosition2;
    private void Start()
    {
        _originalParentPosition =  loc1.transform.position;
        _originalParentPosition1 = loc2.transform.position;
        _originalParentPosition2 = loc3.transform.position;
    }

    public IEnumerator BackToOriginalPosition(int delay)
    {
        yield return new WaitForSeconds(delay);
        
    }
}
