using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetOrthographicSize))]
public class SetOrthographicSizeEditor : Editor
{
  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    SetOrthographicSize setOrthoSize = (SetOrthographicSize)target;

    // Display a button in the inspector
    if (GUILayout.Button("Adjust Orthographic Size"))
    {
      // Call the method to adjust the orthographic size
      setOrthoSize.AdjustOrthographicSize();
    }
  }
}
