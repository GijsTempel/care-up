using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CareUp.Actions;

public class ActionsPanel : MonoBehaviour {
    bool slideState = false;
    int lastStepId = -1;
    int lastComplitedActionsNum = -1;
    float startTime;
    public Text ScoreValueText;
    public GameObject ActionListPanel;
    public GameObject ScoreListPanel;
    public Text ScoreDataText;

    List<ActionStepButton> ActionStepButtons = new List<ActionStepButton>();
    List<ActionStepButton> complitedActionButtons = new List<ActionStepButton>();
    public enum Mode
    {
        ShortDescr,
        Type,
        Comment,
        CommentUA,
        Icons,
        Requirements,
        Score
    };
    public void UpdatePanel()
    {
        lastStepId = -1;
        if (mode == ActionsPanel.Mode.Score)
        {

        }
    }
    public void SetScore(float _score)
    {
        ScoreValueText.text = "Score: " + _score.ToString() + "%";
    }

    public void SetScoreDataText(string dataText)
    {
        ScoreDataText.text = dataText;
    }

    public ActionsPanel.Mode mode;
    ActionManager am;
	// Use this for initialization
	void Start () {
        SetMode((int)mode);
        startTime = Time.time;
        if (GameObject.FindObjectOfType<ActionManager>() != null)
        {
            am = GameObject.FindObjectOfType<ActionManager>();
        }
        buildActionsList();
    }


    public void SetMode(int _mode) 
    {
        mode = (ActionsPanel.Mode)_mode;
        lastStepId = -1;
        print(mode.ToString());
        bool isScoreMode = mode == ActionsPanel.Mode.Score;
        ActionListPanel.GetComponent<CanvasGroup>().alpha = 1f - (isScoreMode ? 1f : 0f);
        ScoreListPanel.GetComponent<CanvasGroup>().alpha =(isScoreMode ? 1f : 0f);

    }


    // Update is called once per frame
    void Update () {
        if (am != null)
        {
            if (lastStepId != am.CurrentActionIndex)
            {
                int numberOfCompletedActions = am.CompletedActions.Count;

                foreach (ActionStepButton ab in ActionStepButtons)
                {
                    Action buttonAction = ab.getAction();
                    if (numberOfCompletedActions > 0)
                    {
                        foreach(Action ca in am.CompletedActions)
                        {
                            if (ca.compareActions(buttonAction) && ab.getComplitTime() < 0f)
                            {
                                ab.setCheckmark();
                                ab.setCompliteTime(Time.time - startTime);
                            }
                        }
                    }
                    ab.updateLook(am.CurrentActionIndex);
                    lastStepId = am.CurrentActionIndex;
                }
            }
        }
    }

    public void toggleSlide()
    {
        slide(!slideState);
    }

    public void buildActionsList()
    {
        Transform content = transform.Find("ActionsList/Viewport/Content").transform;
        if (am != null)
        {
            foreach (Action a in am.actionList)
            {
                GameObject ActionStep = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/ActionStepButton"), content);
                ActionStep.GetComponent<ActionStepButton>().setAction(a);
                ActionStepButtons.Add(ActionStep.GetComponent<ActionStepButton>());
            }
        }
    }

    public void slide(bool value)
    {
        if (slideState != value)
        {
            slideState = value;
            if (value)
                GetComponent<Animator>().SetTrigger("in");
            else
                GetComponent<Animator>().SetTrigger("out");

        }
    }
}
