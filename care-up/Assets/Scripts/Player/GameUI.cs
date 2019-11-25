using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CareUp.Actions;
using System.Linq;
using AssetBundles;
using UnityEngine.SceneManagement;

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
    public WalkToGroupButton LeftSideButton;
    public WalkToGroupButton RightSideButton;
    public Dictionary<string, WalkToGroupButton> WTGButtons;
    //WalkToGroup prevWalkToGroup = null;
    private Tutorial_Combining tutorialCombine;
    private Tutorial_UseOn tutorialUseOn;
    private HandsInventory handsInventory;
    private ActionManager actionManager;
    private Animator controller;
    private float startTimeOut = 0.5f;
    private bool timeOutEnded = false;
    PlayerPrefsManager prefs;
    public string debugSS = "";
    ObjectsIDsController objectsIDsController;
    bool practiceMode = true;
    public QuizTab quiz_tab;
    public bool DropLeftBlink = false;
    public bool DropRightBlink = false;

    GameObject MovementSideButtons;
    public List<string> reqPlaces = new List<string>();
    List<ActionManager.StepData> Current_SubTasks;
    float current_UpdateHintDelay = 0f;
    bool toDelayUpdateHint = false;
    GameObject gameLogic;
    public GameObject TalkBubble;
    GameObject DetailedHintPanel;

    public List<string> activeHighlighted = new List<string>();

    public GameObject IPad;
    public GameObject ItemControlPanel;
    public GameObject combineButton;
    public GameObject decombineButton;
    public GameObject decombineButton_right;
    public GameUI.ItemControlButtonType buttonToBlink;
    public GameUI.ItemControlButtonType moveButtonToBlink;
    public bool prescriptionButtonBlink;
    public bool recordsButtonBlink;
    public bool paperAndPenButtonblink;
    public GameObject theoryPanel;
    public GameObject patientInfo;

    public GameObject noTargetButton;
    public GameObject noTargetButton_right;

    private CameraMode cameraMode;

    public GameObject zoomButtonLeft;
    public GameObject zoomButtonRight;
    public GameObject SubStepsPanel;
    Text SubStepsText;

    public bool currentAnimLock = false;
    public GameObject DropLeftButton;
    public GameObject DropRightButton;
    public GameObject BlockPopUp;
    public Text BlockTitle;
    public Text BlockMessage;

    float cooldownTime = 0;
    float lastCooldownTime = 0;
    int currentActionsCount = 0;

    private bool startTimer = false;
    private float targetTime = 0.7f;
    public PersonObject PersonToTalk = null;
    bool currentItemControlPanelState = false;
    int currentLeft;
    int currentRight;
    string useOnNTtext;
    PlayerScript ps;
    bool ICPCurrentState = false;
    public bool allowObjectControlUI = true;
    public enum ItemControlButtonType
    {
        None,
        Combine,
        DecombineLeft,
        DecombineRight,
        NoTargetLeft,
        NoTargetRight,
        ZoomLeft,
        ZoomRight,
        DropLeft,
        DropRight,
        MoveLeft,
        MoveRight,
        Records,
        Prescription,
        Ipad,
        General,
        PaperAndPen,
        GeneralBack,
        RecordsBack,
        PrescriptionBack,
        MessageTabBack,
        Close
    }


    public void TestOutput()
    {
        AssetBundleManager.PrintLoadedBundles();
    }

    public void UseOn()
    {
        if (!(handsInventory.LeftHandEmpty() && handsInventory.RightHandEmpty()))
        {
            handsInventory.OnCombineAction();
        }
    }

    public void TalkButtonPressed()
    {
        if (PersonToTalk == null)
            return;
        PersonToTalk.CreateSelectionDialogue();
    }

    public void ShowBlockMessage(string Title, string Message)
    {
        if (objectsIDsController != null)
        {
            if (objectsIDsController.cheat)
                return;
        }

        if (Message == "")
            return;
        BlockTitle.text = Title;
        BlockMessage.text = Message;
        BlockPopUp.GetComponent<Animator>().SetTrigger("pop");
    }

    public void HideBlockMessage()
    {
        BlockPopUp.GetComponent<Animator>().SetTrigger("fold");
        if (prefs.practiceMode)
            DetailedHintPanel.SetActive(true);
    }

    public void UseOnNoTarget(bool leftHand = true)
    {
        GeneralAction generalAction = actionManager.CheckGeneralAction();

        if (generalAction == null)
        {
            if (tutorialUseOn != null && !tutorialUseOn.ventAllowed)
            {
                return;
            }

            if (leftHand && !handsInventory.LeftHandEmpty())
            {
                if (actionManager.CompareUseOnInfo(handsInventory.leftHandObject.name, ""))
                {
                    if (handsInventory.LeftHandObject.GetComponent<PickableObject>().Use(true, true))
                    {
                        UpdateWalkToGtoupUI(false);
                    }

                    if (tutorialUseOn != null)
                    {
                        handsInventory.LeftHandObject.GetComponent<PickableObject>().tutorial_usedOn = true;
                    }
                    return;
                }
                else
                    actionManager.OnUseOnAction(handsInventory.leftHandObject.name, "");

            }
            if (!leftHand && !handsInventory.RightHandEmpty())
            {
                if (actionManager.CompareUseOnInfo(handsInventory.rightHandObject.name, ""))
                {
                    if (handsInventory.RightHandObject.GetComponent<PickableObject>().Use(false, true))
                    {
                        UpdateWalkToGtoupUI(false);
                    }

                    if (tutorialUseOn != null)
                    {
                        handsInventory.RightHandObject.GetComponent<PickableObject>().tutorial_usedOn = true;
                    }
                }
                else
                    actionManager.OnUseOnAction(handsInventory.rightHandObject.name, "");
            }
        }
        else
            GeneralAction(generalAction);
    }

    public void OpenRobotUI()
    {
        if (cameraMode.camViewObject)
            return;

        if (PlayerAnimationManager.IsLongAnimation())
            return;

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
        // closeButton.SetActive(!value);

        if (value)
        {
            ps.robotUIopened = true;
        }
        else
        {
            ps.robotUIopened = false;
        }
    }

    public void CloseGame()
    {
        bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
    }

    public void ButtonBlink(bool ToBlink)
    {
        if (prefs != null)
            if (!prefs.practiceMode)
                return;
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
            if (Blink != null)
                Blink.SetTrigger("BlinkStart");
            RobotManager.UIElementsState[1] = true;
        }
        else
        {
            if (Blink != null)
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

    public void GeneralAction(GeneralAction generalAction)
    {
        if (generalAction != null)
        {
            GameObject item = GameObject.Find(generalAction.Item);

            PlayerAnimationManager playerAnimationManager = FindObjectOfType<PlayerAnimationManager>();
            Animator animator;

            if (playerAnimationManager != null)
            {
                animator = playerAnimationManager.GetComponent<Animator>();

                if (animator)
                {
                    animator.SetTrigger(generalAction.Action);
                    animator.SetTrigger("S " + generalAction.Action);
                    actionManager.OnGeneralAction();
                }
            }
            else if (item != null)
            {
                animator = item.GetComponent<Animator>();

                if (animator)
                {
                    animator.SetTrigger(generalAction.Action);
                    actionManager.OnGeneralAction();
                }
            }
        }
    }

    public void UpdateWalkToGroupButtons()
    {
        if (WTGButtons == null)
            return;
        if (WTGButtons.Count == 0)
            return;

        foreach (string k in WTGButtons.Keys)
        {
            WTGButtons[k].gameObject.SetActive(false);
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
        if ((!WTGButtons["Sink"].gameObject.activeSelf || activeGroupButtons <= 2) && WalkToGroupPanel.transform.Find("spacer0") != null)
            WalkToGroupPanel.transform.Find("spacer0").gameObject.SetActive(false);
        if ((!WTGButtons["Patient"].gameObject.activeSelf || activeGroupButtons < 2) && WalkToGroupPanel.transform.Find("spacer2") != null)
            WalkToGroupPanel.transform.Find("spacer2").gameObject.SetActive(false);
        if ((activeGroupButtons < 2) && WalkToGroupPanel.transform.Find("spacer1") != null)
            WalkToGroupPanel.transform.Find("spacer1").gameObject.SetActive(false);
        foreach (WalkToGroup w in GameObject.FindObjectsOfType<WalkToGroup>())
        {
            w.FindNeighbors();
        }
    }

    void ShowWalkToGroupPanel()
    {
        WalkToGroupPanel.SetActive(ps.away);
    }

    public void HideTheoryTab()
    {
        GameObject.Find("PatientInfoTabs/Info/TheoryTab/Continue").gameObject.GetComponent<Button>().onClick.AddListener(
             () => GameObject.FindObjectOfType<PlayerScript>().CloseRobotUI());
    }

    // Use this for initialization
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        objectsIDsController = GameObject.FindObjectOfType<ObjectsIDsController>();
        MovementSideButtons = GameObject.Find("MovementSideButtons");

        ActionManager.generalActionDone = false;
        ActionManager.generalAction = false;
        prefs = GameObject.FindObjectOfType<PlayerPrefsManager>();
        if (prefs != null)
            practiceMode = prefs.practiceMode;
        DetailedHintPanel = GameObject.Find("DetailedHintPanel");
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
        DropRightButton.SetActive(false);
        DropLeftButton.SetActive(false);

        HideTheoryTab();

        IPad.GetComponent<Animator>().enabled = false;

        ActionManager.practiceMode = true;
        if (prefs != null)
        {
            ActionManager.practiceMode = prefs.practiceMode;
        }

        SetAEDLogic();

#if !(UNITY_EDITOR || DEVELOPMENT_BUILD)
        if(GameObject.Find("ActionsPanel") != null)
            GameObject.Find("ActionsPanel").SetActive(false);
        if(GameObject.Find("AssetDebugPanel") != null)
            GameObject.Find("AssetDebugPanel").SetActive(false);
            
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
            }
        }
        UpdateWalkToGroupButtons();
        UpdateWalkToGtoupUI(true);

        foreach (InteractableObject o in Resources.FindObjectsOfTypeAll<InteractableObject>())
        {
            o.assetSource = InteractableObject.AssetSource.Included;
        }
        WalkToGroupPanel.SetActive(false);
        Invoke("ShowWalkToGroupPanel", 0.5f);
    }

    public HighlightObject AddHighlight(Transform target, string prefix, HighlightObject.type hl_type = HighlightObject.type.NoChange, float startDelay = 0, float LifeTime = float.PositiveInfinity)
    {
        string hl_name = prefix + "_" + target.name;
        if (GameObject.Find(hl_name) != null)
            return null;
        //------------

        // assets/resources/necessaryprefabs

        GameObject hl_obj = Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/HighlightObject"), target.position, new Quaternion()) as GameObject;

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
        if (prefs != null)
            practiceMode = prefs.practiceMode;
        if (!practiceMode)
            return;

        List<string> newHLObjects = new List<string>();

        string prefix = "helpHL";

        if (!handsInventory.LeftHandEmpty())
            RemoveHighlight(prefix, handsInventory.leftHandObject.name);

        if (!handsInventory.RightHandEmpty())
            RemoveHighlight(prefix, handsInventory.rightHandObject.name);

        foreach (Action a in actionManager.IncompletedActions)
        {
            string[] ObjectNames = new string[0];
            a.ObjectNames(out ObjectNames);

            //if (a.Type == ActionManager.ActionType.ObjectUse ||
            //a.Type == ActionManager.ActionType.ObjectDrop)
            foreach (string objectToUse in ObjectNames)
            {
                if (GameObject.Find(objectToUse) != null)
                {
                    HighlightObject.type hl_type = HighlightObject.type.NoChange;
                    if (GameObject.Find(objectToUse).GetComponent<WorkField>() != null)
                    {
                        hl_type = HighlightObject.type.Hand;
                    }
                    if (handsInventory.IsInHand(GameObject.Find(objectToUse)))
                        continue;
                    HighlightObject h = AddHighlight(GameObject.Find(objectToUse).transform, prefix, hl_type, 2f + Random.Range(0f, 0.5f));
                    if (h != null)
                    {
                        h.setGold(true);
                    }
                    newHLObjects.Add(objectToUse);
                }
                else
                {
                    GameObject usableHL = null;

                    foreach (UsableObject u in GameObject.FindObjectsOfType<UsableObject>())
                    {
                        if (u.PrefabToAppear == objectToUse && u.PrefabToAppear != "")
                        {
                            usableHL = u.gameObject;
                            break;
                        }
                    }
                    if (usableHL != null)
                    {
                        HighlightObject h = AddHighlight(usableHL.transform, prefix, HighlightObject.type.NoChange, 2f + Random.Range(0f, 0.5f));
                        if (h != null)
                        {
                            h.setGold(true);
                        }
                        newHLObjects.Add(usableHL.name);
                    }
                    else
                    {
                        foreach (PickableObject p in GameObject.FindObjectsOfType<PickableObject>())
                        {
                            if (p.prefabInHands == objectToUse && p.prefabInHands != "")
                            {
                                HighlightObject h = AddHighlight(p.transform, prefix, HighlightObject.type.NoChange, 2f + Random.Range(0f, 0.5f));
                                if (h != null)
                                {
                                    h.setGold(true);
                                }
                                newHLObjects.Add(p.name);
                            }
                        }
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
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(1f, 0f, 0f);
        style.fontSize = 30;


        GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString(), style);
        if (objectsIDsController != null)
        {
            if (objectsIDsController.cheat)
                GUI.Label(new Rect(30, 0, 100, 100), "Cheat enabled", style);
        }

        //debugSS = PlayerAnimationManager.animTimeout.ToString();
        GUI.Label(new Rect(0, 30, 1000, 100), debugSS, style);
#endif
    }

    public void DropFromHand(bool leftHand = true)
    {
        PickableObject item = null;
        if (leftHand && !handsInventory.LeftHandEmpty())
            item = handsInventory.leftHandObject;
        else if (!leftHand && !handsInventory.RightHandEmpty())
            item = handsInventory.rightHandObject;
        if (item != null)
        {
            GameObject ghost = null;
            List<PickableObject> ghosts = item.ghostObjects.OrderBy(x =>
                    Vector3.Distance(x.transform.position, ps.transform.position)).ToList();

            ghost = ghosts[0].gameObject;
            if (leftHand)
                handsInventory.DropLeft(ghost);
            else
                handsInventory.DropRight(ghost);

            for (int i = item.ghostObjects.Count - 1; i >= 0; --i)
            {
                GameObject g = item.ghostObjects[i].gameObject;
                item.ghostObjects.RemoveAt(i);
                Destroy(g);
            }
        }
        ActionManager.UpdateRequirements();
    }

    public void UpdateButtonsBlink()
    {
        bool practiceMode = true;
        if (prefs != null)
            practiceMode = prefs.practiceMode;
        if (!practiceMode)
            return;

        foreach (ItemControlButton b in GameObject.FindObjectsOfType<ItemControlButton>())
        {
            b.UpdateBlinkState();
        }
        foreach (ButtonBlinking b in GameObject.FindObjectsOfType<ButtonBlinking>())
        {
            b.UpdateButtonState();
        }
    }

    public int FindDirection(string neededWalkToGroup, WalkToGroup startWalkToGtoup, int direction)
    {
        if (startWalkToGtoup.name == neededWalkToGroup)
            return 0;
        WalkToGroup l = null;
        WalkToGroup r = null;
        if (startWalkToGtoup.LeftWalkToGroup != null)
            l = startWalkToGtoup.LeftWalkToGroup;
        if (startWalkToGtoup.RightWalkToGroup != null)
            r = startWalkToGtoup.RightWalkToGroup;

        if (direction != 1 && l != null)
        {
            if (l.name == neededWalkToGroup)
                return -1;
            else
            {
                int a = FindDirection(neededWalkToGroup, l, -1);
                if (a != 0)
                    return a;
            }
        }

        if (direction != -1 && r != null)
        {
            if (r.name == neededWalkToGroup)
                return 1;
            else
            {
                int a = FindDirection(neededWalkToGroup, r, 1);
                if (a != 0)
                    return a;
            }
        }

        return 0;
    }

    public void ShowTheory(bool isSequence = false)
    {
        void ShowIPad()
        {
            if (!string.IsNullOrEmpty(actionManager.Message))
            {
                GameObject.FindObjectOfType<PlayerScript>().OpenRobotUI();
                GameObject.FindObjectOfType<GameUI>().theoryPanel.SetActive(true);
                GameObject.FindObjectOfType<GameUI>().theoryPanel.transform.Find("ScrollViewMessege/Viewport/Content/Title").GetComponent<Text>().text = actionManager.MessageTitle;
                GameObject.FindObjectOfType<GameUI>().theoryPanel.transform.Find("ScrollViewMessege/Viewport/Content/Message").GetComponent<Text>().text = actionManager.Message;
            }
        }

        if (actionManager.ShowTheory)
        {
            startTimer = true;
        }

        if (startTimer)
        {
            targetTime -= Time.deltaTime;
        }

        if (targetTime <= 0.0f)
        {
            ShowIPad();
            startTimer = false;
            targetTime = 0.7f;
        }
        else if (isSequence && actionManager.ShowTheory)
        {
            ShowIPad();
            actionManager.Message = null;
        }

        actionManager.ShowTheory = false;
    }

    public void PlaceTalkBubble(GameObject person)
    {
        if (person == null)
            return;
        PersonObject personObject = person.GetComponent<PersonObject>();
        if (personObject == null)
            return;
        if (personObject.TalkBubbleAnchor == null)
            return;
        WalkToGroup near = ActionManager.NearestWalkToGroup(person);
        if (ps.away)
            return;
        if (ps.currentWalkPosition == near)
        {
            TalkBubble.SetActive(true);
            TalkBubble.GetComponent<TutorialHintsN>().WorldObject = personObject.TalkBubbleAnchor;
            TalkBubble.GetComponent<TutorialHintsN>().Update();
            PersonToTalk = personObject;
        }
    }

    void Update()
    {
        if (!timeOutEnded)
        {
            startTimeOut -= Time.deltaTime;
            if (startTimeOut < 0)
            {
                timeOutEnded = true;

                ActionManager.BuildRequirements();
                ActionManager.UpdateRequirements();

                if (actionManager.CheckGeneralAction() == null)
                {
                    UpdateHelpHighlight();
                }
                UpdateWalkToGtoupUI(true);
            }
        }
        //UpdateWalkToGtoupUI(false);
        if (toDelayUpdateHint)
        {
            if (current_UpdateHintDelay > 0)
            {
                current_UpdateHintDelay -= Time.deltaTime;
            }
            else
            {
                toDelayUpdateHint = false;
                UpdateHintPanel(null);
            }
        }

        //Don't show object control panel if animation is playing
        //if animation is longer than 0.2 (is not hold animation)
        bool animationUiBlock = true;

        if (allowObjectControlUI)
        {
            animationUiBlock = !PlayerAnimationManager.IsLongAnimation();
        }

        if (donePanelYesNo.activeSelf)
        {
            ItemControlPanel.SetActive(false);
            return;
        }
        //to show object control panel if no animation block and action block
        bool showItemControlPanel = allowObjectControlUI && animationUiBlock;

        //if some object was added or removed to hands
        if (showItemControlPanel)
        {
            ShowTheory();
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
                decombineButton.SetActive(false);
                decombineButton_right.SetActive(false);
                ActionManager.UpdateRequirements();
                if (actionManager.CheckGeneralAction() == null)
                {
                    UpdateHelpHighlight();
                }
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
                    DropLeftButton.SetActive(true);
                    if (handsInventory.leftHandObject.GetComponent<ExaminableObject>() != null)
                        showZoomLeft = true;
                    if (actionManager.CompareUseOnInfo(handsInventory.leftHandObject.name, "", "", true))
                    {
                        showNoTarget = true;
                        noTargetButton.transform.GetChild(0).GetComponent<Text>().text =
                            actionManager.CurrentButtonText(handsInventory.leftHandObject.name, true);
                    }
                    if (REmpty)
                    {
                        bool show_decomb_left = actionManager.CompareCombineObjects(handsInventory.leftHandObject.name, "", true);

                        if (!practiceMode)
                            show_decomb_left = true;

                        if (objectsIDsController != null)
                        {
                            if (objectsIDsController.cheat)
                                show_decomb_left = true;
                        }

                        decombineButton.SetActive(show_decomb_left && !showNoTarget);
                        decombineButton.GetComponent<Animator>().SetTrigger("BlinkOn");
                        decombineButton.transform.GetChild(0).GetComponent<Text>().text =
                        (actionManager.CompareCombineObjects(handsInventory.leftHandObject.name, "", true)) ?
                            actionManager.CurrentDecombineButtonText(handsInventory.leftHandObject.name, true)
                            : "Openen";
                    }
                    else
                    {
                        decombineButton.SetActive(false);
                    }
                }
                else
                {
                    DropLeftButton.SetActive(false);
                }
                if (!REmpty)
                {
                    DropRightButton.SetActive(true);

                    if (handsInventory.rightHandObject.GetComponent<ExaminableObject>() != null)
                        showZoomRight = true;
                    if (actionManager.CompareUseOnInfo(handsInventory.rightHandObject.name, "", "", true))
                    {
                        showNoTarget_right = true;
                        noTargetButton_right.transform.GetChild(0).GetComponent<Text>().text =
                           actionManager.CurrentButtonText(handsInventory.rightHandObject.name, true);
                    }
                    if (LEmpty)
                    {
                        bool show_decomb_right = actionManager.CompareCombineObjects(handsInventory.rightHandObject.name, "", true);
                        if (!practiceMode)
                            show_decomb_right = true;
                        if (objectsIDsController != null)
                        {
                            if (objectsIDsController.cheat)
                                show_decomb_right = true;
                        }
                        decombineButton_right.SetActive(show_decomb_right && !showNoTarget_right);
                        decombineButton_right.transform.GetChild(0).GetComponent<Text>().text =
                        (actionManager.CompareCombineObjects("", handsInventory.rightHandObject.name, true)) ?
                            actionManager.CurrentDecombineButtonText(handsInventory.rightHandObject.name, true)
                            : "Openen";
                    }
                    else
                    {
                        decombineButton_right.SetActive(false);
                    }
                }
                else
                {
                    DropRightButton.SetActive(false);
                }
                zoomButtonLeft.SetActive(showZoomLeft);
                zoomButtonRight.SetActive(showZoomRight);
                noTargetButton.SetActive(showNoTarget);
                noTargetButton_right.SetActive(showNoTarget_right || (ActionManager.generalAction && !ActionManager.generalActionDone));
                combineButton.SetActive(showCombin);
            }

            if (!currentItemControlPanelState && showItemControlPanel)
            {
                cooldownTime = 1.0f;
            }
            lastCooldownTime = cooldownTime;
            if (cooldownTime > 0)
            {
                cooldownTime -= Time.deltaTime;
            }
            currentItemControlPanelState = showItemControlPanel;
            if (cooldownTime > 0)
            {
                showItemControlPanel = false;

            }
            if (PlayerScript.actionsLocked)
                showItemControlPanel = false;
            ItemControlPanel.SetActive(showItemControlPanel);
            MovementSideButtons.SetActive(showItemControlPanel);

            ICPCurrentState = ItemControlPanel.activeSelf;
        }

        currentAnimLock = animationUiBlock;
        if (!showItemControlPanel)
            currentAnimLock = false;
    }

    public void ShowNoTargetButton()
    {
        ActionManager.generalAction = true;
        noTargetButton_right.SetActive(true);
        noTargetButton_right.transform.GetChild(0).GetComponent<Text>().text =
            actionManager.CurrentButtonText();
    }

    public void UpdateWalkToGtoupUI(bool value)
    {
        allowObjectControlUI = value;
        if (!value)
            cooldownTime = 1.0f;
        MovementSideButtons.SetActive(false);
        if (!allowObjectControlUI && !LeftSideButton.gameObject.activeSelf && !RightSideButton.gameObject.activeSelf &&
            !WalkToGroupPanel.activeSelf && !ItemControlPanel.activeSelf)
        {
            return;
        }
        if (cameraMode != null)
            if (cameraMode.currentMode == CameraMode.Mode.ObjectPreview)
            {
                value = false;
            }
        if (!value)
        {
            LeftSideButton.gameObject.SetActive(false);
            RightSideButton.gameObject.SetActive(false);
            WalkToGroupPanel.SetActive(false);
            ItemControlPanel.SetActive(false);
        }
        else
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
                foreach (WalkToGroupButton b in GameObject.FindObjectsOfType<WalkToGroupButton>())
                {
                    b.UpdateHint();
                }
            }
        }
        UpdateButtonsBlink();
    }


    public void OpenDonePanelYesNo()
    {
        donePanelYesNo.SetActive(true);
    }

    public void DonePanelYes()
    {
        actionManager.OnUseAction("PaperAndPen");
        donePanelYesNo.SetActive(false);
    }

    public void DonePanelNo()
    {
        donePanelYesNo.SetActive(false);
    }

    public void ClearHintPanel()
    {
        if (DetailedHintPanel.transform.Find("HintContainer") != null)
        {
            Transform panel = DetailedHintPanel.transform.Find("HintContainer").transform;
            for (int i = 0; i < panel.childCount; ++i)
            {
                Destroy(panel.GetChild(i).gameObject);
            }
        }
    }

    public void SetHintPanelAlpha(float alpha)
    {
        Color panelColor = DetailedHintPanel.GetComponent<Image>().color;
        panelColor.a = alpha;
        DetailedHintPanel.GetComponent<Image>().color = panelColor;
        foreach (Text t in DetailedHintPanel.GetComponentsInChildren<Text>())
        {
            Color c = t.color;
            c.a = alpha;
            t.GetComponent<Text>().color = c;
        }
    }

    public void UpdateHintPanel(List<ActionManager.StepData> subTasks, float UpdateHintDelay = 0f)
    {
        if (subTasks == null)
            subTasks = Current_SubTasks;
        else
        {
            //Current_SubTasks.Clear();
            Current_SubTasks = subTasks;
        }
        if (UpdateHintDelay > 0)
        {
            current_UpdateHintDelay = UpdateHintDelay;
            toDelayUpdateHint = true;
            return;
        }
        if (current_UpdateHintDelay > 0)
            return;

        ClearHintPanel();
        Text hintText;
        Text subTaskText;

        for (int i = 0; i < actionManager.CurrentDescription.Count; i++)
        {
            GameObject currentHintPanel = null;

            currentHintPanel = Instantiate<GameObject>(Resources.Load<GameObject>("NecessaryPrefabs/UI/HintPanel"),
                DetailedHintPanel.transform.Find("HintContainer").transform);
            currentHintPanel.name = "HintPanel";
            hintText = currentHintPanel.transform.Find("Text").gameObject.GetComponent<Text>();
            hintText.text = (actionManager.CurrentActionType == ActionManager.ActionType.SequenceStep) ?
                "Wat ga je doen?" : actionManager.CurrentDescription[i];

            for (int y = 0; y < subTasks.Count; y++)
            {
                if (subTasks[y].subindex == i)
                {
                    if (Resources.Load<GameObject>("NecessaryPrefabs/UI/SubtaskHints") != null)
                    {
                        if (!subTasks[y].completed)
                        {
                            GameObject subtaskPanel = Instantiate<GameObject>(Resources.Load<GameObject>("NecessaryPrefabs/UI/SubtaskHints"), currentHintPanel.transform);
                            subTaskText = subtaskPanel.transform.Find("Text").GetComponent<Text>();
                            subTaskText.text = subTasks[y].requirement;
                        }
                    }
                }
            }
            float alpha = DetailedHintPanel.GetComponent<Image>().color.a;
            SetHintPanelAlpha(alpha);
        }
    }

    private void SetAEDLogic()
    {
        if (SceneManager.GetActiveScene().name == "Scenes_AED")
        {
            Animator playerAnimator;
            PlayerAnimationManager playerAnimationManager = GameObject.FindObjectOfType<PlayerAnimationManager>();

            if (playerAnimationManager != null)
            {
                playerAnimator = playerAnimationManager.GetComponent<Animator>();

                if (noTargetButton_right != null && playerAnimator != null)
                {
                    noTargetButton_right.SetActive(true);
                    noTargetButton_right.gameObject.GetComponent<Button>().onClick.AddListener(() => playerAnimator.SetTrigger("Start_AED_SQ1"));
                    noTargetButton_right.transform.GetChild(0).GetComponent<Text>().text = "Help de cliënt";
                }
            }
        }
    }
}
