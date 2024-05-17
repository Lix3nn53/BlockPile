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
}
