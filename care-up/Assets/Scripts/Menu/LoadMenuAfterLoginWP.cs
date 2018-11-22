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
            // uncomment this to reset play counter for testing
            //WULogin.onLoggedIn += ClearFields;

            WULogin.onLoggedIn += AddNumberOfLogins;

            WULogin.onLoggedIn += CheckForSerials;
            WULogin.onLoggedOut += LoadStartScene;
            done = true;
        }
    }
 
    void CheckForSerials(CML ignore)
    {
#if WUSKU
        // serial is required for non-android version, so we check if has serial
        if (WULogin.RequireSerialForLogin && !WULogin.HasSerial)
        {
            // we're here either because this is mobile version or player has no serial
            // so let's check if player has purchased android/applestore subscription
            IAPManager manager = GameObject.FindObjectOfType<IAPManager>();
            if (manager != null && manager.SubscriptionPurchased())
            {
                AllowDenyLoadMainMenu(true);
            }
            else
            {
                // we're here only if player has no subscription at all
                // still allow to play for a bit, for the first few plays
                WUData.FetchField("Plays_Number", "AccountStats", GetPlaysNumber, -1, ErrorHandle);
            }
        }
        else
        {
            AllowDenyLoadMainMenu(true);
        }
        #endif
    }

    void GetPlaysNumber(CML response)
    {
        // we're here only if we got data
        PlayerPrefsManager manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
        manager.plays = response[1].Int("Plays_Number");
        Debug.Log("Logged in, loaded plays, current plays: " + manager.plays);
        bool result = manager.plays < 3 ? true : false;
        AllowDenyLoadMainMenu(result, true);
    }

    void ErrorHandle(CMLData response)
    {
        Debug.Log("Logged in, error getting plays");
        print(response.ToString());
        // we're here if we got error or no data which should be equal to 0 plays
        AllowDenyLoadMainMenu((response["message"] == "WPServer error: Empty response. No data found"), true);
    }

    void AllowDenyLoadMainMenu(bool allow, bool noSub = false)
    {
        if (allow)
        {
            GameObject.FindObjectOfType<PlayerPrefsManager>().subscribed = !noSub;
        }

        if (allow)
        {
            bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
        }
        else
        {
            StatusMessage.Message = "Je hebt geen actief product";
        }
    }

    void LoadStartScene(CML response)
    {
        Debug.Log("Uitloggen is gelukt. Je keert nu terug naar het login scherm.");
        bl_SceneLoaderUtils.GetLoader.LoadLevel("LoginMenu");
    }

    void ClearFields(CML ignore)
    {
        WUData.RemoveCategory("AccountStats");
    }

    void AddNumberOfLogins(CML ignore)
    {
        WUData.FetchField("Login_Number", "AccountStats", AddoneToLoginNumber, -1, LoginNumber_Error);
    }
    
    static void AddoneToLoginNumber(CML response)
    {
        int logins = response[1].Int("Login_Number") + 1;

        // update +1
        CMLData data = new CMLData();
        data.Set("Login_Number", logins.ToString());
        WUData.UpdateCategory("AccountStats", data);
    }

    static void LoginNumber_Error(CMLData response)
    {
        if ((response["message"] == "WPServer error: Empty response. No data found"))
        {
            // empty response, need to create field with 1 play
            CMLData data = new CMLData();
            data.Set("Login_Number", "1");
            WUData.UpdateCategory("AccountStats", data);
        }
    }
}
