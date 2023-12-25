using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItems : MonoBehaviour
{

    GridData gridData;


    public int xPos;   
    public int yPos;
    // Start is called before the first frame update
    void Start()
    {
        gridData = GridData.Instance;
        Invoke("GetNeighbor", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Explode()
    {

    }

    void GetNeighbor()
    {
        // get 5 block of neighbor
        Vector2[] offsets = new Vector2[5]
        {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(0, -1),
            new Vector2(1, 0),
            new Vector2(-1, 0)
        };

        int xLimitLow = GridData.Instance.xLimitLow;
        int xLimitHigh = GridData.Instance.xLimitHigh;
        int yLimitLow = GridData.Instance.yLimitLow;
        int yLimitHigh = GridData.Instance.yLimitHigh;

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
            int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == pos.x && element.y == pos.y && element.gameObject.tag == "HexagonBlock");
            if (gridIndex != -1)
            {
                GridData.Instance.gridContainers[gridIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
            }
            else
            {
                DestroyBomb();
            }
            
            
        }

        DestroyBomb();
    }


    public void DestroyBomb()
    {
        LeanTween.scale(gameObject, new Vector3(3, 3, 0), 0.5f).setOnComplete(() => {
            int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == xPos && element.y == yPos);
            gridData.gridContainers[gridIndex].gameObject = GridGenerator.Instance.guideGrid;
            Destroy(gameObject);
        });
        
    }
}