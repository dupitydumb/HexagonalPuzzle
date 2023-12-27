using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum HexagonType
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple,
    Orange,
    White,
    Obstacle,
    Bomb,
    Rocket,

    Empty
}


public class HexagonBlock : MonoBehaviour
{
    [Header("Grid Data")]
    private Grid grid;
    public int x;
    public int y;
    public HexagonType hexagonType;

    [Header("Hexagon Block Data")]
    public float speed = 1f;
    public bool isEmptyBelow = false;

    [Header("Hexagon Block Status")]
    [SerializeField]
    private bool isMoving = false;
    private bool isDestroying = false;

    



    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        
        //add event listener
        GridGenerator.Instance.onGridChanges.AddListener(OnChange);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CheckBelow())
        {
            MoveDown();
        }


        CheckMatch(this);
        
        

    }


    /// <summary>
    /// Called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving && !isDestroying && !GridGenerator.Instance.isBombing)
        {

            if (matchNeighbors.Count >= 6 && matchNeighbors != null)
            {
                GridGenerator.Instance.isBombing = true;
                foreach (Vector2 neighbor in matchNeighbors)
                {
                    int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == (int)neighbor.x && element.y == (int)neighbor.y);
                    GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                    
                }
                GridGenerator.Instance.SpawnRocket(x, y);
                matchNeighbors.Clear();
                visited.Clear();
                GridGenerator.Instance.isBombing = false;
            }

            if (matchNeighbors.Count >= 4 && matchNeighbors != null)
            {
                GridGenerator.Instance.isBombing = true;
                
                foreach (Vector2 neighbor in matchNeighbors)
                {
                    int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == (int)neighbor.x && element.y == (int)neighbor.y);
                    GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                    
                }
                GridGenerator.Instance.SpawnBomb(x, y);
                matchNeighbors.Clear();
                visited.Clear();
                GridGenerator.Instance.isBombing = false;
            }

            if (matchNeighbors.Count >= 2 && matchNeighbors != null)
            {
                foreach (Vector2 neighbor in matchNeighbors)
                {
                    int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == (int)neighbor.x && element.y == (int)neighbor.y);
                    GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                    
                }
                GridGenerator.Instance.SpawnDisco(x, y, hexagonType);
                matchNeighbors.Clear();
                visited.Clear();
            }
            
            
        }
    }

    public void OnChange()
    {
        Log("On Change");
        matchNeighbors.Clear();
        visited.Clear();
    }


    private void MoveDown()
    {
        
        isMoving = true;
        Vector3Int cellPos = new Vector3Int(x, y - 1, 0);
        Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
        transform.position = Vector3.MoveTowards(transform.position, cellCenterPos, speed * Time.deltaTime);

        //Invoke GridGenerator onGridChanges event
        GridGenerator.Instance.onGridChanges.Invoke();
        
        if (transform.position == cellCenterPos)
        {
            isMoving = false;
            //Find Next position index from list
            int nextIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y - 1);
            GridData.Instance.gridContainers[nextIndex].gameObject = this.gameObject;
            //Remove previous position index from list
            int previousIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y);
            GridData.Instance.gridContainers[previousIndex].gameObject = GridGenerator.Instance.guideGrid;
            y -= 1;
            
            
        }
        
    }

    public bool CheckBelow()
    {
        
        if (isDestroying == false)
        {
            Vector3Int cellPos = new Vector3Int(x, y - 1, 0);
            // Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
            //If below is empty return true

            if (GridData.Instance == null || GridData.Instance.gridContainers == null)
            {
                return false;
            }
            if (GridData.Instance.gridContainers.Exists(element => element.x == x && element.y == y - 1 && element.gameObject.tag == "GuideGrid"))
            {
                return true;
            }
        }

        return false;    
        
    }

    public void DestroyHexagonBlock()
    {
        //Check is obstacle below
        int obstacleIndex = GridData.Instance.gridContainers.FindIndex(element => element != null && element.x == x && element.y == y - 1 && element.gameObject.tag == "BoxItems");
        if (obstacleIndex != -1)
        {
            GridData.Instance.gridContainers[obstacleIndex].gameObject.GetComponent<Box>().DestroyBox(hexagonType);
        }


        //Destroy the gameobject
        isDestroying = true;
        //Remove the gameobject from list
        int index = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y);
        LeanTween.scale(this.gameObject, new Vector3(2, 2, 0), 0.8f).setEase(LeanTweenType.easeInBack).setOnComplete(() => 
        {
            GridGenerator.Instance.AddScore(hexagonType);
            GridData.Instance.gridContainers[index].gameObject = GridGenerator.Instance.guideGrid;
            isDestroying = false;
            Destroy(this.gameObject);
            
        });
        
        
    }

    HashSet<HexagonBlock> visited = new HashSet<HexagonBlock>();
    public List<Vector2> matchNeighbors;
    public void CheckMatch(HexagonBlock currentblock)
    {   
        Vector2[] offsets = new Vector2[]
        {
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(1, -1),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(-1, 1)
        };

        foreach (Vector2 offset in offsets)
        {
            int neighborX = currentblock.x + (int)offset.x;
            int neighborY = currentblock.y + (int)offset.y;

            if (GridData.Instance.gridContainers.Exists(element => element.x == neighborX && element.y == neighborY))
            {
                int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == neighborX && element.y == neighborY);
                HexagonBlock neighborBlock = GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<HexagonBlock>();
                if (neighborBlock != null && neighborBlock.hexagonType == currentblock.hexagonType)
                {
                    if (visited.Contains(neighborBlock))
                    {
                        continue;
                    }
                    visited.Add(neighborBlock);
                    matchNeighbors.Add(new Vector2(neighborBlock.x, neighborBlock.y));
                    CheckMatch(neighborBlock);
                }
            }
        }
    }
    public string Log(string message)
    {
        Debug.Log(message);
        return message;
    }

    // Destroy the current block
    // Destroy(currentBlock.gameObject);
    // int index = GridData.Instance.gridContainers.FindIndex(element => element.x == currentBlock.x && element.y == currentBlock.y);
    // GridData.Instance.gridContainers[index].gameObject = GridGenerator.Instance.guideGrid;

    // // Destroy the neighbor block
    // Destroy(neighborBlock.gameObject);

    // // Remove the neighbor block from the grid
    // GridData.Instance.gridContainers[neighborIndex].gameObject = GridGenerator.Instance.guideGrid;

    // HashSet<GridContainer> visited = new HashSet<GridContainer>();
    // public bool CheckAdjacentSameType()
    // {
    //     // Define the offsets for the neighbors
    //     int[][] offsets = new int[][] { new int[] {-1, 0}, new int[] {0, -1}, new int[] {1, -1}, new int[] {1, 0}, new int[] {0, 1}, new int[] {-1, 1} };

    //     // Iterate over the neighbors
    //     foreach (int[] offset in offsets)
    //     {
    //         int neighborX = x + offset[0];
    //         int neighborY = y + offset[1];

    //         // Check if the neighbor is within the grid
    //         if (GridData.Instance.gridContainers.Exists(element => element.x == neighborX && element.y == neighborY))
    //         {
    //             // Check if the neighbor is of the same type
    //             int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == neighborX && element.y == neighborY);
    //             if (GridData.Instance.gridContainers[neighborIndex] != null && GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<HexagonBlock>().hexagonType == hexagonType)
    //             {
    //                 if (visited.Contains(GridData.Instance.gridContainers[neighborIndex]))
    //                 {
    //                     continue;
    //                 }

    //                 visited.Add(GridData.Instance.gridContainers[neighborIndex]);
    //                 // Check the nieghbor
    //                 GameObject hexagonBlock = GridData.Instance.gridContainers[neighborIndex].gameObject;
    //                 hexagonBlock.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                    
    //                 //Also destroy the neighbor
                    
    //                 return true;

    //             }
    //             else
    //             {
    //                 Debug.Log("No same type");
    //             }
                
    //         }
    //     }

    //     // No neighbors of the same type were found
    //     return false;
    // }

    


    //Check if neighbor x and y is the same gameobject if same destroy all like tetris
    
    //TODO: Check if the block is same type
    
    



    

    

    
    
}
