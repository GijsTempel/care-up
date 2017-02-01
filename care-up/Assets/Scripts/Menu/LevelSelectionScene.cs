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

                List<string> scenes = new List<string>();
                scenes.Add(scene.Attributes["name"].Value);
                if (scene.Attributes["alt1"] != null)
                {
                    scenes.Add(scene.Attributes["alt1"].Value);
                }
                if (scene.Attributes["alt2"] != null)
                {
                    scenes.Add(scene.Attributes["alt2"].Value);
                }
                if (scene.Attributes["alt3"] != null)
                {
                    scenes.Add(scene.Attributes["alt3"].Value);
                }
                
                doors[i].sceneName = scenes[Random.Range(0, scenes.Count)];
                doors[i].transform.GetChild(0).FindChild("Name").GetComponent<TextMesh>().text = doors[i].sceneName;
                if ( scene.Attributes["description"].Value != "" )
                {
                    doors[i].transform.GetChild(0).FindChild("Description").GetComponent<TextMesh>().text = scene.Attributes["description"].Value;
                }
                if (ppManager.GetSceneCompleted(doors[i].sceneName))
                {
                    string info = ppManager.GetSceneStars(doors[i].sceneName) + " stars; " +
                        ppManager.GetSceneTime(doors[i].sceneName);
                    doors[i].transform.GetChild(0).FindChild("Result").GetComponent<TextMesh>().text = info;
                }

                ++i;
            }
            else break;
        }
    }

    public void GoToLevel(string levelname)
    {
        SceneManager.LoadScene(levelname);
    }
	
}
