using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Instance of dialogue with up to 4 options.
/// </summary>
public class SelectDialogue : MonoBehaviour
{
    public bool tutorial_lock = false;
    public bool cheated = false;

    public class DialogueOption
    {
        public delegate void OptionAction(string attr = "", List<DialogueOption> additionalOption = null, string question = null);

        public string text;
        public string attribute;
        public string question;
        public OptionAction function;
        public OptionSide side;
        public List<DialogueOption> additional;

        public DialogueOption(string txt, OptionAction func, string attr, List<DialogueOption> additionalOptions = null, string questionText = null)
        {
            text = txt;
            function = func;
            attribute = attr;
            question = questionText;
            additional = additionalOptions;
        }
    };

    public enum OptionSide
    {
        None,
        Top,
        Bottom,
        Right,
        Left
    };

    private List<DialogueOption> options = new List<DialogueOption>();

    //private Vector2 mouseState = new Vector2();
    private OptionSide currentOption = OptionSide.None;
    private Color currentMaterial;

    private bool destroy = true;

    //private Image top;
    //private Image bottom;
    //private Image right;
    //private Image left;

    private Button first;
    private Button second;
    private Button third;
    private Button fourth;

    public List<Button> sqButtons;

    //private ColorBlock firstCB;
    //private ColorBlock secondCB;
    //private ColorBlock thirdCB;
    //private ColorBlock fourthCB;

    //private Color selectedMaterial;
    //private Color defaultMaterial;
    //private Color correctMaterial;
    //private Color wrongMaterial;

    private string text;

    private static CameraMode cameraMode;
    private static Controls controls;

    private bool optionSelected = false;


    void Start()
    {
        if (cameraMode == null)
        {
            cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
            if (cameraMode == null) Debug.LogError("No camera mode found");
        }
        if (controls == null)
        {
            controls = GameObject.Find("GameLogic").GetComponent<Controls>();
            if (controls == null) Debug.LogError("No controls found.");
        }
    }

    public void Init(bool selfDestroy = true)
    {
        //selectedMaterial = Color.blue;

        //defaultMaterial = Color.white;
        //correctMaterial = Color.green;
        //wrongMaterial = Color.red;

        //first = transform.Find("Image/Buttons/b0").GetComponent<Button>();
        //second = transform.Find("Image/Buttons/b1").GetComponent<Button>();
        //third = transform.Find("Image/Buttons/b2").GetComponent<Button>();
        //fourth = transform.Find("Image/Buttons/b3").GetComponent<Button>();

        //top = first.GetComponent<Image>();
        //bottom = second.GetComponent<Image>();
        //right = third.GetComponent<Image>();
        //left = fourth.GetComponent<Image>();

        //firstCB = first.colors;
        //secondCB = second.colors;
        //thirdCB = third.colors;
        //fourthCB = fourth.colors;

        //top.color = defaultMaterial;
        //bottom.color = defaultMaterial;
        //right.color = defaultMaterial;
        //left.color = defaultMaterial;

        //top.gameObject.SetActive(false);
        //bottom.gameObject.SetActive(false);
        //right.gameObject.SetActive(false);
        //left.gameObject.SetActive(false);

        //first.gameObject.SetActive(false);
        //second.gameObject.SetActive(false);
        //third.gameObject.SetActive(false);
        //fourth.gameObject.SetActive(false);

        //mouseState = Vector2.zero;
        //currentOption = OptionSide.None;
        //currentMaterial = defaultMaterial;
        //options.Clear();

        //text = "";
        //cheated = false;
        //tutorial_lock = false;

        //destroy = selfDestroy;
    }

    public void AddOptions(List<DialogueOption> list, bool cheat = false)
    {
        foreach (DialogueOption item in list)
        {
            options.Add(item);
        }
        cheated = cheat;
        InitOptions();
    }

    /// <summary>
    /// Sets options sides based on how many are there.
    /// </summary>
    private void InitOptions()
    {
        if (options.Count == 0)
        {
            Debug.LogError("0 options inited.");
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < options.Count)
            {
                sqButtons[i].gameObject.SetActive(true);
                sqButtons[i].transform.Find("Text").GetComponent<Text>().text = options[i].text;
            }
            else
                sqButtons[i].gameObject.SetActive(false);
        }
     
