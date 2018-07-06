using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardSceneButton : MonoBehaviour
{
    public string sceneName;
    public bool multiple;
    public List<string> sceneNames = new List<string>();
    public List<string> buttonNames = new List<string>();
    
    public void OnMainButtonClick()
    {
        LevelSelectionScene_UI manager = GameObject.FindObjectOfType<LevelSelectionScene_UI>();

        // clear variations to fill
        foreach (Transform t in manager.variations)
        {
            t.gameObject.SetActive(false);
            t.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        // fill variations if needed
        if (multiple)
        {
            for(int i = 0; i < sceneNames.Count; ++i)
            {
                manager.variations[i].gameObject.SetActive(true);
                manager.variations[i].GetChild(0).GetComponent<Text>().text = buttonNames[i];
                string variationSceneName = sceneNames[i];
                manager.variations[i].GetComponent<Button>().onClick.AddListener(
                    delegate { manager.UpdateLeaderBoard(variationSceneName); });
            }
        }

        // update table with info
        manager.UpdateLeaderBoard(sceneName);
    }
}
