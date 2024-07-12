using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] public Animator anime;

    public GameObject selector;
    private FruitSelector _selector;
    private bool _dropFruit;
    
    
    private static readonly int PlayerHappyAnimation = Animator.StringToHash("IsHappy");
    private static readonly int PlayerSadAnimation = Animator.StringToHash("SadAnimation");
    private static readonly int PlayerIdleAnimation = Animator.StringToHash("IsIdle");
    
    private void Awake()
    {
        anime = GetComponent<Animator>();
        _selector = selector.GetComponent<FruitSelector>();
        _selector._placed = selector.GetComponent<FruitSelector>();
    }

    private void Start()
    {
        anime.SetBool(PlayerIdleAnimation, false);
    }

    public void OnDrop()
    {
        bool isHappy = anime.GetBool(PlayerHappyAnimation);
        bool isSad = anime.GetBool(PlayerSadAnimation);

            if (!isHappy && !isSad)
            {
                anime.SetBool(PlayerIdleAnimation, true);
                _dropFruit = true;
            }
            else
            {
                _dropFruit = false;
            }
            if (_selector._placed == true)
            {
                if ( _dropFruit)
                {
                    if (gameObject.CompareTag("Player"))
                    {
                        anime.SetBool(PlayerHappyAnimation, true);
                    }
                    
                }

                if ( !_dropFruit)
                {
                    if (gameObject.CompareTag("Player")) 
                    {
                        anime.SetBool(PlayerSadAnimation, true);
                    }
                }
                
            }
    }
}
