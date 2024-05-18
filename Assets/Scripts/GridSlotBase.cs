using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GridSlotBase : MonoBehaviour
{
  [SerializeField] private float _offsetY;
  private float _placeDuration;
  public MaterialRecolor MaterialRecolor;
  public BlockPile BlockPile => GetComponentInChildren<BlockPile>();
  public bool CanPlaceBlockPile => BlockPile == null;

  public virtual void Start()
  {
    _placeDuration = GameManager.Instance.PlaceDuration;
  }

  public void PlaceBlockPile(BlockPile blockPile)
  {
    Vector3 localPos = OnBlockPileStartPlace(blockPile);

    blockPile.PlaceAnimation(transform.position + localPos, _placeDuration, OnBlockPilePlaceFinished);
  }

  public void SetBlockPile(BlockPile blockPile)
  {
    blockPile.transform.localPosition = OnBlockPileStartPlace(blockPile);
    blockPile.gameObject.SetActive(true);
  }

  private Vector3 OnBlockPileStartPlace(BlockPile blockPile)
  {
    blockPile.transform.parent = transform;
    return new Vector3(0, _offsetY, 0);
  }

  public virtual void OnBlockPilePlaceFinished()
  {

  }
}
