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
    [SerializeField] ColorData colorData;
    public BoxColor boxColor;
    // Start is called before the first frame update
    void Start()
    {
        SetBackgroundColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetBackgroundColor()
    {
        switch (boxColor)
        {
            case BoxColor.Any:
                break;
            case BoxColor.Red:
                GetComponent<SpriteRenderer>().color = colorData.GetColor(HexagonType.Red);
                break;
            case BoxColor.Blue:
                GetComponent<SpriteRenderer>().color = colorData.GetColor(HexagonType.Blue);
                break;
            case BoxColor.Green:
                GetComponent<SpriteRenderer>().color = colorData.GetColor(HexagonType.Green);
                break;
            case BoxColor.Yellow:
                GetComponent<SpriteRenderer>().color = colorData.GetColor(HexagonType.Yellow);
                break;
            default:
                break;
        }
    }

    public void DestroyBox(HexagonType hexagonType)
    {
        switch (hexagonType)
        {
            case HexagonType.Rocket:
                DestroyObstacle();
                break;
        }
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
