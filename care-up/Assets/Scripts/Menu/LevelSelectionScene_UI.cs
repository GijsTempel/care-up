using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Handles Scene selection module
/// </summary>
public class LevelSelectionScene_UI : MonoBehaviour
{
    private PlayerPrefsManager ppManager;

    /// <summary>
    /// Load xml file, set all door variables.
    /// </summary>
    void Start()
    {
        ppManager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        if (ppManager == null) Debug.LogError("No player preferences manager found");
        
        LevelButton[] doors = GameObject.Find("Scroll").GetComponentsInChildren<LevelButton>();
        foreach (LevelButton door in doors)
        {
            door.gameObject.SetActive(false);
        }

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Scenes");
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList doorNodes = xmlFile.FirstChild.NextSibling.FirstChild.ChildNodes;

        int i = 0;
        foreach (XmlNode scene in doorNodes)
        {
            if (i < doors.Length)
            {
                doors[i].gameObject.SetActive(true);
                
                if (scene.Attributes["multiple"] != null)
                {
                    // general name
                    doors[i].transform.Find("Name").gameObject.SetActive(true);
                    doors[i].transform.Find("Name").GetComponent<Text>().text
                        = scene.Attributes["name"].Value;

                    int count = 0;
                    foreach (XmlNode variation in scene.ChildNodes)
                    {
                        string sceneName = variation.Attributes["name"].Value;
                        Transform descr = doors[i].transform.Find("Option_" + ++count);
                        descr.gameObject.SetActive(true);

                        descr.Find("Text").GetComponent<Text>().text
                            = variation.Attributes["displayname"].Value;
                        descr.GetComponent<LevelSelectionScene_UI_Option>().sceneName = sceneName;
                        descr.GetComponent<LevelSelectionScene_UI_Option>().description = variation.Attributes["description"].Value;
                        if (ppManager.GetSceneCompleted(sceneName))
                        {
                            string info = ppManager.GetSceneStars(sceneName) + " stars; " +
                                ppManager.GetSceneTime(sceneName);
                            descr.GetComponent<LevelSelectionScene_UI_Option>().result = info;
                        }

                        if (count == 1)
                        {
                            descr.GetComponent<LevelSelectionScene_UI_Option>().sceneName =
                                doors[i].sceneName = sceneName;

                            descr.GetComponent<LevelSelectionScene_UI_Option>().SetSelected();
                        }
                        else
                        {
                            descr.GetComponent<LevelSelectionScene_UI_Option>().sceneName = sceneName;
                        }
                    }
                }
                else
                {
                    doors[i].sceneName = scene.Attributes["name"].Value;
                    Transform descr = doors[i].transform;
                    descr.Find("Name").GetComponent<Text>().text = doors[i].sceneName;
                    if (scene.Attributes["description"].Value != "")
                    {
                        descr.Find("Description").GetComponent<Text>().text = scene.Attributes["description"].Value;
                    }
                    if (ppManager.GetSceneCompleted(doors[i].sceneName))
                    {
                        string info = ppManager.GetSceneStars(doors[i].sceneName) + " stars; " +
                            ppManager.GetSceneTime(doors[i].sceneName);
                        descr.Find("Result").GetComponent<Text>().text = info;
                    }
                    else
                    {
                        descr.Find("Result").GetComponent<Text>().text = "";
                    }
                }
                ++i;
            }
            else break;
        }
    }
}
