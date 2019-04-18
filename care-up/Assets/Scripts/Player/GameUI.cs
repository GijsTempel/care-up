using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CareUp.Actions;
using System.Linq;


public class GameUI : MonoBehaviour
{
    GameObject Player;
    public Animator Blink;
    public Animator IPadBlink;
    public bool BlinkState = false;
    public bool testValue;
    GameObject donePanel;
    GameObject closeButton;
    GameObject closeDialog;
    GameObject donePanelYesNo;
    GameObject WalkToGroupPanel;
    public GameObject MoveBackButton;
    public WalkToGroupButton LeftSideButton;
    public WalkToGroupButton RightSideButton;
    public Dictionary<string, WalkToGroupButton> WTGButtons;
    WalkToGroup prevWalkToGroup = null;
    private Tutorial_Combining tutorialCombine;
    private Tutorial_UseOn tutorialUseOn;
    private HandsInventory handsInventory;
    private ActionManager actionManager;
    private Animator controller;
    private float startTimeOut = 2f;
    private bool timeOutEnded = false;

    List<string> activeHighlighted = new List<string>();

    public GameObject ItemControlPanel;
    public GameObject combineButton;
    public GameObject decombineButton;
    public GameObject decombineButton_right;

    public GameObject noTargetButton;
    public GameObject noTargetButton_right;

    private CameraMode cameraMode;

    public GameObject zoomButtonLeft;
    public GameObject zoomButtonRight;
    public GameObject SubStepsPanel;
    Text SubStepsText;

    public bool currentAnimLock = false;

    float cooldownTime = 0;
    float lastCooldownTime = 0;
    int currentActionsCount = 0;

    bool currentItemControlPanelState = false;
    int currentLeft;
    int currentRight;
    string useOnNTtext;
    PlayerScript ps;
    bool ICPCurrentState = false;
    public bool allowObjectControlUI = true;

    public void MoveBack()
    {
        Player.GetComponent<PlayerScript>().MoveBackButton();
    }

    public void UseOn()
    {
        if (!(handsInventory.LeftHandEmpty() && handsInventory.RightHandEmpty()))
        {
            handsInventory.OnCombineAction();
        }
    }

    public void UseOnNoTarget()
    {
        if (tutorialUseOn != null && !tutorialUseOn.ventAllowed)
        {
            return;
        }

        if (!handsInventory.LeftHandEmpty())
        {
            if (actionManager.CompareUseOnInfo(handsInventory.leftHandObject.name, ""))
            {
                handsInventory.LeftHandObject.GetComponent<PickableObject>().Use(true, true);

                if (tutorialUseOn != null)
                {
                    handsInventory.LeftHandObject.GetComponent<PickableObject>().tutorial_usedOn = true;
                }
                return;
            }
        }
        if (!handsInventory.RightHandEmpty())
        {
            if (actionManager.CompareUseOnInfo(handsInventory.rightHandObject.name, ""))
            {
                handsInventory.RightHandObject.GetComponent<PickableObject>().Use(false, true);

                if (tutorialUseOn != null)
                {
                    handsInventory.RightHandObject.GetComponent<PickableObject>().tutorial_usedOn = true;
                }
            }
        }
    }

    public void OpenRobotUI()
    {
        RobotManager.UIElementsState[0] = false;
        Player.GetComponent<PlayerScript>().OpenRobotUI();
    }

    public void ToggleUsingOnMode()
    {
        Player.GetComponent<PlayerScript>().ToggleUsingOnMode(false);
    }

    public void CloseButtonPressed(bool value)
    {
        closeDialog.SetActive(value);
        closeButton.SetActive(!value);

        if (value)
        {
            GameObject.FindObjectOfType<PlayerScript>().robotUIopened = true;
        }
        else
        {
            GameObject.FindObjectOfType<PlayerScript>().robotUIopened = false;
        }
    }

    public void CloseGame()
    {
        bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
    }

