using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GridSlotBase : MonoBehaviour
{
  [SerializeField] private float _offsetY;
  public BlockPile BlockPile => GetComponentInChildren<BlockPile>();
  public bool CanPlaceBlockPile => BlockPile == null;
  private float _moveDuration;

  public virtual void Start()
  {
    _moveDuration = GameManager.Instance.MoveDuration;
  }

  public void PlaceBlockPile(BlockPile blockPile, bool withAnimation)
  {
    blockPile.transform.parent = transform;

    Vector3 localPos = new Vector3(0, _offsetY, 0);

    if (withAnimation)
    {
      blockPile.PlaceAnimation(transform.position + localPos, _moveDuration);
    }
    else
    {
      blockPile.transform.localPosition = localPos;
    }
  }
}
