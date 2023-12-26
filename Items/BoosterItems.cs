using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoosterType
{
    Bomb,
    Color,
    Rocket,
}


public class BoosterItems : MonoBehaviour
{
    [HideInInspector]
    public int xLimitLow;
    [HideInInspector]
    public int xLimitHigh;
    [HideInInspector]
    public int yLimitLow;
    [HideInInspector]
    public int yLimitHigh;

    public int xPos;   
    public int yPos;
    public BoosterType boosterType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GetLimits()
    {
        xLimitLow = GridData.Instance.xLimitLow;
        xLimitHigh = GridData.Instance.xLimitHigh;
        yLimitLow = GridData.Instance.yLimitLow;
        yLimitHigh = GridData.Instance.yLimitHigh;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
