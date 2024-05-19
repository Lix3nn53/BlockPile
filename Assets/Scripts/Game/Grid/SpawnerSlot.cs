using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class SpawnerSlot : GridSlotBase
{
  private GameObjectPool _blockPilePool;
  private float _spawnDuration;

  public override void Start()
  {
    base.Start();

    _spawnDuration = GameManager.Instance.MoveBackDuration;

    // Color
    MaterialRecolor = new MaterialRecolor(GetComponentInChildren<Renderer>(), BlockColorType.GRAY);

    // Block Pile Pool
    _blockPilePool = AssetManager.Instance.BlockPilePool;

    // Spawn
    GameManager.Instance.OnSpawnerReady();
  }

  public void SpawnBlockPile(Transform spawnPoint)
  {
    BlockPile BlockPile = _blockPilePool.Pool.Get().GetComponent<BlockPile>();

    BlockPile.Initialize();

    for (int i = 0; i < 1; i++)
    {
      BlockPile.SpawnBlock();
    }

    Vector3 localPos = OnBlockPileStartPlace(BlockPile);

    if (spawnPoint == null)
    {
      BlockPile.transform.localPosition = localPos;
      BlockPile.gameObject.SetActive(true);
    }
    else
    {
      BlockPile.transform.position = spawnPoint.position;
      BlockPile.gameObject.SetActive(true);
      BlockPile.PlaceAnimation(transform.position + localPos, _spawnDuration, OnBlockPilePlaceFinished);
    }
  }

  public override void OnBlockPilePlaceFinished()
  {
    BlockPile.OnSpawnAnimationComplete();
  }
}
