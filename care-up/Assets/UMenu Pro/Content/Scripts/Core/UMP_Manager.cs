using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MBS;

public class UMP_Manager : MonoBehaviour {

    [Header("Level Manager")]
    public List<LevelInfo> Levels = new List<LevelInfo>();    

    public InputField SceneSearchBar;
    public InputField LeaderBoardSearchBar;
    [Header("Settings")]
    public string PlayButtonName = "QUICKPLAY >";

    [Header("References")]
    public List<GameObject> Windows = new List<GameObject>();
    public List<UMP_DialogUI> Dialogs = new List<UMP_DialogUI>();

    public GameObject notificationWindow;
    public GameObject newNotificationIcon;

    public CongratulationTab congratulation;

    public GameObject LevelPrefab;
    public Transform LevelPanel;
    MenuEffects menuEffects;

    private int CurrentWindow = 0;
    private PlayerPrefsManager manager;

    public int GetCurrentWindow()
    {
        return CurrentWindow;
    }

    public void ShowNotificationWindow(bool toShow = true)
    {
        UpdateNotifButton();
        if (notificationWindow != null)
        {
            notificationWindow.SetActive(toShow);
        }
    }

    public void LoadTestNotificationData()
    {
        // it's gonne be sent to server now and saved there

        string longMessage1 = "The Unity game engine launched in 2005, aiming to \"democratize\" game development by making it accessible to more developers. \nThe next year, Unity was named runner-up in the Best Use of Mac OS X Graphics category in Apple Inc.'s Apple Design Awards. Unity was initially released for Mac OS X, later adding support for Microsoft Windows and Web browsers.";
        string longMessage2 = "Godot aims to offer a fully integrated game development environment. \n " +
            "It allows developers to create a game from scratch, needing no other tools beyond those used for content creation (art assets, music, etc.). \n" +
            "The engine's architecture is built around the concept of a tree of \"nodes\". \n" +
            "Nodes are organized inside of \"scenes\", which are reusable, instanceable, inheritable, and nestable groups of nodes. \n" +
            "All game resources, including scripts and graphical assets, are saved as part of the computer's file system (rather than in a database). \n" +
            "This storage solution is intended to facilitate collaboration between game development teams using software version control systems. \n\n" +
            "The engine supports deployment to multiple platforms and allows specification of texture compression and resolution settings for each platform. Currently, supported platforms include Linux, macOS, Microsoft Windows, BSD, Android, iOS, Universal Windows Platform, HTML5, and WebAssembly.";

        DatabaseManager.PushCANotification(0, new PlayerPrefsManager.CANotifications("Title number 111", longMessage1,
                    "William Shakespeare", false, 1599264000));
        DatabaseManager.PushCANotification(1, new PlayerPrefsManager.CANotifications("Title number 222", longMessage2,
                   "Agatha Christie", false, 1600128000));
        DatabaseManager.PushCANotification(2, new PlayerPrefsManager.CANotifications("Title number 3", "More messages",
                    "J. K. Rowling", false, 1601645343));
        //DatabaseManager.PushCANotification(3, new PlayerPrefsManager.CANotifications("Title number 4", "Many many many more messages",
        //            "Stephen King", false, 1601645343));
        //DatabaseManager.PushCANotification(4, new PlayerPrefsManager.CANotifications("Title number 5", "This message can be a bit long. I had to place it here to test, how it will fit to the selected place for the test. If you can read this, it was shown correctly, and there is enough space for message like this",
        //            "Robert Asprin", false, 1601645343));
        //DatabaseManager.PushCANotification(5, new PlayerPrefsManager.CANotifications("Title number 6", "And the last one",
        //            "Ian Fleming", false, 1601645343));
    }

    public void ShowCongratulation(int coins, int diamants = 0)
    {
        if (congratulation != null)
        {
            if (manager == null)
                manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
            manager.muteMusicForEffect = true;
            manager.ToPlayMusic(false);

            congratulation.ShowDialogue(coins, diamants);
        }
    }

