using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScoreWrongStepDescr : MonoBehaviour {

    public string text;

    private Controls controls;

    void Start()
    {
        controls = GameObject.Find("GameLogic").GetComponent<Controls>();
    }

    void Update()
    {
        if (controls.SelectedObject == gameObject)
        {
            foreach (Text text in transform.parent.GetComponentsInChildren<Text>())
            {
                text.color = Color.white;
            }

            GetComponent<Text>().color = Color.green;
            GameObject.Find("StepDescription").GetComponent<Text>().text = text;
        }
    }
}
