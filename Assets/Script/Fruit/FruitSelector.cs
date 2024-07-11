using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSelector : MonoBehaviour
{
    [SerializeField] private Animator fruitAnimation;
    [SerializeField] private GameObject prefabsObject;
    [SerializeField] private SpriteRenderer _renderer;
    public bool _dragging, _placed;
    private Vector2 offset, _originalPosition;
    public FruitHandler _slot;
    private Vector3 originalScale;
    public FruitSelector _selector;

    public void Init(FruitHandler slot)
    {
        _slot = slot;
    }

    private void Awake()
    {
        _originalPosition = transform.position;
    }

    private void Update()
    {
        if (_placed) return;

        if (!_dragging) return;

        var mousePosition = GetMousePos();
        transform.position = mousePosition - offset;


        if (_selector._renderer.enabled == false)
        {
            Debug.Log("Renderer was disabled, now enabling it.");
            _selector._renderer.enabled = true;
            fruitAnimation.SetBool("HappyRabbit", true);
        }
        else if (_selector._renderer.enabled == true)
        {
            if (fruitAnimation.GetBool("HappyRabbit") == true)
            {
                StartCoroutine(HandleFruitSpawn());
            }
            else
            {
                fruitAnimation.SetBool("SadRabbit", true);
            }
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

        if (Vector2.Distance(transform.position, _slot.transform.position) < 3)
        {
            transform.position = _slot.transform.position;
            _placed = true;
            StartCoroutine(DisappearAfterSnap());
        }
        else
        {
            transform.position = _originalPosition;
        }
    }

    private IEnumerator DisappearAfterSnap()
    {
        yield return new WaitForSeconds(0f);

        _renderer.enabled = false;

        if (_slot != null && _slot.Renderer != null)
        {
            _slot.Renderer.enabled = false;
        }

        if (_slot != null)
        {
            _slot.gameObject.SetActive(false);
        } 

        //_slot.gameObject.SetActive(false);
    }

    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    //private AppearAfterSnap()
    //{

    //}

    private IEnumerator HandleFruitSpawn()
    {
        yield return new WaitForSeconds(0.1f);

        float scaleDuration = 1f;
        float time = 0;
        Vector3 targetScale = Vector3.zero;

        if (_slot.gameObject.activeSelf == false)
        {
            _slot.gameObject.SetActive(true);

            while (time < scaleDuration)
            {
                _slot.gameObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, time / scaleDuration);
                time += Time.deltaTime;
                yield return null;
            }

            _slot.gameObject.transform.localScale = targetScale;

            Animator spawnedFruitAnimator = _slot.GetComponent<Animator>();
            if (spawnedFruitAnimator != null)
            {
                spawnedFruitAnimator.SetTrigger("ElevationAnimation");
            }
        }
    }

}
