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
            WULogin.onLoggedIn += CheckForSerials;
            WULogin.onLoggedOut += LoadStartScene;
            done = true;
        }
    }
 
    void CheckForSerials(CML ignore)
    {
        #if WUSKU
        if(WULogin.RequireSerialForLogin && !WULogin.HasSerial)
        {
            StatusMessage.Message ="Je hebt geen actief product";
            //DisplayScreen( panels.login_screen );

        } 
        else
        {
            bl_SceneLoaderUtils.GetLoader.LoadLevel("UMenuPro");
        }
        #endif
    }

    void LoadStartScene(CML response)
    {
        Debug.Log("Logout was successful. Loading first scene.");
        bl_SceneLoaderUtils.GetLoader.LoadLevel("LoginMenu");
    }
}
