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
            WULogin.onLoggedIn += LoadMenuScene;
            done = true;
            Debug.Log("done " + done);
        }
    }

    void LoadMenuScene( CML response )
    {
        Debug.Log("Login was successful. Loading UMenuPro scene.");
        bl_SceneLoaderUtils.GetLoader.LoadLevel("UMenuPro");
    }
}
