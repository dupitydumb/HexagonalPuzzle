using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoItems : BoosterItems
{

    public HexagonType hexagonType;

    private List<int> toDestroyIndex = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        GridGenerator.Instance.isItemsActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver() {
        if(Input.GetMouseButtonDown(0))
        {
            ActivateDisco();
        }
    }

    void ActivateDisco()
    {
        GetHexagonType();
        //LeanTween Animation scale
        
        //Get all neighbours and destroy them
        
    }

    void GetHexagonType()
    {
       
        GridData.Instance.gridContainers.ForEach(element =>
        {
            if (element.gameObject.tag == "HexagonBlock" && element.gameObject.GetComponent<HexagonBlock>().hexagonType == hexagonType)
            {
                toDestroyIndex.Add(GridData.Instance.gridContainers.IndexOf(element));
            }
        });
        DestroyDisco();
    }

    void DestroyDisco()
    {
     
        foreach (int index in toDestroyIndex)
        {
            GridData.Instance.gridContainers[index].gameObject.GetComponent<HexagonBlock>().DestroyHexagonBlock();
        }
        //find this position in gridContainer
        LeanTween.scale(gameObject, new Vector3(3, 3, 0), 0.5f).setOnComplete(() => {
            GridGenerator.Instance.isItemsActive = false;
            
            Destroy(gameObject);
        });
    }
        
}    