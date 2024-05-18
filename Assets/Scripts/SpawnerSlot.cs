using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class SpawnerSlot : GridSlotBase
{
  private GameObjectPool _blockPilePool;

  public override void Start()
  {
    base.Start();

    // Color
    _materialRecolor = new MaterialRecolor(GetComponentInChildren<Renderer>(), BlockColorType.GRAY);

    // Add self to GameManager
    GameManager.Instance.Spawners.Add(this);

    // Block Pile Pool
    _blockPilePool = AssetManager.Instance.BlockPilePool;

    // Spawn
    SpawnBlockPile();
  }

  public void SpawnBlockPile()
  {
    BlockPile BlockPile = _blockPilePool.Pool.Get().GetComponent<BlockPile>();

    BlockPile.Initialize();

    for (int i = 0; i < 1; i++)
    {
      BlockPile.SpawnBlock();
    }

    SetBlockPile(BlockPile);
  }
}
