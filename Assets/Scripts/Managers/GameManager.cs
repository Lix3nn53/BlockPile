using System;
using System.Linq;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  // Singleton
  public static GameManager Instance { get; private set; }

  private void Awake()
  {
    // If there is an instance, and it's not me, delete myself.

    if (Instance != null && Instance != this)
    {
      Destroy(this);
    }
    else
    {
      Instance = this;
    }
  }

  public float BlockHeight;
  public float BlockWidth;
  public float MoveBackDuration = 0.5f;
  public float PlaceDuration = 0.25f;
  public float FlipDuration = 0.2f;
  public List<BlockColorType> AvailableColors = new();
  public Vector2Int SpawnColorAmountRange;
  public Vector2Int SpawnAmountPerColorRange;
  public Ease EaseDefault = Ease.OutSine;
  public Ease EaseDestroy = Ease.OutSine;
  [HideInInspector] public List<SpawnerSlot> Spawners = new();

  public BlockColorType RandomBlockColor(List<BlockColorType> alreadyUsedColors)
  {
    List<BlockColorType> available;
    if (alreadyUsedColors != null)
    {
      available = AvailableColors.Except(alreadyUsedColors).ToList();
    }
    else
    {
      available = AvailableColors;
    }

    int randomIndex = UnityEngine.Random.Range(0, available.Count);

    return available[randomIndex];
  }

  public int RandomSpawnColorAmount()
  {
    return UnityEngine.Random.Range(SpawnColorAmountRange.x, SpawnColorAmountRange.y);
  }

  public int RandomSpawnAmountPerColor()
  {
    return UnityEngine.Random.Range(SpawnAmountPerColorRange.x, SpawnAmountPerColorRange.y);
  }

  public void CheckSpawners()
  {
    bool allEmpty = true;
    foreach (SpawnerSlot slot in Spawners)
    {
      if (slot.BlockPile != null)
      {
        allEmpty = false;
        break;
      }
    }

    if (allEmpty)
    {
      // all spawners are empty, spawn blocks
      foreach (SpawnerSlot slot in Spawners)
      {
        slot.SpawnBlockPile();
      }
    }
  }
}
