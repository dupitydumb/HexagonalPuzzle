using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoosterType
{
    Bomb,
    Color,
    Rocket,
}


public class BoosterItems : MonoBehaviour
{
    [HideInInspector]
    public int xLimitLow;
    [HideInInspector]
    public int xLimitHigh;
    [HideInInspector]
    public int yLimitLow;
    [HideInInspector]
    public int yLimitHigh;

    public int xPos;   
    public int yPos;
    public BoosterType boosterType;
 
    public Grid grid;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
    }

    public void GetLimits()
    {
        xLimitLow = GridData.Instance.xLimitLow;
        xLimitHigh = GridData.Instance.xLimitHigh;
        yLimitLow = GridData.Instance.yLimitLow;
        yLimitHigh = GridData.Instance.yLimitHigh;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (CheckBelow())
        {
            MoveDown();
        }
    }

    Vector3Int cellPos = new Vector3Int();
    bool isMoving = false;

    public void MoveDown()
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
