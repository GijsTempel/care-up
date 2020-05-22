using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBS;
using UnityEngine.SceneManagement;
using System;
using System.Globalization;

public class DatabaseManager : MonoBehaviour
{
    private static string sessionKey = "";
    private static bool sessionTimeOut = false;

    public class Category
    {
        public string name;
        public Dictionary<string, string> fields;

        public Category() { name = ""; fields = new Dictionary<string, string>(); }
        public Category(string _name) : this() { name = _name; }
    };

    private static DatabaseManager instance;
    private static List<Category> database = new List<Category>();

    private static Coroutine sessionCheck;
    private static Coroutine timeCheck;

    private static string formattedTimeSpan = TimeSpan.Zero.ToString();

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "LoginMenu")
        {
            if (sessionTimeOut)
            {
                GameObject.Find("Canvas/WULoginPrefab/SessionTimeOutPanel").SetActive(true);
                sessionTimeOut = false;
            }
        }
    }

    public static void Init()
    {
        // querry all player info here?
        // StartCoroutine(RequestPurchases(WULogin.UID));
        WPServer.RequestPurchases(WULogin.UID);
        WUData.FetchUserGameInfo(WULogin.UID, FetchEverything_success, -1, PostInit);
    }

    public static void Clean()
    {
        database.Clear();
        instance.StopCoroutine(sessionCheck);
        instance.StopCoroutine(timeCheck);
        GameObject.FindObjectOfType<PlayerPrefsManager>().subscribed = false;
    }

    private static void PostInit(CMLData ignore = null)
    {
        // increment logins number
        int loginNumber;
        int.TryParse(FetchField("AccountStats", "Login_Number"), out loginNumber);
        UpdateField("AccountStats", "Login_Number", (loginNumber + 1).ToString());

        // set up plays number
        string plays = FetchField("AccountStats", "Plays_Number");
        int.TryParse(plays, out PlayerPrefsManager.plays);

        // set sub status, for iPhone it's being set in IAPManager
        GameObject.FindObjectOfType<PlayerPrefsManager>().subscribed =
            GameObject.FindObjectOfType<PlayerPrefsManager>().subscribed || WULogin.HasSerial;

        // set character info
        CharacterInfo.sex = FetchField("AccountStats", "Sex");
        int.TryParse(FetchField("AccountStats", "Head"), out CharacterInfo.headType);
        int.TryParse(FetchField("AccountStats", "Body"), out CharacterInfo.bodyType);
        int.TryParse(FetchField("AccountStats", "Glasses"), out CharacterInfo.glassesType);
        int.TryParse(FetchField("AccountStats", "Index"), out CharacterInfo.index);
        CharacterInfo.hat = FetchField("AccountStats", "Hat");

        // set player irl full name
        GameObject.FindObjectOfType<PlayerPrefsManager>().fullPlayerName =
            FetchField("AccountStats", "FullName");

        // set player BIG number
        GameObject.FindObjectOfType<PlayerPrefsManager>().bigNumber =
            FetchField("AccountStats", "BIG_number");

        // initialize store manager, cuz we just got info about current currency etc
        PlayerPrefsManager.storeManager.Init();

        // check if character created, load proper scene
        // load scene at the end of this function
        bool goToMainMenu = FetchField("AccountStats", "CharacterCreated") == "true" &&
             FetchField("AccountStats", "FullName") != "" &&
             (FetchField("AccountStats", "CharSceneV2") == "true" ||
             FetchField("AccountStats", "BIG_number") != "");

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (PlayerPrefsManager.editCharacterOnStart)
            goToMainMenu = false;
#endif
        if (goToMainMenu)
        {
            WULogin.characterCreated = true;
            if (FetchField("AccountStats", "TutorialCompleted") == "true")
            {
                bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
            }
            else
            {
                bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Tutorial", "scenes_tutorial");
            }
        }
        else
        {
            WULogin.characterCreated = false;
            bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Character_Customisation");
        }

        // 1 session restriction, checking once a minute
        sessionKey = PlayerPrefsManager.RandomString(16);
        UpdateField("AccountStats", "SessionKey", sessionKey);
        sessionCheck = instance.StartCoroutine(CheckSession(60.0f));
        timeCheck = instance.StartCoroutine(SetTime(60.0f));
    }

    private static void FetchEverything_success(CML response)
    {
        for (int i = 0; i < response.Elements.Count; ++i)
        {
            // possible data types: DATA, _CATEGORY_, _GAME_
            if (response.Elements[i].data_type != "_CATEGORY_")
            {
                continue;
            }

            // we're here if data type is category
            string categoryName = "";

            for (int j = 0; j < response.Elements[i].Keys.Length; ++j)
            {
                switch (response.Elements[i].Keys[j])
                {
                    // skip trash info
                    case "id":
                    case "_wpnonce":
                    case "woocommerce-login-nonce":
                    case "woocommerce-reset-password-nonce":
                        break;
                    case "category":
                        // this is category name
                        // guaranteed to be found before any actual fields pushed
                        categoryName = response.Elements[i].Values[j];
                        break;
                    default:
                        // everything else should be just normal fields
                        PushField(categoryName,
                            response.Elements[i].Keys[j],
                            response.Elements[i].Values[j]);
                        break;
                }
            }
        }

        PostInit();
    }

    private static void PushField(string category, string fieldName, string fieldValue)
    {
        Category cat = database.Find(x => x.name == category);
        if (cat != default(Category))
        {
            if (cat.fields.ContainsKey(fieldName))
            {
                cat.fields[fieldName] = fieldValue;
            }
            else
            {
                cat.fields.Add(fieldName, fieldValue);
            }
        }
        else
        {
            // no category found, gotta init one
            Category newCat = new Category(category);
            newCat.fields.Add(fieldName, fieldValue);
            database.Add(newCat);
        }
    }

    public static void PrintDatabase()
    {
        string output = "";
        foreach (Category c in database)
        {
            output += "Category: " + c.name + "\n";
            foreach (string key in c.fields.Keys)
            {
                output += "    " + key + ": " + c.fields[key] + "\n";
            }
        }

        Debug.Log(output);
    }

    public static string FetchField(string category, string fieldName)
    {
        Category cat = database.Find(x => x.name == category);
        if (cat != default(Category))
        {
            if (cat.fields.ContainsKey(fieldName))
            {
                return cat.fields[fieldName];
            }
        }

        return "";
    }

    public static string[][] FetchCategory(string category)
    {
        Category cat = database.Find(x => x.name == category);

        if (cat != null && cat.fields.Count > 0)
        {
            string[][] data = new string[cat.fields.Keys.Count][];

            int i = 0;
            foreach (string key in cat.fields.Keys)
            {
                data[i++] = new string[] { key, cat.fields[key] };
            }

            return data;
        }
        else
        {
            return null;
        }
    }

    public static void UpdateField(string category, string fieldName, string newValue)
    {
        // updating locally
        PushField(category, fieldName, newValue);

        // and sending update to server
        CMLData data = new CMLData();
        data.Set(fieldName, newValue);
        WUData.UpdateUserCategory(WULogin.UID, category, data);
    }

    public static void UpdateCategory(string category, string[][] fields)
    {
        CMLData data = new CMLData();
        foreach (string[] field in fields)
        {
            PushField(category, field[0], field[1]);
            data.Set(field[0], field[1]);
        }
        WUData.UpdateUserCategory(WULogin.UID, category, data);
    }

    private static IEnumerator CheckSession(float refreshTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshTime);
            WUData.FetchUserField(WULogin.UID, "SessionKey", "AccountStats", OnSessionCheckResponse);
        }
    }

    private static IEnumerator SetTime(float refreshTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshTime);

            string format = @"dd\:hh\:mm";
            string fetchedTimeSpan = FetchField("AccountStats", "TotalTime");
            TimeSpan currentTimeSpan = new TimeSpan(0, 0, 1, 0, 0);

            if (string.IsNullOrEmpty(fetchedTimeSpan))
            {
                formattedTimeSpan = currentTimeSpan.ToString(format);
            }
            else
            {
                currentTimeSpan += ConvertTimeFormat(fetchedTimeSpan, format);
                formattedTimeSpan = currentTimeSpan.ToString(format);
            }

            UpdateField("AccountStats", "TotalTime", formattedTimeSpan);
        }
    }

    private static void OnSessionCheckResponse(CML response)
    {
        string dbSessionKey = response[1].String("SessionKey");
        if (sessionKey != dbSessionKey)
        {
            Debug.LogWarning("Different session detected, logging out.");
            sessionTimeOut = true;
            WULogin.LogOut();
        }
    }

    private static TimeSpan ConvertTimeFormat(string fetchedTimeSpan, string format)
    {
        if (TimeSpan.TryParseExact(fetchedTimeSpan, format, null, TimeSpanStyles.None, out TimeSpan convertedTimeSpan))
        {
            return convertedTimeSpan;
        }

        return TimeSpan.Zero;
    }
}
