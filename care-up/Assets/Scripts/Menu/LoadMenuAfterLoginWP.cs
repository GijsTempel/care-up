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
        if (WULogin.RequireSerialForLogin && !WULogin.HasSerial)
        {
            IAPManager manager = GameObject.FindObjectOfType<IAPManager>();
            if (manager.SubscriptionPurchased())
            {
                bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
            }
            else
            {
                StatusMessage.Message = "Je hebt geen actief product";
            }
        }
        else
        {
            bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
        }
#endif
    }

    void LoadStartScene(CML response)
    {
        Debug.Log("Uitloggen is gelukt. Je keert nu terug naar het login scherm.");
        bl_SceneLoaderUtils.GetLoader.LoadLevel("LoginMenu");
    }
}
