using System;
using CarterGames.Assets.AudioManager;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class Block : MonoBehaviour
{
  private Collider _collider;
  private float _width;
  private float _duration;
  private float _durationY;
  private float _durationDestroy;
  private float _tweenDurationScaleFactorBase;
  private Ease _ease;
  private Ease _easeDestroy;
  private MaterialRecolor _materialRecolor;
  public BlockColorType Color => _materialRecolor.BlockColor;
  private Vector3 _startScale;
  private AudioManager _audioManager;

  private void Start()
  {
    _collider = GetComponent<Collider>();
    _width = GameManager.Instance.BlockWidth;
    _duration = GameManager.Instance.FlipDuration;
    _durationY = _duration / 2f;
    _durationDestroy = GameManager.Instance.DestroyDuration;
    _ease = GameManager.Instance.EaseDefault;
    _easeDestroy = GameManager.Instance.EaseDestroy;
    _tweenDurationScaleFactorBase = GameManager.Instance.TweenDurationScaleFactorBase;


    _startScale = transform.localScale;

    // SetColor(GameManager.Instance.RandomBlockColor());

    _audioManager = AudioManager.instance;
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
    // Make tweens faster
    // float scaleFactor = Mathf.Pow(_tweenDurationScaleFactorBase, movedBefore + 1);
    float scaleFactor = Mathf.Pow(_tweenDurationScaleFactorBase, 1);

    // Apply scaling factor to durations
    float duration = _duration * scaleFactor;
    float durationY = _durationY * scaleFactor;

    if (transform.localPosition.y != localY)
    {
      Tween.LocalPositionY(transform, endValue: localY, duration: durationY, ease: _ease);
    }

    Rotate(blockRotationDirection, duration, onComplete);

    _audioManager.Play("flip");
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
    // Make tweens faster
    float scaleFactor = Mathf.Pow(_tweenDurationScaleFactorBase, index + 1);

    // Apply scaling factor to durations
    float duration = _durationDestroy * scaleFactor;
    float delay = _durationDestroy * _tweenDurationScaleFactorBase - duration;

    DelayedDestroySFX(delay);

    Tween.Scale(transform, endValue: 0, duration: duration, ease: _easeDestroy, startDelay: delay)
      .OnComplete(() =>
      {
        transform.parent = null;
        gameObject.SetActive(false);
        onComplete?.Invoke();
      });
  }

  private async void DelayedDestroySFX(float delay)
  {
    await UniTask.Delay((int)(delay * 1000));

    _audioManager.Play("destroy");
  }
}
