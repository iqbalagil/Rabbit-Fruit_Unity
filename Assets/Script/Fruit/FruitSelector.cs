using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSelector : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    private bool _dragging, _placed;
    private Vector2 offset, _originalPosition;
    private FruitHandler _slot;
    private GameObject _piece;

    public void Init(FruitHandler slot)
    {
        _renderer.sprite = slot.Renderer.sprite;
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

    }

    private void OnMouseDown()
    {
        _dragging = true;

        offset = GetMousePos() - (Vector2)transform.position;
    }

    private void OnMouseUp()
    {
        if(Vector2.Distance(transform.position, _slot.transform.position) < 3){
            transform.position = -_slot.transform.position;
            _placed = true;
        } else
            {
            transform.position = _originalPosition;
            _dragging = false;
        }
         
        
    }

    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
