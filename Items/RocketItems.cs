using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketItems : BoosterItems
{
    // Start is called before the first frame update
    void Start()
    {
        GridGenerator.Instance.isItemsActive = true;
        base.GetLimits();
        Invoke("CheckNeighbours", 3f);
        

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckNeighbours()
    {
        // Get All Vertical Neighbours and horizontal neighbours
        Vector2[] offsetsVertical = new Vector2[2]
        {
            new Vector2(0, 1),
            new Vector2(0, -1)
        };
        Vector2[] offsetsHorizontal = new Vector2[2]
        {
            new Vector2(1, 0),
            new Vector2(-1, 0)
        };

        //Get all neighbors until it reaches the edge of the grid
        List<Vector2> neighborPos = new List<Vector2>();
        foreach (Vector2 offset in offsetsVertical)
        {
            int x = xPos;
            int y = yPos;
            while (x >= xLimitLow && x <= xLimitHigh && y >= yLimitLow && y <= yLimitHigh)
            {
                neighborPos.Add(new Vector2(x, y));
                x += (int)offset.x;
                y += (int)offset.y;
            }
        }
        foreach (Vector2 offset in offsetsHorizontal)
        {
            int x = xPos;
            int y = yPos;
            while (x >= xLimitLow && x <= xLimitHigh && y >= yLimitLow && y <= yLimitHigh)
            {
                neighborPos.Add(new Vector2(x, y));
                x += (int)offset.x;
                y += (int)offset.y;
            }
        }

        foreach (Vector2 pos in neighborPos)
        {
            int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == pos.x && element.y == pos.y && element.gameObject.tag == "HexagonBlock");
            if (gridIndex != -1)
            {
                GridData.Instance.gridContainers[gridIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
            }
        }
        DestroyRocket();

    }

    void DestroyRocket()
    {
        GridGenerator.Instance.isItemsActive = true;
        //LeanTween Animation move horizontally
        LeanTween.moveX(gameObject, 0, 0.5f).setOnComplete(() => {
            //LeanTween Animation move vertically
            LeanTween.moveY(gameObject, 0, 0.5f).setOnComplete(() => {
                //LeanTween Animation scale
                LeanTween.scale(gameObject, new Vector3(3, 3, 0), 0.5f).setOnComplete(() => {
                    //Destroy the rocket
                    Destroy(gameObject);
                });
            });
        });


        int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == xPos && element.y == yPos);
        GridData.Instance.gridContainers[gridIndex].gameObject = GridGenerator.Instance.guideGrid;
        GridGenerator.Instance.isItemsActive = false;
        Destroy(gameObject);
    }
}
