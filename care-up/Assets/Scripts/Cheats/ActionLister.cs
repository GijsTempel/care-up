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
    // Use this for initialization
    void Start () {
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

        //Write some text to the test.txt file
        writer = new StreamWriter(path, false);
        writer.WriteLine("--- List of actions ---");
        
    }
	
	// Update is called once per frame
	void Update () {
        print(currentAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
	}

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        writer.Close();
    }
}
