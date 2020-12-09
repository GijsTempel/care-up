using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.AddressableAssets;
using UnityEditor.SceneManagement;
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
        if (GUILayout.Button("Select Dependency Prefabs"))
        {
            string scenePath = EditorSceneManager.GetActiveScene().path;
            string[] dep = AssetDatabase.GetDependencies(scenePath);
            GameObject prefabHolder = GameObject.Find("PrefabHolder");
            if (prefabHolder == null)
            {
                GameObject holder = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/Resources/NecessaryPrefabs/PrefabHolder.prefab");
                prefabHolder = Instantiate(holder) as GameObject;
                prefabHolder.name = "PrefabHolder";
            }
            if (prefabHolder != null)
            {
                foreach (string d in dep)
                {
                    if (Path.GetExtension(d.ToLower()) == ".prefab" &&
                        d.Split('/')[2] == "Prefabs")
                    {
                        GameObject prefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(d);
                        if (prefab.GetComponent<PickableObject>() != null)
                        {
                            GameObject prefabInst = Instantiate(prefab, prefabHolder.transform) as GameObject;
                            prefabInst.name = Path.GetFileNameWithoutExtension(d);
                        }
                    }
                }
            }


        }
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
                string groupName = "asset-" + (Mathf.Abs(resContainerName.GetHashCode())).ToString();
                AddAssetToGroup(res, groupName);
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
            { settings.DefaultGroup.Schemas[1] });
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
                if (dictName.Substring(0, 1) != "#")
                {
                    scenes.Add(dictName.Replace("\r", ""));
                }
            }
        }
        return scenes;
    }
}
