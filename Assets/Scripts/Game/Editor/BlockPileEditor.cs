using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockPile))]
public class BlockPileEditor : Editor
{
  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    BlockPile instance = (BlockPile)target;

    // Display a button in the inspector
    if (GUILayout.Button("Spawn Block"))
    {
      // Call the method to adjust the orthographic size
      instance.SpawnBlock();
    }
  }
}
