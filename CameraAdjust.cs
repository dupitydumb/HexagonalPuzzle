using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    public Camera maincamera;

    public float cameraSizeOffset;
    public float xOffSet;
    public float yOffSet;
    float timer = 0.5f;
    bool isCanCenter = true;
    // Start is called before the first frame update
    void Start()
    {
        SetCameraCenter();
        SetCameraSize();
    }

    void Awake()
    {
        
        
    }
    // Update is called once per frame
    void Update()
    {

        if (isCanCenter)
        {
            SetCameraCenter();
        }
        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            isCanCenter = false;
        }
    }

    void GetScreenSize()
    {
        int width = Screen.width;
        int height = Screen.height;
        Debug.Log("Scrren Width: " + width);
        Debug.Log("Scrren Height: " + height);
    }

    void SetCameraCenter()
    {
        
        int xCenter;
        int yCenter;

        //Get median of x and y
        xCenter = (GridData.Instance.xLimitLow + GridData.Instance.xLimitHigh) / 2;
        yCenter = (GridData.Instance.yLimitLow + GridData.Instance.yLimitHigh) / 2;


        Vector3 center;
        //find the center of the grid
        GridData.Instance.gridContainers.ForEach(element =>
        {
            if (element.x == xCenter && element.y == yCenter)
            {
                center = element.gameObject.transform.position;
                maincamera.transform.position = new Vector3(center.x + xOffSet, center.y + yOffSet, -10);
            }
        });
        
    }

    //Get screen size and set camera size



    void SetCameraSize()
    {
     
        int widhtLength =  Math.Abs(GridData.Instance.xLimitLow - GridData.Instance.xLimitHigh);
        
        if (widhtLength <= 4)
        {
            cameraSizeOffset = 2f;
        }
        if (widhtLength >= 5)
        {
            cameraSizeOffset = 4f;
        }
        
        Debug.Log("Width Length: " + widhtLength);
        
        float cameraSize = maincamera.orthographicSize;
        
        maincamera.orthographicSize = cameraSize += cameraSizeOffset;

        Debug.Log("Camera Size: " + cameraSize);

    }
}