using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GridGenerator : MonoBehaviour
{

    public LevelGameData levels;
    public Grid grid;
    public GameObject guideGrid;
    public ColorData colorData;

    public MoveTo moveDirection;
    public bool isMoving = false;
    
    [Header("Hexagon Blocks that will be spawned to the grid"), Tooltip("Hexagon Blocks that will be spawned to the grid")]
    public GameObject[] hexagonBlocks;
    public GameObject bombItem;
    public GameObject rocketItem;
    public GameObject discoItem;



    [HideInInspector]
    public bool isBombing = false;
    public bool isItemsActive = false;
    public bool isSpawning = false;

    public bool isHasMove = false; 

    [Space(30)]
    [Header("Top Grid Where Hexagon Blocks will be spawned")]
    public List<GridContainer> topGridContainers = new List<GridContainer>();
    public Vector3[] topGridCellPos;

    [Header("Is this a guide grid?")]
    [Tooltip("If this is a guide grid, it will generate a grid with text on it")]
    public bool isGuideGrid = true;

    public static GridGenerator Instance;

    [Header("Player Moves")]
    public int moves;
    public TMP_Text movesText;

    [Header("In Game Score")]
    public int RedHex;
    public int BlueHex;
    public int GreenHex;
    public int YellowHex;
    public int PurpleHex;
    public int OrangeHex;
    public int WhiteHex;
    public int ObstacleScore;

    [Header("Grid Width and Height")]
    public int gridWidth;
    public int gridHeight;

    public GameObject CompletePanel;
    public GameObject GameOverPanel;
    public UnityEvent onMoves = new UnityEvent();

    public UnityEvent onMoveChange = new UnityEvent();
    
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

    //Function to change move Direction every second
    public void ChangeMoveDirection()
    {
        
    }

    //Check is there any moving hexagon block
    public void CheckMovingHexagonBlock()
    {
        foreach (var gridContainer in GridData.Instance.gridContainers)
        {
            if (gridContainer.gameObject.tag == "HexagonBlock")
            {
                if (gridContainer.gameObject.GetComponent<HexagonBlock>().isMoving)
                {
                    isMoving = true;
                    return;
                }
            }
        }
        isMoving = false;
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

            //Set Moves
            moves = levels.levels[levels.levelNumber].moves;
            movesText.text = moves.ToString();
            onMoves.AddListener(DecreaseMoves);
            
        }
        InvokeRepeating("MoveChange", 0, 0.03f);
        InvokeRepeating("ChangeMoveDirection", 0, 1f);
        
    }

    void MoveChange()
    {
        onMoveChange.Invoke();
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
                    GameObject box = Instantiate(guideGrid, cellCenterPos, Quaternion.Euler(0,0,0));
                    box.GetComponent<GuideGrid>().text.text = x + "," + y;
                    box.name = x + "," + y;
                    box.GetComponent<GuideGrid>().textCanvas.SetActive(true);
                    GridData.Instance.gridContainers.Add(new GridContainer(x, y, box));
    
                }
            }
    
            
        }
    
        
        
    
        
        //Place randomize hexagon blocks to Grid Container
        public void initializedHexagonBlocks()
        {
            foreach (var gridContainer in GridData.Instance.gridContainers)
            {
                if (gridContainer.gameObject.tag == "GuideGrid")
                {
                    int randomIndex = Random.Range(0, hexagonBlocks.Length);
                    GameObject hexagonBlock = Instantiate(hexagonBlocks[randomIndex], gridContainer.gameObject.transform.position, Quaternion.Euler(0,0,0));
                    hexagonBlock.transform.position = gridContainer.gameObject.transform.position;
                    //Set parent to grid container
                    hexagonBlock.transform.SetParent(GameObject.FindWithTag("HexagonBlockPool").transform);
                    hexagonBlock.GetComponent<HexagonBlock>().x = gridContainer.x;
                    hexagonBlock.GetComponent<HexagonBlock>().y = gridContainer.y;
                    hexagonBlock.name = "Hex" + gridContainer.x + ", " + gridContainer.y;
                    gridContainer.gameObject = hexagonBlock;
                    
                    //Set the color of the hexagon block
                    foreach (var data in colorData.data)
                    {
                        if (hexagonBlock.GetComponent<HexagonBlock>().hexagonType == data.hexagonType)
                        {
                            hexagonBlock.GetComponent<SpriteRenderer>().color = data.color;
                        }
                    }
                    
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
                    GameObject prefab = Instantiate(hexagonalPostion.hexagonPrefab, cellCenterPos, Quaternion.Euler(0,0,0));
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
                    else if (prefab.tag == "GuideGrid")
                    {
                        prefab.GetComponent<GuideGrid>().xPos = hexagonalPostion.x;
                        prefab.GetComponent<GuideGrid>().yPos = hexagonalPostion.y;
                    }

                    if (prefab.tag != "GuideGrid")
                    {
                        GameObject guideGrid = Instantiate(this.guideGrid, cellCenterPos, Quaternion.Euler(0,0,0));
                        guideGrid.GetComponent<GuideGrid>().xPos = hexagonalPostion.x;
                        guideGrid.GetComponent<GuideGrid>().yPos = hexagonalPostion.y;
                        guideGrid.name = "Guide: " + hexagonalPostion.x + ", " + hexagonalPostion.y;
                        guideGrid.transform.SetParent(GameObject.FindWithTag("BehidGridPool").transform);
                        guideGrid.tag = "Untagged";
                    }
                    GridData.Instance.gridContainers.Add(new GridContainer(hexagonalPostion.x, hexagonalPostion.y, prefab));
                    prefab.name = "Behind: " + hexagonalPostion.x + ", " + hexagonalPostion.y;
                    prefab.transform.SetParent(GameObject.FindWithTag("BehidGridPool").transform);
                    
                    //if prefab is not guide grid also place guide grid
                    
                }    
            }
            
            
           
        }


        //Search empty grid container and spawn hexagon block
        
    #endregion


    #region Moves
        public void DecreaseMoves()
        {
            moves--;
            movesText.text = moves.ToString();
            if (moves <= 0)
            {
                isHasMove = false;
                //Game Over
                Debug.Log("Game Over");
                GameOverPanel.SetActive(true);
                Time.timeScale = 0f;
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
        public void RestartLevel()
        {
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
            Vector3Int cellPos = new Vector3Int(x, y, 0);
            Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
            GameObject bomb = Instantiate(bombItem, cellCenterPos, Quaternion.Euler(0,0,0));
            int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y);
            GridData.Instance.gridContainers[gridIndex].gameObject = bomb;
            bomb.GetComponent<BombItems>().xPos = x;
            bomb.GetComponent<BombItems>().yPos = y;
            
        }
    
        public void SpawnRocket(int x, int y)
        {
            int randomType = Random.Range(0, 2);
            isSpawning = true;
            Vector3Int cellPos = new Vector3Int(x, y, 0);
            Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
            GameObject rocket = Instantiate(rocketItem, cellCenterPos, Quaternion.Euler(0,0,0));
            int index = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y);
            GridData.Instance.gridContainers[index].gameObject = gameObject;
            rocket.GetComponent<RocketItems>().xPos = x;
            rocket.GetComponent<RocketItems>().yPos = y;
            Debug.Log("RandomType = " + randomType);
            rocket.GetComponent<RocketItems>().rocketDirection = (RocketDirection)randomType;
            //Set Rotation
            if (randomType == 0)
            {
                rocket.transform.GetChild(0).transform.rotation = Quaternion.Euler(0,0,90);
            }
            else
            {
                rocket.transform.GetChild(0).transform.rotation = Quaternion.Euler(0,0,0);
            }
            
        }

        public void SpawnDisco(int x, int y, HexagonType hexagonType)
        {
            isSpawning = true;
            Vector3Int cellPos = new Vector3Int(x, y, 0);
            Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
            GameObject disco = Instantiate(discoItem, cellCenterPos, Quaternion.Euler(0,0,0));
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
                    GameObject hexagonBlock = Instantiate(hexagonBlocks[randomIndex], topGridCellPos[i], Quaternion.Euler(0,0,0));
                    //Spawn animation
                    hexagonBlock.transform.localScale = new Vector3(0,0,0);
                    LeanTween.scale(hexagonBlock, new Vector3(1f ,1f ,1), 0.1f).setEase(LeanTweenType.easeInBack);
                    hexagonBlock.transform.position = topGridCellPos[i];
                    hexagonBlock.GetComponent<HexagonBlock>().x = topGridContainers[i].x;
                    hexagonBlock.GetComponent<HexagonBlock>().y = topGridContainers[i].y;
                    hexagonBlock.GetComponent<HexagonBlock>().CheckMatch(hexagonBlock.GetComponent<HexagonBlock>());
                    hexagonBlock.name = "Hex" + topGridContainers[i].x + "," + topGridContainers[i].y;
                    topGridContainers[i].gameObject = hexagonBlock;
                    onGridChanges.Invoke();
                    hexagonBlock.transform.SetParent(GameObject.FindWithTag("HexagonBlockPool").transform);
                    foreach (var data in colorData.data)
                    {
                        if (hexagonBlock.GetComponent<HexagonBlock>().hexagonType == data.hexagonType)
                        {
                            hexagonBlock.GetComponent<SpriteRenderer>().color = data.color;
                        }
                    }
                }
                
            }
        }
    
        public void CheckTopGrid()
        {
            //Get the highest y value
            int highestX = 0;
            foreach (GridContainer gridContainer in GridData.Instance.gridContainers)
            {
                if (gridContainer.y > highestX)
                {
                    highestX = gridContainer.x;
                }

            }
            //Find the grid with the highest y value in gridContainers
            foreach (GridContainer gridContainer in GridData.Instance.gridContainers)
            {
                if (gridContainer.x == highestX)
                {
                    topGridContainers.Add(gridContainer);
                    //Add top grid vector3Int to topGridCellPos
                    
                }
                
            }

            // Get lower right cell of top grid
            Vector2 lowerRightCellOffset = new Vector2(-1, 1);

            
            List<GridContainer> newGridContainers = new List<GridContainer>();

            foreach (var gridContainer in topGridContainers)
            {
                Vector2 lowerRightCellPos = new Vector2(gridContainer.x, gridContainer.y) + lowerRightCellOffset;
                int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == lowerRightCellPos.x && element.y == lowerRightCellPos.y);
                if (gridIndex != -1)
                {
                    newGridContainers.Add(GridData.Instance.gridContainers[gridIndex]);
                }
            }

            topGridContainers.AddRange(newGridContainers);
            topGridCellPos = new Vector3[topGridContainers.Count];
            AddTopGridCellPos();
            
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
