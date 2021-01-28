using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotUITabs : MonoBehaviour
{
    protected static List<RobotUITabs> tabs = new List<RobotUITabs>();
    protected static Transform icons;

    protected GameObject tabTrigger;

    protected RectTransform[] children;

    [HideInInspector]
    public static bool tutorial_back = false;
    [HideInInspector]
    public static bool tutorial_generalOpened = false;
    [HideInInspector]
    public static bool tutorial_messageCenterOpened = false;
    [HideInInspector]
    public static bool tutorial_infoTabOpened = false;
    [HideInInspector]
    public static bool tutorial_recordsOpened = false;
    [HideInInspector]
    public static bool tutorial_prescriptionOpened = false;

    Tutorial_UI tutorial_UI;
    Tutorial_Theory tutorial_theory;

    static RobotUITabs generalTab;

    protected virtual void Start()
    {
        tabs.Add(this);
        tabs.RemoveAll(item => item == null);

        icons = transform.parent.Find("TabletIcons");

        if (transform.tag == "alg")
        {
            tabTrigger = transform.parent.Find("GeneralTab").Find(name).gameObject;
        }
        else
        {
            tabTrigger = icons.Find(name).gameObject;
        }

        children = transform.GetComponentsInChildren<RectTransform>();

        GameObject sceneTitle = GameObject.Find("SceneTitle");
        GameObject manager = GameObject.Find("Preferences");
        if (sceneTitle != null && (manager != null && manager.GetComponent<PlayerPrefsManager>() != null))
        {
            sceneTitle.GetComponent<Text>().text = manager.GetComponent<PlayerPrefsManager>().currentSceneVisualName;
        }

        tutorial_UI = GameObject.FindObjectOfType<Tutorial_UI>();
        tutorial_theory = GameObject.FindObjectOfType<Tutorial_Theory>();
        if (tutorial_UI != null)
        {
            GameObject exitBtn = GameObject.Find("Exit");
            if (exitBtn != null)
            {
                exitBtn.GetComponent<Button>().interactable = false;
            }
        }

        SetTabActive(false);

        if (GameObject.Find("Preferences") != null)
        {
            if (!GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().practiceMode)
            {
                if (name == "InfoTab")
                {
                    SetTabActive(false);
                    gameObject.SetActive(false);
                    tabTrigger.SetActive(false);
                    tabs.Remove(this);
                }
            }
        }

        tabTrigger.GetComponent<Button>().onClick.AddListener(OnTabSwitch);

        GameObject backBtn = null;

        if (transform.Find("Button") != null)
        {
            backBtn = transform.Find("Button").gameObject;
        }
        if (transform.tag == "alg")
        {
            if (backBtn != null)
                backBtn.GetComponent<Button>().onClick.AddListener(BackBtnToGeneral);
        }
        else
        {
            if (backBtn != null)
                backBtn.GetComponent<Button>().onClick.AddListener(BackButton);
        }

        if (name == "GeneralTab")
        {
            generalTab = this;
        }
    }

    public void OnTabSwitch()
    {
        if ((tutorial_UI != null && tutorial_UI.tabToOpen != name) ||
            (tutorial_theory != null && tutorial_theory.tabToOpen != name))
        {
            return;
        }

        QuizTab tab = GameObject.FindObjectOfType<QuizTab>();
        if (tab != null)
        {
            if (tab.transform.Find("Continue") != null && tab.transform.Find("Continue").gameObject.activeSelf)
            {
                GameObject.FindObjectOfType<QuizTab>().Continue();
            }
        }

        foreach (RobotUITabs t in tabs)
        {
            t.SetTabActive(false);
        }

        icons.gameObject.SetActive(false);

        SetTabActive(true);

        switch (name)
        {
            case "PrescriptionTab":
                FindObjectOfType<ActionManager>().OnExamineAction("PrescriptionForm", "good");
                break;
            case "RecordsTab":
                FindObjectOfType<ActionManager>().OnExamineAction("PatientRecords", "good");
                break;
        }
    }

    protected virtual void SetTabActive(bool value)
    {
        foreach (RectTransform child in children)
        {
            if (child.name != "Tab" && child.parent.name != "Tab" && child.GetComponent<RobotUITabs>() == null)
            {
                child.gameObject.SetActive(value);
            }
        }

        switch (name)
        {
            case "GeneralTab":
                tutorial_generalOpened = true;
                break;
            case "MessageCenter":
                tutorial_messageCenterOpened = true;
                break;
            case "InfoTab":
                tutorial_infoTabOpened = true;
                GameObject.FindObjectOfType<RobotUITabInfo>().SwitchItemList(false);
                break;
            case "PrescriptionTab":
                tutorial_prescriptionOpened = true;
                break;
            case "RecordsTab":
                tutorial_recordsOpened = true;
                break;
        }
    }

    protected void BackButton()
    {
        if ((tutorial_UI != null && tutorial_UI.closeTab == false) ||
            (tutorial_theory != null && tutorial_theory.closeTab == false))
        {
            return;
        }

        tutorial_back = true;

        foreach (RobotUITabs t in tabs)
        {
            t.SetTabActive(false);
        }

        icons.gameObject.SetActive(true);
    }

    public void OnIpadRecordButtonClick()
    {
        PlayerPrefsManager prefsManager = GameObject.FindObjectOfType<PlayerPrefsManager>();
        if (prefsManager != null && !prefsManager.practiceMode)
        {
            FindObjectOfType<GameUI>().OpenDonePanelYesNo();
        }
        else
        {
            FindObjectOfType<ActionManager>().OnUseAction("PaperAndPen");
        }

        GameObject.FindObjectOfType<PlayerScript>().CloseRobotUI();
    }

    protected void BackBtnToGeneral()
    {
        if ((tutorial_UI != null && tutorial_UI.closeTab == false) ||
            (tutorial_theory != null && tutorial_theory.closeTab == false))
        {
            return;
        }

        tutorial_back = true;

        generalTab.OnTabSwitch();
    }
}
