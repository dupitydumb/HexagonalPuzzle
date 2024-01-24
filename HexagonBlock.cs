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
        Invoke("AfterSpawn", 0.2f);
        
    }

    void AfterSpawn()
    {
        CheckMatch(this);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CheckBelow())
        {
            MoveDown();
        }
        else
        {
            if (isMoving == false && isDestroying == false)
            {
                CheckMatch(this);
            }
        }

    }


    /// <summary>
    /// Called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving && !isDestroying && !GridGenerator.Instance.isBombing && !GridGenerator.Instance.isHasMove)
        {

            if (matchNeighbors.Count >= 11 && matchNeighbors != null)
            {
                GridGenerator.Instance.onMoves.Invoke();
                GridGenerator.Instance.SpawnDisco(x, y, hexagonType);
                GridGenerator.Instance.isBombing = true;
                foreach (Vector2 neighbor in matchNeighbors)
                {
                    int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == (int)neighbor.x && element.y == (int)neighbor.y);
                    if (GridData.Instance.gridContainers[neighborIndex].gameObject.tag == "HexagonBlock")
                    {
                        GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                    }
                    
                }
                GridGenerator.Instance.onMoves.Invoke();
                matchNeighbors.Clear();
                visited.Clear();
                GridGenerator.Instance.isBombing = false;
                Destroy(this.gameObject);
            }

            if (matchNeighbors.Count >= 9 && matchNeighbors != null)
            {
                GridGenerator.Instance.SpawnBomb(x, y);
                foreach (Vector2 neighbor in matchNeighbors)
                {
                    int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == (int)neighbor.x && element.y == (int)neighbor.y);
                    if (GridData.Instance.gridContainers[neighborIndex].gameObject.tag == "HexagonBlock")
                    {
                        GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                    }
                    
                }
                GridGenerator.Instance.onMoves.Invoke();
                matchNeighbors.Clear();
                visited.Clear();
                Destroy(this.gameObject);
            }

            if (matchNeighbors.Count >= 6 && matchNeighbors != null)
            {
                GridGenerator.Instance.SpawnRocket(x, y);
                foreach (Vector2 neighbor in matchNeighbors)
                {
                    int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == (int)neighbor.x && element.y == (int)neighbor.y);
                    if (GridData.Instance.gridContainers[neighborIndex].gameObject.tag == "HexagonBlock")
                    {
                        GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                    }
                    
                }
                GridGenerator.Instance.onMoves.Invoke();
                matchNeighbors.Clear();
                visited.Clear();
                Destroy(this.gameObject);
            }

            if (matchNeighbors.Count >= 2 && matchNeighbors != null)
            {
                foreach (Vector2 neighbor in matchNeighbors)
                {
                    int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == (int)neighbor.x && element.y == (int)neighbor.y);
                    if (GridData.Instance.gridContainers[neighborIndex].gameObject.tag == "HexagonBlock")
                    {
                        GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                    }
                    
                }
                GridGenerator.Instance.onMoves.Invoke();
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
        CheckMatch(this);
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
        CheckMatch(this);
        
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
            if (GridData.Instance.gridContainers.Exists(element => element.x == x && element.y == y - 1 && element.gameObject.tag == "GuideGrid" && element.gameObject.tag != "HexagonBlock"))
            {
                return true;
            }
        }

        return false;    
        
    }

    public void DestroyHexagonBlock()
    {
        //obstacle offset
        Vector2[] offsetsEven = new Vector2[]
        {
            new Vector2(-1, -1),
            new Vector2(0, -1)

        };
        Vector2[] offsetsOdd = new Vector2[]
        {
            new Vector2(0, -1),
            new Vector2(1, -1)
        };

        
        //Check is obstacle below
        Vector2[] offsets = (y % 2 == 0) ? offsetsEven : offsetsOdd;
        foreach (Vector2 offset in offsets)
        {
            int neighborX = x + (int)offset.x;
            int neighborY = y + (int)offset.y;
            if (GridData.Instance.gridContainers.Exists(element => element.x == neighborX && element.y == neighborY))
            {
                int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == neighborX && element.y == neighborY);
                if (GridData.Instance.gridContainers[neighborIndex].gameObject.tag == "BoxItems")
                {
                    GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<Box>().DestroyBox(hexagonType);
                }
            }
        }


        //Destroy the gameobject
        isDestroying = true;
        //Remove the gameobject from list
        int index = GridData.Instance.gridContainers.FindIndex(element => element.x == x && element.y == y);
        LeanTween.scale(this.gameObject.transform.GetChild(0).gameObject, new Vector3(1, 1, 0), 0.5f).setEase(LeanTweenType.easeShake).setOnComplete(() => 
        {
            GridGenerator.Instance.AddScore(hexagonType);
            GridData.Instance.gridContainers[index].gameObject = GridGenerator.Instance.guideGrid;
            isDestroying = false;
            GridGenerator.Instance.onGridChanges.Invoke();
            Destroy(this.gameObject);
            
        });
        
        
    }

    HashSet<HexagonBlock> visited = new HashSet<HexagonBlock>();
    public List<Vector2> matchNeighbors;

    public void CheckMatch(HexagonBlock currentblock)
    {   
        
        Vector2[] offsetsEven = new Vector2[]
        {
            new Vector2(-1, 0),
            new Vector2(-1,-1),
            new Vector2(0, -1),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(-1, 1)
        };

        Vector2[] offsetsOdd = new Vector2[]
        {
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(1, -1),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

        Vector2[] offsets = (currentblock.y % 2 == 0) ? offsetsEven : offsetsOdd;

        
        foreach (Vector2 offset in offsets)
        {
            int neighborX = currentblock.x + (int)offset.x;
            int neighborY = currentblock.y + (int)offset.y;
            
            if (GridData.Instance.gridContainers.Exists(element => element.x == neighborX && element.y == neighborY ))
            {
                int neighborIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == neighborX && element.y == neighborY);
                HexagonBlock neighborBlock = GridData.Instance.gridContainers[neighborIndex].gameObject.GetComponent<HexagonBlock>();
                if (neighborBlock != null && neighborBlock.hexagonType == currentblock.hexagonType)
                {
                    if (visited.Contains(neighborBlock))
                    {
                        continue;
                    }
                    matchNeighbors.Add(new Vector2(neighborBlock.x, neighborBlock.y));
                    visited.Add(neighborBlock);
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
    
}
