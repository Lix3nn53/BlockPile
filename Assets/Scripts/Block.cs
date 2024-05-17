using System;
using PrimeTween;
using UnityEngine;

public class Block : MonoBehaviour
{
  private Collider _collider;
  private float _width;
  private float _duration;

  private void Start()
  {
    _collider = GetComponent<Collider>();
    _width = GameManager.Instance.BlockWidth;
    _duration = GameManager.Instance.MoveDuration;
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
    Rotate(BlockDirection.RIGHT);
  }

  public void Rotate(BlockDirection blockRotationDirection, Action onComplete = null)
  {
    BlockRotation blockRotation = blockRotationDirection.GetBlockRotation();
    Vector3 direction = blockRotation.Direction;
    Vector3 rotation = blockRotation.Rotation;

    Transform child = transform.GetChild(0);

    child.position = child.position + (direction * _width / 2);
    transform.position = transform.position + (-direction * _width / 2);

    Tween.EulerAngles(transform, startValue: Vector3.zero, endValue: rotation, duration: _duration, ease: Ease.OutSine).OnComplete(() =>
    {
      transform.SetPositionAndRotation(child.position, Quaternion.identity);
      child.localPosition = Vector3.zero;

      onComplete?.Invoke();
    });
  }

  public void Move(BlockDirection blockRotationDirection, float localY, Action onComplete)
  {
    Tween.LocalPositionY(transform, endValue: localY, duration: _duration, ease: Ease.OutSine);
    Rotate(blockRotationDirection, onComplete);
  }
}
