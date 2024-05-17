using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOrthographicSize : MonoBehaviour
{
    public float DesiredWidth = 10f; // Desired width of the viewable area
    public float MaxRatio = 0.7f; // Desired width of the viewable area
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();

        AdjustOrthographicSize();
    }

    public void AdjustOrthographicSize()
    {
        // Get the aspect ratio of the screen
        float aspectRatio = (float)UnityEngine.Device.Screen.width / UnityEngine.Device.Screen.height;

        if (aspectRatio > MaxRatio)
        {
            aspectRatio = MaxRatio;
        }

        // Calculate the orthographic size based on the desired width
        float orthographicSize = DesiredWidth / aspectRatio / 2f;

        // Set the orthographic size of the camera
        _camera.orthographicSize = orthographicSize;
    }
}
