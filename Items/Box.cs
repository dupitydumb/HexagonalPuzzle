using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxColor
{
    Any,
    Red,
    Blue,
    Green,
    Yellow
}

public class Box : ObstacleItems
{
    public BoxColor boxColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyBox(HexagonType hexagonType)
    {
        switch (boxColor)
        {
            case BoxColor.Any:
                DestroyObstacle();
                break;
            case BoxColor.Red:
                if (hexagonType == HexagonType.Red)
                {
                    DestroyObstacle();
                }
                break;
            case BoxColor.Blue:
                if (hexagonType == HexagonType.Blue)
                {
                    DestroyObstacle();
                }
                break;
            case BoxColor.Green:
                if (hexagonType == HexagonType.Green)
                {
                    DestroyObstacle();
                }
                break;
            case BoxColor.Yellow:
                if (hexagonType == HexagonType.Yellow)
                {
                    DestroyObstacle();
                }
                break;
            default:
                break;
        }
        
    }
}
