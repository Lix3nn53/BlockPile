using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class SpawnerSlot : GridSlotBase
{

  public override void Start()
  {
    base.Start();

    BlockPile BlockPile = AssetManager.Instance.BlockPilePool.Pool.Get().GetComponent<BlockPile>();

    BlockPile.Initialize();

    SpawnBlockPile(BlockPile);

    for (int i = 0; i < 1; i++)
    {
      BlockPile.SpawnBlock();
    }
  }
}
