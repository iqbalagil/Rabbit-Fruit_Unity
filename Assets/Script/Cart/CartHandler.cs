using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CartHandler : MonoBehaviour
{
    public GameObject[] slots;
    private int filledSlots = 0;
    private PlayerBehavior _playerBehavior;
    public float delay;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("fruit") && filledSlots < slots.Length)
        {
            PlaceFruitInSlot(other.gameObject, filledSlots);
            filledSlots++;

            if (filledSlots == slots.Length)
            {
                JumpTriggered();
                StartCoroutine(ResetAfterCompleted(delay));
            }
        }   
    }

    void PlaceFruitInSlot(GameObject fruit, int slotIndex)
    {
        fruit.transform.position = slots[slotIndex].transform.position;
        fruit.transform.parent = slots[slotIndex].transform;
    }

    void JumpTriggered()
    {
        _playerBehavior.anime.SetBool(_playerBehavior.playerJumpAnimation, true);
    }

    private IEnumerator ResetAfterCompleted(float resetGameDelay)
    {
        yield return new WaitForSeconds(resetGameDelay);
        SceneManager.LoadScene(0);
    }

}