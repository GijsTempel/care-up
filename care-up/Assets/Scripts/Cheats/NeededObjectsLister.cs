using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class NeededObjectsLister : MonoBehaviour {
    public bool buildList = false;
    public List<string> objectsNeeded = new List<string>();
    StreamWriter writer;


    public bool addNeeded(string value)
    {
        return true;
        /* ?????????????
        bool has = false;
        foreach (string o in objectsNeeded)
        {
            if (o == value)
            {
                has = true;
                break;
            }
        }
        if (!has)
        {
            objectsNeeded.Add(value);
            if (writer != null)
            {
                writer.WriteLine(value + "\n");

            }
            return false;
        }

        return true;
        */
    }


    void Start () {
        return;
        /* ???????????
#if (UNITY_EDITOR)
        string scneName = SceneManager.GetActiveScene().name;
        string path = "Assets/ListOfActions/__o__" + scneName + ".txt";

        if (buildList)
        {
            writer = new StreamWriter(path, false);
        }
#endif
        */
    }

    // Update is called once per frame
    void Update () {
		
	}


    void OnApplicationQuit()
    {
        return;
        /*  ????????????
#if (UNITY_EDITOR)
        Debug.Log("Application ending after " + Time.time + " seconds");
        if (writer != null)
            writer.Close();
#endif
        */
    }
}
