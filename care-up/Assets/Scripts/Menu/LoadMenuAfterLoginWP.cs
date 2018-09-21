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
            done = true;
            Debug.Log("done " + done);
        }
    }
 

    void CheckForSerials(CML ignore)

    {

        if(WULogin.RequireSerialForLogin && !WULogin.HasSerial)
        {
            StatusMessage.Message ="Je hebt geen actief product";
            //DisplayScreen( panels.login_screen );

        } 
        else
        {
            bl_SceneLoaderUtils.GetLoader.LoadLevel("UMenuPro");
        }
            

    }
    /*void LoadMenuScene( CML response )
    {
        Debug.Log("Login was successful. Loading UMenuPro scene.");

    }*/
}
