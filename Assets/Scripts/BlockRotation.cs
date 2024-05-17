using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class BlockRotation
{
  public Vector3 Direction;
  public Vector3 Rotation;
  public BlockRotation(Vector3 direction, Vector3 rotation)
  {
    Direction = direction;
    Rotation = rotation;
  }
}