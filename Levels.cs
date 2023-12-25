using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "ScriptableObjects/Levels", order = 1)]
public class Levels : ScriptableObject
{
    public int levelNumber;

    [Header("Level Objectives")]
    [Space(10)]
    public List<LevelObjectives> levelObjectives = new List<LevelObjectives>();

    [Header("Level Data")]
    [Space(10)]
    [SerializeField]
    public List<LevelData> levels = new List<LevelData>();
    
    
}



[System.Serializable]
public class LevelData
{
    public GameObject prefab;
    public List<HexagonalPostion> hexagonalPostions = new List<HexagonalPostion>();

    //Objectives

}

[System.Serializable]
public class LevelObjectives
{
    public HexagonType hexagonType;
    public int count;

    public bool isCompleted;
}




[System.Serializable]
public class HexagonalPostion
{
    public int x;
    public int y;

    public HexagonalPostion(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}    

