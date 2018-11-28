using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

class ScreenshotMailerSettingsWindow : EditorWindow {

    ScreenshotMailerSettings settings;

    string emailAddress;
    int superSize;

    [MenuItem("Window/ScreenshotMailer Settings")]
    public static void ShowWindow() {
        EditorWindow window = EditorWindow.GetWindow(typeof(ScreenshotMailerSettingsWindow));
        window.position = new Rect(100, 100, 328, 180);
    }

    void OnEnable() {
        ScreenshotMailerSettings[] results = Resources.FindObjectsOfTypeAll<ScreenshotMailerSettings>();
        if (results.Length == 0) {
            settings = ScriptableObject.CreateInstance<ScreenshotMailerSettings>();
        } else {
            settings = results[0];
        }
        emailAddress = settings.EmailAddress;
        superSize = settings.SuperSize;
    }

    void OnGUI() {
        GUILayout.Label("ScreenshotMailer Settings", EditorStyles.boldLabel);
        GUILayout.Space(10);
        GUILayout.Label("Email address to send the screenshots to");
        emailAddress = GUILayout.TextField(emailAddress, GUILayout.Width(320));
        GUILayout.Space(5);
        GUILayout.Label("The scale of the created screenshots, 1 is default");

        GUILayout.BeginHorizontal();
        superSize = (int)GUILayout.HorizontalSlider(superSize, 1, 4, GUILayout.Width(100));
        GUILayout.Label("" + superSize);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        if (GUILayout.Button("Save", GUILayout.Width(320), GUILayout.Height(40))) {
            if (!Directory.Exists("Assets/ScreenshotMailer/Resources/")) {
                Directory.CreateDirectory("Assets/ScreenshotMailer/Resources/");
            }
            settings.EmailAddress = emailAddress;
            settings.SuperSize = superSize;
            if (!AssetDatabase.Contains(settings)) {
                AssetDatabase.CreateAsset(settings, "Assets/ScreenshotMailer/Resources/ScreenshotMailerSettings.asset");
                UnityEditor.Selection.activeObject = settings;
            }

            AssetDatabase.SaveAssets();

            Debug.Log("Screenshotter settings saved.");

            this.Close();
        }
    }
}