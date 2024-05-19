using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Block))]
public class BlockEditor : Editor
{
  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    Block instance = (Block)target;

    // Display a button in the inspector
    if (GUILayout.Button("Rotate LEFT"))
    {
      // Call the method to adjust the orthographic size
      instance.Rotate(BlockDirection.LEFT, 0.2f);
    }

    if (GUILayout.Button("Rotate RIGHT"))
    {
      // Call the method to adjust the orthographic size
      instance.Rotate(BlockDirection.RIGHT, 0.2f);
    }

    if (GUILayout.Button("Rotate FORWARD"))
    {
      // Call the method to adjust the orthographic size
      instance.Rotate(BlockDirection.FORWARD, 0.2f);
    }

    if (GUILayout.Button("Rotate BACK"))
    {
      // Call the method to adjust the orthographic size
      instance.Rotate(BlockDirection.BACK, 0.2f);
    }
  }
}
