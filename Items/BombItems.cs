using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItems : BoosterItems
{

    GridData gridData;



    // Start is called before the first frame update
    void Start()
    {
        base.GetLimits();
        grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        gridData = GridData.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckBelow())
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
            ActivateBomb();
        }
    }


    void ActivateBomb()
    {
        
        GetNeighbor();

        
    }

    void GetNeighbor()
    {
        // get 5 block of neighbor
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

        Vector2[] offsets = (this.yPos % 2 == 0) ? offsetsEven : offsetsOdd;

        List<Vector2> neighborPos = new List<Vector2>();
        foreach (Vector2 offset in offsets)
        {
            Vector2 neighborCoord = new Vector2(this.xPos + offset.x, this.yPos + offset.y);
            neighborPos.Add(neighborCoord);
        }

        foreach (Vector2 pos in neighborPos)
        {
            int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == pos.x && element.y == pos.y );
            if (gridIndex != -1)
            {
                if (gridData.gridContainers[gridIndex].gameObject.tag == "HexagonBlock")
                {
                    gridData.gridContainers[gridIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                }

                if (gridData.gridContainers[gridIndex].gameObject.tag == "BoxItems")
                {
                    gridData.gridContainers[gridIndex].gameObject.GetComponent<Box>().DestroyObstacle();
                }    
            }
            
        }

        neighborPoss = neighborPos;
        DestroyBomb();
    }

    public List<Vector2> neighborPoss = new List<Vector2>();
    public void DestroyBomb()
    {
        GridGenerator.Instance.isBombing = true;
        LeanTween.scale(gameObject, new Vector3(3, 3, 0), 0.5f).setOnComplete(() => {
            int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == xPos && element.y == yPos);
            gridData.gridContainers[gridIndex].gameObject = GridGenerator.Instance.guideGrid;
            GridGenerator.Instance.isBombing = false;
            
            Destroy(gameObject);
        });
        
    }
    Vector3Int cellPos = new Vector3Int();
    bool isMoving = false;
    private void MoveDown()
    {
        isMoving = true;
        GridGenerator.Instance.isMoving = true;
        Vector3 cellCenterPos = grid.GetCellCenterWorld(cellPos);
        transform.position = Vector3.MoveTowards(transform.position, cellCenterPos, speed * Time.deltaTime);

        //Invoke GridGenerator onGridChanges event
        GridGenerator.Instance.onGridChanges.Invoke();
        
        if (transform.position == cellCenterPos)
        {
            isMoving = false;
            GridGenerator.Instance.isMoving = false;
            //Find Next position index from list
            int nextIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == cellPos.x && element.y == cellPos.y);
            GridData.Instance.gridContainers[nextIndex].gameObject = this.gameObject;
            //Remove previous position index from list
            int previousIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == xPos && element.y == yPos);
            GridData.Instance.gridContainers[previousIndex].gameObject = GridGenerator.Instance.guideGrid;
            //Set new position
            xPos = cellPos.x;
            yPos = cellPos.y;

            //Remove occupied
            GridData.Instance.gridContainers.Find(element => element.x == xPos && element.y == yPos).isOccupied = null;
            
        }
        
    }
    bool isDestroying = false;
    public bool CheckBelow()
    {

        if (isDestroying == false )
        {
            bool isChecking = false;
            //PointTop Hexagon
            cellPos = new Vector3Int(xPos - 1, yPos, 0);

            if (isChecking == false)
            {
                if (GridData.Instance == null || GridData.Instance.gridContainers == null)
                {
                    return false;
                }
                if (GridData.Instance.gridContainers.Exists(element => element.x == cellPos.x && element.y == cellPos.y && element.gameObject.tag == "GuideGrid" && element.gameObject.tag != "HexagonBlock" && element.gameObject != null))
                {
                    GridData.Instance.gridContainers.Find(element => element.x == cellPos.x && element.y == cellPos.y).isOccupied = this.gameObject;
                    return true;
                }
                if (GridData.Instance.gridContainers.Exists(element => element.x == cellPos.x && element.y == cellPos.y && element.isOccupied == this.gameObject))
                {
                    return true;
                
                }
            }
            
        }
        return false;
        
        
    }
}