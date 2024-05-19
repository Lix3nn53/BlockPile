using System;
using PrimeTween;
using UnityEngine;

public class MaterialRecolor
{
  private Renderer _renderer;
  private BlockColorType _blockColor = BlockColorType.GRAY;
  public BlockColorType BlockColor => _blockColor;
  private MaterialPropertyBlock _propertyBlock;

  public MaterialRecolor(Renderer renderer, BlockColorType blockColor)
  {
    _renderer = renderer;
    _blockColor = blockColor;

    _propertyBlock = new MaterialPropertyBlock();
    SetColor(_blockColor);
  }

  public void SetColor(BlockColorType color)
  {
    if (_renderer == null)
    {
      return;
    }

    _blockColor = color;

    _propertyBlock.SetColor("_BaseColor", _blockColor.GetColor());

    _renderer.SetPropertyBlock(_propertyBlock);
  }
}
