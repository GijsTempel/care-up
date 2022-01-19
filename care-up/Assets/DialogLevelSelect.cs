using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogLevelSelect : MonoBehaviour
{
    public LevelButton mainBtn;
    public Button VideoLevelSelectButton;
    PlayerPrefsManager manager;

    private void OnEnable()
    {
        if (manager == null)
            manager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        VideoLevelSelectButton.interactable = false;
        SceneInfo selectedSceneInfo = manager.GetSceneInfoByName(mainBtn.sceneName);
        if (selectedSceneInfo != null)
        {
            VideoLevelSelectButton.interactable = selectedSceneInfo.hasVideoMode;
        }

    }

}
