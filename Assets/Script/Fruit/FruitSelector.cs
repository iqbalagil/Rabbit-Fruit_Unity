using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSelector : MonoBehaviour
{
    [SerializeField] public SpriteRenderer _renderer;
    public bool _dragging,_placed,animateTriggered;
    private Vector2 offset, _originalPosition;
    private FruitHandler _slot;
    private PlayerBehavior player;
    public Animator anim;

    private FruitCompleted _fruitCompleted;
    private static int nextSlotIndex = 0;
    public Transform fruit;

    private static readonly int IsPlaced = Animator.StringToHash("Placed");
    private float _originalScale;
    public Vector3 _originalParentPosition;

    private static int placedFruitsCount = 0;
    [SerializeField] private PlayerBehavior playerBehavior;


    public void Init(FruitHandler slot)
    {
        _slot = slot;
    }

    private void Awake()
    {
        _originalPosition = transform.position;
        if (transform.parent != null)
        {
            _originalParentPosition = transform.parent.position;
        }
        player = FindObjectOfType<PlayerBehavior>();
        anim = GetComponent<Animator>();
        _fruitCompleted = FindObjectOfType<FruitCompleted>();
    }

    private void Start()
    {
        placedFruitsCount++;
        _fruitCompleted = FindObjectOfType<FruitCompleted>();
    }
    private void Update()
    {
        if (_placed) return;

        anim.enabled = false;
        
        if (!_dragging) return;

        var mousePosition = GetMousePos();
        transform.position = mousePosition - offset;

        if (animateTriggered == true)
        {
            transform.localScale = new Vector3(10f, 10f, 0f);
        }

    }

    private void OnMouseDown()
    {
        if (_placed) return;

        _dragging = true;
        offset = GetMousePos() - (Vector2)transform.position;
    }

    private void OnMouseUp()
    {
        _dragging = false;

        if (Vector2.Distance(transform.position, _slot.transform.position) < 10f)
        {
            transform.position = _slot.transform.position;
            _placed = true;
            StartCoroutine(DisappearAfterSnap());
            placedFruitsCount++; 

            if (placedFruitsCount % 3 == 0)
            {
                transform.parent.position = _originalPosition;
                _fruitCompleted.ResetGamePrepareNextStage(); 
            }

            FindObjectOfType<FruitCompleted>().onFruitPlaced();

            if (transform.parent != null)
            {
                // transform.parent.position = _slot.transform.position;
            }
        }
        else
        {
            if (!_placed)
            {
                transform.position = _originalPosition;
            }
        }
    }

    private IEnumerator DisappearAfterSnap()
    {
        yield return new WaitForSeconds(0f);

        if (_renderer != null)
        {
            _renderer.enabled = false;
        } else
        {
            Debug.Log("Sprite is DissapearAfterSnap");
        }

        if (_slot != null)
        {
            if (_slot.Renderer != null)
            {
                _slot.Renderer.enabled = false;
            }
            _slot.gameObject.SetActive(false);
        }
        if (player != null)
        {
            player.OnDrop();

            if (gameObject.CompareTag("fruit"))
            {
                anim.enabled = true;
                Debug.Log("fruit fly");
                _renderer.enabled = true;
                if (_placed)
                {
                    transform.position = _slot.transform.position;
                    OnElevation();
                }
            }
        }

    }

    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    
    private void OnElevation()
    {
        // animateTriggered = true;
        anim.transform.position = new Vector3(transform.position.x / 9f, transform.position.y / 9f, 0f);
        anim.SetBool(IsPlaced,true);
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
            Transform slot = fruit.GetChild(slotIndex);
            transform.position = slot.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("basket"))
        {
            Debug.Log("Fruit is placed");
            StartCoroutine(PlacedAfterAnimation(0, nextSlotIndex));
            anim.enabled = false;
            nextSlotIndex = (nextSlotIndex + 1) % fruit.transform.childCount;
        }
    }
}
