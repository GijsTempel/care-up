using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Instance of dialogue with up to 4 options.
/// </summary>
public class SelectDialogue : MonoBehaviour {
    
    public bool tutorial_lock = false;
    public bool cheated = false;

    public class DialogueOption
    {
        public delegate void OptionAction(string attr);

        public string text;

        public string attribute;
        public OptionAction function;

        public OptionSide side;

        public DialogueOption(string txt, OptionAction func, string attr)
        {
            text = txt;

            function = func;
            attribute = attr;
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

    private Vector2 mouseState = new Vector2();
    private OptionSide currentOption = OptionSide.None;
    private Color currentMaterial;

    private bool destroy = true;

    private Image top;
    private Image bottom;
    private Image right;
    private Image left;

    private Color selectedMaterial;
    private Color defaultMaterial;
    private Color correctMaterial;

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
        selectedMaterial = Color.blue;
        defaultMaterial = Color.white;
        correctMaterial = Color.green;

        top = transform.GetChild(0).GetComponent<Image>();
        bottom = transform.GetChild(1).GetComponent<Image>();
        right = transform.GetChild(2).GetComponent<Image>();
        left = transform.GetChild(3).GetComponent<Image>();

        top.color = defaultMaterial;
        bottom.color = defaultMaterial;
        right.color = defaultMaterial;
        left.color = defaultMaterial;
        
        top.gameObject.SetActive(false);
        bottom.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        left.gameObject.SetActive(false);

        mouseState = Vector2.zero;
        currentOption = OptionSide.None;
        currentMaterial = defaultMaterial;
        options.Clear();

        text = "";
        cheated = false;
        tutorial_lock = false;

        destroy = selfDestroy;
    }

    public void AddOptions(List<DialogueOption> list, bool cheat = false)
    {
        foreach(DialogueOption item in list )
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

        if (options.Count > 0)
        {
            top.gameObject.SetActive(true);
            options[0].side = OptionSide.Top;
            top.transform.GetChild(0).GetComponent<Text>().text = options[0].text;
        }

        if (options.Count > 1)
        {
            bottom.gameObject.SetActive(true);
            options[1].side = OptionSide.Bottom;
            bottom.transform.GetChild(0).GetComponent<Text>().text = options[1].text;
        }

        if (options.Count > 2)
        {
            right.gameObject.SetActive(true);
            options[2].side = OptionSide.Right;
            right.transform.GetChild(0).GetComponent<Text>().text = options[2].text;
        }

        if (options.Count > 3)
        {
            left.gameObject.SetActive(true);
            options[3].side = OptionSide.Left;
            left.transform.GetChild(0).GetComponent<Text>().text = options[3].text;
        }

        if ( cheated )
        {
            ShowAnswer();
        }
    }

    void Update()
    {
        if (!tutorial_lock)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (optionSelected && currentOption != OptionSide.None)
            {
                optionSelected = false;

                DialogueOption option = options.Find(x => x.side == currentOption);
                if (option != null)
                {
                    option.function(option.attribute);
                    if (destroy)
                    {
                        Destroy(gameObject);
                        cameraMode.ToggleCameraMode(CameraMode.Mode.Free);
                    }
                }
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void SetSelected(OptionSide option)
    {
        if ( option != currentOption )
        {
            SetOptionTo(currentOption, false);
            SetOptionTo(option, true);
            currentOption = option;
        }
    }
    
    private void SetOptionTo(OptionSide option, bool state)
    {
        switch (option)
        {
            case OptionSide.Top:
                if ( state )
                {
                    currentMaterial = top.color;
                    top.color = selectedMaterial;
                }
                else
                {
                    top.color = currentMaterial;
                }
                break;
            case OptionSide.Bottom:
                if (state)
                {
                    currentMaterial = bottom.color;
                    bottom.color = selectedMaterial;
                }
                else
                {
                    bottom.color = currentMaterial;
                }
                break;
            case OptionSide.Right:
                if (state)
                {
                    currentMaterial = right.color;
                    right.color = selectedMaterial;
                }
                else
                {
                    right.color = currentMaterial;
                }
                break;
            case OptionSide.Left:
                if (state)
                {
                    currentMaterial = left.color;
                    left.color = selectedMaterial;
                }
                else
                {
                    left.color = currentMaterial;
                }
                break;
            default:
                break;
        }
    }

    public void SetText(string t)
    {
        text = t;
    }

    void OnGUI()
    {
        if (text != "")
        {
            GUIStyle style = GUI.skin.GetStyle("Label");
            style.alignment = TextAnchor.LowerCenter;
            style.fontSize = 30;
            style.normal.textColor = Color.white;

            GUI.Label(new Rect(0, 0, Screen.width, Screen.height),
                text, style);
        }
    }

    public void ShowAnswer()
    {
        if ( cheated )
        {
            foreach (DialogueOption o in options)
            {
                if ( o.attribute != "" && o.attribute != "CM_Leave")
                {
                    switch (o.side)
                    {
                        case OptionSide.Bottom:
                            bottom.color = correctMaterial;
                            break;
                        case OptionSide.Left:
                            left.color = correctMaterial;
                            break;
                        case OptionSide.Right:
                            right.color = correctMaterial;
                            break;
                        case OptionSide.Top:
                            top.color = correctMaterial;
                            break;
                    }
                }
            }
        }
    }

    public void ButtonClick(int num)
    {
        OptionSide side = OptionSide.None;

        switch (num)
        {
            case (0):
                side = OptionSide.Top;
                break;
            case (1):
                side = OptionSide.Bottom;
                break;
            case (2):
                side = OptionSide.Right;
                break;
            case (3):
                side = OptionSide.Left ;
                break;
        }

        currentOption = side;
        optionSelected = true;
    }
}
