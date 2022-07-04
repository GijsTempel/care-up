using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using CareUp.Localize;
using CareUp.Actions;
using UnityEngine.UI;
using UnityEngine.Networking;

public class VideoActionManager : MonoBehaviour
{
    public static string baseURL = "https://leren.careup.online/Video/001/";
    string actionDataText = "";
    public VideoPlayerManager videoPlayerManager;
    private PlayerPrefsManager manager;
    public List<VideoAction> videoActions = new List<VideoAction>();
    public TextAsset textAsset;
    bool actionDataWasLoaded = false;
    public bool initialized = false;
    private IEnumerator coroutine;

    void Awake()
    {

        videoActions.Clear();
        VideoAction startAction = new VideoAction("Begin", "", 0);
        videoActions.Add(startAction);
        manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
        if (manager == null)
        { 
            ReadActionsFromAsset();
            videoPlayerManager.BuildVideoActionsPanel(this);
            initialized = true;
        }
        else
        {
            coroutine = LoadActions();
            StartCoroutine(coroutine);
        }
    }

    private void Update()
    {
        if (!initialized)
        {
            if (actionDataWasLoaded)
            {
                initialized = true;
                enabled = false;
                ReadActions(actionDataText);
                videoPlayerManager.BuildVideoActionsPanel(this);
            }
        }
    }
  
    static public IEnumerator LoadActions()
    {
        PlayerPrefsManager _manager = GameObject.FindObjectOfType<PlayerPrefsManager>();

        string actionsURL = baseURL + _manager.videoSceneName + "/videoActions.xml";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(actionsURL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("NetworkError");
            }
            else if (webRequest.downloadHandler.text != "")
            {
                VideoActionManager _videoActionManager = GameObject.FindObjectOfType<VideoActionManager>();
                _videoActionManager.actionDataText = webRequest.downloadHandler.text;
                _videoActionManager.actionDataWasLoaded = true;
            }
            else
            {

            }
        }
    }


    void ReadActionsFromAsset()
    {
        ReadActions(textAsset.text);
    }

    void ReadActions(string _actionsText)
    {
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(_actionsText);
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

}
