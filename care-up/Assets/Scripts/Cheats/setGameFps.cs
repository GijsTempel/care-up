using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setGameFps : MonoBehaviour
{
    public int frameRate;
    public int _getTotalReservedMemory;
    // Start is called before the first frame update
    void Start()
    {
#if (UNITY_EDITOR)
        if (frameRate !=0 || frameRate>0)
        {
            Application.targetFrameRate = frameRate;
        }
#endif
        _getTotalReservedMemory = Convert.ToInt32(UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong()/ 1000000f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}