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


        Vector2[] neighborPos = new Vector2[5];
        for (int i = 0; i < 5; i++)
        {
            neighborPos[i] = new Vector2(transform.position.x + offsets[i].x, transform.position.y + offsets[i].y);
            //Find the block in the grid
            foreach (GridContainer gridContainer in gridData.gridContainers)
            {
                if (gridContainer.x == neighborPos[i].x && gridContainer.y == neighborPos[i].y)
                {
                    //Destroy the block
                    int gridIndex = gridData.gridContainers.FindIndex(x => x.x == gridContainer.x && x.y == gridContainer.y);
                    gridData.gridContainers[gridIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                }
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