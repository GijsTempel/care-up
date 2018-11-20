using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using LoginProAsset;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
using MBS;

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

    // sets up after selecting scene in "scene selection"
    public string currentSceneVisualName;

    // post processing on camera
    public bool postProcessingEnabled = false;

    // is this game version is demo version
    public bool demoVersion = false;

    // save info about subscription
    [HideInInspector]
    public bool subscribed = false;
    [HideInInspector]
    public static int plays = 0;

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
              s.name == "MainMenu" ||
              s.name == "SceneSelection"))
        {
            // game scenes
            GetComponent<AudioSource>().Stop();
            if (Camera.main.GetComponent<PostProcessingBehaviour>() != null)
            {
                Camera.main.GetComponent<PostProcessingBehaviour>().enabled = postProcessingEnabled;
            }
        }

        if (s.name == "EndScore" ||
            (s.name == "MainMenu" && 
            !GetComponent<AudioSource>().isPlaying))
        {
            GetComponent<AudioSource>().Play();
        }

        if (s.name == "MainMenu")
        {
            GameObject.Find("UMenuProManager/MenuCanvas/Opties/Panel_UI/PostProcessingToggle").GetComponent<Toggle>().isOn = postProcessingEnabled;
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

        postProcessingEnabled = PlayerPrefs.GetInt("PostProcessing") == 1;
        Debug.Log("PostProcessing is set to saved value: " + postProcessingEnabled);
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

    public void SetSceneCompletionData(string sceneName, int score, int time)
    {
        if (demoVersion) return;
        // hashes are NOT a clean solution
        int hash = Mathf.Abs(sceneName.GetHashCode());
        MBS.WUScoring.SubmitScore(score, hash);
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

    public void SetSerial(string serial)
    {
        string[] data = new string[1];
        data[0] = serial;

       // LoginPro.Manager.ExecuteOnServer("SetSerial", SetSerialSuccess, Debug.LogError, data);
    }

    private void CheckSerial_Success(string[] datas)
    {
        activatedScenes.Clear();

        foreach(string data in datas)
        {
            if (data == "Received") break;
            string[] result;
            result = data.Split('|');
            
            SetSceneActivated(result[0], true);
            if (!activatedScenes.Contains(result[1]))
                activatedScenes.Add(result[1]);
        }

        // set new activated scenes in news
        GameObject.Find("UMenuProManager/MenuCanvas/Home/BannerArea/RegisterProtocols/News/Text")
            .GetComponent<Text>().text = ActivatedScenes;
    }

    private void CheckSerialAfterLogIn_Success(string[] datas)
    {
        activatedScenes.Clear();

        foreach (string data in datas)
        {
            if (data == "Received")
                break;

            string[] result;
            result = data.Split('|');

            SetSceneActivated(result[0], true);
            if (!activatedScenes.Contains(result[1]))
                activatedScenes.Add(result[1]);
        }

        // now after login
        // we check the serial for the scenes
        // and then load new menu scene
        //LoginPro_Security.Load("UMenuPro");
        bl_SceneLoaderUtils.GetLoader.LoadLevel("UMenuPro");
    }

    public void AfterLoginCheck()
    {
        // deactivate scenes
        //LoginPro.Manager.ExecuteOnServer("GetScenes", GetScenes_Success, Debug.LogError, null);
        
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
               // LoginPro.Manager.ExecuteOnServer("SetTutorialCompleted", Blank, Debug.LogError, null);
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
           // LoginPro.Manager.ExecuteOnServer("GetTutorialCompleted", GetTutorialCompleted_Success, Debug.LogError, null);
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
       
    }

    public void GetSceneLeaders(string scene, int top, System.Action<string[]> method)
    {
        string[] datas = new string[2];
        datas[0] = scene;
        datas[1] = top.ToString();

        //LoginPro.Manager.ExecuteOnServer("GetSceneLeaders", method, Debug.LogError, datas);
    }

    public void GetSceneDatabaseInfo(string scene, System.Action<string[]> method)
    {
        string[] datas = new string[1];
        datas[0] = scene;
        Debug.Log(datas[0]);

       // LoginPro.Manager.ExecuteOnServer("GetSceneInfo", method, Debug.LogError, datas);
    }

    public void Blank(string[] s) { }

    public static void AddOneToPlaysNumber()
    {
        WUData.FetchField("Plays_Number", "AccountStats", GetPlaysNumber, -1, GetPlaysNumber_Error);
    }
    
    static void GetPlaysNumber(CML response)
    {
        plays = response[1].Int("Plays_Number") + 1;
        
        // update +1
        CMLData data = new CMLData();
        data.Set("Plays_Number", plays.ToString());
        WUData.UpdateCategory("AccountStats", data);
    }

    static void GetPlaysNumber_Error(CMLData response)
    {
        if ((response["message"] == "WPServer error: Empty response. No data found"))
        {
            // empty response, need to create field with 1 play
            CMLData data = new CMLData();
            data.Set("Plays_Number", "1");
            WUData.UpdateCategory("AccountStats", data);
        }
    }
}
