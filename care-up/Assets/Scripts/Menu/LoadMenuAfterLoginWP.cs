using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBS;

public class LoadMenuAfterLoginWP : MonoBehaviour {
    
	void Start ()
    {
        WULogin.onLoggedIn += LoadMenuScene;
	}

    void LoadMenuScene( CML response )
    {
        Debug.Log("Login was successful. Loading UMenuPro scene.");
        bl_SceneLoaderUtils.GetLoader.LoadLevel("UMenuPro");
    }
}
