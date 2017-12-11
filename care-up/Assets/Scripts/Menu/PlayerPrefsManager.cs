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

    private string activatedScenes;

    public string ActivatedScenes
    {
        get { return activatedScenes; }
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
    
    private string SerialNumber
    {
        get { return PlayerPrefs.GetString("__SerialNumber"); }
        set { PlayerPrefs.SetString("__SerialNumber", value); }
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
        string[] data = new string[1];
        data[0] = SerialNumber;

        LoginPro.Manager.ExecuteOnServer("CheckSerial", CheckSerial_Success, Debug.LogError, data);
    }

    public void SetSerial(string serial)
    {
        //support for old keys
        PlayerPrefs.SetString("SerialKey", serial);

        SerialNumber = serial;
        CheckSerial();
    }

    public void CheckSerial_Success(string[] datas)
    {
        foreach(string data in datas)
        {
            SetSceneActivated(data, true);
            activatedScenes += data + " activated\n";
        }
    }
    
    public void AfterLoginCheck()
    {
        LoginPro.Manager.ExecuteOnServer("GetScenes", GetScenes_Success, Debug.LogError, null);

        CheckSerial();

        // support for old keys
        if (PlayerPrefs.GetString("SerialKey") != ""
            && PlayerPrefs.GetString("SerialKey") != SerialNumber)
        {
            SetSerial(PlayerPrefs.GetString("SerialKey"));
        }
    }

    public void GetScenes_Success(string[] datas)
    {
        foreach(string data in datas)
        {
            SetSceneActivated(data, false);
            Debug.Log("Scene " + data + " found on server!");
        }
    }
}
