using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    public Camera maincamera;

    public float cameraSizeOffset;
    public float xOffSet;
    public float yOffSet;
    // Start is called before the first frame update
    void Start()
    {
        // Invoke("SetCameraCenter", 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        // SetCameraCenter();
        // SetCameraSize();
    }

    void SetCameraCenter()
    {
        GridData gridData = GridData.Instance;
        float xHigh = gridData.xLimitHigh;
        float xLow = gridData.xLimitLow;
        float yHigh = gridData.yLimitHigh;
        float yLow = gridData.yLimitLow;

        Debug.Log("xHigh: " + xHigh);
        Debug.Log("xLow: " + xLow);
        Debug.Log("yHigh: " + yHigh);
        Debug.Log("yLow: " + yLow);
        //Center of the grid from 4 corners
        float xCenter = (xHigh + xLow) / 2;
        float yCenter = (yHigh + yLow) / 2;

        //Set camera to center of the grid
        maincamera.transform.position = new Vector3(xCenter + xOffSet, yCenter + yOffSet, -10);
    }

    //Get screen size and set camera size



    void SetCameraSize()
    {
        int width = Screen.width;
        int height = Screen.height;
        //Adjust camera offset base on screen size
        if (width > 1600)
        {
            cameraSizeOffset = -4.5f;
        }
        if (width > 1000)
        {
            cameraSizeOffset = -4.00f;
        }
        else if (width > 800)
        {
            cameraSizeOffset = 0.3f;
        }
        else if (width > 600)
        {
            cameraSizeOffset = 0.5f;
        }
        else
        {
            cameraSizeOffset = 0f;
        }
        float cameraSize = (height / 2) / 100f;
        cameraSize += cameraSizeOffset;
        maincamera.orthographicSize = cameraSize;

    }
}
