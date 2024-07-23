using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBasket : MonoBehaviour
{
    private PlayerBehavior _playerBehavior;
    public GameObject[] slotbasket;
    private int filledSlots = 0;
    public float delay;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("fruit") && filledSlots < slotbasket.Length)
        {
            PlaceFruitInBasket(other.gameObject, filledSlots);
            filledSlots++;
            if (filledSlots == slotbasket.Length)
            {
                PlayerTuang();
            }
        }
    }

    void PlaceFruitInBasket(GameObject fruit, int slotIndex)
    {
        fruit.transform.position = slotbasket[slotIndex].transform.position;
        fruit.transform.parent = slotbasket[slotIndex].transform;
    }

    void PlayerTuang()
    {
        _playerBehavior.anime.SetBool(_playerBehavior.playerTuangAnimation,true);
    }
}
