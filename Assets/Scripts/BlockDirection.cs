using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public enum BlockDirection
{
  LEFT,
  RIGHT,
  FORWARD,
  BACK
}
public static class BlockDirectionExtensions
{
  public static BlockRotation GetBlockRotation(this BlockDirection direction)
  {
    return direction switch
    {
      BlockDirection.LEFT => new BlockRotation(Vector3.left, new Vector3(0, 0, -180)),
      BlockDirection.RIGHT => new BlockRotation(Vector3.right, new Vector3(0, 0, 180)),
      BlockDirection.FORWARD => new BlockRotation(Vector3.forward, new Vector3(-180, 0, 0)),
      BlockDirection.BACK => new BlockRotation(Vector3.back, new Vector3(180, 0, 0)),
      _ => throw new System.NotImplementedException(),
    };
  }
  public static Vector2Int GetGridPositionOffset(this BlockDirection direction, Vector2Int center)
  {
    Vector2Int result = new Vector2Int();
    if (direction == BlockDirection.LEFT)
    {
      result.y = 1;

      if (center.y % 2 == 0)
      {
        // Even
        result.x = 1;
      }
    }
    else if (direction == BlockDirection.RIGHT)
    {
      result.y = -1;

      if (center.y % 2 == 1)
      {
        // Odd
        result.x = -1;
      }
    }
    else if (direction == BlockDirection.FORWARD)
    {
      result.y = -1;

      if (center.y % 2 == 0)
      {
        // Even
        result.x = 1;
      }
    }
    else if (direction == BlockDirection.BACK)
    {
      result.y = 1;

      if (center.y % 2 == 1)
      {
        // Odd
        result.x = -1;
      }
    }

    return result;
  }
  public static BlockDirection Opposite(this BlockDirection direction)
  {
    return direction switch
    {
      BlockDirection.LEFT => BlockDirection.RIGHT,
      BlockDirection.RIGHT => BlockDirection.LEFT,
      BlockDirection.FORWARD => BlockDirection.BACK,
      BlockDirection.BACK => BlockDirection.FORWARD,
      _ => throw new System.NotImplementedException(),
    };
  }
}