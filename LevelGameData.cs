using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelGameData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelGameData : ScriptableObject
{
    public int unlockedLevel;
    public int levelNumber;
    public Levels[] levels;
}
