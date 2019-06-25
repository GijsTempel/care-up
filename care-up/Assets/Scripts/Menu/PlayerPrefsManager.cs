using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using MBS;
using PaperPlaneTools;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Collections;

/// <summary>
/// Handles quick access to saved data.
/// Volume | Tutorial completion | Scene completion + results
/// </summary>
public class PlayerPrefsManager : MonoBehaviour
{
    private LocalizationManager localizationManager; // = new LocalizationManager();
    public bool VR = true;
    public bool practiceMode = true;
    public bool TextDebug = false;
    // store value here after getting from server
    public bool tutorialCompleted;

    private static PlayerPrefsManager instance;

    private List<string> activatedScenes = new List<string>();

    // sets up after selecting scene in "scene selection"
    public string currentSceneVisualName;
    public bool validatedScene;

    // post processing on camera
    public bool postProcessingEnabled = false;

    // is this game version is demo version
    public bool demoVersion = false;

    // indicates if game is played in testing mode (only editor mode)
    public bool testingMode = false;

    // save info about subscription
    // [HideInInspector]
    public bool subscribed = false;
    [HideInInspector]
    public static int plays = 0;
    
    public static int currentPracticeScore = 0;
    public static int currentPracticeStars = 0;
    
    private UMP_Manager manager;
    private MainMenu mainMenu;
    private Scene currentScene;
    
    public string fullPlayerName = "";
    public string bigNumber = "";

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

    public void Update()
    {
        SetEscapeButtonLogic();
    }

    public LocalizationManager GetLocalization()
    {
        return localizationManager;
    }

    private void OnLoaded(Scene s, LoadSceneMode m)
    {
        transform.position =
            GameObject.FindObjectOfType<AudioListener>().transform.position;

        currentScene = s;

        if (!(s.name == "LoginMenu" ||
                s.name == "MainMenu" ||
                s.name == "SceneSelection" ||
                s.name == "Scenes_Character_Customisation"))
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
            GameObject.Find("UMenuProManager/MenuCanvas/Opties/Panel_UI/OptionsGrid/PostProcessingToggle").GetComponent<Toggle>().isOn = postProcessingEnabled;
        }

