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
      instance.Rotate(BlockRotationDirection.LEFT);
    }

    if (GUILayout.Button("Rotate RIGHT"))
    {
      // Call the method to adjust the orthographic size
      instance.Rotate(BlockRotationDirection.RIGHT);
    }

    if (GUILayout.Button("Rotate FORWARD"))
    {
      // Call the method to adjust the orthographic size
      instance.Rotate(BlockRotationDirection.FORWARD);
    }

    if (GUILayout.Button("Rotate BACK"))
    {
      // Call the method to adjust the orthographic size
      instance.Rotate(BlockRotationDirection.BACK);
    }
  }
}
