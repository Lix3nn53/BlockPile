using UnityEngine;

public abstract class WithExperience
{
  [SerializeField] private int _totalExp;
  public int TotalExp
  {
    get
    {
      return _totalExp;
    }
    set
    {
      _totalExp = value;
      OnExperienceChange?.Invoke();
    }
  }
  public delegate void ExperienceChangeCallback();
  public event ExperienceChangeCallback OnExperienceChange;
  public abstract int GetRequiredExperience(int level);
  public int GetRequiredExperience()
  {
    return GetRequiredExperience(GetLevel());
  }

  public virtual int GetLevel()
  {
    if (TotalExp < 0)
    {
      return 0;
    }

    int experience = 0;
    for (int i = 1; i < 90; i++)
    {
      experience += GetRequiredExperience(i);
      if (TotalExp < experience)
      {
        return i;
      }
    }
    return 90;
  }

  public int GetTotalRequiredExperience(int level)
  {
    int experience = 0;
    for (int i = 1; i < level; i++)
    {
      experience += GetRequiredExperience(i);
    }
    return experience;
  }

  public int GetCurrentExperience()
  {
    int level = GetLevel();
    int totalExpForLevel = GetTotalRequiredExperience(level);
    return TotalExp - totalExpForLevel;
  }

  public int GetCurrentRequiredExperience()
  {
    return GetRequiredExperience(GetLevel());
  }
}
