using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using CareUp.Localize;
using CareUp.Actions;
using UnityEngine.UI;

public class VideoActionManager : MonoBehaviour
{
    public VideoPlayerManager videoPlayerManager;
    private PlayerPrefsManager manager;
    public List<VideoAction> videoActions = new List<VideoAction>();
    void Awake()
    {
        manager = GameObject.FindObjectOfType<PlayerPrefsManager>();

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/VideoActions/" + "TextVideoActions");
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

        videoPlayerManager.BuildVideoActionsPanel(this);
    }
    void Start()
    {
        
    }
}
