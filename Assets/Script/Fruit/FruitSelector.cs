using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSelector : MonoBehaviour
{
    [SerializeField] public SpriteRenderer _renderer;
    public bool _dragging,_placed;
    private Vector2 offset, _originalPosition;
    private FruitHandler _slot;
    private PlayerBehavior player;
    public Animator anim;

    private static readonly int IsPlaced = Animator.StringToHash("ElevationAnimation");
    

    private static int placedFruitsCount = 0;
    [SerializeField] private PlayerBehavior playerBehavior;


    public void Init(FruitHandler slot)
    {
        _slot = slot;
    }

    private void Awake()
    {
        _originalPosition = transform.position;
        player = FindObjectOfType<PlayerBehavior>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.SetBool(IsPlaced,false);
    }
    private void Update()
    {
        if (_placed) return;

        if (!_dragging) return;

        var mousePosition = GetMousePos();
        transform.position = mousePosition - offset;

    }

    private void OnMouseDown()
    {
        if (_placed) return;

        _dragging = true;
        offset = GetMousePos() - (Vector2)transform.position;
    }

    public void OnMouseUp()
    {
        _dragging = false;

        if (Vector2.Distance(transform.position, _slot.transform.position) < 3)
        {
            transform.position = _slot.transform.position;
            _placed = true;
            StartCoroutine(DisappearAfterSnap());
            if (placedFruitsCount % 3 == 0 && placedFruitsCount != 0)
            {
                PlayerBehavior.FindObjectOfType<PlayerBehavior>().anime.SetFloat("IsPlaced", 1);
                PlayerBehavior.FindObjectOfType<PlayerBehavior>().OnDrop();
            }

            if (placedFruitsCount == 9)
            {
                playerBehavior.anime.SetBool("IsJump",true);
            }
        }
        else
        {
            transform.position = _originalPosition;
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
                Debug.Log("fruit fly");
                _renderer.enabled = true;
                if ( _slot.gameObject.activeSelf == false)
                {
                    OnElevation();
                }
            }
            _placed = false;
        }

    }

    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    private void OnElevation()
    {
        anim.SetBool(IsPlaced,true);
    }
}
