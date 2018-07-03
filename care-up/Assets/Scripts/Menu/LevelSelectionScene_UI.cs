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
        // load xml
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Scenes");
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList xmlSceneList = xmlFile.FirstChild.NextSibling.FirstChild.ChildNodes;
        
        foreach (XmlNode xmlSceneNode in xmlSceneList)
        {
            bool activated = PlayerPrefs.GetInt(xmlSceneNode.Attributes["id"].Value + " activated") == 1;
            bool hidden = xmlSceneNode.Attributes["hidden"] != null;
            if (!activated && hidden)
            {
                // not activated and hidden scene should not even create a panel, so just end up here
                continue;
            }

            // if we're here, then we have real scene, that is not hidden
            // instantiating panel
            GameObject sceneUnitObject = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SceneSelectionUnit"),
                GameObject.Find("UMenuProManager/MenuCanvas/Play/LevelPanel/ViewPoint/List").transform);
            sceneUnitObject.name = "SceneSelectionUnit"; // i dont like that 'clone' word at the end, ugh
            LevelButton sceneUnit = sceneUnitObject.GetComponent<LevelButton>();
            
            if (!activated && !hidden)
            {
                // but if scene is not activated and NOT hidden either
                // just disable play button, but show the panel to the player
                sceneUnit.transform.Find("BottomBar/Play").GetComponent<Button>().interactable = false;
            }

            // now let's fill some actual info about the scene
            if (xmlSceneNode.Attributes["multiple"] != null)
            {
                sceneUnit.multiple = true;

                // so we're setting only scene title and picture 
                // saving everything else for dialogues

                // setting scene title
                sceneUnit.transform.Find("Degradado/Title").GetComponent<Text>().text
                    = sceneUnit.displayName = xmlSceneNode.Attributes["name"].Value;

                // saving bundle name for later
                string bundleName = sceneUnit.bundleName = xmlSceneNode.Attributes["bundleName"].Value;

                int i = 0;
                foreach (XmlNode variation in xmlSceneNode.ChildNodes)
                {
                    LevelButton.Info info = new LevelButton.Info();
                    // saving all the info for scene variation, use in dialogue
                    info.bundleName = bundleName;
                    info.sceneName = variation.Attributes["name"].Value;
                    info.displayName = variation.Attributes["displayname"].Value;
                    info.description = variation.Attributes["description"].Value;
                    info.image = Resources.Load<Sprite>("Sprites/ScenePreview/" + variation.Attributes["image"].Value);

                    sceneUnit.variations.Add(info);

                    if (i == 0)
                    {
                        // set the image as main if this is 1st variation
                        sceneUnit.image = sceneUnit.variations[i].image;
                        sceneUnit.transform.Find("LevelPreview").GetComponent<Image>().sprite = sceneUnit.image;

                        // also make 1st option 'selected'
                        sceneUnit.sceneName = sceneUnit.variations[i].sceneName;
                    }

                    // setting max 3 for now, dont see that UI supports more
                    if (++i > 2)
                        break;
                }
            }
            else
            {
                // saving info for loading
                sceneUnit.multiple = false;
                sceneUnit.sceneName = xmlSceneNode.Attributes["sceneName"].Value;
                sceneUnit.bundleName = xmlSceneNode.Attributes["bundleName"].Value;

                // setting scene title
                sceneUnit.transform.Find("Degradado/Title").GetComponent<Text>().text
                    = sceneUnit.displayName = xmlSceneNode.Attributes["name"].Value;

                // setting description
                if (xmlSceneNode.Attributes["description"].Value != "")
                {
                    // no description atm
                    //  = scene.Attributes["description"].Value;
                }

                // setting image
                if (xmlSceneNode.Attributes["image"] != null)
                {
                    sceneUnit.image = Resources.Load<Sprite>("Sprites/ScenePreview/" + xmlSceneNode.Attributes["image"].Value);
                    sceneUnit.transform.Find("LevelPreview").GetComponent<Image>().sprite = sceneUnit.image;
                }
            }
        }
    }
}
