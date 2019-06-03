using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBS;

public class DatabaseManager : MonoBehaviour {

    public class Category
    {
        public string name;
        public Dictionary<string, string> fields;

        public Category() { name = ""; fields = new Dictionary<string, string>(); }
        public Category(string _name) : this() { name = _name; }
    };

    private static DatabaseManager instance;
    private static List<Category> database = new List<Category>();

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
    }

    public static void Init()
    {
        // querry all player info here?
        WUData.FetchUserGameInfo(WULogin.UID, FetchEverything_success);
    }

    private static void PostInit()
    {
        PrintDatabase();

        // increment logins number
        int loginNumber;
        int.TryParse(FetchField("AccountStats", "Login_Number"), out loginNumber);
        UpdateField("AccountStats", "Login_Number", (loginNumber + 1).ToString());

        // set up plays number (not sure if needed anymore)
        string plays = FetchField("AccountStats", "Plays_Number");
        int.TryParse(plays, out GameObject.FindObjectOfType<PlayerPrefsManager>().plays);

        // set sub status
        GameObject.FindObjectOfType<PlayerPrefsManager>().subscribed = 
            WULogin.HasSerial || (WULogin.RequireSerialForLogin == false);

        // set character info
        CharacterInfo.sex = FetchField("AccountStats", "CharacterSex");
        int.TryParse(FetchField("AccountStats", "CharacterHeadType"), out CharacterInfo.headType);
        int.TryParse(FetchField("AccountStats", "CharacterBodyType"), out CharacterInfo.bodyType);
        int.TryParse(FetchField("AccountStats", "CharacterGlassesType"), out CharacterInfo.glassesType);

        // check if character created, load proper scene
        // load scene at the end of this function
        if ( FetchField("AccountStats", "CharacterCreated") == "true" )
        {
            WULogin.characterCreated = true;
            bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
        }
        else
        {
            WULogin.characterCreated = false;
            bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Character_Customisation");
        }
    }

    private static void FetchEverything_success(CML response)
    {
        for (int i = 0; i < response.Elements.Count; ++i)
        {
            // data types: DATA, _CATEGORY_, _GAME_
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
                    case "woocommerce-login-nonce":
                    case "_wpnonce":
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
            database.Add(newCat);
        }
    }

    private static void PrintDatabase()
    {
        string output = "";
        foreach(Category c in database)
        {
            output += "Category: " + c.name + "\n";
            foreach(string key in c.fields.Keys)
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
        foreach(string[] field in fields)
        {
            PushField(category, field[0], field[1]);
            data.Set(field[0], field[1]);    
        }
        WUData.UpdateUserCategory(WULogin.UID, category, data);
    }
}
