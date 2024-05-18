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
    OnBlockPilePlaceFinished(true);
  }

  public void OnBlockPilePlaceFinished(bool recursive)
  {
    // SetBlockPlacingNeighbour(false);

    TryGetBlockPileFromNeighbours(Enum.GetValues(typeof(BlockDirection)).Cast<BlockDirection>().ToList(), recursive);
    _gameManager.CheckSpawners();
  }

  public void TryGetBlockPileFromNeighbours(List<BlockDirection> directionsNotChecked, bool recursive)
  {
    BlockPile current = BlockPile;

    if (current == null)
    {
      return;
    }

    if (directionsNotChecked.Count == 0)
    {
      OnBlockMovementFinished(current, recursive);
      return;
    }

    List<BlockDirection> copy = new List<BlockDirection>(directionsNotChecked);

    foreach (BlockDirection direction in directionsNotChecked)
    {
      copy.Remove(direction);

      if (TryGetBlockPileFromNeighbour(current, direction, copy))
      {
        break;
      }

      if (copy.Count == 0)
      {
        OnBlockMovementFinished(current, recursive);
        break;
      }
    }
  }
  public bool TryGetBlockPileFromNeighbour(BlockPile current, BlockDirection direction, List<BlockDirection> directionsNotChecked)
  {
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

    if (current.IsMovingBlocks)
    {
      return false;
    }

    current.IsMovingBlocks = true;
    target.IsMovingBlocks = true;

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
      slotOther.TryGetBlockPileFromNeighbours(directionsNotChecked, true);
    }
  }

  public void OnBlockMovementFinished(BlockPile current, bool recursive)
  {
    // Block movement animation is played and finished on this slot
    // current.OnBlockMovementFinished(() => SetBlockPlacingNeighbour(true));

    if (recursive)
    {
      OnBlockPilePlaceFinished(false);
    }
    else
    {
      bool destroyed = current.TryDestroy(() => SetIsMovingBlocksNeighbours(false));

      if (!destroyed)
      {
        // TryDestroy doesnt call this if not destroyed so we call it here
        SetIsMovingBlocksNeighbours(false);
      }
    }
  }

  // public void SetBlockPlacing(bool isEnabled)
  // {
  //   if (isEnabled)
  //   {
  //     _materialRecolor.SetColor(BlockColorType.GRAY);
  //     _disableBlockPlacing = false;
  //   }
  //   else
  //   {
  //     _materialRecolor.SetColor(BlockColorType.RED);
  //     _disableBlockPlacing = true;
  //   }
  // }

  // public void SetBlockPlacingNeighbours(bool isEnabled)
  // {
  //   foreach (BlockDirection direction in Enum.GetValues(typeof(BlockDirection)))
  //   {
  //     Vector2Int gridPosOther = GridPosition + direction.GetGridPositionOffset(GridPosition);

  //     GridSlot slotOther = _gameGrid.GetGridSlot(gridPosOther);
  //     if (slotOther == null)
  //     {
  //       continue;
  //     }

  //     slotOther.SetBlockPlacing(isEnabled);
  //   }
  // }

  public void SetIsMovingBlocksNeighbours(bool isEnabled)
  {
    BlockPile current = BlockPile;

    if (current != null)
    {
      current.IsMovingBlocks = isEnabled;
    }

    foreach (BlockDirection direction in Enum.GetValues(typeof(BlockDirection)))
    {
      Vector2Int gridPosOther = GridPosition + direction.GetGridPositionOffset(GridPosition);

      GridSlot slotOther = _gameGrid.GetGridSlot(gridPosOther);
      if (slotOther == null)
      {
        continue;
      }

      current = slotOther.BlockPile;

      if (current != null)
      {
        current.IsMovingBlocks = isEnabled;
      }
    }
  }
}
