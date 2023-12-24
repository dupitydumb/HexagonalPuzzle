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
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CheckBelow() == true)
        {
            MoveDown();
        }
        
        

    }


    /// <summary>
    /// Called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckAdjacentSameType(this);
            
        }
    }


    private void MoveDown()
    {
        //Instantiate a new hexagon block to previous position
        isMoving = true;
        Vector3Int cellPos = new Vector3Int(x, y - 1, 0);
        Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
        transform.position = Vector3.MoveTowards(transform.position, cellCenterPos, speed * Time.deltaTime);
        if (transform.position == cellCenterPos)
        {
            
            //Find Next position index from list
            int nextIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y - 1);
            GridData.Instance.gridContainers[nextIndex].gameObject = this.gameObject;
            //Remove previous position index from list
            int previousIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y);
            GridData.Instance.gridContainers[previousIndex].gameObject = GridGenerator.Instance.guideGrid;
            y -= 1;
            

            //Check After move down
            isMoving = false;
            // if (isMoving == false)
            // {
            //     CheckAdjacentSameType(this);
            // }
        }
        
    }

    public bool CheckBelow()
    {
        
        if (isDestroying == false)
        {
            Vector3Int cellPos = new Vector3Int(x, y - 1, 0);
            Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
            if (GridData.Instance.gridContainers.Exists(element => element.x == x && element.y == y - 1 && element.gameObject.tag == "HexagonBlock"))
            {
                return false;
            }
            //If nothing below return false
            if (!GridData.Instance.gridContainers.Exists(element => element.x == x && element.y == y - 1))
            {
                return false;
            }
            //If below is empty return true
            if (GridData.Instance.gridContainers.Exists(element => element.x == x && element.y == y - 1 && element.gameObject.tag == "GuideGrid"))
            {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
        
    }

    public void DestroyHexagonBlock()
    {
        //Destroy the gameobject
        
        //Remove the gameobject from list
        int index = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y);
        LeanTween.scale(this.gameObject, new Vector3(2, 2, 0), 0.8f).setEase(LeanTweenType.easeInBack).setOnComplete(() => 
        {
            GridData.Instance.gridContainers[index].gameObject = GridGenerator.Instance.guideGrid;
            Destroy(this.gameObject);
            Log("Destroying : " + hexagonType.ToString() + " at " + x + ", " + y);
        });
        
        

        // if (CheckAdjacentSameType() == true)
        // {
        //     int index = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y);
        //     GridData.Instance.gridContainers[index].gameObject = GridGenerator.Instance.guideGrid;
        //     Destroy(this.gameObject);
            
        // }
        // else
        // {
        //     Debug.Log("No adjacent same type");
        // }
        
    }

   
    public void CheckAdjacentSameType(HexagonBlock startBlock)
    {   
        Log("Checking : " + startBlock.hexagonType.ToString() + " at " + startBlock.x + ", " + startBlock.y);   
        if (startBlock.isMoving)
        {
            return;
        }


        // Define the offsets for the neighbors
        int[][] offsets = new int[][] { new int[] {-1, 0}, new int[] {0, -1}, new int[] {1, -1}, new int[] {1, 0}, new int[] {0, 1}, new int[] {-1, 1} };

        // Create a queue for BFS and add the start block to it
        Queue<HexagonBlock> queue = new Queue<HexagonBlock>();
        queue.Enqueue(startBlock);

        // Create a set to keep track of visited blocks
        HashSet<HexagonBlock> visited = new HashSet<HexagonBlock>();
        visited.Add(startBlock);

        while (queue.Count > 0)
        {
            HexagonBlock currentBlock = queue.Dequeue();
            List<HexagonBlock> sameTypeNeighbors = new List<HexagonBlock>();

            foreach (int[] offset in offsets)
            {
                int neighborX = currentBlock.x + offset[0];
                int neighborY = currentBlock.y + offset[1];

                // Check if the neighbor is within the grid
                if (GridData.Instance.gridContainers.Exists(element => element.x == neighborX && element.y == neighborY))
                {
                    // Check if the neighbor is of the same type
                    int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == neighborX && element.y == neighborY);
                    HexagonBlock neighborBlock = GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<HexagonBlock>();
                    if (neighborBlock != null && neighborBlock.hexagonType == currentBlock.hexagonType)
                    {
                        
                        // If the neighbor has already been visited, skip it
                        if (visited.Contains(neighborBlock))
                        {
                            continue;
                        }
                        sameTypeNeighbors.Add(neighborBlock);
                        sameTypeNeighbors.Add(currentBlock);
                        Log("Same type neighbor found");
                        
                    }
                    
                }
                
            }

            // If there are more than 3 neighbors of the same type, destroy them
            if (sameTypeNeighbors.Count >= 2)
            {
                foreach (HexagonBlock neighborBlock in sameTypeNeighbors)
                {
                    
                    // Add the neighbor to the visited set and the queue
                    visited.Add(neighborBlock);
                    
                    queue.Enqueue(neighborBlock);

                    neighborBlock.gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                    // Destroy the neighbor block
                    // Remove the neighbor block from the grid
                    // int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == neighborBlock.x && element.y == neighborBlock.y);
                    // GridData.Instance.gridContainers[neighborIndex].gameObject = GridGenerator.Instance.guideGrid;
                    
                }
                // currentBlock.gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                // int index = GridData.Instance.gridContainers.FindIndex(element => element.x == currentBlock.x && element.y == currentBlock.y);
                // GridData.Instance.gridContainers[index].gameObject = GridGenerator.Instance.guideGrid;
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
