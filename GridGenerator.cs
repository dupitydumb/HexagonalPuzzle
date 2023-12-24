using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Levels[] levels;
    
    public Grid grid;

    public GameObject guideGrid;
    public GameObject[] hexagonBlocks;

    public bool isGuideGrid = false;

    public static GridGenerator Instance;

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
        PlacePrefab();
        if (isGuideGrid == true)
        {
            GenerateGuideGrid();
        }
        initializedHexagonBlocks();
        
        Time.timeScale = 1f;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            hexagonBlock.GetComponent<HexagonBlock>().x = GridData.Instance.gridContainers[i].x;
            hexagonBlock.GetComponent<HexagonBlock>().y = GridData.Instance.gridContainers[i].y;
            hexagonBlock.name = "Hex" + GridData.Instance.gridContainers[i].x + "," + GridData.Instance.gridContainers[i].y;
            GridData.Instance.gridContainers[i].gameObject = hexagonBlock;
        }
    }

    void PlacePrefab()
    {
        foreach (var level in levels)
        {
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
                    prefab.name = "Hex" + hexagonalPostion.x + "," + hexagonalPostion.y;


                }
            }
        }
    }
}