    public void ButtonBlink(bool ToBlink)
    {
        if (BlinkState == ToBlink)
            return;
        BlinkState = ToBlink;
        if (transform.Find("Extra").gameObject.activeSelf)
        {
            Blink.SetTrigger("BlinkOnes");
            BlinkState = false;
        }
        else if (ToBlink)
        {
            Blink.SetTrigger("BlinkStart");
            RobotManager.UIElementsState[1] = true;
        }
        else
        {
            Blink.SetTrigger("BlinkStop");
            RobotManager.UIElementsState[1] = false;
        }
    }

    public void Examine(bool leftHand = true)
    {
        if (leftHand && handsInventory.LeftHandEmpty())
            return;
        if (!leftHand && handsInventory.RightHandEmpty())
            return;
        GameObject initedObject = null;
        if (leftHand)
            initedObject = handsInventory.leftHandObject.gameObject;
        else
            initedObject = handsInventory.rightHandObject.gameObject;

        cameraMode.selectedObject = initedObject.GetComponent<ExaminableObject>();
        if (cameraMode.selectedObject != null) // if there is a component
        {
            if (tutorialUseOn != null)
            {
                tutorialUseOn.examined = true;
            }
            cameraMode.selectedObject.OnExamine();
            //controls.ResetObject();
        }
    }

    // Use this for initialization
    void Start()
    {
        useOnNTtext = noTargetButton.transform.GetChild(0).GetComponent<Text>().text;
        ps = GameObject.FindObjectOfType<PlayerScript>();
        controller = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        handsInventory = GameObject.FindObjectOfType<HandsInventory>();
        tutorialCombine = GameObject.FindObjectOfType<Tutorial_Combining>();
        tutorialUseOn = GameObject.FindObjectOfType<Tutorial_UseOn>();
        actionManager = GameObject.FindObjectOfType<ActionManager>();
        ActionManager.BuildRequirements();
        zoomButtonLeft.SetActive(false);
        zoomButtonRight.SetActive(false);
        combineButton.SetActive(false);
        decombineButton.SetActive(false);
        decombineButton_right.SetActive(false);
        noTargetButton.SetActive(false);
        ItemControlPanel.SetActive(false);
        noTargetButton_right.SetActive(false);

#if !UNITY_EDITOR
        if(GameObject.Find("ActionsPanel") != null)
            GameObject.Find("ActionsPanel").SetActive(false);
#endif
        WalkToGroupPanel = GameObject.Find("MovementButtons");
        Player = GameObject.Find("Player");
        closeButton = transform.Find("CloseBtn").gameObject;
        closeDialog = transform.Find("CloseDialog").gameObject;
        closeDialog.SetActive(false);
        donePanel = transform.Find("DonePanel").gameObject;
        donePanel.SetActive(false);

        donePanelYesNo = transform.Find("DonePanelYesNo").gameObject;
        donePanelYesNo.SetActive(false);
        if (WalkToGroupPanel != null)
        {
            WTGButtons = new Dictionary<string, WalkToGroupButton>();

            foreach (WalkToGroupButton b in GameObject.FindObjectsOfType<WalkToGroupButton>())
            {
                if (b.name == "MoveLeft")
                {
                    LeftSideButton = b;
                }
                else if (b.name == "MoveRight")
                {
                    RightSideButton = b;
                }
                else
                {
                    string key = "";
                    switch (b.name)
                    {
                        case "MoveWorkfield":
                            key = "WorkField";
                            break;
                        case "Movepatient":
                            key = "Patient";
                            break;
                        case "MoveCollegue":
                            key = "Doctor";
                            break;
                        case "MoveSink":
                            key = "Sink";
                            break;
                    }
                    if (key != "")
                    {
                        WTGButtons.Add(key, b);
                    }
                }
                b.gameObject.SetActive(false);
            }
            int activeGroupButtons = 0;
            foreach (WalkToGroup g in GameObject.FindObjectsOfType<WalkToGroup>())
            {
                switch (g.WalkToGroupType)
                {
                    case WalkToGroup.GroupType.WorkField:
                        WTGButtons["WorkField"].setWalkToGroup(g);
                        WTGButtons["WorkField"].gameObject.SetActive(true);
                        activeGroupButtons++;
                        break;
                    case WalkToGroup.GroupType.Doctor:
                        WTGButtons["Doctor"].setWalkToGroup(g);
                        WTGButtons["Doctor"].gameObject.SetActive(true);
                        activeGroupButtons++;
                        break;
                    case WalkToGroup.GroupType.Patient:
                        WTGButtons["Patient"].setWalkToGroup(g);
                        WTGButtons["Patient"].gameObject.SetActive(true);
                        break;
                    case WalkToGroup.GroupType.Sink:
                        WTGButtons["Sink"].setWalkToGroup(g);
                        WTGButtons["Sink"].gameObject.SetActive(true);
                        activeGroupButtons++;
                        break;
                }
            }
            if (!WTGButtons["Sink"].gameObject.activeSelf || activeGroupButtons <= 2)
                WalkToGroupPanel.transform.Find("spacer0").gameObject.SetActive(false);
            if (!WTGButtons["Patient"].gameObject.activeSelf || activeGroupButtons < 2)
                WalkToGroupPanel.transform.Find("spacer2").gameObject.SetActive(false);
            if (activeGroupButtons < 2)
                WalkToGroupPanel.transform.Find("spacer1").gameObject.SetActive(false);
        }
        //ActionManager.UpdateRequirements();
    }

