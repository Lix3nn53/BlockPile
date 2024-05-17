using System;
using PrimeTween;
using UnityEngine;

/*
 * Swipe Input script for Unity by @fonserbc, free to use wherever
 *
 * Attack to a gameObject, check the static booleans to check if a swipe has been detected this frame
 * Eg: if (SwipeInput.swipedRight) ...
 *
 * 
 */

public class SwipeInput : MonoBehaviour
{
    // References
    private EventManager _eventManager;
    [SerializeField] private float TARGET_HEIGHT = 5f;
    private float _moveDuration;
    private BlockPile _blockPile;
    private Vector3 _startPos;
    private void Start()
    {
        _eventManager = EventManager.Instance;
        _moveDuration = GameManager.Instance.MoveDuration;
    }

    private void Update()
    {
        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 pos = new Vector2(touch.position.x, touch.position.y);

                RayBlockPile(pos);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (_blockPile == null)
                {
                    return;
                }

                Vector2 pos = new Vector2(touch.position.x, touch.position.y);

                GridSlot gridSlot = RayGridSlot(pos);

                if (gridSlot != null && gridSlot.CanPlaceBlockPile)
                {
                    gridSlot.PlaceBlockPile(_blockPile);
                }
                else
                {
                    // Return to spawner slot
                    _blockPile.PlaceAnimation(_startPos, _moveDuration);
                }

                _blockPile = null;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (_blockPile == null)
                {
                    return;
                }

                // Move Current BlockPile
                Vector2 pos = new Vector2(touch.position.x, touch.position.y);
                Ray ray = Camera.main.ScreenPointToRay(pos);

                _blockPile.transform.position = ray.GetPoint((TARGET_HEIGHT - ray.origin.y) / ray.direction.y);
            }
        }
    }

    private void RayBlockPile(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Hit an active BoardItem hit.collider.gameObject
            BlockPile blockPile = hit.collider.gameObject.GetComponentInParent<BlockPile>();

            if (blockPile != null && blockPile.IsMovable)
            {
                _blockPile = blockPile;
                _startPos = _blockPile.transform.position;
                _blockPile.OnPickUp();
            }
        }
    }
    private GridSlot RayGridSlot(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Hit an active BoardItem hit.collider.gameObject
            GridSlot gridSlot = hit.collider.gameObject.GetComponent<GridSlot>();

            return gridSlot;
        }

        return null;
    }
}