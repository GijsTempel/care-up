using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelectionManager : MonoBehaviour {

    public SceneSelection CurrentMenu;

    private Image practiceButton;
    private Image testButton;
    private Text description;

    private PlayerPrefsManager manager;

    private string practiceText =
        "Oefenen: Oefen het protocol waarin je elke stap van de werkwijze bovenin het scherm kunt zien. Volg de stappen om ze te oefenen en te leren.";
    //private string testText =
        //"Toetsen: Toets je kennis. Tijdens een toets zie je geen hints en moet je de stappen van de protocol uit je hoofd uitvoeren. Zoals in het echt.";

    public void Start()
    {
        manager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();

        ShowMenu(CurrentMenu);

        practiceButton = GameObject.Find("PracticeButton").GetComponent<Image>();
        //testButton = GameObject.Find("TestButton").GetComponent<Image>();

        //practiceButton.color = Color.green;
        manager.practiceMode = true;
        
       description = GameObject.Find("PracticeTestDescription").GetComponent<Text>();
        description.text = practiceText;
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
        practiceButton.color = Color.green;
        //testButton.color = Color.white;

        description.text = practiceText;

        manager.practiceMode = true;
    }

   /* public void OnTestButtonClick()
    {
        practiceButton.color = Color.white;
        testButton.color = Color.green;

        description.text = testText;

        manager.practiceMode = false; 
    }*/
}
