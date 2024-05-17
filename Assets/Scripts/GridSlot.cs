using System;
using System.Linq;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class GridSlot : GridSlotBase
{
  public static Vector2Int INVALID_POS = new Vector2Int(-999, -999);
  public Vector2Int GridPosition = INVALID_POS;
  private GameManager _gameManager;
  private GameGrid _gameGrid;
  public override void Start()
  {
    base.Start();

    _gameManager = GameManager.Instance;
    _gameGrid = GameGrid.Instance;

    _materialRecolor = new MaterialRecolor(GetComponent<Renderer>(), BlockColorType.GRAY);
  }

  public bool IsValid()
  {
    return GridPosition != INVALID_POS;
  }

  public override void OnBlockPilePlaceFinished()
  {
    TryGetBlockPileFromNeighbours(Enum.GetValues(typeof(BlockDirection)).Cast<BlockDirection>().ToList());
    _gameManager.CheckSpawners();
  }

  public void TryGetBlockPileFromNeighbours(List<BlockDirection> directionsNotChecked)
  {
    List<BlockDirection> copy = new List<BlockDirection>(directionsNotChecked);

    foreach (BlockDirection direction in directionsNotChecked)
    {
      copy.Remove(direction);

      if (TryGetBlockPileFromNeighbour(direction, copy))
      {
        break;
      }
    }
  }
  public bool TryGetBlockPileFromNeighbour(BlockDirection direction, List<BlockDirection> directionsNotChecked)
  {
    BlockPile current = BlockPile;

    if (current == null)
    {
      return false;
    }

    Vector2Int gridPosOther = GridPosition + direction.GetGridPositionOffset(GridPosition);
    // Debug.Log("Original: " + GridPosition + " - " + direction + ": " + gridPosOther);

    GridSlot slotOther = _gameGrid.GetGridSlot(gridPosOther);
    if (slotOther == null)
    {
      return false;
    }

    // Try to move blocks from other slot to this slot
    BlockDirection opposite = direction.Opposite();
    if (slotOther.TryMoveBlockPileTo(opposite, current, directionsNotChecked))
    {
      return true;
    }

    return false;
  }

  public bool TryMoveBlockPileTo(BlockDirection direction, BlockPile target, List<BlockDirection> directionsNotChecked)
  {
    BlockPile current = BlockPile;

    if (current == null)
    {
      return false;
    }

    TryMoveTopBlockRecursive(direction, current, target, directionsNotChecked);

    return true;
  }

  public void OnMoveTopBlock(BlockDirection direction, BlockPile current, BlockPile target, Block block, List<BlockDirection> directionsNotChecked)
  {
    target.PlaceBlock(block);

    TryMoveTopBlockRecursive(direction, current, target, directionsNotChecked);
  }

  public void TryMoveTopBlockRecursive(BlockDirection direction, BlockPile current, BlockPile target, List<BlockDirection> directionsNotChecked)
  {
    Debug.Log("A1: " + direction);

    bool moved = false;
    Block topBlock = current.GetTopBlock();

    if (topBlock != null)
    {
      // Try to move top block from current pile to target pile

      if (target.CanMove(topBlock))
      {
        float localY = target.GetNextLocalHeight();
        topBlock.Move(direction, localY, () => OnMoveTopBlock(direction, current, target, topBlock, directionsNotChecked));
        moved = true;
      }
    }
    else
    {
      // current pile is empty
      // return current pile to pool
      current.gameObject.SetActive(false); // Also returns to pool, so do NOT need to change parent
    }

    if (!moved)
    {
      // Couldnt move from current to target
      // Try to move to target from other neighbours

      Vector2Int gridPosOther = GridPosition + direction.GetGridPositionOffset(GridPosition);
      GridSlot slotOther = _gameGrid.GetGridSlot(gridPosOther);
      slotOther.TryGetBlockPileFromNeighbours(directionsNotChecked);
    }
  }
}