    public void UpdateNotifButton()
    {
        newNotificationIcon.SetActive(PlayerPrefsManager.HasNewNorifications());
    }

    void Awake()
    {
        //InstanceLevels();
        GameObject.Find("Leaderboard").SetActive(false);
        GameObject.Find("Opties").SetActive(false);
        GameObject.Find("Store").SetActive(false);
        GameObject.Find("AdjustCharacter").SetActive(false);
        menuEffects = GameObject.FindObjectOfType<MenuEffects>();
    }

    private void Start()
    {
        UpdateNotifButton();
        ShowNotificationWindow(PlayerPrefsManager.HasNewNorifications());
    }

    //void InstanceLevels()
    //{
    //    for (int i = 0; i < Levels.Count; i++)
    //    {
    //        GameObject l = Instantiate(LevelPrefab) as GameObject;

    //        UMP_LevelInfo li = l.GetComponent<UMP_LevelInfo>();
    //        li.GetInfo(Levels[i].Title, Levels[i].Description, Levels[i].Preview, Levels[i].LevelName,PlayButtonName);

    //        l.transform.SetParent(LevelPanel, false);
    //    }
    //}
   
    /// <param name="id">window to active</param>
    /// <param name="disable">disabled currents window?</param>
    public void ChangeWindow(int id)
    {
        SceneSearchBar.text = "";
        LeaderBoardSearchBar.text = "";

        if (CurrentWindow == id)
            return;

        for (int i = 0; i < Windows.Count; i++)
        {
        if (Windows[i] != null)
            Windows[i].SetActive(false);
        }           
        
        CurrentWindow = id;
        Windows[id].SetActive(true);
        menuEffects.ApplyMenuEffect();            
    }

    /// <summary>
    /// Open URL
    /// </summary>
    /// <param name="url"></param>
    public void SocialButton(string url) { Application.OpenURL(url); }

  
    /// <param name="indexDialog"></param>
    public void ShowDialog(int indexDialog)
    {
        if(Dialogs[indexDialog] != null)
        {
            Dialogs[indexDialog].gameObject.SetActive(true);
        }
        else
        {
            Debug.Log(string.Format("Does't exits a dialog in the index {0} of list", indexDialog));
        }
    }


    public void ShowDialogByName(string dialogName)
    {
        int dialogIndex = -1;
        for (int i = 0; i < Dialogs.Count; i++)
        {
            if (Dialogs[i].name == dialogName)
            {
                dialogIndex = i;
                break;
            }
        }
        if (dialogIndex != -1)
        {
            ShowDialog(dialogIndex);
        }
        else
        {
            Debug.Log(string.Format("Does't exits a dialog with name " + dialogName));
        }
    }

    public void TestDialiog()
    {
        int value = 0;
        int.TryParse(transform.Find("MenuCanvas/InputField").GetComponent<InputField>().text, out value);
        ShowDialog(value);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="indexDialog"></param>
    public void ShowDialog(int indexDialog,string text)
    {
        if (Dialogs[indexDialog] != null)
        {
            Dialogs[indexDialog].gameObject.SetActive(true);
             if (!string.IsNullOrEmpty(text)) { Dialogs[indexDialog].SetText(text); }
        }
        else
        {
            Debug.Log(string.Format("Does't exits a dialog in the index {0} of list", indexDialog));
        }
    }

    /// <summary>
    /// Quit
    /// </summary>
    public void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    public void LogOff()
    {
        Windows[3].SetActive (false);
        WULogin.LogOut();
    }

    public void TutorialClicked () 
    {
        Windows[2].SetActive (false);
    }

    [System.Serializable]
    public class LevelInfo
    {
        /// <summary>
        /// Name of scene of build setting
        /// </summary>
        public string LevelName;
        [Space(5)]
        public string Title;
        public string Description;
        public Sprite Preview;
    }
}