using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEditor.IMGUI.Controls;

public class QuickCareUpTools
{
    [MenuItem("Tools/Start Action %#x")]
    private static void SetTriggers()
    {
        ActionStarter actionStarter = GameObject.FindObjectOfType<ActionStarter>();
        if (actionStarter != null)
        {
            actionStarter.StartAction();
        } 
    }
    [MenuItem("Tools/Quck Jump to..")]
    public static void SelectWindowsPackageItems()
    {
        string[] paths = { "Assets/editor/QuickCareUpTools.cs" };
        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(paths[0]);
    }

}




namespace CareUpBookmarks
{

    struct Bookmark
    {
        public string filePath;

        public Bookmark(string _path)
        {
            filePath = _path;
        }

        public void Save(int slot)
        {
            var key = GetKey(slot);
            var json = JsonUtility.ToJson(this);
            EditorPrefs.SetString(key, json);
        }

        public static bool Exists(int slot)
        {
            var key = GetKey(slot);
            return EditorPrefs.HasKey(key);
        }

        public static Bookmark Read(int slot)
        {
            var key = GetKey(slot);
            var json = EditorPrefs.GetString(key);
            return JsonUtility.FromJson<Bookmark>(json);
        }

        static string GetKey(int slot)
        {
            return "CareUpBookmark" + slot;
        }
    }


    public class BookmarkEditor : EditorWindow

    {
        [MenuItem("Tools/Asset Bookmarks")]
        static void ShowWindow()
        {
            var window = GetWindow<BookmarkEditor>();
            window.titleContent = new GUIContent("Asset Bookmark Editor");
            window.Show();
        }

        void OnGUI()
        {
            for (int i = 0; i < 20; i++)
            {
                if (Bookmark.Exists(i))
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Go", GUILayout.Width(50)))
                    {
                        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(Bookmark.Read(i).filePath);
                    }
                    string[] label = Bookmark.Read(i).filePath.Split('/');
                    GUILayout.Label(label[label.Length - 1]);

                    if (GUILayout.Button("X", GUILayout.Width(50)))
                    {
                        EditorPrefs.DeleteKey("CareUpBookmark" + i);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            if (GUILayout.Button("add")) {

                for (int i = 0; i < 20; i++)
                {
                    if (!(Bookmark.Exists(i)))
                    {
                        Bookmark b = new Bookmark(AssetDatabase.GetAssetPath(Selection.activeObject));
                        b.Save(i);
                        break;
                    }
                }
            }
        }
    }
}