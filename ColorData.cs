using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ColorData", menuName = "ColorData", order = 0)]
public class ColorData : ScriptableObject
    
{
    public List<Data> data = new List<Data>();
}

[System.Serializable]
public class Data
{
    //Hexagonal type enum
    public HexagonType hexagonType;
    public Color32 color;
}
