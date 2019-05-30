using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelectionManager : MonoBehaviour {

    public SceneSelection CurrentMenu;

    public Image practiceButton;
    public Image testButton;

    private PlayerPrefsManager manager;

    public LevelButton startButton;
    
    //private string practiceText = "Kies je voor oefenen, dan zie je bovenin het scherm elke stap van de werkwijze. ";
    //private string testText = "Kies je voor toetsen, dan zie je geen hints en moet je de stappen van de handelingen uit je hoofd uitvoeren. Let op de volgorde van de acties!";
    
    public void Start()
    {
        if (GameObject.Find("Preferences") != null)
        {
            manager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
            if (manager == null) Debug.LogWarning("No prefs manager ( start from 1st scene? )");
        }
        else
        {
            Debug.LogWarning("No prefs manager ( start from 1st scene? )");
        }

        //ShowMenu(CurrentMenu);

        //practiceButton = GameObject.Find("PracticeButton").GetComponent<Image>();
        //testButton = GameObject.Find("TestButton").GetComponent<Image>();

        // dont need to make one green anymore, player need to actually select one of them
        //practiceButton.color = Color.green;
        if (manager)
        {
            manager.practiceMode = true;
        }

        //description = GameObject.Find("PracticeTestDescription").GetComponent<Text>();
        //description.text = practiceText;

        // we need start button to exist but hide it
        startButton.gameObject.SetActive(false);
    }

    public void ShowMenu (SceneSelection sceneselection)
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.IsOpen = false;

            CurrentMenu = sceneselection;
            CurrentMenu.IsOpen = true;
        }
    }

    public void OnPracticeButtonClick() 
    {
        //practiceButton.color = Color.green;
        //testButton.color = Color.white;

        

        if (manager)
        {
            manager.practiceMode = true;
        }

        // imitate pressing start
        if (startButton != null)
        {
            startButton.OnStartButtonClick();
        }

        // imitate closing dialogue
        GameObject.Find("DialogTestPractice").SetActive(false);
    }

    public void OnTestButtonClick() 
    {
        practiceButton.color = Color.white;
        testButton.color = Color.green;

       

        if (manager)
        {
            manager.practiceMode = false;
        }

        // imitate pressing start
        if (startButton != null)
        {
            startButton.OnStartButtonClick();
        }

        // imitate closing dialogue
        GameObject.Find("DialogTestPractice").SetActive(false);
    }
}
