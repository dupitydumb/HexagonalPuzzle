using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridContainer
{
    public int x;
    public int y;
    public GameObject gameObject;
    public GridContainer(int x, int y, GameObject gameObject)
    {
        this.x = x;
        this.y = y;
        this.gameObject = gameObject;
    }
}

public class GridData : MonoBehaviour
{
    public List<GridContainer> gridContainers = new List<GridContainer>();

    public static GridData Instance;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

}
