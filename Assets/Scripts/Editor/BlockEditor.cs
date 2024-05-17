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
    if (GUILayout.Button("Test"))
    {
      // Call the method to adjust the orthographic size
      instance.Test();
    }
  }
}
