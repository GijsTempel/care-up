using UnityEngine;
using System.Collections;

/// <summary>
/// Handles quick access to saved data.
/// Volume | Tutorial completion | Scene completion + results
/// </summary>
public class PlayerPrefsManager : MonoBehaviour
{
    private static PlayerPrefsManager instance;
    
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
}
