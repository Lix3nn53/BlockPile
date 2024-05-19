using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;
using PrimeTween;
using System;

public class BlockPile : MonoBehaviour
{
  private bool _isPickable = true;
  public bool IsPickable => _isPickable;
  private GameObjectPool _blockPool;
  private float _blockHeight;
  private Ease _ease;
  private bool _isMovingBlocks;
  public bool IsMovingBlocks
  {
    get
    {
      return _isMovingBlocks;
    }
    set
    {
      _isMovingBlocks = value;

      GridSlot slot = GetComponentInParent<GridSlot>();
      if (slot != null)
      {
        BlockColorType color = _isMovingBlocks ? BlockColorType.SLOT_DISABLED : BlockColorType.SLOT;
        slot.MaterialRecolor.SetColor(color);
      }
    }
  }

  public void Initialize()
  {
    _blockPool = AssetManager.Instance.BlockPool;
    _blockHeight = GameManager.Instance.BlockHeight;
    _ease = GameManager.Instance.EaseDefault;

    _isPickable = true;
    IsMovingBlocks = false;
  }

  public void OnSpawnAnimationComplete()
  {
    _isPickable = true;
  }

  public void SpawnBlock()
  {
    List<BlockColorType> alreadyUsedColors = new List<BlockColorType>();

    int colorAmount = GameManager.Instance.RandomSpawnColorAmount();

    for (int i = 0; i < colorAmount; i++)
    {
      BlockColorType? color = GameManager.Instance.RandomBlockColor(alreadyUsedColors);

      if (!color.HasValue)
      {
        // Used all available colors from GameManager
        break;
      }

      alreadyUsedColors.Add(color.Value);

      int amount = GameManager.Instance.RandomSpawnAmountPerColor();
      for (int y = 0; y < amount; y++)
      {
        SpawnBlock(color.Value);
      }
    }
  }

  public void SpawnBlock(BlockColorType color)
  {
    GameObject go = _blockPool.Pool.Get();
    Block block = go.GetComponent<Block>();

    block.transform.parent = transform;

    block.transform.localPosition = new Vector3(0, _blockHeight * transform.childCount, 0);
    block.SetColor(color);
    block.gameObject.SetActive(true);
  }

  public void PlaceAnimation(Vector3 targetPos, float duration, Action onComplete = null)
  {
    _isPickable = false;

    Tween.Position(transform, targetPos, duration, ease: Ease.OutSine)
      .OnComplete(() =>
      {
        OnPlace();
        onComplete?.Invoke();
      });
  }

  public void OnPickUp()
  {
    _isPickable = false;

    for (int i = 0; i < transform.childCount; i++)
    {
      Block block = transform.GetChild(i).GetComponent<Block>();

      if (block != null)
      {
        block.OnPickUp();
      }
    }
  }

  public void OnPlace()
  {
  }

  public void OnMoveBackToSpawner()
  {
    _isPickable = true;

    for (int i = 0; i < transform.childCount; i++)
    {
      Block block = transform.GetChild(i).GetComponent<Block>();

      if (block != null)
      {
        block.OnMoveBackToSpawner();
      }
    }
  }

  public Vector3 GetNextBlockPosition()
  {
    return transform.position + new Vector3(0, _blockHeight * (transform.childCount + 1), 0);
  }
  public float GetNextLocalHeight()
  {
    return _blockHeight * (transform.childCount + 1);
  }

  public Block GetBlock(int index)
  {
    if (index >= transform.childCount || index < 0)
    {
      return null;
    }

    return transform.GetChild(index).GetComponent<Block>();
  }
  public Block GetTopBlock()
  {
    return GetBlock(transform.childCount - 1);
  }

  public void PlaceBlock(Block block)
  {
    block.transform.parent = transform;
  }

  public bool CanMove(Block block)
  {
    Block topBlock = GetTopBlock();

    if (topBlock == null)
    {
      return true;
    }

    if (topBlock.Color == block.Color)
    {
      return true;
    }

    return false;
  }

  private int CountTopColor()
  {
    Block topBlock = GetTopBlock();
    BlockColorType color = topBlock.Color;

    int count = 0;

    int childCount = transform.childCount;

    for (int i = childCount - 1; i >= 0; i--)
    {
      Block block = transform.GetChild(i).GetComponent<Block>();

      if (block != null)
      {
        if (color != block.Color)
        {
          break;
        }

        count++;
      }
    }

    return count;
  }

  public bool TryDestroy(Action onCompleteWithBlocksRemaining)
  {
    int topColorCount = CountTopColor();
    if (topColorCount >= 10)
    {
      // DestroyAnimation
      IsMovingBlocks = true;

      Block topBlock = GetTopBlock();
      BlockColorType color = topBlock.Color;

      int childCount = transform.childCount;
      for (int i = childCount - 1; i >= 0; i--)
      {
        Block block = transform.GetChild(i).GetComponent<Block>();

        if (block != null)
        {
          if (color != block.Color)
          {
            break;
          }



          if (i == 0)
          {
            // Last block
            block.DestroyAnimation(childCount - 1 - i, topColorCount, () =>
            {
              IsMovingBlocks = false;
              transform.parent = null;
              gameObject.SetActive(false); // Disable self and return to pool
            });
          }
          else if (i == childCount - topColorCount)
          {
            // Last block of current color
            block.DestroyAnimation(childCount - 1 - i, topColorCount, () =>
            {
              IsMovingBlocks = false;
              onCompleteWithBlocksRemaining?.Invoke();
            });
          }
          else
          {
            block.DestroyAnimation(childCount - 1 - i, topColorCount, null);
          }
        }
      }

      return true;
    }

    return false;
  }
}
