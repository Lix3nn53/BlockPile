using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class BlockPile : MonoBehaviour
{
  public bool IsMovable = false;
  private GameObjectPool _blockPool;
  private List<Block> _blocks = new();

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

    Vector3 pos = new Vector3(0, _blockHeight * _blocks.Count, 0);
    block.transform.localPosition = pos;

    block.gameObject.SetActive(true);

    _blocks.Add(block);
  }
}
