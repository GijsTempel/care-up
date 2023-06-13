using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAutomation : MonoBehaviour
{
    const float STEP_TIMEOUT_VALUE = 0.2f;
    float stepTimeout = 0.1f;
    int currentStep = 0;
    MainMenuAutomationData mainMenuAutomationData;
    public List<SceneGroupPageButton> sceneGroupPageButtons = new List<SceneGroupPageButton>();
    public List<SceneGroupButton> sceneGroupButtons = new List<SceneGroupButton>();
    public GameObject sceneCover;
    public List<LevelButton> levelButtons = new List<LevelButton>();

    void SwitchToCurrentSGPage()
    {
        int currentSGPage = mainMenuAutomationData.GetCurrentSGPage();
        int sceneGroupPageButtonsVisible = 0;
        for (int i = 0; i < sceneGroupPageButtons.Count; i++)
        {
            if (sceneGroupPageButtons[0].gameObject.activeSelf)
                sceneGroupPageButtonsVisible += 1;
        }
        if (currentSGPage >= 0 && currentSGPage < sceneGroupPageButtonsVisible)
        {
            sceneGroupPageButtons[currentSGPage].ButtonClicked();
        }
        else
            currentStep = 99;
    }
    void SelectCurrentSceneGroup()
    {
        int currentSceneGroup = mainMenuAutomationData.GetCurrentSceneGroup();
        if (currentSceneGroup >= 0)
            sceneGroupButtons[currentSceneGroup].ButtonClicked();
        else
            currentStep = 999;
    }

    void SelectCurrentScene()
    {
        int currentSceneButtonID = mainMenuAutomationData.GetCurrentLevelButtonID();
        if (currentSceneButtonID >= 0)
            levelButtons[currentSceneButtonID].OnLevelButtonClick();
        else
            currentStep = 99;
    }
    // Start is called before the first frame update
    void Start()
    {
        mainMenuAutomationData = GameObject.FindObjectOfType<MainMenuAutomationData>();
        int _currentSGPage = mainMenuAutomationData.GetCurrentSGPage();
        sceneCover.SetActive(mainMenuAutomationData.GetCurrentSGPage() != -1);
        if (mainMenuAutomationData.GetCurrentSGPage() == -1)
            currentStep = -1;
        Debug.Log(mainMenuAutomationData.GetCurrentSGPage());
    }

    void AutomationStep()
    {
        switch (currentStep)
        {
            
            case 0:
                SwitchToCurrentSGPage();
                break;
            case 1:
                SelectCurrentSceneGroup();
                break;
            case 2:
                SelectCurrentScene();
                break;
            default:
                sceneCover.SetActive(false);
                break;
        }
        currentStep++;
        if (currentStep > 4)
        {
            sceneCover.SetActive(false);
            enabled = false;
        }
        stepTimeout = STEP_TIMEOUT_VALUE;
    }
    private void Update()
    {
        if (currentStep >= 0)
        {
            if (stepTimeout > 0)
            {
                stepTimeout -= Time.deltaTime;
                if (stepTimeout < 0)
                {
                    AutomationStep();
                }
            }
        }
    }
}
