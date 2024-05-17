using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;
using PrimeTween;
using System;

public class BlockPile : MonoBehaviour
{
  private bool _isMovable = true;
  public bool IsMovable => _isMovable;
  private GameObjectPool _blockPool;
  private float _blockHeight;

  public void Initialize()
  {
    _blockPool = AssetManager.Instance.BlockPool;

    _blockHeight = GameManager.Instance.BlockHeight;
  }

  public void SpawnBlock()
  {
    GameObject go = _blockPool.Pool.Get();
    Block block = go.GetComponent<Block>();

    block.transform.parent = transform;

    block.transform.localPosition = new Vector3(0, _blockHeight * transform.childCount, 0);

    block.gameObject.SetActive(true);
  }

  public void PlaceAnimation(Vector3 targetPos, float duration, Action onComplete = null)
  {
    _isMovable = false;

    Tween.Position(transform, targetPos, duration, ease: Ease.OutSine)
      .OnComplete(() =>
      {
        OnPlace();
        onComplete?.Invoke();
      });
  }

  public void OnPickUp()
  {
    _isMovable = false;

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
    _isMovable = true;

    for (int i = 0; i < transform.childCount; i++)
    {
      Block block = transform.GetChild(i).GetComponent<Block>();

      if (block != null)
      {
        block.OnPlace();
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
}
