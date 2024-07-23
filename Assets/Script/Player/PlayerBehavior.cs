using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerBehavior : MonoBehaviour
{
    public Animator anime;
    private FruitSelector _selector;
    
  public int playerHappyAnimation = Animator.StringToHash("IsHappy");
  public int playerSadAnimation = Animator.StringToHash("SadAnimation");
  public int playerIdleAnimation = Animator.StringToHash("IsSended");
  public int playerTuangAnimation = Animator.StringToHash("IsTuang");
  public int playerJumpAnimation = Animator.StringToHash("IsJumping");
    
    private void Awake()
    {
        anime = GetComponent<Animator>();
    }

    private void Start()
    {
        anime.SetBool(playerIdleAnimation, false);
        StartCoroutine(InitializeSelector());
        StartCoroutine(PlayerTuangFruit());
    }
    
    private IEnumerator InitializeSelector()
    {
        _selector = FindObjectOfType<FruitSelector>();
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator PlayerTuangFruit()
    {
        yield return new WaitForSeconds(6f);
        anime.SetBool(playerTuangAnimation, false);
    }
    
    public IEnumerator PlayerJumping()
    {
        yield return new WaitForSeconds(1f);
        if (!_selector)
        {
            anime.SetBool(playerJumpAnimation, false);
        }
    }
    
}