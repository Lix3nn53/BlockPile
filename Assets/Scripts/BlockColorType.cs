using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public enum BlockColorType
{
  BLUE,
  CYAN,
  RED,
  ORANGE,
  YELLOW,
  PURPLE,
  GRAY // LAST INDEX IS ONLY SLOT COLOR AND IGNORED BY BLOCKS
}
public static class BlockColorTypeExtensions
{
  public static Color GetColor(this BlockColorType color)
  {
    Color result = Color.white;

    switch (color)
    {
      case BlockColorType.BLUE:
        ColorUtility.TryParseHtmlString("#41a6f6", out result);
        break;
      case BlockColorType.CYAN:
        ColorUtility.TryParseHtmlString("#73eff7", out result);
        break;
      case BlockColorType.RED:
        ColorUtility.TryParseHtmlString("#b13e53", out result);
        break;
      case BlockColorType.ORANGE:
        ColorUtility.TryParseHtmlString("#ef7d57", out result);
        break;
      case BlockColorType.YELLOW:
        ColorUtility.TryParseHtmlString("#ffcd75", out result);
        break;
      case BlockColorType.PURPLE:
        ColorUtility.TryParseHtmlString("#5d275d", out result);
        break;
      case BlockColorType.GRAY:
        ColorUtility.TryParseHtmlString("#94b0c2", out result);
        break;
    };

    return result;
  }
  // public static BlockColorType RandomBlockColor()
  // {
  //   int randomIndex = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(BlockColorType)).Length - 1);

  //   return (BlockColorType)randomIndex;
  // }
}