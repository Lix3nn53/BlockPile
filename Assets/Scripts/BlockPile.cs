using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;
using PrimeTween;

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

  public void PlaceAnimation(Vector3 targetPos, float duration)
  {
    _isMovable = false;

    Tween.Position(transform, targetPos, duration, ease: Ease.OutSine).OnComplete(() => OnPlace());
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
}
