using System.Collections;
using UnityEngine;

public class FruitBasketAnimation : MonoBehaviour
{
    public GameObject slot1, slot2, slot3;

    public float moveUpDuration = 1f;
    public float dropDownDuration = 1f;
    public float moveUpDistance = 2f;
    public float dropDownDistance = 2f;
    public float rotateAngle = 120f;

    private void Start()
    {
        slot1 = transform.GetChild(0).gameObject;
        slot2 = transform.GetChild(1).gameObject;
        slot3 = transform.GetChild(2).gameObject;
    }

    public void AnimateSlot(GameObject slot)
    {
        StartCoroutine(AnimateSlotCoroutine(slot));
    }

    private IEnumerator AnimateSlotCoroutine(GameObject slot)
    {
        Vector3 originalPosition = slot.transform.position;
        Quaternion originalRotation = slot.transform.rotation;
        Vector3 targetPosition = originalPosition + new Vector3(moveUpDistance, moveUpDistance, 0);
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0, 0, 10);

        float elapsedTime = 0;
        while (elapsedTime < moveUpDuration)
        {
            slot.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveUpDuration);
            slot.transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, elapsedTime / moveUpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        slot.transform.position = targetPosition;
        slot.transform.rotation = targetRotation;
        
        originalPosition = slot.transform.position;
        originalRotation = slot.transform.rotation;
        targetPosition = originalPosition - new Vector3(0, dropDownDistance, 0);
        targetRotation = originalRotation * Quaternion.Euler(0, 0, rotateAngle);

        elapsedTime = 0;
        while (elapsedTime < dropDownDuration)
        {
            slot.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / dropDownDuration);
            slot.transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, elapsedTime / dropDownDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        slot.transform.position = targetPosition;
        slot.transform.rotation = targetRotation;
    }
}