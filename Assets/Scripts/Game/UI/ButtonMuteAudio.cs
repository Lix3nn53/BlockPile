using CarterGames.Assets.AudioManager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ButtonMuteAudio : MonoBehaviour
{
  private Button _button;
  private AudioMixer _audioMixer;
  [SerializeField] private Sprite _spriteOn;
  [SerializeField] private Sprite _spriteOff;

  private bool _isMuted = false;

  private void Start()
  {
    _button = GetComponent<Button>();

    _audioMixer = AssetManager.Instance.AudioMixer;

    _button.onClick.AddListener(OnClick);
  }

  private void OnDestroy()
  {
    _button.onClick.RemoveListener(OnClick);
  }

  private void OnClick()
  {
    _isMuted = !_isMuted;

    float volume = _isMuted ? -80 : 0;

    _audioMixer.SetFloat("MasterVolume", volume);

    Sprite sprite = _isMuted ? _spriteOff : _spriteOn;
    _button.image.sprite = sprite;
  }
}
