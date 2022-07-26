using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAutomationData : MonoBehaviour
{
    int currentSGPage = -1;
    int currentSceneGroup = -1;
    int currentLevelButtonID = -1;
    public bool toAutomate = false;
    public void SetCurrentLevelButtonID(int value)
    {
        currentLevelButtonID = value;
    }
    public int GetCurrentLevelButtonID()
    {
        if (!toAutomate)
            return -1;
        return currentLevelButtonID;
    }
    public void SetCurrentSceneGroup(int value)
    {
        currentSceneGroup = value;
    }
    public int GetCurrentSceneGroup()
    {
        if (!toAutomate)
            return -1;
        return currentSceneGroup;
    }
    public void SetCurrentSGPage(int value)
    {
        currentSGPage = value;
    }
    public int GetCurrentSGPage()
    {
        if (!toAutomate)
            return -1;
        return currentSGPage;
    }
}
