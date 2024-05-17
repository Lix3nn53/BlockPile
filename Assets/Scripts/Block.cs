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
    Transform child = transform.GetChild(0);

    child.position = child.position + (Vector3.left * _width / 2);
    transform.position = transform.position + (Vector3.right * _width / 2);

    Tween.EulerAngles(transform, startValue: Vector3.zero, endValue: new Vector3(0, 0, -180), duration: 1).OnComplete(() =>
    {
      transform.SetPositionAndRotation(child.position, Quaternion.identity);
      child.localPosition = Vector3.zero;
    });

  }
}
