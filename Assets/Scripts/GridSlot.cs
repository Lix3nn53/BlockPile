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

    MaterialRecolor = new MaterialRecolor(GetComponent<Renderer>(), BlockColorType.GRAY);
  }

  public bool IsValid()
  {
    return GridPosition != INVALID_POS;
  }

  public override void OnBlockPilePlaceFinished()
  {
    if (!IsAnyNeighbourMoving())
    {
      TryGetBlockPileFromNeighboursAllDirections(true);
    }
    _gameManager.CheckSpawners();
  }

  public void TryGetBlockPileFromNeighboursAllDirections(bool recursive)
  {
    // SetBlockPlacingNeighbour(false);

    TryGetBlockPileFromNeighbours(Enum.GetValues(typeof(BlockDirection)).Cast<BlockDirection>().ToList(), recursive);
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

    return slotOther.TryMoveBlockPileTo(opposite, current, directionsNotChecked);
  }

  public bool TryMoveBlockPileTo(BlockDirection direction, BlockPile target, List<BlockDirection> directionsNotChecked)
  {
    BlockPile current = BlockPile;

    if (current == null)
    {
      return false;
    }

    if (target.IsMovingBlocks)
    {
      return false;
    }

    current.IsMovingBlocks = true;
    target.IsMovingBlocks = true;

    return TryMoveTopBlockRecursive(direction, current, target, directionsNotChecked, false);
  }

  public void OnMoveTopBlock(BlockDirection direction, BlockPile current, BlockPile target, Block block,
    List<BlockDirection> directionsNotChecked, bool movedBefore)
  {
    target.PlaceBlock(block);

    TryMoveTopBlockRecursive(direction, current, target, directionsNotChecked, movedBefore);
  }

  public bool TryMoveTopBlockRecursive(BlockDirection direction, BlockPile current, BlockPile target,
    List<BlockDirection> directionsNotChecked, bool movedBefore)
  {
    bool moved = false;
    Block topBlock = current.GetTopBlock();

    if (topBlock != null)
    {
      // Try to move top block from current pile to target pile

      if (target.CanMove(topBlock))
      {
        float localY = target.GetNextLocalHeight();
        topBlock.Move(direction, localY, () => OnMoveTopBlock(direction, current, target, topBlock, directionsNotChecked, true));
        moved = true;
      }
    }
    else
    {
      // current pile is empty
      // return current pile to pool
      current.IsMovingBlocks = false;
      current.transform.parent = null;
      current.gameObject.SetActive(false);
    }


    if (!moved)
    {
      current.IsMovingBlocks = false;
      target.IsMovingBlocks = false;
    }

    if (!moved && movedBefore)
    {
      // Couldnt move from current to target
      // Try to move to target from other neighbours

      Vector2Int targetGridPos = GridPosition + direction.GetGridPositionOffset(GridPosition);
      GridSlot targetSlot = _gameGrid.GetGridSlot(targetGridPos);

      // targetSlot.TryGetBlockPileFromNeighbours(directionsNotChecked, true);

      List<BlockDirection> directionsNotCheckedTryMove = Enum.GetValues(typeof(BlockDirection)).Cast<BlockDirection>().ToList();
      // directionsNotCheckedTryMove.Remove(direction.Opposite());

      targetSlot.TryMoveBlockPileToNeighbours(directionsNotCheckedTryMove, false);
    }

    return moved;
  }

  public void OnBlockMovementFinished(BlockPile current, bool recursive)
  {
    // Block movement animation is played and finished on this slot

    if (recursive)
    {
      // TryGetBlockPileFromNeighboursAllDirections(false);
      List<BlockDirection> directionsNotCheckedTryMove = Enum.GetValues(typeof(BlockDirection)).Cast<BlockDirection>().ToList();
      TryMoveBlockPileToNeighbours(directionsNotCheckedTryMove, false);
    }
    else
    {
      current.TryDestroy();
    }
  }

  public void TryMoveBlockPileToNeighbours(List<BlockDirection> directionsNotChecked, bool recursive)
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

      Vector2Int gridPosOther = GridPosition + direction.GetGridPositionOffset(GridPosition);

      GridSlot slotOther = _gameGrid.GetGridSlot(gridPosOther);

      if (slotOther != null)
      {
        BlockPile currentOther = slotOther.BlockPile;

        if (currentOther != null)
        {
          // List<BlockDirection> copyOpposite = new List<BlockDirection>();

          // foreach (BlockDirection directionCopy in copy)
          // {
          //   copyOpposite.Add(directionCopy.Opposite());
          // }

          if (slotOther.TryGetBlockPileFromNeighbour(currentOther, direction.Opposite(), copy))
          {
            break;
          }
        }
      }

      if (copy.Count == 0)
      {
        OnBlockMovementFinished(current, recursive);
        break;
      }
    }
  }

  public bool IsAnyNeighbourMoving()
  {
    foreach (BlockDirection direction in Enum.GetValues(typeof(BlockDirection)))
    {
      Vector2Int gridPosOther = GridPosition + direction.GetGridPositionOffset(GridPosition);

      GridSlot slotOther = _gameGrid.GetGridSlot(gridPosOther);

      if (slotOther != null)
      {
        BlockPile currentOther = slotOther.BlockPile;

        if (currentOther != null)
        {
          if (currentOther.IsMovingBlocks)
          {
            return true;
          }
        }
      }
    }

    return false;
  }
}
