using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitchtest : MonoBehaviour
{
    public bool bbb = false;
    bool prevBBB = false;

    // Start is called before the first frame update
    void Start()
    {
        prevBBB = bbb;
    }

    // Update is called once per frame
    void Update()
    {
        if (bbb != prevBBB)
        {
            prevBBB = bbb;
            if (bbb)
                GameObject.FindObjectOfType<PlayerScript>().SwitchCamera("PanoFlyCamera");
            else
                GameObject.FindObjectOfType<PlayerScript>().SwitchCamera("");
        }

    }
}
