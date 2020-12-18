using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.AddressableAssets;
using UnityEditor.SceneManagement;

    public class CareUpAssetOrganizer : EditorWindow
{
    public static int itemsToProcess = 1;
    public static int itemsProcessed = 1;

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
        if (GUILayout.Button("Set prefabs to Object Layer"))
        {
            string[] __prefabs = Directory.GetFiles("Assets/Resources_moved/Prefabs/");
            foreach(string p in __prefabs)
            {
                if (Path.GetExtension(p.ToLower()) == ".prefab")
                {
                    GameObject __prefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(p);
                    if (__prefab.GetComponent<PickableObject>() != null)
                    {
                        PrefabUtility.InstantiatePrefab(__prefab);
                        Debug.Log(p);
                    }
                }
            }
        }

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
                            GameObject prefabInst = PrefabUtility.InstantiatePrefab(prefab, prefabHolder.transform) as GameObject;
                            //GameObject prefabInst = Instantiate(prefab, prefabHolder.transform) as GameObject;
                            prefabInst.name = Path.GetFileNameWithoutExtension(d);
                        }
                    }
                }
            }


        }
        if (GUILayout.Button("++Start Creating Addressable Groups++"))
        {
            itemsToProcess = 1;
            itemsProcessed = 0;

            List<string> _resources = new List<string>();
            scenesData.Clear();
            List<string> scenes = LoadScenesList();
            var scenesFolder = "Assets/Scenes/";
            List<string> matExt = new List<string> { ".mat", ".jpg", ".fbx", ".png", ".exr", ".ogg", ".prefab", ".ttf",
                ".wav", ".controller", ".anim", ".otf", ".mask", ".shader", ".psd", ".mp3", ".asset", ".tif", ".tga", ".tiff", ".jpeg",
                ".mesh", ".exr", ".renderTexture"};
            int i = 0;
            itemsToProcess = scenes.Count;
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
                itemsProcessed++;
                EditorUtility.DisplayProgressBar("Progress", "Processing scenes", (float)itemsProcessed / (float)itemsToProcess);
            }
            Dictionary<string, string> _resCont = new Dictionary<string, string>();
            List<string> bundleNames = new List<string>();
            itemsToProcess = _resources.Count;
            itemsProcessed = 0;
            foreach (string res in _resources)
            {
                string resContainerName = GetContainerName(res);
                if (!bundleNames.Contains(resContainerName))
                    bundleNames.Add(resContainerName);
                _resCont.Add(res, resContainerName);
                string groupName = "asset-" + (Mathf.Abs(resContainerName.GetHashCode())).ToString();
                AddAssetToGroup(res, groupName);
                itemsProcessed++;
                string titleMessage = "Progressed " + itemsProcessed.ToString() + " of " + itemsToProcess.ToString();
                EditorUtility.DisplayProgressBar(titleMessage, "Processing assets | " + res, (float)itemsProcessed / (float)itemsToProcess);
            }
            Debug.Log("Finished");
        }
        EditorUtility.ClearProgressBar();
        if (GUILayout.Button("Remove Empty Addressable Groups"))
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var groups = settings.groups;
            List<UnityEditor.AddressableAssets.Settings.AddressableAssetGroup> groupsToDelete = 
                new List<UnityEditor.AddressableAssets.Settings.AddressableAssetGroup>();
            foreach(var g in groups)
            {
                Debug.Log(g.name + " " + g.entries.Count.ToString());
                if (g.entries.Count == 0)
                    groupsToDelete.Add(g);
            }
            foreach(var g in groupsToDelete)
            {
                settings.RemoveGroup(g);
            }
        }
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
            group = settings.CreateGroup(groupName, false, false, true, new List<UnityEditor.AddressableAssets.Settings.AddressableAssetGroupSchema>
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
