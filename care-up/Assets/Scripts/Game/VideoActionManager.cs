using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using CareUp.Localize;
using CareUp.Actions;
using UnityEngine.UI;

public class VideoActionManager : MonoBehaviour
{
    public string baseURL = "https://leren.careup.online/Video/001/";
    public VideoPlayerManager videoPlayerManager;
    private PlayerPrefsManager manager;
    public List<VideoAction> videoActions = new List<VideoAction>();
    public TextAsset textAsset;
    bool actionDataWasLoaded = false;

    void Awake()
    {
        videoActions.Clear();
        VideoAction startAction = new VideoAction("Begining", "", 0);
        videoActions.Add(startAction);
        manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
        if (manager != null)
        {
            string actionsURL = baseURL + manager.videoSceneName + "/videoActions.xml";
            ReadActionsFromURL(actionsURL);
        }
        else
        {
            ReadActionsFromAsset();
        }    

        videoPlayerManager.BuildVideoActionsPanel(this);
    }

    void ReadActionsFromURL(string actionsURL)
    {
        XmlTextReader reader = new XmlTextReader(actionsURL);
        while (reader.Read())
        {
            // Do some work here on the data.
            if (reader.Name == "action")
            {
                VideoAction videoAction = new VideoAction();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    while (reader.MoveToNextAttribute()) // Read the attributes.
                        switch (reader.Name)
                        {
                            case "title":
                                videoAction.title = LocalizationManager.GetValueIfKey(reader.Value);
                                break;
                            case "description":
                                videoAction.description = LocalizationManager.GetValueIfKey(reader.Value);
                                break;
                            case "frame":
                                int.TryParse(reader.Value, out videoAction.startFrame);
                                videoAction.startFrame = videoAction.startFrame + 5;
                                break;
                        }
                    Debug.Log(" " + reader.Name + "=='" + reader.Value + "'");
                    videoActions.Add(videoAction);
                }

            }
        }
    }
    void ReadActionsFromAsset()
    {
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList actions = xmlFile.FirstChild.NextSibling.ChildNodes;
        foreach (XmlNode action in actions)
        {
            VideoAction videoAction = new VideoAction();
            videoAction.description = LocalizationManager.GetValueIfKey(action.Attributes["description"].Value);
            videoAction.title = LocalizationManager.GetValueIfKey(action.Attributes["title"].Value);
            int.TryParse(action.Attributes["frame"].Value, out videoAction.startFrame);
            videoActions.Add(videoAction);
        }

    }
    void Start()
    {
        
    }
}
