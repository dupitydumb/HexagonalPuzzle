using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveCount : MonoBehaviour
{
    public TMP_Text countText;
    public HexagonType hexagonType;
    public int objectiveCount;
    public int currentCount;

    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        CountObjective();
    }

    void CountObjective()
    {
        int RedHex = GridGenerator.Instance.RedHex;
        int BlueHex = GridGenerator.Instance.BlueHex;
        int GreenHex = GridGenerator.Instance.GreenHex;
        int YellowHex = GridGenerator.Instance.YellowHex;
        int PurpleHex = GridGenerator.Instance.PurpleHex;
        int OrangeHex = GridGenerator.Instance.OrangeHex;
        int WhiteHex = GridGenerator.Instance.WhiteHex;
        int ObstacleScore = GridGenerator.Instance.ObstacleScore;

        switch (hexagonType)
        {
            case HexagonType.Red:
                currentCount = RedHex;
                break;
            case HexagonType.Blue:
                currentCount = BlueHex;
                break;
            case HexagonType.Green:
                currentCount = GreenHex;
                break;
            case HexagonType.Yellow:
                currentCount = YellowHex;
                break;
            case HexagonType.Purple:
                currentCount = PurpleHex;
                break;
            case HexagonType.Orange:
                currentCount = OrangeHex;
                break;
            case HexagonType.White:
                currentCount = WhiteHex;
                break;
            case HexagonType.Obstacle:
                currentCount = ObstacleScore;
                break;
        }

        countText.text = currentCount.ToString() + "/" + objectiveCount.ToString();
        
    }
}
