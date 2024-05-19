using UnityEngine;
using UnityEngine.UI;

public class ExperienceSliderController : MonoBehaviour
{
  private Slider _slider;
  private Text _valueText;
  private Player _player;

  private void Start()
  {
    _slider = GetComponent<Slider>();
    _valueText = GetComponentInChildren<Text>();

    _player = Player.Instance;
    _player.Data.OnExperienceChange += OnExpChanged;

    OnExpChanged();
  }

  private void OnDestroy()
  {
    _player.Data.OnExperienceChange -= OnExpChanged;
  }

  private void OnExpChanged()
  {
    int current = _player.Data.GetCurrentExperience();
    int req = _player.Data.GetRequiredExperience();

    _slider.value = (float)current / req;


    _valueText.text = current + " / " + req;
  }
}
