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
        gridData = GridData.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        Vector2[] neighborPos = new Vector2[5];
        for (int i = 0; i < 5; i++)
        {
            if (xPos + offsets[i].x >= xLimitLow && xPos + offsets[i].x <= xLimitHigh && yPos + offsets[i].y >= yLimitLow && yPos + offsets[i].y <= yLimitHigh)
            {
                neighborPos[i] = new Vector2(xPos + offsets[i].x, yPos + offsets[i].y);
            }
        }

        foreach (Vector2 pos in neighborPos)
        {
            int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == pos.x && element.y == pos.y);

            if (gridData.gridContainers[gridIndex].gameObject.tag == "HexagonBlock")
            {
                gridData.gridContainers[gridIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
            }

            if (gridData.gridContainers[gridIndex].gameObject.tag == "Box")
            {
                gridData.gridContainers[gridIndex].gameObject.GetComponent<Box>().DestroyObstacle();
            }    
           
            
            
        }

        DestroyBomb();
    }


    public void DestroyBomb()
    {
        LeanTween.scale(gameObject, new Vector3(3, 3, 0), 0.5f).setOnComplete(() => {
            int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == xPos && element.y == yPos);
            gridData.gridContainers[gridIndex].gameObject = GridGenerator.Instance.guideGrid;
            GridGenerator.Instance.isBombing = false;
            Destroy(gameObject);
        });
        
    }
}