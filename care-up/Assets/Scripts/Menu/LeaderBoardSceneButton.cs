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
   
    public static List<LeaderBoardSceneButton> buttons = new List<LeaderBoardSceneButton>();
    public static string Descripton { get; set; }

    private void Start()
    {
        buttons.Add(this);
    }

    public void OnMainButtonClick()
    {
        // loading icon is shown
        HideElements();

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
            for (int i = 0; i < sceneNames.Count; ++i)
            {
                manager.variations[i].gameObject.SetActive(true);
                manager.variations[i].GetComponent<Text>().text = buttonNames[i];

                string variationSceneName = sceneNames[i];
                manager.variations[i].GetComponent<Button>().onClick.AddListener(
                    delegate { manager.UpdateLeaderBoard(variationSceneName); });
            }
        }

        // update table with info
        manager.UpdateLeaderBoard(sceneName);

        // clear color
        foreach (LeaderBoardSceneButton b in buttons)
        {
            b.GetComponent<Button>().interactable = true;
        }
    }
   
    public void HideElements()
    {
        GameObject.Find("ButtonClickSound").GetComponent<AudioSource>().Play();
        Descripton = transform.Find("Text").GetComponent<Text>().text;

        GameObject.FindObjectOfType<UMP_Manager>().LeaderBoardSearchBar.gameObject.SetActive(false);
        GameObject.FindObjectOfType<LeaderBoard>().topDescription.SetActive(false);
        GameObject.FindObjectOfType<LeaderBoard>().leftBar.SetActive(false);
        GameObject.FindObjectOfType<LeaderBoard>().infoBar.SetActive(false);
        GameObject.FindObjectOfType<LeaderBoard>().leaderboard.SetActive(true);
    }
}
