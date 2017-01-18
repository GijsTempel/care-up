using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.SceneManagement;

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
                doors[i].sceneName = doors[i].description = scene.Attributes["name"].Value;
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
