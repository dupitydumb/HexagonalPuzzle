using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialActive
{
    public int levelNumber;
    public Vector3 objectToTouch;
}



public class TutorFinger : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentLevel;
    public List<TutorialActive> tutorialActives = new List<TutorialActive>();
    RectTransform rectTransform;
    Grid grid;

    void Start()
    {
        grid = GridGenerator.Instance.grid;
        currentLevel = GridGenerator.Instance.levels.levelNumber;
        rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }

    //onmouse hover
    

    void SetPosition()
    {
        //Set position of finger from world to screen
        
        foreach (TutorialActive tutorialActive in tutorialActives)
        {
            if (tutorialActive.levelNumber == currentLevel)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(tutorialActive.objectToTouch);
                rectTransform.position = screenPos;
            }
        }
        
        

    }
    
}
