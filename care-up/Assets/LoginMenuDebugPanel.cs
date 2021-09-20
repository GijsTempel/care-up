using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class LoginMenuDebugPanel : MonoBehaviour
{

    public Text TestOutputComponentText;
    bool _visible = false;
    public void Toggle()
    {
        _visible = !_visible;
        if (_visible)
            GetComponent<Animator>().SetTrigger("show");
        else
            GetComponent<Animator>().SetTrigger("hide");

    }
}
