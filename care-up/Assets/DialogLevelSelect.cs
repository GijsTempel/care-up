using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogLevelSelect : MonoBehaviour
{
    public LevelButton mainBtn;
    public Button VideoLevelSelectButton;
    public Button ButtonLevel2;
    public Button ButtonLevel3;
    public Button ButtonLevel4;
    public Button ButtonLevel5;
    public GameObject WaitPanel;
    PlayerPrefsManager manager;
    public float timeoutValue = 3f;
    private void OnEnable()
    {
        timeoutValue = 3f;
        WaitPanel.SetActive(true);
        if (manager == null)
            manager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        VideoLevelSelectButton.interactable = false;
        LockAllButtons();
    }

    void LockAllButtons()
    {
        List<Button> buttons = new List<Button> { VideoLevelSelectButton, ButtonLevel2, ButtonLevel3, ButtonLevel4, ButtonLevel5 };
        for (int i = 0; i < buttons.Count; i++ )
        {
            buttons[i].interactable = false;
        }
    }

    private void Update()
    {
        if (timeoutValue > 0)
        {
            timeoutValue -= Time.deltaTime;
            if (timeoutValue <= 0)
            {
                UnlockLevelButtons();
            }
        }
    }
    public void ResetPracticePlays()
    {
        PlayerPrefsManager.SetValueToSceneInCategory(manager.currentSceneVisualName, "PracticePlays", 0);
    }

    void UnlockLevelButtons()
    {
        WaitPanel.SetActive(false);
        Debug.Log(manager.currentPracticePlays);
        SceneInfo selectedSceneInfo = manager.GetSceneInfoByName(mainBtn.sceneName);
        if (selectedSceneInfo != null)
        {
            VideoLevelSelectButton.interactable = selectedSceneInfo.hasVideoMode;
        }
        List<Button> buttons = new List<Button> { VideoLevelSelectButton, ButtonLevel2, ButtonLevel3, ButtonLevel4, ButtonLevel5 };

        buttons[1].interactable = true;
        buttons[2].interactable = true;
           
        if (manager.currentPracticePlays >= 1)
        {
            buttons[3].interactable = true;
        }
        if (manager.currentPracticePlays >= 3)
        {
            buttons[4].interactable = true;
        }
    }
}

