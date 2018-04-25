using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndScoreWrongStepDescr : MonoBehaviour, IPointerEnterHandler
{ 
    public string text;
    public bool wrong;

    private Controls controls;

    bool UIflag = false;

    void Start()
    {
        
        if (GameObject.Find("GameLogic") != null)
        {
            controls = GameObject.Find("GameLogic").GetComponent<Controls>();
        }
        else
        {
            UIflag = true;
        }
    }

    void Update()
    {
        if (!UIflag)
        {
            if (controls.SelectedObject == gameObject)
            {
                foreach (Text text in transform.parent.GetComponentsInChildren<Text>())
                {
                    text.color = text.GetComponent<EndScoreWrongStepDescr>().wrong ? Color.red : Color.grey;
                }

                GetComponent<Text>().color = Color.black;
                GameObject.Find("StepDescription").GetComponent<Text>().text = text;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIflag)
        {
            foreach (Text text in transform.parent.GetComponentsInChildren<Text>())
            {
                text.color = text.GetComponent<EndScoreWrongStepDescr>().wrong ? Color.red : Color.grey;
            }

            GetComponent<Text>().color = Color.black;
            GameObject.Find("StepDescription").GetComponent<Text>().text = text;
        }
    }
}
