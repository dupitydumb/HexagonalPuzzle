using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RocketDirection
{
    Vertical,
    Horizontal
}
public class RocketItems : BoosterItems
{

    public RocketDirection rocketDirection;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        GridGenerator.Instance.isItemsActive = true;
        base.GetLimits();
        
        
        anim = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        Debug.Log("Mouse Over");
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Clicked");
            ActivateRocket();
        }
    }

    void ActivateRocket()
    {
        CheckNeighbours();
        
        
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
        
        Vector2[] offsets = (rocketDirection == RocketDirection.Vertical) ? offsetsVertical : offsetsHorizontal;

        foreach (Vector2 offset in offsets)
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
            int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == pos.x && element.y == pos.y && element.gameObject.tag == "HexagonBlock" || element.x == pos.x && element.y == pos.y && element.gameObject.tag == "BoxItems");
            if (gridIndex != -1)
            {
                var element = GridData.Instance.gridContainers[gridIndex];
                
                if (element.gameObject.tag == "HexagonBlock")
                {
                    GridData.Instance.gridContainers[gridIndex].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
                }
                if (element.gameObject.tag == "BoxItems")
                {
                    GridData.Instance.gridContainers[gridIndex].gameObject.GetComponent<Box>().DestroyBox(HexagonType.Rocket);
                }
                
            }
        }
        StartCoroutine(RocketAnimation());
        
    }

    IEnumerator RocketAnimation()
    {
        //when anim is done
        anim.SetTrigger("Rocket");
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(1f);
        DestroyRocket();
        
    }

    void DestroyRocket()
    {
        GridGenerator.Instance.isItemsActive = true;
        int gridIndex = GridData.Instance.gridContainers.FindIndex(element => element.x == xPos && element.y == yPos);
        GridData.Instance.gridContainers[gridIndex].gameObject = GridGenerator.Instance.guideGrid;
        GridGenerator.Instance.isItemsActive = false;
        Destroy(gameObject);
    }
}
