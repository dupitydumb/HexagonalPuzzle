using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ObjectiveUI : MonoBehaviour
{
    public LevelGameData levels;

    private List<LevelObjectives> levelObjectives = new List<LevelObjectives>();
    public GameObject prefabUI;

    private List<GameObject> objectiveUI = new List<GameObject>();

    public TMP_Text levelNumberText;

    // Start is called before the first frame update
    void Start()
    {
        GetLevelData();
        SetObjectiveUI();
        levelNumberText.text = levels.levelNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void GetLevelData()
    {
        
        int currentLevel = levels.levelNumber;
        levelObjectives = levels.levels[currentLevel].levelObjectives;
    }

    void SetObjectiveUI()
    {
        for (int i = 0; i < levelObjectives.Count; i++)
        {
            GameObject go = Instantiate(prefabUI, GameObject.FindWithTag("BottomUI").transform);
            go.transform.GetChild(1).GetComponent<TMP_Text>().text = levelObjectives[i].count.ToString();
            Image img = go.transform.GetChild(0).GetComponent<Image>();
            HexagonType hexagonType = levelObjectives[i].hexagonType;
            objectiveUI.Add(go);
            
            switch (hexagonType)
            {
                case HexagonType.Red:
                    //load red hexagon sprite
                    img.sprite = Resources.Load<Sprite>("Sprites/PNG/Ball/Red_Ball");
                    go.GetComponent<ObjectiveCount>().hexagonType = HexagonType.Red;
                    go.GetComponent<ObjectiveCount>().objectiveCount = levelObjectives[i].count;
                    break;
                case HexagonType.Blue:
                    Debug.Log("Blue");
                    //load blue hexagon sprite
                    img.sprite = Resources.Load<Sprite>("Sprites/PNG/Ball/Blue_Ball");
                    go.GetComponent<ObjectiveCount>().hexagonType = HexagonType.Blue;
                    go.GetComponent<ObjectiveCount>().objectiveCount = levelObjectives[i].count;
                    break;
                case HexagonType.Green:
                    //load green hexagon sprite
                    img.sprite = Resources.Load<Sprite>("Sprites/PNG/Ball/Green_Ball");
                    go.GetComponent<ObjectiveCount>().hexagonType = HexagonType.Green;
                    go.GetComponent<ObjectiveCount>().objectiveCount = levelObjectives[i].count;
                    break;
                case HexagonType.Yellow:
                    //load yellow hexagon sprite
                    img.sprite = Resources.Load<Sprite>("Sprites/PNG/Ball/Yellow_Ball");
                    go.GetComponent<ObjectiveCount>().hexagonType = HexagonType.Yellow;
                    go.GetComponent<ObjectiveCount>().objectiveCount = levelObjectives[i].count;
                    break;
                case HexagonType.Purple:
                    //load purple hexagon sprite
                    img.sprite = Resources.Load<Sprite>("Sprites/PNG/Ball/Purple_Ball");
                    go.GetComponent<ObjectiveCount>().hexagonType = HexagonType.Purple;
                    go.GetComponent<ObjectiveCount>().objectiveCount = levelObjectives[i].count;
                    break;
                case HexagonType.Orange:
                    //load orange hexagon sprite
                    img.sprite = Resources.Load<Sprite>("Sprites/PNG/Ball/Orange_Ball");
                    go.GetComponent<ObjectiveCount>().hexagonType = HexagonType.Orange;
                    go.GetComponent<ObjectiveCount>().objectiveCount = levelObjectives[i].count;
                    break;
                case HexagonType.White:
                    //load white hexagon sprite
                    img.sprite = Resources.Load<Sprite>("Sprites/PNG/Ball/White_Ball");
                    go.GetComponent<ObjectiveCount>().hexagonType = HexagonType.White;
                    go.GetComponent<ObjectiveCount>().objectiveCount = levelObjectives[i].count;
                    break;        


                default:
                    break;
            }
            

        }
    }

    public void UpdateObjectiveUI()
    {
        //update objective UI text from GridGenerator
        foreach (GameObject go in objectiveUI)
        {
            
        }
    }
}
