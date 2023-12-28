using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public GameObject LevelPrefab;

    public LevelGameData levelGameData;

    private Sprite openLevelImage;
    private Sprite closeLevelImage;
    // Start is called before the first frame update

    void Awake()
    {
        openLevelImage = Resources.Load<Sprite>("Sprites/PNG/UI/opened_level");
        closeLevelImage = Resources.Load<Sprite>("Sprites/PNG/UI/closed_level");
    }
    void Start()
    {
        int unlockedLevel = levelGameData.unlockedLevel;
        int selectedLevel = levelGameData.levelNumber;
        foreach (Levels level in levelGameData.levels)
        {
            int thisLevel = level.levelNumber;
            GameObject levelObject = Instantiate(LevelPrefab, GameObject.FindWithTag("LevelPanel").transform);
            levelObject.transform.SetParent(GameObject.FindWithTag("LevelPanel").transform);
            levelObject.GetComponentInChildren<TextMeshProUGUI>().text = level.levelNumber.ToString();
            if (level.levelNumber <= unlockedLevel)
            {
                levelObject.GetComponent<Image>().sprite = openLevelImage;
                levelObject.GetComponent<Button>().onClick.AddListener(() => LoadLevel(thisLevel));
            }
            else if (level.levelNumber > unlockedLevel)
            {
                levelObject.GetComponent<Image>().sprite = closeLevelImage;
            }
        }
        
        //foreach levelObjectives in leves in levelGameData
        foreach (Levels level in levelGameData.levels)
        {
            foreach (LevelObjectives levelObjective in level.levelObjectives)
            {
                levelObjective.isCompleted = false;
            }
        }
    }

    public void LoadLevel(int levelNumber)
    {
        levelGameData.levelNumber = levelNumber;
        SceneManager.LoadScene("Game");
    }

    
}
