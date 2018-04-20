﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using LoginProAsset;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles quick access to saved data.
/// Volume | Tutorial completion | Scene completion + results
/// </summary>
public class PlayerPrefsManager : MonoBehaviour
{
    public bool VR = true;
    public bool practiceMode = true;

    // store value here after getting from server
    public bool tutorialCompleted;

    private static PlayerPrefsManager instance;

    private List<string> activatedScenes = new List<string>();

    public string ActivatedScenes
    {
        get
        {
            string result = "";
            foreach (string s in activatedScenes)
                result += s + " geactiveerd.\n";
            return result;
        }
    }

    private void OnLoaded(Scene s, LoadSceneMode m)
    {
        transform.position =
        GameObject.FindObjectOfType<AudioListener>().transform.position;

        if (!(s.name == "Launch me 1" ||
              s.name == "Menu" ||
              s.name == "SceneSelection"))
        {
            GetComponent<AudioSource>().Stop();
        }

        if (s.name == "EndScore" ||
            (s.name == "Menu" && 
            !GetComponent<AudioSource>().isPlaying))
        {
            GetComponent<AudioSource>().Play();
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
        SceneManager.sceneLoaded += OnLoaded;

        AudioListener.volume = Volume;
        Debug.Log("Volume is set to saved value: " + Volume);
    }

    public float Volume
    {
        get { return PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : 1.0f; }
        set { PlayerPrefs.SetFloat("Volume", value); }
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

    public void CheckSerial()
    { 
        LoginPro.Manager.ExecuteOnServer("CheckSerial", CheckSerial_Success, Debug.LogError, null);
    }

    public void SetSerial(string serial)
    {
        string[] data = new string[1];
        data[0] = serial;

        LoginPro.Manager.ExecuteOnServer("SetSerial", SetSerialSuccess, Debug.LogError, data);
    }

    public void SetSerialSuccess(string[] datas)
    {
        CheckSerial();
    }

    private void CheckSerial_Success(string[] datas)
    {
        activatedScenes.Clear();

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
        
        // support for old key type
        if (PlayerPrefs.GetString("SerialKey") != "")
        {
            SetSerial(PlayerPrefs.GetString("SerialKey"));

            PlayerPrefs.SetString("SerialKey", ""); // clear this, so it happens once
        }

        // time for checking if tutorial is completed
        if (PlayerPrefs.HasKey("TutorialCompleted"))
        {
            if (PlayerPrefs.GetInt("TutorialCompleted") == 1)
            {
                // tutorial was completed, let's send to server and remove this info from PC
                LoginPro.Manager.ExecuteOnServer("SetTutorialCompleted", Blank, Debug.LogError, null);
                tutorialCompleted = true; // store for current session
            }
            else
            {
                tutorialCompleted = false;// store for current session
            }

            PlayerPrefs.DeleteKey("TutorialCompleted"); // delete 
        }
        else // when info is deleted from PC
        {
            LoginPro.Manager.ExecuteOnServer("GetTutorialCompleted", GetTutorialCompleted_Success, Debug.LogError, null);
        }
    }

    public void GetTutorialCompleted_Success(string[] datas)
    {
        if (datas.Length > 0)
        {
            // getting numerical value, not boolean
            int value = 0;
            int.TryParse(datas[0], out value);

            // 0 = false, 1 = true
            tutorialCompleted = (value > 0);
        }
    }

    public void GetScenes_Success(string[] datas)
    {
        foreach(string data in datas)
        {
            SetSceneActivated(data, false);
        }

        // activate scenes corresponding to serials
        CheckSerial();
    }

    public void Blank(string[] s) { }
}
