using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ColorData", menuName = "ColorData", order = 0)]
public class ColorData : ScriptableObject
    
{
    public List<Data> data = new List<Data>();

    //Color Data Need to add form list
    public Color32 GetColor(HexagonType hexagonType)
    {
        foreach (var item in data)
        {
            if (item.hexagonType == hexagonType)
            {
                return item.color;
            }
        }
        return Color.white;
    }
}

[System.Serializable]
public class Data
{
    //Hexagonal type enum
    public HexagonType hexagonType;
    public Color32 color;
}
