using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayer : MonoBehaviour
{
    private struct sceneData
    {
        public string sceneName;
        public string bundleName;
    }
    List<sceneData> AutoplayScenes = new List<sceneData>();

    void Start()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    public int AddSceneToList(string sceneName, string bundleName, bool toAdd = true)
    {
        if (!PlayerPrefsManager.simulatePlayerActions)
            return -1;
        int inListID = IsSceneInList(sceneName);
        if (toAdd)
        {
            if (inListID != -1)
                return -1;
            sceneData _data = new sceneData();
            _data.sceneName = sceneName;
            _data.bundleName = bundleName;
            AutoplayScenes.Add(_data);
            return(IsSceneInList(sceneName));
        }
        else
        {
            if (inListID == -1)
                return -1;
            AutoplayScenes.Remove(AutoplayScenes[inListID]);
        }
        return -1;
    }

    public int IsSceneInList(string sceneName)
    {
        if (AutoplayScenes != null)
        {
            for (int i = 0; i < AutoplayScenes.Count; i++)
            {
                if (AutoplayScenes[i].sceneName == sceneName)
                    return i;
            }
        }
        return -1;
    }


}
