using System;
using PrimeTween;
using UnityEngine;

public class Block : MonoBehaviour
{
  private Collider _collider;
  private float _width;
  private float _duration;
  private float _durationHalf;
  private MaterialRecolor _materialRecolor;
  public BlockColorType Color => _materialRecolor.BlockColor;
  private Vector3 _startScale;

  private void Start()
  {
    _collider = GetComponent<Collider>();
    _width = GameManager.Instance.BlockWidth;
    _duration = GameManager.Instance.FlipDuration;
    _durationHalf = _duration / 2f;

    _materialRecolor = new MaterialRecolor(GetComponentInChildren<Renderer>(), GameManager.Instance.RandomBlockColor());
    _startScale = transform.localScale;
  }

  public void OnPickUp()
  {
    _collider.enabled = false;
  }

  public void OnMoveBackToSpawner()
  {
    _collider.enabled = true;
  }

  public void OnEnable()
  {
    // Get From Pool
    if (_collider != null)
    {
      // Start is called

      _collider.enabled = true;
      transform.localScale = _startScale;
    }
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
    if (transform.localPosition.y != localY)
    {
      Tween.LocalPositionY(transform, endValue: localY, duration: _durationHalf, ease: Ease.OutSine);
    }
    Rotate(blockRotationDirection, onComplete);
  }

  public void SetColor(BlockColorType color)
  {
    _materialRecolor.SetColor(color);
  }

  public void DestroyAnimation(int index, Action onComplete)
  {
    Tween.Scale(transform, endValue: 0, duration: _duration, ease: Ease.OutSine, startDelay: _durationHalf * index)
      .OnComplete(() =>
      {
        gameObject.SetActive(false);
        onComplete?.Invoke();
      });
  }
}
