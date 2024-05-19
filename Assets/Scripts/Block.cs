using System;
using PrimeTween;
using UnityEngine;

public class Block : MonoBehaviour
{
  private Collider _collider;
  private float _width;
  private float _duration;
  private float _durationDestroyMin;
  private float _durationDestroyMax;
  private Ease _ease;
  private Ease _easeDestroy;
  private MaterialRecolor _materialRecolor;
  public BlockColorType Color => _materialRecolor.BlockColor;
  private Vector3 _startScale;

  private void Start()
  {
    _collider = GetComponent<Collider>();
    _width = GameManager.Instance.BlockWidth;
    _duration = GameManager.Instance.FlipDuration;
    _ease = GameManager.Instance.EaseDefault;
    _easeDestroy = GameManager.Instance.EaseDestroy;
    _durationDestroyMin = _duration / 2f;
    _durationDestroyMax = _duration * 4f;

    _startScale = transform.localScale;

    // SetColor(GameManager.Instance.RandomBlockColor());
  }

  public void OnPickUp()
  {
    _collider.enabled = false;
  }

  public void OnMoveBackToSpawner()
  {
    _collider.enabled = true;
  }

  private void OnEnable()
  {
    // Get From Pool
    if (_collider != null)
    {
      // Start is called

      _collider.enabled = true;
      transform.localScale = _startScale;
    }
  }

  public void Rotate(BlockDirection blockRotationDirection, float duration, Action onComplete = null)
  {
    BlockRotation blockRotation = blockRotationDirection.GetBlockRotation();
    Vector3 direction = blockRotation.Direction;
    Vector3 rotation = blockRotation.Rotation;

    Transform child = transform.GetChild(0);

    child.position = child.position + (direction * _width / 2);
    transform.position = transform.position + (-direction * _width / 2);

    Tween.EulerAngles(transform, startValue: Vector3.zero, endValue: rotation, duration: duration, ease: _ease).OnComplete(() =>
    {
      transform.SetPositionAndRotation(child.position, Quaternion.identity);
      child.localPosition = Vector3.zero;

      onComplete?.Invoke();
    });
  }

  public void Flip(BlockDirection blockRotationDirection, float localY, int movedBefore, Action onComplete)
  {
    float duration = _duration;
    float durationY = _durationDestroyMin;

    if (movedBefore > 0)
    {
      // Make tweens faster

      float scaleFactor = Mathf.Pow(0.9f, movedBefore); // Adjust the base (0.9f) as needed

      Debug.Log("scaleFactor: " + scaleFactor);

      // Apply scaling factor to durations
      duration *= scaleFactor;
      durationY *= scaleFactor;
    }

    if (transform.localPosition.y != localY)
    {
      Tween.LocalPositionY(transform, endValue: localY, duration: durationY, ease: _ease);
    }


    Rotate(blockRotationDirection, duration, onComplete);
  }

  public void SetColor(BlockColorType color)
  {
    if (_materialRecolor == null)
    {
      _materialRecolor = new MaterialRecolor(GetComponentInChildren<Renderer>(), color);
    }
    else
    {
      _materialRecolor.SetColor(color);
    }
  }

  public void DestroyAnimation(int index, int amount, Action onComplete)
  {
    float t = 1 - (float)(index + 1) / amount;

    float duration = Mathf.Lerp(_durationDestroyMin, _durationDestroyMax, t);
    float delay = _durationDestroyMax - duration;

    Tween.Scale(transform, endValue: 0, duration: duration, ease: _easeDestroy, startDelay: delay)
      .OnComplete(() =>
      {
        transform.parent = null;
        gameObject.SetActive(false);
        onComplete?.Invoke();
      });
  }
}