        //if (options.Count > 0)
        //{
        //    top.gameObject.SetActive(true);
        //    options[0].side = OptionSide.Top;
        //    top.transform.GetChild(0).GetComponent<Text>().text = options[0].text;
        //}

        //if (options.Count > 1)
        //{
        //    bottom.gameObject.SetActive(true);
        //    options[1].side = OptionSide.Bottom;
        //    bottom.transform.GetChild(0).GetComponent<Text>().text = options[1].text;
        //}

        //if (options.Count > 2)
        //{
        //    right.gameObject.SetActive(true);
        //    options[2].side = OptionSide.Right;
        //    right.transform.GetChild(0).GetComponent<Text>().text = options[2].text;
        //}

        //if (options.Count > 3)
        //{
        //    left.gameObject.SetActive(true);
        //    options[3].side = OptionSide.Left;
        //    left.transform.GetChild(0).GetComponent<Text>().text = options[3].text;
        //}

        // if (cheated)
        // {
            ShowAnswer();
        // }
    }

    static DialogueOption optionWithAdditions = null;
    static string questionWithHint = null;

    void Update()
    {
        /*
        if (!tutorial_lock)
        {
            // if (!GamepadSwitch.HandleUpdate(top.GetComponent<Button>()))
            // {
            //     Cursor.lockState = CursorLockMode.None;
            //     Cursor.visible = true;
            // }

            if (optionSelected && currentOption != OptionSide.None)
            {
                optionSelected = false;
                DialogueOption option = options.Find(x => x.side == currentOption);

                if (option != null)
                {
                    bool testingMode = false;

#if UNITY_EDITOR
                    PlayerPrefsManager playerPrefsManager = GameObject.FindObjectOfType<PlayerPrefsManager>();
                    ObjectsIDsController objectsIDsController = GameObject.FindObjectOfType<ObjectsIDsController>();
                    if (playerPrefsManager != null)
                    {
                        if (playerPrefsManager.testingMode)
                            testingMode = true;
                    }
                    if (objectsIDsController != null)
                    {
                        if (objectsIDsController.testingMode)
                            testingMode = true;
                    }
#endif
                    if (testingMode)
                    {
                        foreach (DialogueOption dialoqueOption in options)
                        {
                            if (dialoqueOption.attribute != "" && dialoqueOption.attribute != "CM_Leave")
                                option.attribute = dialoqueOption.attribute;
                        }

                        option.function(option.attribute, option.additional, option.question);

                        if (destroy)
                        {
                            Destroy(gameObject);
                            cameraMode.ToggleCameraMode(CameraMode.Mode.Free);
                        }

                        return;
                    }

                    option.function(option.attribute, option.additional, option.question);

                    if (option.attribute != "")
                    {
                        if (destroy)
                        {
                            Destroy(gameObject);
                            cameraMode.ToggleCameraMode(CameraMode.Mode.Free);
                        }
                    }

                    else // no info means wrong choice
                    {
                        switch (currentOption)
                        {
                            // case OptionSide.Bottom:
                            //     bottom.color = wrongMaterial;
                            //     secondCB.highlightedColor = new Color(255, 0, 0, 255);
                            //     second.colors = secondCB;
                            //     break;
                            // case OptionSide.Left:
                            //     left.color = wrongMaterial;
                            //     fourthCB.highlightedColor = new Color(255, 0, 0, 255);
                            //     fourth.colors = fourthCB;
                            //     break;
                            // case OptionSide.Right:
                            //     right.color = wrongMaterial;
                            //     thirdCB.highlightedColor = new Color(255, 0, 0, 255);
                            //     third.colors = thirdCB;
                            //     break;
                            // case OptionSide.Top:
                            //     top.color = wrongMaterial;
                            //     firstCB.highlightedColor = new Color(255, 0, 0, 255);
                            //     first.colors = firstCB;
                            //     break;
                        }

                        GameObject.FindObjectOfType<ActionManager>().OnSequenceStepAction("");
                        GameObject currentHintPanel = GameObject.Find("HintPanel");

                        if (!ActionManager.practiceMode)
                        {
                            if (option.question != null)
                            {
                                //dialogueTitle.text = option.question;
                            }
                        }
                        else if (currentHintPanel != null)
                        {
                            string hintText = FindObjectOfType<ActionManager>().CurrentDescription[0];

                            foreach (DialogueOption dialoqueOption in options)
                            {
                                if (dialoqueOption.additional != null)
                                {
                                    optionWithAdditions = dialoqueOption;
                                    hintText = dialoqueOption.text;
                                    break;
                                }
                            }

                            if (currentHintPanel.transform.Find("Text") != null)
                            {
                                Text hint = currentHintPanel.transform.Find("Text").gameObject.GetComponent<Text>();

                                if (optionWithAdditions != null)
                                {
                                    if (hint.text == optionWithAdditions.question || hint.text == questionWithHint)
                                    {
                                        questionWithHint = optionWithAdditions.question + " " + FindObjectOfType<ActionManager>().CurrentDescription[0];
                                        hint.text = questionWithHint;
                                    }
                                    else if (option == optionWithAdditions)
                                        hint.text = optionWithAdditions.question;
                                }                               
                                else
                                    hint.text = hintText;
                            }
                        }
                    }
                }
            }
        }
        */
    }

    // private void SetSelected(OptionSide option)
    // {
    //     if (option != currentOption)
    //     {
    //         SetOptionTo(currentOption, false);
    //         SetOptionTo(option, true);
    //         currentOption = option;
    //     }
    // }

    // private void SetOptionTo(OptionSide option, bool state)
    // {
        //switch (option)
        //{
        //    case OptionSide.Top:
        //        if (state)
        //        {
        //            currentMaterial = top.color;
        //            //top.color = selectedMaterial;
        //        }
        //        else
        //        {
        //            top.color = currentMaterial;
        //        }
        //        break;
        //    case OptionSide.Bottom:
        //        if (state)
        //        {
        //            currentMaterial = bottom.color;
        //            //bottom.color = selectedMaterial;
        //        }
        //        else
        //        {
        //            bottom.color = currentMaterial;
        //        }
        //        break;
        //    case OptionSide.Right:
        //        if (state)
        //        {
        //            currentMaterial = right.color;
        //            //right.color = selectedMaterial;
        //        }
        //        else
        //        {
        //            right.color = currentMaterial;
        //        }
        //        break;
        //    case OptionSide.Left:
        //        if (state)
        //        {
        //            currentMaterial = left.color;
        //            //left.color = selectedMaterial;
        //        }
        //        else
        //        {
        //            left.color = currentMaterial;
        //        }
        //        break;
        //    default:
        //        break;
        //}
    // }

    public void SetText(string t)
    {
        text = t;
    }

    public void ShowAnswer()
    {
        int i = 0;
        foreach (DialogueOption o in options)
        {
            print(o.attribute);
            if (o.attribute != "" && o.attribute != "CM_Leave")
            {
                sqButtons[i].GetComponent<Image>().color = Color.yellow;
            }
            i++;
        }
    }


    public void ButtonClick(int num)
    {
        print(options[num].text);
        sqButtons[num].interactable = false;

        options[num].function(options[num].attribute, options[num].additional, options[num].question);

        if (options[num].attribute != "")
        {
            if (destroy)
            {
                Destroy(gameObject);
                cameraMode.ToggleCameraMode(CameraMode.Mode.Free);
            }
        }
        // OptionSide side = OptionSide.None;

        // switch (num)
        // {
        //     case (0):
        //         side = OptionSide.Top;
        //         break;
        //     case (1):
        //         side = OptionSide.Bottom;
        //         break;
        //     case (2):
        //         side = OptionSide.Right;
        //         break;
        //     case (3):
        //         side = OptionSide.Left;
        //         break;
        // }

        // currentOption = side;
        // optionSelected = true;
    }
}
