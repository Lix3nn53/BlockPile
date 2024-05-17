using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class GridSlot : GridSlotBase
{
  public static Vector2Int INVALID_POS = new Vector2Int(-999, -999);
  public Vector2Int GridPosition = INVALID_POS;
  public BlockPile BlockPile => GetComponentInChildren<BlockPile>();
  public bool CanPlaceBlockPile => BlockPile == null;
  private GameGrid _gameGrid;
  public override void Start()
  {
    base.Start();

    _gameGrid = GameGrid.Instance;
  }

  public bool IsValid()
  {
    return GridPosition != INVALID_POS;
  }

  public override void OnBlockPilePlace(BlockPile blockPile)
  {
    foreach (BlockDirection direction in Enum.GetValues(typeof(BlockDirection)))
    {
      Vector2Int gridPosOther = GridPosition + direction.GetGridPositionOffset(GridPosition);
      Debug.Log("Original: " + GridPosition + " - " + direction + ": " + gridPosOther);

      GridSlot slotOther = _gameGrid.GetGridSlot(gridPosOther);
      if (slotOther == null)
      {
        continue;
      }

      // Try to move blocks from other slot to this slot
      BlockDirection opposite = direction.Opposite();
      if (slotOther.MoveBlockPileTo(opposite, blockPile))
      {
        Debug.Log(direction);
        Debug.Log(opposite);
        break;
      }
      Renderer renderer = slotOther.GetComponent<Renderer>();
      MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
      propertyBlock.SetColor("_BaseColor", Color.red);
      renderer.SetPropertyBlock(propertyBlock);
    }
  }

  public bool MoveBlockPileTo(BlockDirection direction, BlockPile target)
  {
    BlockPile current = BlockPile;

    if (current == null)
    {
      return false;
    }

    // Renderer renderer = GetComponent<Renderer>();
    // MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
    // propertyBlock.SetColor("_BaseColor", Color.red);
    // renderer.SetPropertyBlock(propertyBlock);

    TryMoveTopBlockRecursive(direction, current, target);

    return true;
  }

  public void OnMoveBlockPileToComplete(BlockDirection direction, BlockPile current, BlockPile target, Block block)
  {
    target.PlaceBlock(block);

    TryMoveTopBlockRecursive(direction, current, target);
  }

  public void TryMoveTopBlockRecursive(BlockDirection direction, BlockPile current, BlockPile target)
  {
    Block topBlock = current.GetTopBlock();
    if (topBlock != null)
    {
      float localY = target.GetNextLocalHeight();
      topBlock.Move(direction, localY, () => OnMoveBlockPileToComplete(direction, current, target, topBlock));
    }
    else
    {
      current.gameObject.SetActive(false); // Also returns to pool, so do NOT need to change parent
    }
  }
}
