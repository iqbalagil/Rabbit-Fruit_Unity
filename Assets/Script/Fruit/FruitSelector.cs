using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FruitSelector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    private ChangePosition _changePosition;
    private FruitHandler _slot;
    private PlayerBehavior _player;
    private FruitCompleted _fruitCompleted;

    public bool dragging, placed, animateTriggered, isHappy, isSad, isTuang, isCartPlaced, isElevate;
    private Vector2 _offset, _originalPosition;
    private Vector3 _originalParentPosition;
    public Animator anim;
    private static int _nextSlotIndex = 0;
    private static int _placedFruitsCount = 0;
    public Transform fruit;

    public Vector3 originalParentPosition;
    
    private static readonly int IsPlaced = Animator.StringToHash("Placed");
    private static readonly int IsPlacedCart = Animator.StringToHash("PlacedCart");

    public void Init(FruitHandler slot)
    {
        _slot = slot;
    }
    
    private void Awake()
    {
        _originalPosition = transform.position;
        _slot = FindObjectOfType<FruitHandler>();
        _player = FindObjectOfType<PlayerBehavior>();
        anim = GetComponent<Animator>();
        _fruitCompleted = FindObjectOfType<FruitCompleted>();
        _changePosition = FindObjectOfType<ChangePosition>();
        _originalParentPosition = transform.parent.position;
    }

    private void Update()
    {
        if (placed) return;

        anim.enabled = false;
        if (!dragging) return;

        transform.position = GetMousePos() - _offset;
        TriggredListFruitObject();

        _player.anime.SetBool(_player.playerTuangAnimation, isTuang);
        _player.anime.SetBool(_player.playerSadAnimation, isSad && dragging);
        _player.anime.SetBool(_player.playerHappyAnimation, isHappy);

        if (animateTriggered)
        {
            transform.localScale = new Vector3(10f, 10f, 0f);
        }
    }

    private void OnMouseDown()
    {
        if (placed) return;

        dragging = true;
        _offset = GetMousePos() - (Vector2)transform.position;
        _player.anime.SetTrigger(_player.playerIdleAnimation);
        isSad = false;
    }

    private void OnMouseUp()
    {
        dragging = false;

        if (Vector2.Distance(transform.position, _slot.transform.position) < 10f)
        {
            var fruitBody = gameObject.AddComponent<Rigidbody2D>();
            fruitBody.gravityScale = 0;
            transform.position = _slot.transform.position;
            placed = true;
            isSad = false;
            isHappy = true;
            StartCoroutine(DisappearAfterSnap());
            _placedFruitsCount++;

            if (transform.parent != null)
            {
                transform.parent.position = _slot.transform.position;
            }

            if (_placedFruitsCount % 3 == 0)
            {
                isCartPlaced = true;
                isElevate = false;
                StartCoroutine(_changePosition.BackToPositionBefore());
                StartCoroutine(FruitTuang());
                StartCoroutine(TuangAnimation());
                StartCoroutine(_player.PlayerTuangFruit());
                StartCoroutine(_fruitCompleted.ResetGamePrepareNextStage());
            }

            _fruitCompleted.OnFruitPlaced();
        }
        else
        {
            if (!placed)
            {
                isSad = true;
                isHappy = false;
                transform.position = _originalPosition;
            }
        }

        UpdateAnimationStates();
    }

    private void UpdateAnimationStates()
    {
        _player.anime.SetBool(_player.playerSadAnimation, isSad);
        _player.anime.SetBool(_player.playerHappyAnimation, isHappy);
    }

    private IEnumerator DisappearAfterSnap()
    {
        yield return new WaitForSeconds(0f);

        if (renderer != null)
        {
            renderer.enabled = true;
        }

        if (_slot != null)
        {
            if (_slot.Renderer != null)
            {
                _slot.Renderer.enabled = false;
            }
            _slot.gameObject.SetActive(false);
        }

        if (_player != null && gameObject.CompareTag("fruit"))
        {
            anim.enabled = true;
            renderer.enabled = true;
            if (placed)
            {
                OnElevation();
            }
        }
    }

    private Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnElevation()
    {
        anim.transform.position = new Vector3(transform.position.x / 9f, transform.position.y / 9f, 0f);
        anim.SetBool(IsPlaced, true);
        StartCoroutine(ChangeScaleAfterAnimation(0));
    }

    private IEnumerator ChangeScaleAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.localScale = new Vector3(25f, 25f, 0f);
    }

    private IEnumerator PlacedAfterAnimation(float delay, int slotIndex)
    {
        yield return new WaitForSeconds(delay);
        if (fruit.transform.childCount > slotIndex)
        {
            GameObject locGameObject = GameObject.Find($"Player/Basket/rabBasket/Loc {slotIndex + 1}");

            if (locGameObject != null)
            {
                transform.position = locGameObject.transform.position;
                transform.SetParent(locGameObject.transform);
                transform.localPosition = Vector3.zero;
            }
            else
            {
                Debug.LogError($"Loc GameObject with name Loc {slotIndex + 1} not found under Player/Basket");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("basket"))
        {
            _player.anime.SetBool(_player.playerHappyAnimation, false);
            animateTriggered = true;
            anim.enabled = false;

            _nextSlotIndex = (_nextSlotIndex + 1) % 3;
            StartCoroutine(PlacedAfterAnimation(0, _nextSlotIndex));

            string locPath = $"Player/Basket/rabBasket/Loc {_nextSlotIndex + 1}";
            Transform playerBasketLoc = GameObject.Find(locPath)?.transform;

            if (playerBasketLoc != null)
            {
                transform.SetParent(playerBasketLoc);
                transform.localPosition = Vector3.zero;
            }
            else
            {
                Debug.LogError($"Loc {_nextSlotIndex} not found under Player/Basket. Check if it exists and is active.");
            }
        }
    }

    private IEnumerator TuangAnimation()
    {
        yield return new WaitForSeconds(4f);
        _player.anime.SetBool(_player.playerTuangAnimation, true);
    }

    private IEnumerator FruitTuang()
    {
        yield return new WaitForSeconds(4f);
        // anim.enabled = true;
        // anim.SetTrigger(IsPlacedCart);
        transform.parent.position += new Vector3(5f, 0, 0);
    }

    private void TriggredListFruitObject()
    {
        anim.SetBool(IsPlaced, isElevate);
        anim.SetBool(IsPlacedCart, isCartPlaced);
    }
}