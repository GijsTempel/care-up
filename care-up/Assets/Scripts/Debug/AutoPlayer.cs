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

    public void AddSceneToList(string sceneName, string bundleName, bool toAdd = true)
    {
        if (!PlayerPrefsManager.simulatePlayerActions)
            return;
        int inListID = IsSceneInList(sceneName);
        if (toAdd)
        {
            if (inListID != -1)
                return;
            sceneData _data = new sceneData();
            _data.sceneName = sceneName;
            _data.bundleName = bundleName;
            AutoplayScenes.Add(_data);
        }
        else
        {
            if (inListID == -1)
                return;
            AutoplayScenes.Remove(AutoplayScenes[inListID]);
        }
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