        // handle platform-dependant objects (deleting unnecesarry)
        if (s.name == "MainMenu")
        {
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                Destroy(GameObject.Find("UMenuProManager/MenuCanvas/VersionUpdatePanel/Panel_Version_UI" +
                    "/NewVersionButtonGreenApple"));

                Destroy(GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/WUSerialScreen" +
                    "/RegisterArea/Buttons/MoreInfo_Apple"));
                Destroy(GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/WUSerialScreen" +
                    "/RegisterArea/Buttons/Purchase_Apple"));
            }

            if (Application.platform != RuntimePlatform.Android)
            {
                Destroy(GameObject.Find("UMenuProManager/MenuCanvas/VersionUpdatePanel/Panel_Version_UI" +
                    "/NewVersionButtonGreenAndroid"));
            }

            if (Application.platform != RuntimePlatform.WindowsPlayer)
            {
                Destroy(GameObject.Find("UMenuProManager/MenuCanvas/VersionUpdatePanel/Panel_Version_UI" +
                    "/NewVersionButtonGreenWindows"));
                Destroy(GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/WUSerialScreen" +
                    "/RegisterArea/Buttons/PurchaseButton_UWP"));
            }

            if ((Application.platform != RuntimePlatform.Android) &&
                (Application.platform != RuntimePlatform.WindowsPlayer))
            {
                Destroy(GameObject.Find("UMenuProManager/MenuCanvas/Dialogs/WUSerialScreen" +
                    "/RegisterArea/Buttons/PurchaseButton_AndroidWeb"));
            }
        }

        if (s.name == "LoginMenu")
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                Destroy(GameObject.Find("Canvas/WULoginPrefab/WUSerialScreen/RegisterArea/Purchase_Android_WebGL"));
            }

            if (Application.platform != RuntimePlatform.WindowsPlayer)
            {
                Destroy(GameObject.Find("Canvas/WULoginPrefab/WUSerialScreen/RegisterArea/Purchase_UWP"));
            }

            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                Destroy(GameObject.Find("Canvas/WULoginPrefab/WUSerialScreen/RegisterArea/MoreInfo_Apple"));
                Destroy(GameObject.Find("Canvas/WULoginPrefab/WUSerialScreen/RegisterArea/Purchase_Apple"));
            }
        }
    }

    void Awake()
    {
        if (!Application.isEditor)
        {
            testingMode = false;
        }
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        localizationManager = new LocalizationManager();
        localizationManager.LoadAllDictionaries();

        // uncomment this, fill with correct info and start game
        // p.s. dont forget to comment this again and not push instead :)
        //PlayerPrefsManager.__dev__customCertificate("playerFullName", "sceneName", "06202019");
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnLoaded;

        AudioListener.volume = Volume;
        //Debug.Log ("Volume is set to saved value: " + Volume);

        postProcessingEnabled = PlayerPrefs.GetInt("PostProcessing") == 1;
        //Debug.Log ("PostProcessing is set to saved value: " + postProcessingEnabled);

        // OnLoaded doesnt launch on initial scene? so force it in start function separately
        OnLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        manager = GameObject.FindObjectOfType<UMP_Manager>();
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

        foreach (string data in datas)
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
                tutorialCompleted = false; // store for current session
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
        foreach (string data in datas)
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
        RateBox.Instance.IncrementCustomCounter();
        RateBox.Instance.Show();
        
        PlayerPrefsManager.plays += 1;

        DatabaseManager.UpdateField("AccountStats", "Plays_Number", PlayerPrefsManager.plays.ToString());
    }

    public static void __sendMail(string topic, string message)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        { // apparently SMTP doesnt work with webgl
            __sendMailApp(topic, message);
            return;
        }
        else
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("info@careup.nl");
            mail.To.Add("info@careup.nl");
            mail.Subject = topic;
            mail.Body = message;

            SmtpClient smtpServer = new SmtpClient("smtp.strato.de");
            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential("info@careup.nl", "TripleMotionMedia3") as ICredentialsByHost;
            smtpServer.EnableSsl = true;

            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            smtpServer.Send(mail);
        }
    }

    public static string MyEscapeURL(string url)
    {
#pragma warning disable
        return WWW.EscapeURL(url).Replace("+", "%20");
#pragma warning restore
    }

    public static void __sendMailApp(string topic, string message)
    {
        topic = MyEscapeURL(topic);
        message = MyEscapeURL(message);
        Application.OpenURL("mailto:" + "info@careup.nl" + "?subject=" + topic + "&body=" + message);
    }

    /// <summary>
    /// Updates % highscore on database if new one is higher then old one.
    /// Also saves certificate date if there was no such previously.
    /// </summary>
    /// <param name="score"></param>
    public void UpdateTestHighscore(float score)
    {
        float currentTestScore = score * 100.0f;
        string currentTestScene = FormatSceneName(currentSceneVisualName);
        
        string highscoreStr = DatabaseManager.FetchField("TestHighscores", currentTestScene);
        float highscore = 0;
        float.TryParse(highscoreStr.Replace(",", "."), out highscore);

        if (highscore < currentTestScore)
        {
            DatabaseManager.UpdateField("TestHighscores", currentTestScene, currentTestScore.ToString());
        }

        // save certificate date here too
        string date = GetTodaysDateFormatted();
        DatabaseManager.UpdateField("CertificateDates", currentTestScene, date);
    }

    public static void AddOneToPracticePlays(string scene)
    {
        string practiceScene = FormatSceneName(scene);
        
        int plays;
        int.TryParse(DatabaseManager.FetchField("PracticePlays", practiceScene), out plays);
        DatabaseManager.UpdateField("PracticePlays", practiceScene, (plays + 1).ToString());
    }
    
    public void SetTutorialCompletedWU()
    {
        tutorialCompleted = true;

        CMLData data = new CMLData();
        data.Set("TutorialCompleted", "true");
        WUData.UpdateCategory("AccountStats", data);
    }

    static void GetTutorialCompleted(CML response)
    {
        //bool completed = response[1].Bool("TutorialCompleted");
        // do smth
    }

    static void GetTutorialCompleted_Error(CMLData response)
    {
        if ((response["message"] == "WPServer error: Empty response. No data found"))
        {
            CMLData data = new CMLData();
            data.Set("TutorialCompleted", "false");
            WUData.UpdateCategory("AccountStats", data);
        }
    }

    public void FetchLatestVersion()
    {
        WUData.FetchSharedField("LatestVersion", "GameInfo", GetLatestVersion, -1, GetLatestVersionError);
    }

    static void GetLatestVersion(CML response)
    {
        string currentVersion = Application.version;
        string latestVersion = response[1].String("LatestVersion");
        string[] cvSplit = currentVersion.Split('.');
        string[] lvSplit = latestVersion.Split('.');

        int currentVersionNum = int.Parse(cvSplit[0]) * 1000 + int.Parse(cvSplit[1]) * 100 + int.Parse(cvSplit[2]) * 10;
        int latestVersionNum = int.Parse(lvSplit[0]) * 1000 + int.Parse(lvSplit[1]) * 100 + int.Parse(lvSplit[2]) * 10;
        //print ("______Current: " + currentVersionNum.ToString () + " __latest: " + latestVersionNum.ToString ());
        if (currentVersionNum < latestVersionNum)
        {
            // player can download new version
            GameObject.Find("UMenuProManager/MenuCanvas/VersionUpdatePanel").SetActive(true);
        }
        else
        {
            GameObject.Find("UMenuProManager/MenuCanvas/VersionUpdatePanel").SetActive(false);
        }
    }

    static void GetLatestVersionError(CMLData response)
    {
        if ((response["message"] == "WPServer error: Empty response. No data found"))
        {
            GameObject.Find("UMenuProManager/MenuCanvas/VersionUpdatePanel").SetActive(false);
        }
    }

    public static string FormatSceneName(string sceneName)
    {
        string res = sceneName.Replace(" ", "_");
        res = res.Replace(".", "");
        return res;
    }

    public void UpdatePracticeHighscore(int score, int stars)
    {
        string practiceScene = FormatSceneName(currentSceneVisualName);
        
        int highscore;
        int.TryParse(DatabaseManager.FetchField("PracticeHighscores", "score_" + practiceScene), out highscore);
        if (highscore < score)
        {
            string[][] data = new string[][]
            {
                new string[] { "score_" + practiceScene, score.ToString() },
                new string[] { "stars_" + practiceScene, stars.ToString() }
            };
            DatabaseManager.UpdateCategory("PracticeHighscores", data);
        }
    }

    public static void __dev__customCertificate(string playerFullName, string sceneName, string date)
    {
        int keyValue = 192378; // salt
        keyValue += __sumString(playerFullName);
        keyValue += __sumString(sceneName);

        if (date == "")
        {
            date = GetTodaysDateFormatted();
        }
        keyValue += __sumString(date) * 13;

        string hexKey = Convert.ToString(keyValue, 16);
        hexKey = __trashFillString(hexKey);

        string link = "https://leren.careup.online/certificate.php";
        link += "name=" + playerFullName;
        link += "&scene=" + sceneName;
        link += "&date=" + date;
        link += "&misc=" + hexKey;

        Debug.LogWarning("OPENING LINK " + link);
        Application.OpenURL(link.Replace(" ", "%20"));
    }
    
    public static string __getCertificateLinkParams(string scene, string date = "", bool mail = false)
    {
        PlayerPrefsManager manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
        string name = manager.fullPlayerName;
        string email = WULogin.email;

        int keyValue = 192378; // salt
        keyValue += __sumString(name);
        keyValue += __sumString(scene);

        if (date == "")
        {
            date = GetTodaysDateFormatted();
        }
        keyValue += __sumString(date) * 13;

        string hexKey = Convert.ToString(keyValue, 16);
        hexKey = __trashFillString(hexKey);

        string link = "?";
        link += "name=" + name;
        link += "&scene=" + scene;
        link += "&date=" + date;
        link += "&misc=" + hexKey;
        if (mail)
        {
            link += "&mail=" + email;
        }

        return link;
    }

    /// <summary>
    /// Generates and opens link for certificate generation.
    /// Mirrors the safety measures of database script to generate safety key, included in the link.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="secondName"></param>
    /// <param name="scene"></param>
    /// <param name="score"></param>
    public static void __openCertificate(string scene, string date = "")
    {
        string link = "https://leren.careup.online/certificate.php";
        link += PlayerPrefsManager.__getCertificateLinkParams(scene, date);

        Debug.LogWarning("OPENING LINK " + link);
        Application.OpenURL(link.Replace(" ", "%20"));
    }

    public static void __sendCertificateToUserMail(string scene, string date = "")
    {
        string link = "https://leren.careup.online/Certificate_sendMail.php";
        link += PlayerPrefsManager.__getCertificateLinkParams(scene, date, true);
        
        Debug.LogWarning("Sending email with certificate to user.");
        PlayerPrefsManager.instance.StartCoroutine(__handleMailPDFRequest(link));
        Debug.Log(link);
    }

    static public IEnumerator __handleMailPDFRequest(string link)
    {
        UnityWebRequest unityWebRequest = new UnityWebRequest(link);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isDone)
        {
            Debug.Log(unityWebRequest.downloadHandler.text);
        }
    }

    public static string GetTodaysDateFormatted()
    {
        string day = DateTime.Now.Day.ToString();
        if (day.Length == 1) day = "0" + day;
        string month = DateTime.Now.Month.ToString();
        if (month.Length == 1) month = "0" + month;

        string date = day + month + DateTime.Now.Year.ToString();

        return date;
    }

    static int __sumString(string str)
    {
        int sum = 0;
        for (int i = 0; i < str.Length; ++i)
        {
            sum += (byte)str[i];
        }
        return sum;
    }

    static string __trashFillString(string str)
    {
        string res = "";

        string allowedChars = "ghijkmnopqrstuvwxyzGHJKLMNOPQRSTUVWXYZ";
        System.Random random = new System.Random((int)DateTime.Now.Ticks);

        for (int i = 0; i < str.Length; ++i)
        {
            res += (random.Next(0, 1) > 0 ? str[i].ToString().ToUpper() : str[i].ToString().ToLower());
            res += allowedChars[random.Next(0, allowedChars.Length)];
        }

        return res;
    }

    public static void SetFullName(string fullName)
    {
        GameObject.FindObjectOfType<PlayerPrefsManager>().fullPlayerName = fullName;
        DatabaseManager.UpdateField("AccountStats", "FullName", fullName);
    }

    public static void SetBIGNumber(string number)
    {
        GameObject.FindObjectOfType<PlayerPrefsManager>().bigNumber = number;
        DatabaseManager.UpdateField("AccountStats", "BIG_number", number);
    }
    
    private void SetEscapeButtonLogic()
    {
        //if (startTimer)
        //{
        //    timeLeft -= Time.deltaTime;
        //    timeOut = timeLeft > 0f ? true : false;                         
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape button pressed");

            //if (timeOut == false)
            //{
            //    print("Are you sure you want to leave?");
            //    timeLeft = 3.0f;
            //    startTimer = true;
            //    return;
            //}

            manager = GameObject.FindObjectOfType<UMP_Manager>();

            // Escape button logic for login scene
            if (GameObject.Find("WULoginPrefab") != null)
            {
                mainMenu = GameObject.FindObjectOfType<MainMenu>();

                if (GameObject.Find("RegisterWindow") != null)
                {
                    GameObject.Find("LoginRegisterArea/RegisterArea")?.transform.GetChild(0)?.GetComponent<Button>().onClick.Invoke();
                }

                else if (GameObject.Find("TermsAndConditionScreen") != null && GameObject.Find("Terms_Condition_Screen") == null && GameObject.Find("Voorwaarden_Screen") == null)
                {
                    GameObject.Find("TermsAndConditionScreen/RegisterArea")?.transform.GetChild(3)?.GetComponent<Button>().onClick.Invoke();
                }

                else if (GameObject.Find("LoginWindow") != null)
                {
                    GameObject.Find("LoginRegisterArea/LoginArea")?.transform.GetChild(6)?.GetComponent<Button>().onClick.Invoke();
                }

                else if (GameObject.Find("PassResetScreen") != null)
                {
                    GameObject.Find("PassResetArea")?.transform.GetChild(2)?.GetComponent<Button>().onClick.Invoke();
                }
            }

            // Escape button logic for main menu scene
            else if (GameObject.Find("UMenuProManager") != null)
            {
                GameObject accauntAchievementWindow = manager.Windows[7];
                GameObject accauntScoresWindow = manager.Windows[8];

                if (GameObject.Find("Dialogs/DialogTestPractice") != null)
                {
                    GameObject.Find("DialogTestPractice/Panel_UI").transform.GetChild(2)?.GetComponent<Button>().onClick.Invoke();
                }

                else if (GameObject.Find("InfoBar") != null)
                {
                    GameObject.Find("InfoBar").transform.GetChild(0)?.GetComponent<Button>().onClick.Invoke();
                }

                else if (accauntAchievementWindow.activeSelf)
                {
                    accauntAchievementWindow.transform.GetChild(0)?.GetComponent<Button>().onClick.Invoke();
                    accauntAchievementWindow.transform.GetChild(0)?.GetComponent<UMP_ButtonGroup>().OnSelect();
                }

                else if (accauntScoresWindow.activeSelf)
                {
                    accauntScoresWindow.transform.GetChild(1)?.GetComponent<Button>().onClick.Invoke();
                    accauntScoresWindow.transform.GetChild(1)?.GetComponent<UMP_ButtonGroup>().OnSelect();
                }

                else if (manager.Windows[3].activeSelf)
                {
                    return;
                }

                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (manager.Windows[i].activeSelf)
                        {
                            GameObject quitWindowButton = GameObject.Find("Menu/slot (4)");

                            if (quitWindowButton != null)
                            {
                                quitWindowButton.GetComponent<Button>().onClick.Invoke();
                                quitWindowButton.GetComponent<Button>().GetComponent<UMP_ButtonGroup>().OnSelect();
                            }

                            break;
                        }
                    }
                }
            }

            // Escape button logic for protocol scenes
            else
            {
                if (GameObject.Find("UI/CloseBtn") != null)
                {
                    GameObject.Find("UI/CloseBtn").transform?.GetComponent<Button>().onClick.Invoke();
                }

                else if (currentScene.name == "Scenes_Character_Customisation" && WULogin.characterCreated)
                {
                    bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
                }
            }

            //startTimer = false;
        }
    }

    public static string RandomString(int length)
    {
        System.Random random = new System.Random((int)DateTime.Now.Ticks);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}