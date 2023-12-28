using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GridGenerator : MonoBehaviour
{

    public LevelGameData levels;
    public Grid grid;
    public GameObject guideGrid;
    
    [Header("Hexagon Blocks that will be spawned to the grid"), Tooltip("Hexagon Blocks that will be spawned to the grid")]
    public GameObject[] hexagonBlocks;
    public GameObject bombItem;
    public GameObject rocketItem;
    public GameObject discoItem;



    [HideInInspector]
    public bool isBombing = false;
    public bool isItemsActive = false;
    public bool isSpawning = false;

    [Space(30)]
    [Header("Top Grid Where Hexagon Blocks will be spawned")]
    public List<GridContainer> topGridContainers = new List<GridContainer>();
    public Vector3[] topGridCellPos;

    [Header("Is this a guide grid?")]
    [Tooltip("If this is a guide grid, it will generate a grid with text on it")]
    public bool isGuideGrid = false;

    public static GridGenerator Instance;

    [Header("In Game Score")]
    public int RedHex;
    public int BlueHex;
    public int GreenHex;
    public int YellowHex;
    public int PurpleHex;
    public int OrangeHex;
    public int WhiteHex;
    public int ObstacleScore;

    public GameObject CompletePanel;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }    
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        CompletePanel.SetActive(false);
        if (isGuideGrid == true)
        {
            GenerateGuideGrid();
        }
        else
        {
            PlacePrefab();
            initializedHexagonBlocks();
            Time.timeScale = 1f;
            CheckTopGrid();
            AddTopGridCellPos();
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGuideGrid)
        {
            FillTopGrid();
            CheckScore();
        }
        
        
        
    }

    public UnityEvent onGridChanges;

    #region Initialized Level
        public void GenerateGuideGrid()
        {
            //Generate Hexagonal Grid
            int hexagonalX = 7;
            int hexagonalY = 15;    
        
    
            for (int x = -5; x < hexagonalX; x++)
            {
                for (int y = -2; y < hexagonalY; y++)
                {
                    //Check if the cell is empty
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
                    GameObject box = Instantiate(guideGrid, cellCenterPos, Quaternion.Euler(0,0,90));
                    box.GetComponent<GuideGrid>().text.text = x + "," + y;
                    box.name = x + "," + y;
                    box.GetComponent<GuideGrid>().textCanvas.SetActive(true);
                    GridData.Instance.gridContainers.Add(new GridContainer(x, y, box));
    
                }
            }
    
            
        }
    
        //Check if top of the grid is empty
        
    
        
        //Place randomize hexagon blocks to Grid Container
        public void initializedHexagonBlocks()
        {
            foreach (var gridContainer in GridData.Instance.gridContainers)
            {
                if (gridContainer.gameObject.tag == "GuideGrid")
                {
                    int randomIndex = Random.Range(0, hexagonBlocks.Length);
                    GameObject hexagonBlock = Instantiate(hexagonBlocks[randomIndex], gridContainer.gameObject.transform.position, Quaternion.Euler(0,0,90));
                    hexagonBlock.transform.position = gridContainer.gameObject.transform.position;
                    //Set parent to grid container
                    hexagonBlock.transform.SetParent(GameObject.FindWithTag("HexagonBlockPool").transform);
                    hexagonBlock.GetComponent<HexagonBlock>().x = gridContainer.x;
                    hexagonBlock.GetComponent<HexagonBlock>().y = gridContainer.y;
                    hexagonBlock.name = "Hex" + gridContainer.x + ", " + gridContainer.y;
                    gridContainer.gameObject = hexagonBlock;
                }
            }
            
            
            
            
            // for (int i = 0; i < GridData.Instance.gridContainers.Count; i++)
            // {
            //     int randomIndex = Random.Range(0, hexagonBlocks.Length);
            //     GameObject hexagonBlock = Instantiate(hexagonBlocks[randomIndex], GridData.Instance.gridContainers[i].gameObject.transform.position, Quaternion.Euler(0,0,90));
            //     hexagonBlock.transform.position = GridData.Instance.gridContainers[i].gameObject.transform.position;
            //     //Set parent to grid container
            //     hexagonBlock.transform.SetParent(GameObject.FindWithTag("HexagonBlockPool").transform);
            //     hexagonBlock.GetComponent<HexagonBlock>().x = GridData.Instance.gridContainers[i].x;
            //     hexagonBlock.GetComponent<HexagonBlock>().y = GridData.Instance.gridContainers[i].y;
            //     hexagonBlock.name = "Hex" + GridData.Instance.gridContainers[i].x + ", " + GridData.Instance.gridContainers[i].y;
            //     GridData.Instance.gridContainers[i].gameObject = hexagonBlock;
            // }
        }
    
        void PlacePrefab()
        {
            
            int currentLevel = levels.levelNumber;
            Levels level = levels.levels[currentLevel];
    
            foreach (var levelData in level.levels)
            {
                foreach (var hexagonalPostion in levelData.hexagonalPostions)
                {
                    // Instantiate to grid cell
                    //Get the grid cell
                    Vector3Int cellPos = new Vector3Int(hexagonalPostion.x, hexagonalPostion.y, 0);
                    //Get the center of the grid cell
                    Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
                    //Instantiate the prefab to the center of the grid cell
                    GameObject prefab = Instantiate(hexagonalPostion.hexagonPrefab, cellCenterPos, Quaternion.Euler(0,0,90));
                    prefab.transform.position = cellCenterPos;
                    if (prefab.tag == "BoxItems")
                    {
                        prefab.GetComponent<ObstacleItems>().xPos = hexagonalPostion.x;
                        prefab.GetComponent<ObstacleItems>().yPos = hexagonalPostion.y;
                        prefab.gameObject.name = "Box: " + hexagonalPostion.x + ", " + hexagonalPostion.y;
                    }
                    else if (prefab.tag == "Bomb")
                    {
                        prefab.GetComponent<BombItems>().xPos = hexagonalPostion.x;
                        prefab.GetComponent<BombItems>().yPos = hexagonalPostion.y;
                        prefab.gameObject.name = "Bomb: " + hexagonalPostion.x + ", " + hexagonalPostion.y;
                    }
                    else if (prefab.tag == "Rocket")
                    {
                        prefab.GetComponent<RocketItems>().xPos = hexagonalPostion.x;
                        prefab.GetComponent<RocketItems>().yPos = hexagonalPostion.y;
                    }
                    else if (prefab.tag == "HexagonBlock")
                    {
                        prefab.GetComponent<HexagonBlock>().x = hexagonalPostion.x;
                        prefab.GetComponent<HexagonBlock>().y = hexagonalPostion.y;
                    }
                    GridData.Instance.gridContainers.Add(new GridContainer(hexagonalPostion.x, hexagonalPostion.y, prefab));
                    prefab.name = "Behind: " + hexagonalPostion.x + ", " + hexagonalPostion.y;
                    prefab.transform.SetParent(GameObject.FindWithTag("BehidGridPool").transform);
                }    
            }
            
            
           
        }
    #endregion

    #region Score
        public void AddScore(HexagonType type)
            {
                switch (type)
                {
                    case HexagonType.Red:
                        RedHex++;
                        break;
                    case HexagonType.Blue:
                        BlueHex++;
                        break;
                    case HexagonType.Green:
                        GreenHex++;
                        break;
                    case HexagonType.Yellow:
                        YellowHex++;
                        break;
                    case HexagonType.Purple:
                        PurpleHex++;
                        break;
                    case HexagonType.Orange:
                        OrangeHex++;
                        break;
                    case HexagonType.White:
                        WhiteHex++;
                        break;
                    case HexagonType.Obstacle:
                        ObstacleScore++;
                        break;    
                    default:
                        break;
                }
            }
    
        //Check Score if it meets the objective



        public void CheckScore()
        {
            //Get the current level
            int currentLevel = levels.levelNumber;
            Levels level = levels.levels[currentLevel];
            //Get the level objectives
            List<LevelObjectives> levelObjectives = level.levelObjectives;
            //Check if the score meets the objective
            foreach (var levelObjective in levelObjectives)
            {
                switch (levelObjective.hexagonType)
                {
                    case HexagonType.Red:
                        if (RedHex >= levelObjective.count)
                        {
                            //Level Complete
                            levelObjective.isCompleted = true;
                            
                        }
                        break;
                    case HexagonType.Blue:
                        if (BlueHex >= levelObjective.count)
                        {
                            //Level Complete
                            levelObjective.isCompleted = true;
                            
                        }
                        break;
                    case HexagonType.Green:
                        if (GreenHex >= levelObjective.count)
                        {
                            //Level Complete
                            levelObjective.isCompleted = true;
                            
                        }
                        break;
                    case HexagonType.Yellow:
                        if (YellowHex >= levelObjective.count)
                        {
                            //Level Complete
                            levelObjective.isCompleted = true;
                            
                        }
                        break;
                    case HexagonType.Purple:
                        if (PurpleHex >= levelObjective.count)
                        {
                            //Level Complete
                            levelObjective.isCompleted = true;
                            
                        }
                        break;
                    case HexagonType.Orange:
                        if (OrangeHex >= levelObjective.count)
                        {
                            //Level Complete
                            levelObjective.isCompleted = true;
                            
                        }
                        break;
                    case HexagonType.White:
                        if (WhiteHex >= levelObjective.count)
                        {
                            //Level Complete
                            levelObjective.isCompleted = true;
                            
                        }
                        break;
                    case HexagonType.Obstacle:
                        if (ObstacleScore >= levelObjective.count)
                        {
                            //Level Complete
                            levelObjective.isCompleted = true;
                            
                        }    
                        break;
                    default:
                        break;
                }
            }
    
            //Check if all objectives are completed
    
            foreach (var levelObjective in levelObjectives)
            {
                if (levelObjective.isCompleted == false)
                {
                    return;
                }
            }
    
            //Level Complete
            LevelComplete();
            
        }    
    
        public void LevelComplete()
        {
            
            CompletePanel.SetActive(true);
        }
    #endregion
    #region Navigation
        public void NextLevel()
        {

            Debug.Log("Next Level");
            int currentLevel = levels.levelNumber;
            levels.levelNumber++;
            if (currentLevel >= levels.unlockedLevel)
            {
                levels.unlockedLevel = levels.levelNumber;
            }
            //Reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
            
                
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene("LevelMenu");
        }
    #endregion
    


    #region Spawn Items
        public void SpawnBomb(int x, int y)
        {
            isSpawning = true;
            isBombing = true;
            Vector3Int cellPos = new Vector3Int(x, y, 0);
            Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
            GameObject bomb = Instantiate(bombItem, cellCenterPos, Quaternion.Euler(0,0,90));
            int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y);
            GridData.Instance.gridContainers[gridIndex].gameObject = bomb;
            bomb.GetComponent<BombItems>().xPos = x;
            bomb.GetComponent<BombItems>().yPos = y;
        }
    
        public void SpawnRocket(int x, int y)
        {
            isSpawning = true;
            Vector3Int cellPos = new Vector3Int(x, y, 0);
            Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
            GameObject rocket = Instantiate(rocketItem, cellCenterPos, Quaternion.Euler(0,0,90));
            int index = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y);
            GridData.Instance.gridContainers[index].gameObject = gameObject;
            rocket.GetComponent<RocketItems>().xPos = x;
            rocket.GetComponent<RocketItems>().yPos = y;
            
        }

        public void SpawnDisco(int x, int y, HexagonType hexagonType)
        {
            isSpawning = true;
            Vector3Int cellPos = new Vector3Int(x, y, 0);
            Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
            GameObject disco = Instantiate(discoItem, cellCenterPos, Quaternion.Euler(0,0,90));
            int index = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y);
            GridData.Instance.gridContainers[index].gameObject = gameObject;
            disco.GetComponent<DiscoItems>().xPos = x;
            disco.GetComponent<DiscoItems>().yPos = y;
            disco.GetComponent<DiscoItems>().hexagonType = hexagonType;

            
        }
    #endregion
    
    

    #region Fill Top Grid
        public void FillTopGrid()
        {
            for (int i = 0; i < topGridContainers.Count; i++)
            {
                if (topGridContainers[i].gameObject.tag == "GuideGrid")
                {
                    int randomIndex = Random.Range(0, hexagonBlocks.Length);
                    GameObject hexagonBlock = Instantiate(hexagonBlocks[randomIndex], topGridCellPos[i], Quaternion.Euler(0,0,90));
                    hexagonBlock.transform.position = topGridCellPos[i];
                    hexagonBlock.GetComponent<HexagonBlock>().x = topGridContainers[i].x;
                    hexagonBlock.GetComponent<HexagonBlock>().y = topGridContainers[i].y;
                    hexagonBlock.GetComponent<HexagonBlock>().CheckMatch(hexagonBlock.GetComponent<HexagonBlock>());
                    hexagonBlock.name = "Hex" + topGridContainers[i].x + "," + topGridContainers[i].y;
                    topGridContainers[i].gameObject = hexagonBlock;
                    onGridChanges.Invoke();
                    hexagonBlock.transform.SetParent(GameObject.FindWithTag("HexagonBlockPool").transform);
                }
                
            }
        }
    
        public void CheckTopGrid()
        {
            //Get the highest y value
            int highestY = 0;
            foreach (GridContainer gridContainer in GridData.Instance.gridContainers)
            {
                if (gridContainer.y > highestY)
                {
                    highestY = gridContainer.y;
                }
            }
            //Find the grid with the highest y value in gridContainers
            foreach (GridContainer gridContainer in GridData.Instance.gridContainers)
            {
                if (gridContainer.y == highestY)
                {
                    topGridContainers.Add(gridContainer);
                    //Add top grid vector3Int to topGridCellPos
                    
                }
            }
            topGridCellPos = new Vector3[topGridContainers.Count];
            
            
        }
    
        void AddTopGridCellPos()
        {
            for (int i = 0; i < topGridContainers.Count; i++)
            {
                //Add Vector3 to topGridCellPos gameobject transform position
                topGridCellPos[i] = topGridContainers[i].gameObject.transform.position;
            }
        }
    #endregion

    
}
