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
    }
    
    private IEnumerator InitializeSelector()
    {
        _selector = FindObjectOfType<FruitSelector>();
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator PlayerTuangFruit()
    {
        yield return new WaitForSeconds(1f);
        if ( _selector == null)
        {
            Debug.LogError("is null");
        }
        if (!_selector.isTuang)
        {
            anime.SetBool(playerTuangAnimation, false);
        }
        
    }

    public IEnumerator PlayerJumping()
    {
        yield return new WaitForSeconds(1f);
        if (!_selector.isJumping)
        {
            anime.SetBool(playerJumpAnimation, false);
        }
    }
    
}