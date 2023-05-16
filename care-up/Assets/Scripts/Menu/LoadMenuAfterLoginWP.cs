using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBS;
using UnityEngine.Networking;

public class LoadMenuAfterLoginWP : MonoBehaviour {

    static bool done = false;

    void Start()
    {
        if (!done)
        {
            WULogin.onLoggedIn += InitDatabase;
            WULogin.onLoggedIn += SetProperCookiesWebGLWrap;
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

    public void SetProperCookiesWebGLWrap(CML ignore)
    {
        Debug.Log("Cookie::SetProperCookiesWebGLWrap()");
        // why webgl? because webGL doesnt allow to set custom cookies from client
        // so we're doing this hack to pass the data we need and set cookie on server side
        StartCoroutine(SetProperCookiesWebGL());
    }

    public IEnumerator SetProperCookiesWebGL()
    {
        Debug.Log("Cookie::SetProperCookiesWebGL()");
        //prep data
        int equalsIndex = WUCookie.CookieVal.IndexOf('=');
        int semicolonIndex = WUCookie.CookieVal.IndexOf(';');

        string cookie_name = WUCookie.CookieVal.Substring(0, equalsIndex);
        string cookie_value = WUCookie.CookieVal.Substring(equalsIndex + 1, semicolonIndex - equalsIndex - 1);

        string data = string.Format("{{\"cookie_name\":\"{0}\",\"cookie_value\":\"{1}\"}}", cookie_name, cookie_value);

        // send data
        string _uri = "https://careup.online/wp-json/cookies/set";
        UnityWebRequest w = UnityWebRequest.Put(_uri, data);

        w.SetRequestHeader("Content-Type", "application/json");

        yield return w.SendWebRequest();

        if (w.result != UnityWebRequest.Result.Success)
        {
            if (w.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Cookie HTTP Error: " + w.responseCode);
            }
            else if (w.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Cookie Network Error: " + w.error);
            }
        } else Debug.Log("Cookie Set Success");
    }
}
