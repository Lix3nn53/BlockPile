using System;
using PrimeTween;
using UnityEngine;

public class MaterialRecolor
{
  private AssetManager _assetManager;
  private Renderer _renderer;
  private BlockColorType _blockColor = BlockColorType.SLOT;
  public BlockColorType BlockColor => _blockColor;
  private MaterialPropertyBlock _propertyBlock;

  public MaterialRecolor(Renderer renderer, BlockColorType blockColor)
  {
    _assetManager = AssetManager.Instance;
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

    _propertyBlock.SetColor("_BaseColor", _assetManager.GetColor(_blockColor));

    _renderer.SetPropertyBlock(_propertyBlock);
  }
}
