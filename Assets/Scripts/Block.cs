using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class Block : MonoBehaviour
{
  private Collider _collider;
  private float _width;

  private void Start()
  {
    _collider = GetComponent<Collider>();
    _width = GameManager.Instance.BlockWidth;
  }

  public void OnPickUp()
  {
    _collider.enabled = false;
  }

  public void OnPlace()
  {
    _collider.enabled = true;
  }

  public void Test()
  {
    Rotate(BlockRotationDirection.RIGHT);
  }

  public void Rotate(BlockRotationDirection blockRotationDirection)
  {
    BlockRotation blockRotation = blockRotationDirection.GetBlockRotation();
    Vector3 direction = blockRotation.Direction;
    Vector3 rotation = blockRotation.Rotation;

    Transform child = transform.GetChild(0);

    child.position = child.position + (direction * _width / 2);
    transform.position = transform.position + (-direction * _width / 2);

    Tween.EulerAngles(transform, startValue: Vector3.zero, endValue: rotation, duration: 1).OnComplete(() =>
    {
      transform.SetPositionAndRotation(child.position, Quaternion.identity);
      child.localPosition = Vector3.zero;
    });
  }
}
