using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUp.Actions;
using UnityEngine.UI;
using TMPro;

public class ActionSubjectWindow : MonoBehaviour
{
    public Transform scrollContent;
    ActionManager actionManager;
    Color subjectBaseColor = new Color();
    Color actionBaseColor = new Color();

    bool subjectColorSet = false;
    bool actionColorSet = false;
    Dictionary<int, List<GameObject>> ActionStepElements = new Dictionary<int, List<GameObject>>();
    void Start()
    {
        actionManager = GameObject.FindObjectOfType<ActionManager>();
        BuildActionsList();
    }

    public void RefrashWindow()
    {
        if (actionManager != null)
        {
            int currentActionIndex = actionManager.CurrentActionIndex;
            foreach (Action a in actionManager.actionList)
            {
                if (ActionStepElements.ContainsKey(a.sequentialNumber))
                {
                    int index = a.SubIndex;
                    foreach(GameObject g in ActionStepElements[a.sequentialNumber])
                    {
                        Image img = g.transform.GetChild(0).GetComponent<Image>();
                        if (currentActionIndex == index)
                        {
                            img.color = new Color(0f, 1f, 0f, 0.4f);
                        }
                        else if (index < currentActionIndex)
                        {
                            img.color = new Color(.5f, .5f, .5f, 0.4f);
                        }
                        else
                        {
                            img.color = new Color(1f, .6f, 0f, 0.4f);
                        }
                    }
                }
                Debug.Log(a.SubIndex);
            }
        }
    }

    public void BuildActionsList()
    {
        if (actionManager != null)
        {
            foreach (Action a in actionManager.actionList)
            {   
                List<GameObject> currentActionElements = new List<GameObject>();
                if (a.subjectTitle != "")
                {
                    GameObject subjectTitleObj = GameObject.Instantiate(Resources.Load<GameObject>(
                        "NecessaryPrefabs/UI/SubjectTitle"), scrollContent);
                    subjectTitleObj.transform.Find("Panel/Text").GetComponent<TextMeshProUGUI>().text =
                        a.subjectTitle;
                    currentActionElements.Add(subjectTitleObj);
                    if (!subjectColorSet)
                    {
                        subjectBaseColor = subjectTitleObj.GetComponent<Image>().color;
                        subjectColorSet = true;
                    }
                }
                GameObject actionStep = GameObject.Instantiate(Resources.Load<GameObject>(
                    "NecessaryPrefabs/UI/SubjectStep"), scrollContent);
                actionStep.transform.Find("Panel/Text").GetComponent<TextMeshProUGUI>().text =
                    a.shortDescr;
                currentActionElements.Add(actionStep);
                ActionStepElements.Add(a.sequentialNumber, currentActionElements);
                if (!actionColorSet)
                {
                    actionBaseColor = actionStep.GetComponent<Image>().color;
                    actionColorSet = true;
                }
            }
            GameObject.Instantiate(Resources.Load<GameObject>(
                "NecessaryPrefabs/UI/SubjectEmpty"), scrollContent);
            
        }
        RefrashWindow();
    }
}
