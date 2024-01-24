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
        Invoke("SetCameraCenter", 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        SetCameraCenter();
        SetCameraSize();
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

    void SetCameraSize()
    {
        GridData gridData = GridData.Instance;
        float xHigh = gridData.xLimitHigh;
        float xLow = gridData.xLimitLow;
        float yHigh = gridData.yLimitHigh;
        float yLow = gridData.yLimitLow;

        float xSize = xHigh - xLow;
        float ySize = yHigh - yLow;

        float cameraSize = 0;
        if (xSize > ySize)
        {
            cameraSize = xSize / 2;
        }
        else
        {
            cameraSize = ySize / 2;
        }

        maincamera.orthographicSize = cameraSize + cameraSizeOffset;
    }
}
