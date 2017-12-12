using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using LoginProAsset;

/// <summary>
/// Handles quick access to saved data.
/// Volume | Tutorial completion | Scene completion + results
/// </summary>
public class PlayerPrefsManager : MonoBehaviour
{
    public bool VR = true;
    public bool practiceMode = true;

    private static PlayerPrefsManager instance;

    private List<string> activatedScenes;

    public string ActivatedScenes
    {
        get
        {
            string result = "";
            foreach (string s in activatedScenes)
                result += s + " activated.\n";
            return result;
        }
    }
    
    void Awake()
    {
        if ( instance )
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    void Start()
    {
        AudioListener.volume = Volume;
        Debug.Log("Volume is set to saved value: " + Volume);
    }

    public float Volume
    {
        get { return PlayerPrefs.GetFloat("Volume"); }
        set { PlayerPrefs.SetFloat("Volume", value); }
    }

    public bool TutorialCompleted
    {
        get { return PlayerPrefs.GetInt("TutorialCompleted") == 1; }
        set { PlayerPrefs.SetInt("TutorialCompleted", value ? 1 : 0); }
    }
    
    public void SetSceneActivated(string sceneName, bool value)
    {
        if (value) Debug.Log(sceneName + " activated");
        PlayerPrefs.SetInt(sceneName + " activated", value ? 1 : 0);
    }

    public bool GetSceneActivated(string sceneName)
    {
        return PlayerPrefs.GetInt(sceneName + " activated") == 1;
    }

    public void SetSceneCompletionData(string sceneName, int stars, string time)
    {
        PlayerPrefs.SetInt(sceneName + "_completed", 1);
        PlayerPrefs.SetInt(sceneName + "_stars", stars);
        PlayerPrefs.SetString(sceneName + "_time", time);
    }

    public bool GetSceneCompleted(string sceneName)
    {
        return PlayerPrefs.GetInt(sceneName + "_completed") == 1;
    }

    public int GetSceneStars(string sceneName)
    {
        return PlayerPrefs.GetInt(sceneName + "_stars");
    }

    public string GetSceneTime(string sceneName)
    {
        return PlayerPrefs.GetString(sceneName + "_time");
    }

    public bool TutorialPopUpDeclined
    {
        get { return PlayerPrefs.GetInt("TutorialPopUpDeclined") == 1; }
        set { PlayerPrefs.SetInt("TutorialPopUpDeclined", value ? 1 : 0); }
    }

    private void CheckSerial()
    {
        LoginPro.Manager.ExecuteOnServer("CheckSerial", CheckSerial_Success, Debug.LogError, null);
    }

    public void SetSerial(string serial)
    {
        string[] data = new string[1];
        data[0] = serial;

        LoginPro.Manager.ExecuteOnServer("SetSerial", Blank, Debug.LogError, data);

        CheckSerial();
    }

    public void CheckSerial_Success(string[] datas)
    {
        foreach(string data in datas)
        {
            string[] result;
            result = data.Split('|');
            
            SetSceneActivated(result[0], true);
            if (!activatedScenes.Contains(result[1]))
                activatedScenes.Add(result[1]);
        }

        // setting news
        GameObject menuWindow = GameObject.Find("MenuWindow");
        if (menuWindow != null)
        {
            menuWindow.GetComponent<LoginPro_Menu>().News.text = ActivatedScenes;
        }
    }
    
    public void AfterLoginCheck()
    {
        // deactivate scenes
        LoginPro.Manager.ExecuteOnServer("GetScenes", GetScenes_Success, Debug.LogError, null);

        // activate scenes corresponding to serials
        CheckSerial();

        // support for old key type
        if (PlayerPrefs.GetString("SerialKey") != "")
        {
            SetSerial(PlayerPrefs.GetString("SerialKey"));

            PlayerPrefs.SetString("SerialKey", ""); // clear this, so it happens once
        }
    }

    public void GetScenes_Success(string[] datas)
    {
        foreach(string data in datas)
        {
            SetSceneActivated(data, false);
        }
    }

    public void Blank(string[] s) { }
}
