using System;
using System.Collections;
using System.Collections.Generic;
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
  public List<SpawnerSlot> Spawners = new();

  public BlockColorType RandomBlockColor()
  {
    int randomIndex = UnityEngine.Random.Range(0, AvailableColors.Count);

    return AvailableColors[randomIndex];
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
