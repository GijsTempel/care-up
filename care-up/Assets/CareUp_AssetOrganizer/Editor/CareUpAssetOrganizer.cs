using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.AddressableAssets;


public class EditorUtilityDisplayProgressBar : EditorWindow
{
    public float secs = 10f;
    public float startVal = 0f;
    public float progress = 0f;
    [MenuItem("Tools/Progress Bar Usage")]
    static void Init()
    {
        UnityEditor.EditorWindow window = GetWindow(typeof(EditorUtilityDisplayProgressBar));
        window.Show();
    }

    void OnGUI()
    {
        secs = EditorGUILayout.FloatField("Time to wait:", secs);
        if (GUILayout.Button("Display bar"))
        {
            if (secs < 1)
            {
                Debug.LogError("Seconds should be bigger than 1");
                return;
            }
            startVal = (float)EditorApplication.timeSinceStartup;
        }
        if (progress < secs)
            EditorUtility.DisplayProgressBar("Simple Progress Bar", "Shows a progress bar for the given seconds", progress / secs);
        else
            EditorUtility.ClearProgressBar();
        progress = (float)(EditorApplication.timeSinceStartup - startVal);
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}


public class CareUpAssetOrganizer : EditorWindow
{
    public static int itemsToProsess = 1;
    public static int itemsProsessed = 1;

    static string ListOfScenes = "BundleBuilderScenes";
    static Dictionary<string, List<string>> scenesData = new Dictionary<string, List<string>>();

    [MenuItem("Tools/Organize Assets in Bundles")]
    static void Init()
    {
        UnityEditor.EditorWindow window = GetWindow(typeof(CareUpAssetOrganizer));
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Start Process"))
        {
            itemsToProsess = 1;
            itemsProsessed = 0;

            List<string> _resources = new List<string>();
            scenesData.Clear();
            List<string> scenes = LoadScenesList();
            var scenesFolder = "Assets/Scenes/";
            List<string> matExt = new List<string> { ".mat", ".jpg", ".fbx", ".png", ".exr", ".ogg", ".prefab", ".ttf",
                ".wav", ".controller", ".anim", ".otf", ".mask", ".shader", ".psd", ".mp3", ".asset", ".tif", ".tga", ".tiff", ".jpeg",
                ".mesh", ".exr", ".renderTexture"};
            int i = 0;
            itemsToProsess = scenes.Count;
            foreach (string scene in scenes)
            {
                string scenePath = scenesFolder + scene + ".unity";

                Object sceneObject = AssetDatabase.LoadAssetAtPath(scenePath, typeof(SceneAsset));
                if (sceneObject == null)
                    continue;
                if (!scenesData.ContainsKey(scene))
                    scenesData.Add(scene, new List<string>());


                //AssetImporter.GetAtPath(scenePath).assetBundleName = "scene/" + scene.ToLower().Replace(' ', '_');

                AddAssetToGroup(scenePath, "scene-" + scene.ToLower().Replace(' ', '_'));

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

                i++;
                itemsProsessed++;
                EditorUtility.DisplayProgressBar("Progress", "Processing scenes", (float)itemsProsessed / (float)itemsToProsess);
            }
            Dictionary<string, string> _resCont = new Dictionary<string, string>();
            List<string> bundleNames = new List<string>();
            itemsToProsess = _resources.Count;
            itemsProsessed = 0;
            foreach (string res in _resources)
            {
                string resContainerName = GetContainerName(res);
                if (!bundleNames.Contains(resContainerName))
                    bundleNames.Add(resContainerName);
                _resCont.Add(res, resContainerName);
                AddAssetToGroup(res, "asset-" + resContainerName);
                //AssetImporter.GetAtPath(res).assetBundleName = "asset/" + resContainerName;
                itemsProsessed++;
                string titleMessage = "Progressed " + itemsProsessed.ToString() + " of " + itemsToProsess.ToString();
                EditorUtility.DisplayProgressBar(titleMessage, "Processing assets | " + res, (float)itemsProsessed / (float)itemsToProsess);
            }
            Debug.Log("Finished");
        }
            
        EditorUtility.ClearProgressBar();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    public static void AddAssetToGroup(string path, string groupName)
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        var group = settings.FindGroup(groupName);
        if (!group)
        {
            group = settings.CreateGroup(groupName, false, false, false, new List<UnityEditor.AddressableAssets.Settings.AddressableAssetGroupSchema>
            { settings.DefaultGroup.Schemas[0] });
        }
        var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), group, false, true);

        if (entry == null)
        {
            Debug.Log($"Addressable : can't add {path} to group {groupName}");
        }
        else
        {
            if (groupName.Substring(0, 5) == "scene")
            {
                entry.address = path;
            }
            else
            {
                entry.address = path.ToLower().Replace("resources_moved", "resources");
            }
        }
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
