using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CartHandler : MonoBehaviour
{
    public BoxCollider2D _bc2d;
    public GameObject[] slots;
    public GameObject rabBasket;
    public int _filledSlots = 0;
    public PlayerBehavior _playerBehavior;
    public float delay;

    private void Awake()
    {
        _bc2d = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ParticleSystem ps = GameObject.FindGameObjectWithTag("particleconfetti").GetComponent<ParticleSystem>();
        if (other.gameObject.CompareTag("fruit") && _filledSlots < slots.Length)
        {
            PlaceFruitInSlot(other.gameObject, _filledSlots);
            _filledSlots++;

            if (_filledSlots == slots.Length)
            {
                JumpTriggered();
                ps.Play();
                rabBasket.SetActive(false);
                StartCoroutine(ResetAfterCompleted(delay));
            }
        }   
    }

    
  public void PlaceFruitInSlot(GameObject fruit, int slotIndex)
    {
        fruit.transform.position = slots[slotIndex].transform.position;
        fruit.transform.parent = slots[slotIndex].transform;
    }

    void JumpTriggered()
    {
        StartCoroutine(JumpDelay());
    }

    private IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(0f);
        _playerBehavior.anime.SetBool(_playerBehavior.playerJumpAnimation, true);
        yield return new WaitForSeconds(3f);
        _playerBehavior.anime.SetBool(_playerBehavior.playerJumpAnimation, false);
    }

    private IEnumerator ResetAfterCompleted(float resetGameDelay)
    {
        yield return new WaitForSeconds(resetGameDelay);
        SceneManager.LoadScene(0);
    }

}