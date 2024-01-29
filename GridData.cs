using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridContainer
{
    public int x;
    public int y;
    public GameObject isOccupied;
    public GameObject gameObject;
    public GridContainer(int x, int y, GameObject gameObject)
    {
        this.x = x;
        this.y = y;
        this.gameObject = gameObject;
    }
}

public class GridData : MonoBehaviour
{
    public int xLimitLow;
    public int xLimitHigh;
    public int yLimitLow;
    public int yLimitHigh;
    public List<GridContainer> gridContainers = new List<GridContainer>();
    public static GridData Instance;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        GetLimit();
    }


    void GetLimit()
    {
        xLimitLow = 0;
        xLimitHigh = 0;
        yLimitLow = 0;
        yLimitHigh = 0;
        foreach (GridContainer gridContainer in gridContainers)
        {
            if (gridContainer.x < xLimitLow)
            {
                xLimitLow = gridContainer.x;
            }
            if (gridContainer.x > xLimitHigh)
            {
                xLimitHigh = gridContainer.x;
            }
            if (gridContainer.y < yLimitLow)
            {
                yLimitLow = gridContainer.y;
            }
            if (gridContainer.y > yLimitHigh)
            {
                yLimitHigh = gridContainer.y;
            }
        }
    }

    
}
