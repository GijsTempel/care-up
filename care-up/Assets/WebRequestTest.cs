using UnityEngine;
using UnityEngine.Networking;
using System.Collections;



public class WebRequestTest : MonoBehaviour
{
    public class JsonHelper
    {
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }
    
        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
         
    [System.Serializable]
    public class SceteStoreData
    {
        public int product_id;
        public string product_name;
    }

    public void TestWebRequest()
    {
        StartCoroutine(GetRequest("https://ab.3dvit.in.ua/?user_id=1"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else if (webRequest.downloadHandler.text != "")
            {
                Debug.Log(webRequest.downloadHandler.text);
                
                SceteStoreData[] sceteStoreData; 
                sceteStoreData = JsonHelper.getJsonArray<SceteStoreData>(webRequest.downloadHandler.text); 
                foreach(SceteStoreData ssd in sceteStoreData)
                {
                    PlayerPrefsManager.AddSKU(ssd.product_name);
                }
            }
        }
    }
}