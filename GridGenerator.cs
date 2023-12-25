using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{


    public LevelGameData levels;
    
    public Grid grid;

    public GameObject guideGrid;
    
    [
        Header("Hexagon Blocks that will be spawned to the grid"),
        Tooltip("Hexagon Blocks that will be spawned to the grid")
    ]
    public GameObject[] hexagonBlocks;

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
        }
        
        
        
    }

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
                    GridData.Instance.gridContainers.Add(new GridContainer(x, y, box));
    
                }
            }
    
            
        }
    
        //Check if top of the grid is empty
        
    
        
        //Place randomize hexagon blocks to Grid Container
        public void initializedHexagonBlocks()
        {
            for (int i = 0; i < GridData.Instance.gridContainers.Count; i++)
            {
                int randomIndex = Random.Range(0, hexagonBlocks.Length);
                GameObject hexagonBlock = Instantiate(hexagonBlocks[randomIndex], GridData.Instance.gridContainers[i].gameObject.transform.position, Quaternion.Euler(0,0,90));
                hexagonBlock.transform.position = GridData.Instance.gridContainers[i].gameObject.transform.position;
                //Set parent to grid container
                hexagonBlock.transform.SetParent(GameObject.FindWithTag("HexagonBlockPool").transform);
                hexagonBlock.GetComponent<HexagonBlock>().x = GridData.Instance.gridContainers[i].x;
                hexagonBlock.GetComponent<HexagonBlock>().y = GridData.Instance.gridContainers[i].y;
                hexagonBlock.name = "Hex" + GridData.Instance.gridContainers[i].x + ", " + GridData.Instance.gridContainers[i].y;
                GridData.Instance.gridContainers[i].gameObject = hexagonBlock;
            }
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
                    var prefab = Instantiate(levelData.prefab, cellPos, Quaternion.Euler(0,0,90));
                    prefab.transform.position = cellCenterPos;
                    // Set the prefab to the center of the grid cell
                    // prefab.GetComponent<HexagonBlock>().x = hexagonalPostion.x;
                    // prefab.GetComponent<HexagonBlock>().y = hexagonalPostion.y;
                    prefab.GetComponent<GuideGrid>().text.text = "";
                    GridData.Instance.gridContainers.Add(new GridContainer(hexagonalPostion.x, hexagonalPostion.y, prefab));
                    prefab.name = "Behind: " + hexagonalPostion.x + ", " + hexagonalPostion.y;
                    prefab.transform.SetParent(GameObject.FindWithTag("BehidGridPool").transform);
                }    
            }
            
            
           
        }
    #endregion

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
                default:
                    break;
            }
        }

    

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
                    hexagonBlock.name = "Hex" + topGridContainers[i].x + "," + topGridContainers[i].y;
                    topGridContainers[i].gameObject = hexagonBlock;
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
