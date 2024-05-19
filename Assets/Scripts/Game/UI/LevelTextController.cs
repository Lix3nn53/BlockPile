using UnityEngine;
using UnityEngine.UI;

public class LevelTextController : MonoBehaviour
{
  private Text _valueText;
  private Player _player;

  private void Start()
  {
    _valueText = GetComponent<Text>();

    _player = Player.Instance;
    _player.Data.OnExperienceChange += OnExpChanged;

    OnExpChanged();
  }

  private void OnDestroy()
  {
    _player.Data.OnExperienceChange -= OnExpChanged;
  }

  // This method is called whenever the slider value changes
  void OnExpChanged()
  {
    _valueText.text = "Level " + _player.Data.GetLevel();
  }
}
