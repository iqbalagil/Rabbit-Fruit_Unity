using System.Collections;
using UnityEngine;

public class ChangePosition : MonoBehaviour
{
    public GameObject slot1, slot2, slot3;
    private Vector3 _originalParentPosition1, _originalParentPosition2, _originalParentPosition3; 
    FruitSelector _selector;
    public float delayAnimation;
    
    // Save default position for slotParent
    private void Start()
    {

        _originalParentPosition1 = slot1.transform.position;
        _originalParentPosition2 = slot2.transform.position;
        _originalParentPosition3 = slot3.transform.position;
    }
    
 //Change position slotParent gameObject back to default
    public IEnumerator BackToPositionBefore()
    {
        yield return new WaitForSeconds(4f);
        _selector = FindObjectOfType<FruitSelector>();
        slot1.transform.position = _originalParentPosition1;
        slot2.transform.position = _originalParentPosition2; 
        slot3.transform.position = _originalParentPosition3;

    }
}