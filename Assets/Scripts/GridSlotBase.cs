using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GridSlotBase : MonoBehaviour
{
  [SerializeField] private float _offsetY;
  private float _moveDuration;
  protected MaterialRecolor _materialRecolor;

  public virtual void Start()
  {
    _moveDuration = GameManager.Instance.MoveDuration;
  }

  public void PlaceBlockPile(BlockPile blockPile)
  {
    Vector3 localPos = OnBlockPileStartPlace(blockPile);

    blockPile.PlaceAnimation(transform.position + localPos, _moveDuration, OnBlockPilePlace);
  }

  public void SpawnBlockPile(BlockPile blockPile)
  {
    blockPile.transform.localPosition = OnBlockPileStartPlace(blockPile);
    blockPile.gameObject.SetActive(true);
  }

  private Vector3 OnBlockPileStartPlace(BlockPile blockPile)
  {
    blockPile.transform.parent = transform;
    return new Vector3(0, _offsetY, 0);
  }

  public virtual void OnBlockPilePlace(BlockPile blockPile)
  {

  }
}
