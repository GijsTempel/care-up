using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
class WebGLMemorySetup 
{
    static WebGLMemorySetup()
    {
        PlayerSettings.WebGL.memorySize = 800;
    }
}
