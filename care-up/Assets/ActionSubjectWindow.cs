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
    private PlayerPrefsManager manager;

    bool subjectColorSet = false;
    bool actionColorSet = false;

    Dictionary<int, List<GameObject>> ActionStepElements = new Dictionary<int, List<GameObject>>();
    void Start()
    {
        actionManager = GameObject.FindObjectOfType<ActionManager>();
        manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
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

                        //Is current step
                        if (currentActionIndex == index)
                        {
                            if (!manager.practiceMode)
                                g.SetActive(false);
                            else
                                img.color = new Color(0f, 0.7f, 0f, 0.2f);
                        }

                        //Passed step
                        else if (index < currentActionIndex)
                        {
                            if (!manager.practiceMode)
                                g.SetActive(true);
                            else
                                img.color = new Color(.3f, .3f, .3f, 0.4f);
                        }
                        //A step that has not yet been reached
                        else
                        {
                            if (!manager.practiceMode)
                                g.SetActive(false);
                            else
                                img.color = new Color(.6f, .6f, .6f, 0.4f);
                        }
                        //Complited correctly step
                        if (a.matched)
                        {
                            if (!manager.practiceMode)
                                g.SetActive(true);
                        }
                    }
                }
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
