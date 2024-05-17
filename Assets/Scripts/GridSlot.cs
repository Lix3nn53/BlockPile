using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
  public static Vector2Int INVALID_POS = new Vector2Int(-999, -999);
  public Vector2Int GridPosition = INVALID_POS;

  public bool IsValid()
  {
    return GridPosition != INVALID_POS;
  }

  public List<Block> Blocks;
}
