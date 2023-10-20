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

    Dictionary<int, List<GameObject>> ActionStepElements = new Dictionary<int, List<GameObject>>();
    void Start()
    {
        actionManager = GameObject.FindObjectOfType<ActionManager>();
        BuildActionsList();
    }

    public void UpdateWindowView()
    {

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
                }
                GameObject actionStep = GameObject.Instantiate(Resources.Load<GameObject>(
                    "NecessaryPrefabs/UI/SubjectStep"), scrollContent);
                actionStep.transform.Find("Panel/Text").GetComponent<TextMeshProUGUI>().text =
                    a.shortDescr;
                currentActionElements.Add(actionStep);
                ActionStepElements.Add(a.sequentialNumber, currentActionElements);
            }
            GameObject.Instantiate(Resources.Load<GameObject>(
                "NecessaryPrefabs/UI/SubjectEmpty"), scrollContent);
            
        }
    }
}
