using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Microsoft.Unity.VisualStudio.Editor;

[System.Serializable]
public class TutorialActive
{
    public int levelNumber;
    public Vector3 objectToTouch;

    public Sprite image;
    public string textDescription;
    public string bottomTextDescription;
}



public class TutorFinger : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentLevel;
    public LevelGameData gameLevelData;
    public List<TutorialActive> tutorialActives = new List<TutorialActive>();
    [SerializeField] private Transform rectTransform;
    [SerializeField] private UnityEngine.UI.Image image;
    [SerializeField] private TMP_Text textDescription;
    [SerializeField] private TMP_Text bottomTextDescription;

    [SerializeField] private GameObject ImageWindow;
    Grid grid;

    

    private void Start()
    {
        grid = GridGenerator.Instance.grid;
        currentLevel = gameLevelData.levelNumber;
        gameObject.SetActive(false);
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
                
                gameObject.SetActive(true);
                rectTransform.localPosition = tutorialActive.objectToTouch;
                textDescription.text = tutorialActive.textDescription;
                bottomTextDescription.text = tutorialActive.bottomTextDescription;
                Debug.Log("Set position" + tutorialActive.objectToTouch);

                if (tutorialActive.image != null)
                {
                    image.sprite = tutorialActive.image;
                }
                else
                {
                    ImageWindow.SetActive(false);
                }
                
            }
            
        }

        
        

    }
    
}
