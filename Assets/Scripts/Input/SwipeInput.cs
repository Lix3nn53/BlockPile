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
    [SerializeField] private float DURATION = 0.5f;

    private BlockPile _blockPile;
    private Vector3 _startPos;
    private void Start()
    {
        _eventManager = EventManager.Instance;
    }

    private void Update()
    {
        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 pos = new Vector2(touch.position.x, touch.position.y);

                Ray ray = Camera.main.ScreenPointToRay(pos);
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
                        _blockPile.IsMovable = false;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (_blockPile == null)
                {
                    return;
                }

                BlockPile blockPile = _blockPile;

                Tween.Position(_blockPile.transform, _startPos, DURATION, ease: Ease.OutSine).OnComplete(() => blockPile.IsMovable = true);

                _blockPile = null;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (_blockPile == null)
                {
                    return;
                }

                Vector2 pos = new Vector2(touch.position.x, touch.position.y);

                Ray ray = Camera.main.ScreenPointToRay(pos);

                Vector3 targetPoint = ray.GetPoint((TARGET_HEIGHT - ray.origin.y) / ray.direction.y);



                _blockPile.transform.position = targetPoint;
            }
        }
    }
}