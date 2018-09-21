using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBS;

public class LoadStartSceneAfterLogoutWP : MonoBehaviour {
    
	void Start () {
        WULogin.onLoggedOut += LoadStartScene;

    }

    void LoadStartScene(CML response)
    {
        Debug.Log("Logout was successful. Loading first scene.");
        bl_SceneLoaderUtils.GetLoader.LoadLevel("LoginMenu");
    }
}
