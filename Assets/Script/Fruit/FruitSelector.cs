using System.Collections;
using UnityEngine;

public class FruitSelector : MonoBehaviour
{
    [SerializeField] public new SpriteRenderer renderer;
    public bool dragging,placed,animateTriggered;
    private Vector2 _offset, _originalPosition;
    private Vector3 _originalParentPosition;
    private FruitHandler _slot;
    private PlayerBehavior _player;
    public Animator anim;

    private FruitCompleted _fruitCompleted;
    private static int _nextSlotIndex = 0;
    public Transform fruit;

    private static readonly int IsPlaced = Animator.StringToHash("Placed");
    private float _originalScale;
    public Vector3 originalParentPosition;

    private static int _placedFruitsCount = 0;
    [SerializeField] private PlayerBehavior playerBehavior;

    public float delayAnimation;


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
        _originalParentPosition = transform.parent.position;
    }

    private void Start()
    {
        _placedFruitsCount++;
        _fruitCompleted = FindObjectOfType<FruitCompleted>();
    }
    private void Update()
    {
        if (placed) return;

        anim.enabled = false;
        
        if (!dragging) return;

        var mousePosition = GetMousePos();
        transform.position = mousePosition - _offset;

        if (animateTriggered == true)
        {
            transform.localScale = new Vector3(10f, 10f, 0f);
        }

    }

    private void OnMouseDown()
    {
        if (placed) return;

        dragging = true;
        _offset = GetMousePos() - (Vector2)transform.position;
    }

    private void OnMouseUp()
    {
        dragging = false;

        if (Vector2.Distance(transform.position, _slot.transform.position) < 10f)
        {
            transform.position = _slot.transform.position;
            placed = true;
            StartCoroutine(DisappearAfterSnap());
            _placedFruitsCount++; 
            
            if (transform.parent != null)
            {
                transform.parent.position = _slot.transform.position;
                animateTriggered = true;
                StartCoroutine(DelayAfterPlaced());
            }
            
            if (_placedFruitsCount % 3 == 0)
            {
                StartCoroutine(_fruitCompleted.ResetGamePrepareNextStage());
            }

            FindObjectOfType<FruitCompleted>().OnFruitPlaced();

        }
        else
        {
            if (!placed)
            {
                transform.position = _originalPosition;
            }
        }
    }

    private IEnumerator DisappearAfterSnap()
    {
        yield return new WaitForSeconds(0f);

        if (renderer != null)
        {
            renderer.enabled = false;
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
        if (_player != null)
        {
            _player.OnDrop();

            if (gameObject.CompareTag("fruit"))
            {
                anim.enabled = true;
                Debug.Log("fruit fly");
                renderer.enabled = true;
                if (placed)
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
            StartCoroutine(PlacedAfterAnimation(0, _nextSlotIndex));
            anim.enabled = false;
            _nextSlotIndex = (_nextSlotIndex + 1) % fruit.transform.childCount;
        }
    }

    private IEnumerator DelayAfterPlaced()
    {
        yield return new WaitForSeconds(delayAnimation);
        if (animateTriggered)
        {
            if ((transform.parent.position - _originalParentPosition).magnitude > 0.1f)
            {
                Debug.Log("Change position back to original");
                transform.parent.position = _originalParentPosition;
            }
        }
    }
}
