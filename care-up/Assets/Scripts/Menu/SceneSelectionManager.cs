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
        "This is practice mode. You will be able to see every step, that you need to do, at the top of the screen.";
    private string testText =
        "This is test mode. You will not see any hints, so you can test your knowsledge.";

    public void Start()
    {
        ShowMenu(CurrentMenu);

        practiceButton = GameObject.Find("PracticeButton").GetComponent<Image>();
        testButton = GameObject.Find("TestButton").GetComponent<Image>();

        practiceButton.color = Color.green;

        manager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();

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
        testButton.color = Color.white;

        description.text = practiceText;

        manager.practiceMode = true;
    }

    public void OnTestButtonClick()
    {
        practiceButton.color = Color.white;
        testButton.color = Color.green;

        description.text = testText;

        manager.practiceMode = false; 
    }

}