    public HighlightObject AddHighlight(Transform target, string prefix, HighlightObject.type hl_type = HighlightObject.type.NoChange, float startDelay = 0, float LifeTime = float.PositiveInfinity)
    {
        string hl_name = prefix + "_" + target.name;
        if (GameObject.Find(hl_name) != null || target.GetComponent<WorkField>() != null)
            return null;
        GameObject hl_obj = Instantiate(Resources.Load<GameObject>("Prefabs\\HighlightObject"), target.position, new Quaternion()) as GameObject;

        HighlightObject hl = hl_obj.GetComponent<HighlightObject>();
        hl.name = hl_name;
        hl.setTarget(target);
        if (hl_type != HighlightObject.type.NoChange)
            hl.setType(hl_type);
        hl.setTimer(LifeTime);
        if (startDelay > 0)
            hl.setStartDelay(startDelay);
        return hl_obj.GetComponent<HighlightObject>();
    }

    public void RemoveHighlight(string prefix, string _name)
    {
        string hl_name = prefix + "_" + _name;
        if (GameObject.Find(hl_name) != null)
        {
            if (GameObject.Find(hl_name).GetComponent<HighlightObject>())
                GameObject.Find(hl_name).GetComponent<HighlightObject>().Destroy();
        }
    }


    //----------------------------------------------------------------------------------------------------------
    public void UpdateHelpHighlight()
    {
        bool practiceMode = true;
        if (GameObject.FindObjectOfType<PlayerPrefsManager>() != null)
            practiceMode = GameObject.FindObjectOfType<PlayerPrefsManager>().practiceMode;
        if (!practiceMode)
            return;

        List<string> newHLObjects = new List<string>();

        string prefix = "helpHL";

        if (!handsInventory.LeftHandEmpty())
            RemoveHighlight(prefix, handsInventory.leftHandObject.name);

        if (!handsInventory.RightHandEmpty())
            RemoveHighlight(prefix, handsInventory.rightHandObject.name);

        List<Action> sublist = actionManager.actionList.Where(action =>
           action.SubIndex == actionManager.currentActionIndex &&
           action.matched == false).ToList();

        foreach (Action a in sublist)
        {
            string[] ObjectNames = new string[0];
            a.ObjectNames(out ObjectNames);

            //if (a.Type == ActionManager.ActionType.ObjectUse ||
            //a.Type == ActionManager.ActionType.ObjectDrop)
            foreach(string objectToUse in ObjectNames)
            {
                if (GameObject.Find(objectToUse) != null)
                {
                    if (handsInventory.IsInHand(GameObject.Find(objectToUse)))
                        continue;
                    HighlightObject h = AddHighlight(GameObject.Find(objectToUse).transform, prefix, HighlightObject.type.NoChange, 2f + Random.Range(0f, 0.5f));
                    if (h != null)
                        h.setGold(true);
                    newHLObjects.Add(objectToUse);
                }
                else
                {
                    GameObject usableHL = null;
                    foreach (UsableObject u in GameObject.FindObjectsOfType<UsableObject>())
                    {
                        if (u.PrefabToAppear == objectToUse)
                        {
                            usableHL = u.gameObject;
                            break;
                        }
                    }
                    if (usableHL != null)
                    {
                        HighlightObject h = AddHighlight(usableHL.transform, prefix, HighlightObject.type.NoChange, 2f + Random.Range(0f, 0.5f));
                        if (h != null)
                            h.setGold(true);
                        newHLObjects.Add(usableHL.name);
                    }
                }
            }
        }

        //clear highlights

        for (int i = 0; i < activeHighlighted.Count; i++)
        {
            if (!newHLObjects.Contains(activeHighlighted[i]))
            {
                RemoveHighlight(prefix, activeHighlighted[i]);
            }
        }
        activeHighlighted.Clear();
        foreach (string s in newHLObjects)
        {
            activeHighlighted.Add(s);
        }
       
    }





