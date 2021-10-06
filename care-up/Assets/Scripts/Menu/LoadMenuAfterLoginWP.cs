using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBS;

public class LoadMenuAfterLoginWP : MonoBehaviour {

    static bool done = false;

    void Start()
    {
        if (!done)
        {
            WULogin.onLoggedIn += InitDatabase;
            WULogin.onLoggedOut += CleanDatabase;
            WULogin.onLoggedOut += LoadStartScene;
            done = true;
        }
    }

    public void InitDatabase(CML ignore)
    {
        DatabaseManager.Init();
        WULogin.justLoggedOff = false;
    }

    public void CleanDatabase(CML ignore)
    {
        DatabaseManager.Clean();
    }
    
    void LoadStartScene(CML response)
    {
        //Debug.Log("Uitloggen is gelukt. Je keert nu terug naar het login scherm.");
        bl_SceneLoaderUtils.GetLoader.LoadLevel("LoginMenu");
    }

    /// <summary>
    /// This function will wipe account data.
    /// Do not launch this function unless you know what you are doing.
    /// </summary>
    /// <param name="ignore"></param>
    void ClearFields(CML ignore)
    {
        WUData.RemoveCategory("AccountStats");
    }
}
