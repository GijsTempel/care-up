using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour
{

    /// <summary>
    /// License validate state
    /// </summary>
    bool ok_LicenseVerify;


    void Start()
    {
        // Initializing styles
        InitStyles();

        //
        // Verify curent serial number
        //
        ok_LicenseVerify = DevXUnityTools.SerialNumberValidateTools.Verify();
    }



    // Update is called once per frame
    void Update ()
    {

    }

    string eMail="";

    /// <summary>
    /// Show GUI
    /// </summary>
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width/2-500/2, 50, 500, Screen.height));
        GUILayout.BeginVertical(st_group, GUILayout.Width(500));


        GUILayout.Label("Current date: "+System.DateTime.Now.ToString("yyyy.MM.dd"));

        GUILayout.Label("HardwareID"); GUILayout.TextField(DevXUnityTools.SerialNumberValidateTools.HardwareID);

        GUILayout.Label("eMail"); eMail=GUILayout.TextField(eMail);

        GUILayout.Space(10);
        
        if(ok_LicenseVerify==false)
        {
            if(GUILayout.Button("Send license email request"))
            {

                GUI.FocusControl("");

                //
                // Show email client for license request
                //
                System.Diagnostics.Process.Start(("mailto:"+ eMail + "?subject=" +"Serial number request"
                    + "&body="
                    + "ProductName=" + "License test"+ "%0A"
                    + "HardwareID=" + DevXUnityTools.SerialNumberValidateTools.HardwareID + "%0A"
                    + "eMail=" + eMail + "%0A"
                    )
                );
            }
        }


        GUILayout.Label("Enter serial number key:");
        string res=GUILayout.TextArea(DevXUnityTools.SerialNumberValidateTools.SerialNumberKey);
        if (res != DevXUnityTools.SerialNumberValidateTools.SerialNumberKey)
        {
            //
            // Save new user serial number key
            //
            DevXUnityTools.SerialNumberValidateTools.SerialNumberKey = res;

            // Verify new user license key
            ok_LicenseVerify = DevXUnityTools.SerialNumberValidateTools.Verify();
        }

        GUILayout.Space(10);

   
        GUILayout.BeginVertical(
            ok_LicenseVerify?
                st_group_green: // valid
                st_group_red,   // not valid
            GUILayout.Width(500));

        GUILayout.Space(10);

        var old1 = GUI.backgroundColor;
        if (ok_LicenseVerify)
        {
            GUILayout.Label("License - Valid");
        }
        else
        {
            GUILayout.Label("License - NOT VALID!" );
        }
        GUI.backgroundColor = old1;

        GUILayout.Space(10);
        GUILayout.EndVertical();
        GUILayout.Space(10);
        GUILayout.EndVertical();

        GUILayout.EndArea();
    }



    #region InitStyles
    /// <summary>
    /// Style valiables
    /// </summary>
    GUIStyle st_group;
    GUIStyle st_group_red;
    GUIStyle st_group_green;

    /// <summary>
    /// Initializing styles
    /// </summary>
    void InitStyles()
    {
        // Load end decript images

        st_group = new GUIStyle();
        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.3f));
        texture.Apply();
        st_group.normal.background = texture;


        st_group_red = new GUIStyle();
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(1f, 0f, 0f, 0.3f));
        texture.Apply();
        st_group_red.normal.background = texture;

        st_group_green = new GUIStyle();
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(0f, 1f, 0f, 0.3f));
        texture.Apply();
        st_group_green.normal.background = texture;
    }
    #endregion
}
