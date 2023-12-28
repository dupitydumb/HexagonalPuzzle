using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleItems : MonoBehaviour
{
    
    
    public int xPos;
    public int yPos;

    GridData gridData;



    void Awake()
    {
        gridData = GridData.Instance;
    }


    
    public virtual void DestroyObstacle()
    {
        int index = gridData.gridContainers.FindIndex(x => x.x == xPos && x.y == yPos);
        gridData.gridContainers[index].gameObject = GridGenerator.Instance.guideGrid;
        Destroy(gameObject);
    }
        

}