using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setGameFps : MonoBehaviour
{
    public int frameRate;
    // Start is called before the first frame update
    void Start()
    {
#if (UNITY_EDITOR)
        if (frameRate != null || frameRate !=0 || frameRate>0)
        {
            Application.targetFrameRate = frameRate;
        }
#endif

    }

    // Update is called once per frame
    void Update()
    {

    }
}