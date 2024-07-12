using UnityEngine;

public class FruitAnimation : MonoBehaviour
{
    [Header("==== Animator ====")]
    [SerializeField] public Animator fruitAnimation;
    public Animator playerAnimation;

    private void Start()
    {
        if (fruitAnimation == null)
        {
            Debug.LogError("Fruit Animation is not assigned");
        }
        if (playerAnimation == null)
        {
            Debug.LogError("Player Animation is not assigned");
        }
    }

    public void SetPlayerAnimation(string animationName, bool anime)
    {
        if (playerAnimation != null)
        {
            playerAnimation.SetBool(animationName, anime);
        }
        else
        {
            Debug.LogError("Player Animation is not assigned in the inspector.");
        }
    }

    public void PlayFruitAnimation(string animationFruit)
    {
        if (fruitAnimation != null)
        {
            fruitAnimation.Play(animationFruit);
        }
        else
        {
            Debug.LogError("Fruit Animation is not assigned in the inspector.");
        }
    }
}
