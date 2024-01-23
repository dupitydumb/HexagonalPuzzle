using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideGrid : MonoBehaviour
{
    public Grid grid;
    public TMP_Text text;
    public GameObject textCanvas;

    public int xPos;
    public int yPos;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        if (GridGenerator.Instance.isGuideGrid)
        {
            textCanvas.SetActive(true);
        }
        else
        {
            textCanvas.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
