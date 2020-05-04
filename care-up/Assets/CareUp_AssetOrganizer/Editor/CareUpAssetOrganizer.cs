using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CareUpAssetOrganizer : EditorWindow
{
    static string ListOfScenes = "BundleBuilderScenes";
    static Dictionary<string, List<string>> scenesData = new Dictionary<string, List<string>>();

    [MenuItem("Tools/Organize Assets in Bundles")]
    static void Init()
    {
        List<string> _resources = new List<string>();
        scenesData.Clear();
        List<string> scenes = LoadScenesList();
        var scenesFolder = "Assets/Scenes/";
        List<string> matExt = new List<string> { ".mat", ".jpg", ".fbx", ".png", ".exr", ".ogg", ".prefab", ".ttf",
        ".wav", ".controller", ".anim", ".otf", ".mask", ".shader", ".psd", ".mp3", ".asset", ".tif", ".tga", ".tiff", ".jpeg", 
        ".mesh", ".exr", ".renderTexture"};
        int i = 0;
        foreach (string scene in scenes)
        {
            if (!scenesData.ContainsKey(scene))
                scenesData.Add(scene, new List<string>());

            string scenePath = scenesFolder + scene + ".unity";

            AssetImporter.GetAtPath(scenePath).assetBundleName = "scene/" + scene.ToLower().Replace(' ', '_');
            string sceneBundleName = AssetImporter.GetAtPath(scenePath).assetBundleName;
            if (sceneBundleName != "")
            {
                string[] dep = AssetDatabase.GetDependencies(scenePath);
                foreach (string d in dep)
                {
                    if (matExt.Contains(Path.GetExtension(d.ToLower())))
                    {
                        if (!_resources.Contains(d))
                            _resources.Add(d);

                        scenesData[scene].Add(d);
                    }
                }
            }
            i++;
        }
        Dictionary<string, string> _resCont = new Dictionary<string, string>();
        List<string> bundleNames = new List<string>();
        foreach(string res in _resources)
        {
            string resContainerName = GetContainerName(res);
            if (!bundleNames.Contains(resContainerName))
                bundleNames.Add(resContainerName);
            _resCont.Add(res, resContainerName);
            AssetImporter.GetAtPath(res).assetBundleName = "asset/" + resContainerName;
        }
        Debug.Log("Finished");
    }

    static string IntToCode(int value)
    {
        string simbols = "abcdefghijklmnopqrstuvwxyz";
        int lNum = (int)(value / 10);
        int dNum = value % 10;
        string _code = simbols[lNum] + dNum.ToString();
        return _code;
    }

    static string GetContainerName(string resourcePath)
    {
        string containerName = "";
        int i = 0;
        foreach(string scene in scenesData.Keys)
        {
            if (scenesData[scene].Contains(resourcePath))
            {
                containerName += IntToCode(i);
            }
            i++;
        }

        return containerName;
    }

    static List<string> LoadScenesList()
    {
        List<string> scenes = new List<string>();
        TextAsset dictListData = (TextAsset)Resources.Load(ListOfScenes);
        foreach (string dictName in dictListData.text.Split('\n'))
        {
            if (!string.IsNullOrEmpty(dictName))
            {
                scenes.Add(dictName.Replace("\r", ""));
            }
        }
        return scenes;
    }
}
