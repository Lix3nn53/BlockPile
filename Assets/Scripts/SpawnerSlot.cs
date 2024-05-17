using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class SpawnerSlot : MonoBehaviour
{
  public BlockPile BlockPile;

  private void Start()
  {
    BlockPile = AssetManager.Instance.BlockPilePool.Pool.Get().GetComponent<BlockPile>();

    BlockPile.Initialize();
    BlockPile.transform.position = transform.position;
    BlockPile.gameObject.SetActive(true);
    BlockPile.IsMovable = true;

    for (int i = 0; i < 1; i++)
    {
      BlockPile.SpawnBlock();
    }
  }

  public void OnBlockPileTaken()
  {
    // BlockPile = AssetManager.Instance.BlockPilePool.Pool.Get().GetComponent<BlockPile>();
  }
}
