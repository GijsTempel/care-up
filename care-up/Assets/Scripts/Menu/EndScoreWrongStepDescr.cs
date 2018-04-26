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
    
    private Color redish = new Color(0.5f, 0.0f, 0.0f, 1.0f);
    private Color greyish = new Color(0.25f, 0.25f, 0.25f, 1.0f);

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

        GetComponent<Text>().color = wrong ? Color.red : Color.black;
    }

    void Update()
    {
        if (!UIflag)
        {
            if (controls.SelectedObject == gameObject)
            {
                foreach (Text text in transform.parent.GetComponentsInChildren<Text>())
                {
                    text.color = text.GetComponent<EndScoreWrongStepDescr>().wrong ? redish : greyish;
                }

                GetComponent<Text>().color = wrong ? Color.red : Color.black;
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
                text.color = text.GetComponent<EndScoreWrongStepDescr>().wrong ? redish : greyish;
            }

            GetComponent<Text>().color = wrong ? Color.red : Color.black;
            GameObject.Find("StepDescription").GetComponent<Text>().text = text;
        }
    }
}
