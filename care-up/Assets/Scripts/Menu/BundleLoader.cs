using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundles;
using UnityEngine.UI;


public class BundleLoader : MonoBehaviour {

    string sceneAssetBundle;
    string sceneName;
    
    public IEnumerator Load(string scene, string bundle)
    {
        sceneAssetBundle = bundle;
        sceneName = scene;

        yield return StartCoroutine(Initialize());

        // Load level.
        yield return StartCoroutine(InitializeLevelAsync(sceneName, false));
    }


    public void ClearLoader()
    {
        AssetBundleManager.ClearLoader();
    }

    // Initialize the downloading url and AssetBundleManifest object.
    protected IEnumerator Initialize()
    {
       
        #if DEVELOPMENT_BUILD || UNITY_EDITOR
        AssetBundleManager.SetDevelopmentAssetBundleServer();
#else
        // Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
        //AssetBundleManager.SetSourceAssetBundleURL(Application.dataPath + "/");

		// Or customize the URL based on your deployment or configuration
		AssetBundleManager.SetSourceAssetBundleURL("https://leren.careup.online/AssetBundles/" + Application.version + "/");
#endif
        if (GameObject.Find("url_test") != null)
        {
            if (GameObject.Find("url_test").GetComponent<InputField>().text != "")
            {

                AssetBundleManager.SetSourceAssetBundleURL(GameObject.Find("url_test").GetComponent<InputField>().text);
                print("____++++++++++++++++++_______  " + GameObject.Find("url_test").GetComponent<InputField>().text);
            }
        }
  
        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
        var request = AssetBundleManager.Initialize();

        if (request != null)
            yield return StartCoroutine(request);
    }

    protected IEnumerator InitializeLevelAsync(string levelName, bool isAdditive)
    {
        // This is simply to get the elapsed time for this phase of AssetLoading.
        float startTime = Time.realtimeSinceStartup;

        // Load level from assetBundle.
        AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(sceneAssetBundle, levelName, isAdditive);
        if (request == null)
            yield break;
        yield return StartCoroutine(request);

        // Calculate and display the elapsed time. // never used now
        //float elapsedTime = Time.realtimeSinceStartup - startTime;
        //print(elapsedTime);
    }
}
