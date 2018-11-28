using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using System.Timers;
using System.Threading;

public class ScreenshotMailer : MonoBehaviour {

    public static string result = "";
    static ScreenshotMailer Instance;

    static ScreenshotMailerSettings settings;

    byte[] mostRecentScreenshot;

    public static void CaptureScreenshot() {
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            Debug.LogWarning("Screenshot Mailer requires an internet connection!");
            return;
        }

        if (Instance == null) {
            GameObject go = new GameObject();
            ScreenshotMailer comp = go.AddComponent<ScreenshotMailer>();
            Instance = comp;
            go.name = "_screenshotMailer";
        }
        Instance.CaptureScreenshotNonStatic();
    }

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }

    public void CaptureScreenshotNonStatic() {

        if (settings == null) {
            UnityEngine.Object result = Resources.Load("ScreenshotMailerSettings.asset");
            if (result == null) {
                ScreenshotMailerSettings[] results = Resources.FindObjectsOfTypeAll<ScreenshotMailerSettings>();
                if (results.Length >= 1) {
                    settings = results[0];
                }
            } else {
                settings = (ScreenshotMailerSettings)result;
            }
        }

        
        if (settings == null || string.IsNullOrEmpty(settings.EmailAddress)) {
            Debug.LogWarning("Screenshotter requires an email address to send the screenshots to! This can be done in Window/Screenshotter Settings");
            Destroy(this.gameObject);
            return;
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.WebGLPlayer) {
            DoScreenshotWithReadPixels();
            StartCoroutine(SendScreenshotToServerUsingWWW(mostRecentScreenshot));
        } else {
            StartCoroutine(DoScreenshotWithCaptureScreenshot());
        }
    }

    IEnumerator SendScreenshotToServerUsingWWW(byte[] screenshot) {
        yield return new WaitForSeconds(0.5f);
        WWWForm form = new WWWForm();
        form.AddField("image", System.Convert.ToBase64String(mostRecentScreenshot));
        form.AddField("emailaddress", settings.EmailAddress);
        
        WWW www = new WWW("http://marijnzwemmer.com/screenshotmailer/SendScreenshotEmail.php", form.data);
        yield return www;
        Debug.Log("Screenshot Mailer: " + www.text);
        result = www.text.Substring(1, www.text.Length - 1);
        Destroy(this.gameObject);
        yield return null;
    }

    IEnumerator DoScreenshotWithCaptureScreenshot() {
        string fileName = Application.persistentDataPath + "/temp_screenshot" + UnityEngine.Random.Range(1, 100000) + ".png";
        ScreenCapture.CaptureScreenshot(fileName, settings.SuperSize);
        yield return new WaitForSeconds(1.5f);
        mostRecentScreenshot = GetBytesFromFile(fileName);
        File.Delete(fileName);
        StartCoroutine(SendScreenshotToServerUsingWWW(mostRecentScreenshot));
    }

    void DoScreenshotWithReadPixels() {
        RenderTexture rt = new RenderTexture(Screen.width * settings.SuperSize, Screen.height * settings.SuperSize, 24);
        Camera.main.targetTexture = rt;
        Texture2D screenShot = new Texture2D(Screen.width * settings.SuperSize, Screen.height * settings.SuperSize, TextureFormat.RGB24, false);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width * settings.SuperSize, Screen.height * settings.SuperSize), 0, 0);
        Camera.main.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        mostRecentScreenshot =  screenShot.EncodeToPNG();
    }

    static byte[] GetBytesFromFile(string fullFilePath) {
        FileStream fs = null;
        try {
            fs = File.OpenRead(fullFilePath);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
            return bytes;
        } finally {
            if (fs != null) {
                fs.Close();
                fs.Dispose();
            }
        }
    }
}
