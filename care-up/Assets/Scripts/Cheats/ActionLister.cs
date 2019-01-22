using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;

public class ActionLister : MonoBehaviour {
    List<string> actions;
    public Animator currentAnimator;
    StreamWriter writer;
    bool buildActionList = false;



    void Start () {
#if (UNITY_EDITOR)

        buildActionList = GameObject.FindObjectOfType<ObjectsIDController>().buildActionList;
        actions = new List<string>();
        if (GetComponent<Animator>() != null)
        {
            currentAnimator = GetComponent<Animator>();
        }
        else
        {
            currentAnimator = GetComponentInChildren<Animator>();
        }
        string scneName = SceneManager.GetActiveScene().name;
        string path = "Assets/ListOfActions/" + scneName + ".txt";

        if (buildActionList)
        {
            writer = new StreamWriter(path, false);
        }
#endif
    }


    void Update()
    {
#if (UNITY_EDITOR)
        if (currentAnimator != null && writer != null)
        {
            for (int i = 0; i <= currentAnimator.layerCount - 1; i++)
            {
                if (!actions.Contains(currentAnimator.GetCurrentAnimatorClipInfo(i)[0].clip.name))
                {
                    print(">>>  Action <<<  " + currentAnimator.GetCurrentAnimatorClipInfo(i)[0].clip.name);
                    actions.Add(currentAnimator.GetCurrentAnimatorClipInfo(i)[0].clip.name);
                    writer.WriteLine(currentAnimator.GetCurrentAnimatorClipInfo(i)[0].clip.name);
                }
            }
        }
#endif
    }

    void OnApplicationQuit()
    {
#if (UNITY_EDITOR)
        Debug.Log("Application ending after " + Time.time + " seconds");
        writer.Close();
#endif
    }
}
