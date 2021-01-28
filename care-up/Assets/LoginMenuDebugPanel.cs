using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoginMenuDebugPanel : MonoBehaviour
{
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
