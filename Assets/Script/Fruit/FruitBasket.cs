using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBasket : MonoBehaviour
{ 
    private GameObject fruit;
    public GameObject basket;

    private void Start()
    {
        fruit = GameObject.FindGameObjectWithTag("fruit");
    }
    
}
