using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelSelectionScene : MonoBehaviour {

    private PlayerPrefsManager ppManager;

    void Start()
    {
        ppManager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        if (ppManager == null) Debug.LogError("No player preferences manager found");

        SystemObject[] doors = GameObject.Find("Interactable Objects").GetComponentsInChildren<SystemObject>();
        foreach (SystemObject door in doors)
        {
            door.gameObject.SetActive(false);
        }

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Scenes");
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList doorNodes = xmlFile.FirstChild.NextSibling.FirstChild.ChildNodes;

        int i = 0; 
        foreach (XmlNode scene in doorNodes )
        {
            if (i < doors.Length)
            {
                doors[i].gameObject.SetActive(true);

                if (scene.Attributes["multiple"] != null)
                {
                    // general name
                    doors[i].transform.FindChild("Name").gameObject.SetActive(true);
                    doors[i].transform.FindChild("Name").GetComponent<TextMesh>().text
                        = scene.Attributes["name"].Value;

                    int count = 0;
                    foreach (XmlNode variation in scene.ChildNodes)
                    {
                        string sceneName = variation.Attributes["name"].Value;
                        Transform descr = doors[i].transform.FindChild("Description_" + ++count);
                        if (count == 1)
                        {
                            descr.GetComponent<SelectionScene_DoorUI>().sceneName =
                                doors[i].sceneName = doors[i].description = sceneName;
                            descr.GetComponent<SelectionScene_DoorUI>().SetSelected(true);
                        }
                        else
                        {
                            descr.gameObject.SetActive(true);
                            descr.GetComponent<SelectionScene_DoorUI>().sceneName = sceneName;
                        }

                        descr.FindChild("Name").GetComponent<TextMesh>().text 
                            = variation.Attributes["displayname"].Value;
                        
                        descr.FindChild("Description").GetComponent<TextMesh>().text 
                            = variation.Attributes["description"].Value;

                        if (ppManager.GetSceneCompleted(sceneName))
                        {
                            string info = ppManager.GetSceneStars(sceneName) + " stars; " +
                                ppManager.GetSceneTime(sceneName);
                            descr.FindChild("Result").GetComponent<TextMesh>().text = info;
                        }
                    }
                }
                else
                {
                    doors[i].sceneName = doors[i].description = scene.Attributes["name"].Value;
                    Transform descr = doors[i].transform.FindChild("Description_1");
                    descr.FindChild("Name").GetComponent<TextMesh>().text = doors[i].sceneName;
                    if (scene.Attributes["description"].Value != "")
                    {
                        descr.FindChild("Description").GetComponent<TextMesh>().text = scene.Attributes["description"].Value;
                    }
                    if (ppManager.GetSceneCompleted(doors[i].sceneName))
                    {
                        string info = ppManager.GetSceneStars(doors[i].sceneName) + " stars; " +
                            ppManager.GetSceneTime(doors[i].sceneName);
                        descr.FindChild("Result").GetComponent<TextMesh>().text = info;
                    }
                }
                ++i;
            }
            else break;
        }
    }
}
