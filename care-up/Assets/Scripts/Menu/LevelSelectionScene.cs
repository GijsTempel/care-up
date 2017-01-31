using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelSelectionScene : MonoBehaviour {
    
    void Start()
    {
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
                
                doors[i].sceneName = doors[i].description = scenes[Random.Range(0, scenes.Count)];
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
