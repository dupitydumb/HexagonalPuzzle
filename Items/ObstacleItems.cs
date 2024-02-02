using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleItems : MonoBehaviour
{
    
    
    public int xPos;
    public int yPos;

    GridData gridData;
    public Grid grid;
    public int speed;


    void Awake()
    {
        gridData = GridData.Instance;
        grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        
    }


    
    public virtual void DestroyObstacle()
    {
        int index = gridData.gridContainers.FindIndex(x => x.x == xPos && x.y == yPos);
        gridData.gridContainers[index].gameObject = GridGenerator.Instance.guideGrid;
        Destroy(gameObject);
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