    public void ShowDonePanel(bool value)
    {
        donePanel.SetActive(value);
    }

    public void EndScene()
    {
        if (GameObject.Find("Preferences") != null)
        {
            GameObject.Find("Preferences").GetComponent<EndScoreManager>().LoadEndScoreScene();
        }
        else
        {
            bl_SceneLoaderUtils.GetLoader.LoadLevel("UMenuPro");
        }

        donePanel.SetActive(false);
    }
    void OnGUI()
    {
#if UNITY_EDITOR
        GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString());
#endif
    }
    // Update is called once per frame
    void Update()
    {

        if (!timeOutEnded)
        {
            startTimeOut -= Time.deltaTime;
            if (startTimeOut < 0)
            {
                timeOutEnded = true;
                ActionManager.UpdateRequirements();
                UpdateHelpHighlight();
            }
        }

        //Don't show object control panel if animation is playing
        //if animation is longer than 0.2 (is not hold animation)
        bool animationUiBlock = true;
        for (int i = 0; i < 3; i++)
        {
            if (controller.GetCurrentAnimatorStateInfo(i).length > 0.2f && controller.GetCurrentAnimatorStateInfo(i).normalizedTime < 1f)
                animationUiBlock = false;
            if (controller.GetNextAnimatorStateInfo(i).length > 0.2f && controller.GetAnimatorTransitionInfo(i).normalizedTime < 0.01)
                animationUiBlock = false;
            if (i < 2)
            {
                if (controller.GetCurrentAnimatorStateInfo(i).length > 0.2f &&
                    controller.GetAnimatorTransitionInfo(i).normalizedTime < 0.01 &&
                    controller.GetNextAnimatorStateInfo(i).length < 0.2f)
                    animationUiBlock = false;
            }
        }

        //to show object control panel if no animation block and action block
        bool showItemControlPanel = allowObjectControlUI && animationUiBlock;

        //if some object was added or removed to hands
        int lHash = 0;
        if (handsInventory.leftHandObject != null)
            lHash = handsInventory.leftHandObject.gameObject.GetHashCode();
        int rHash = 0;
        if (handsInventory.rightHandObject != null)
            rHash = handsInventory.rightHandObject.gameObject.GetHashCode();

        bool handsStateChanged = (currentLeft != lHash || currentRight != rHash
        || (ICPCurrentState != ItemControlPanel.activeSelf)
        || currentActionsCount != actionManager.actionsCount);

        if (handsStateChanged)
        {
            ActionManager.UpdateRequirements();
            UpdateHelpHighlight();
            currentActionsCount = actionManager.actionsCount;
            //hide panel for the first frame of hands state change
            //prevent quick blinking of buttons before animation starts
            showItemControlPanel = false;

            //Update current hands state 
            currentLeft = lHash;
            currentRight = rHash;

            bool LEmpty = handsInventory.LeftHandEmpty();
            bool REmpty = handsInventory.RightHandEmpty();
            bool showDecomb = (LEmpty && !REmpty) || (!LEmpty && REmpty);
            bool showCombin = !LEmpty && !REmpty;
            bool showZoomLeft = false;
            bool showZoomRight = false;

            bool showNoTarget = false;
            bool showNoTarget_right = false;

            if (!LEmpty)
            {
                if (handsInventory.leftHandObject.GetComponent<ExaminableObject>() != null)
                    showZoomLeft = true;
                if (actionManager.CompareUseOnInfo(handsInventory.leftHandObject.name, ""))
                {
                    showNoTarget = true;
                    noTargetButton.transform.GetChild(0).GetComponent<Text>().text =
                        actionManager.CurrentButtonText(handsInventory.leftHandObject.name);
                }

                decombineButton.transform.GetChild(0).GetComponent<Text>().text =
                (actionManager.CompareCombineObjects(handsInventory.leftHandObject.name, "")) ?
                    actionManager.CurrentDecombineButtonText(handsInventory.leftHandObject.name)
                    : "Openen";
            }
            if (!REmpty)
            {
                if (handsInventory.rightHandObject.GetComponent<ExaminableObject>() != null)
                    showZoomRight = true;
                if (actionManager.CompareUseOnInfo(handsInventory.rightHandObject.name, ""))
                {
                    showNoTarget_right = true;
                    noTargetButton_right.transform.GetChild(0).GetComponent<Text>().text =
                       actionManager.CurrentButtonText(handsInventory.rightHandObject.name);
                }

                decombineButton_right.transform.GetChild(0).GetComponent<Text>().text =
                (actionManager.CompareCombineObjects("", handsInventory.rightHandObject.name)) ?
                    actionManager.CurrentDecombineButtonText(handsInventory.rightHandObject.name)
                    : "Openen";
            }
            zoomButtonLeft.SetActive(showZoomLeft);
            zoomButtonRight.SetActive(showZoomRight);
            noTargetButton.SetActive(showNoTarget);
            noTargetButton_right.SetActive(showNoTarget_right);

            decombineButton.SetActive(showDecomb && REmpty && !showNoTarget);
            decombineButton_right.SetActive(showDecomb && LEmpty && !showNoTarget_right);
            combineButton.SetActive(showCombin);
        }

        if (!currentItemControlPanelState && showItemControlPanel)
        {
            cooldownTime = 0.4f;
        }
        lastCooldownTime = cooldownTime;
        if (cooldownTime > 0)
            cooldownTime -= Time.deltaTime;
        currentItemControlPanelState = showItemControlPanel;
        if (cooldownTime > 0)
            showItemControlPanel = false;
        if (PlayerScript.actionsLocked)
            showItemControlPanel = false;
        ItemControlPanel.SetActive(showItemControlPanel);

        ICPCurrentState = ItemControlPanel.activeSelf;
        if (WalkToGroupPanel != null)
        {
            if (prevWalkToGroup != ps.currentWalkPosition)
            {
                WalkToGroupPanel.SetActive(ps.away);

                if (!ps.away)
                {
                    LeftSideButton.gameObject.SetActive(ps.currentWalkPosition.LeftWalkToGroup != null);
                    RightSideButton.gameObject.SetActive(ps.currentWalkPosition.RightWalkToGroup != null);

                    if (ps.currentWalkPosition.LeftWalkToGroup != null)
                    {
                        LeftSideButton.setWalkToGroup(ps.currentWalkPosition.LeftWalkToGroup);
                    }

                    if (ps.currentWalkPosition.RightWalkToGroup != null)
                    {
                        RightSideButton.setWalkToGroup(ps.currentWalkPosition.RightWalkToGroup);
                    }
                }
                else
                {
                    LeftSideButton.gameObject.SetActive(false);
                    RightSideButton.gameObject.SetActive(false);
                }
                prevWalkToGroup = ps.currentWalkPosition;
            }
            if (!MoveBackButton.activeSelf)
            {
                LeftSideButton.gameObject.SetActive(false);
                RightSideButton.gameObject.SetActive(false);
                prevWalkToGroup = null;
            }
        }
        testValue = RobotManager.UIElementsState[0];
        currentAnimLock = animationUiBlock;
    }


    public void OpenDonePanelYesNo()
    {
        donePanelYesNo.SetActive(true);
    }

    public void DonePanelYes()
    {
        FindObjectOfType<ActionManager>().OnUseAction("PaperAndPen");
        donePanelYesNo.SetActive(false);
    }

    public void DonePanelNo()
    {
        donePanelYesNo.SetActive(false);
    }      

    public void ClearHintPanel()
    {
        if(GameObject.Find("UI/DetailedHintPanel/HintContainer") != null)
        {
            Transform panel = GameObject.Find("UI/DetailedHintPanel/HintContainer").transform;
            for (int i = 0; i < panel.childCount; ++i)
            {
                Destroy(panel.GetChild(i).gameObject);
            }
        }        
    }

    public void SetHintPanelAlpha(float alpha)
    {
        if(GameObject.Find("UI/DetailedHintPanel") != null)
        {
            Transform panel = GameObject.Find("UI/DetailedHintPanel").transform;
            Color panelColor = panel.GetComponent<Image>().color;
            panelColor.a = alpha;
            panel.GetComponent<Image>().color = panelColor;
            foreach (Text t in panel.GetComponentsInChildren<Text>())
            {
                Color c = t.color;
                c.a = alpha;
                t.GetComponent<Text>().color = c;
            }
        }       
    }
    public void UpdateRequirements(List<ActionManager.StepData> subTasks)
    {
        ClearHintPanel();

        GameObject hintPanel = GameObject.Find("DetailedHintPanel");
        Text hintText;
        Text subTaskText;

        for (int i = 0; i < actionManager.CurrentDescription.Count; i++)
        {
            GameObject currentHintPanel = null;
            if (Resources.Load<GameObject>("Prefabs/UI/HintPanel") != null && GameObject.Find("UI/DetailedHintPanel/HintContainer") != null)
            {
                currentHintPanel = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/HintPanel"), GameObject.Find("UI/DetailedHintPanel/HintContainer").transform);
                hintText = currentHintPanel.transform.Find("Text").gameObject.GetComponent<Text>();
                hintText.text = actionManager.CurrentDescription[i];
            }

            for (int y = 0; y < subTasks.Count; y++)
            {

                if (subTasks[y].subindex == i)
                {
                    if (Resources.Load<GameObject>("Prefabs/UI/SubtaskHints") != null )
                    {
                        if (!subTasks[y].completed)
                        {
                            GameObject subtaskPanel = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/SubtaskHints"), currentHintPanel.transform);
                            subTaskText = subtaskPanel.transform.Find("Text").GetComponent<Text>();
                            subTaskText.text = subTasks[y].requirement;
                        }
                    }
                }
            }
            float alpha = hintPanel.GetComponent<Image>().color.a;
            SetHintPanelAlpha(alpha);
        }
    }
}
