using UnityEngine;

public class PlayerData : WithExperience
{
  public override int GetRequiredExperience(int level)
  {
    return (int)(10 + Mathf.Round(10 * Mathf.Pow(level, 2)) + 0.5); // exp formula
  }
}