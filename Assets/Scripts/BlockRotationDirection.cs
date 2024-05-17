using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public enum BlockRotationDirection
{
  LEFT,
  RIGHT,
  FORWARD,
  BACK
}
public static class BlockRotationExtensions
{
  public static BlockRotation GetBlockRotation(this BlockRotationDirection direction)
  {
    switch (direction)
    {
      case BlockRotationDirection.LEFT:
        return new BlockRotation(Vector3.left, new Vector3(0, 0, -180));
      case BlockRotationDirection.RIGHT:
        return new BlockRotation(Vector3.right, new Vector3(0, 0, 180));
      case BlockRotationDirection.FORWARD:
        return new BlockRotation(Vector3.forward, new Vector3(-180, 0, 0));
      case BlockRotationDirection.BACK:
        return new BlockRotation(Vector3.back, new Vector3(180, 0, 0));
    }

    return null;
  }
}