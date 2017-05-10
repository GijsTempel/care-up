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

    public float sensetivity = 1.0f;

    private List<DialogueOption> options = new List<DialogueOption>();

    private Vector2 mouseState = new Vector2();
    private OptionSide currentOption = OptionSide.None;
    private Material currentMaterial;

    private bool destroy = true;

    private Renderer top;
    private Renderer bottom;
    private Renderer right;
    private Renderer left;

    private Material selectedMaterial;
    private Material defaultMaterial;
    private Material correctMaterial;

    private string text;

    private static CameraMode cameraMode;
    private static Controls controls;

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
        selectedMaterial = Resources.Load<Material>("Materials/Blue Material");
        defaultMaterial = Resources.Load<Material>("Materials/Floor Material");
        correctMaterial = Resources.Load<Material>("Materials/Object Material");

        top = transform.FindChild("Top").GetComponent<Renderer>();
        bottom = transform.FindChild("Bottom").GetComponent<Renderer>();
        right = transform.FindChild("Right").GetComponent<Renderer>();
        left = transform.FindChild("Left").GetComponent<Renderer>();

        top.material = defaultMaterial;
        bottom.material = defaultMaterial;
        right.material = defaultMaterial;
        left.material = defaultMaterial;

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

        // 1 - top
        if (options.Count == 1)
        {
            top.gameObject.SetActive(true);
            options[0].side = OptionSide.Top;
            top.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = options[0].text;
        }
        // 2 - left/right
        else if (options.Count == 2)
        {
            left.gameObject.SetActive(true);
            options[0].side = OptionSide.Left;
            left.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = options[0].text;

            right.gameObject.SetActive(true);
            options[1].side = OptionSide.Right;
            right.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = options[1].text;
        }
        // 3 - top/left/right
        else if (options.Count == 3)
        {
            top.gameObject.SetActive(true);
            options[0].side = OptionSide.Top;
            top.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = options[0].text;

            left.gameObject.SetActive(true);
            options[1].side = OptionSide.Left;
            left.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = options[1].text;

            right.gameObject.SetActive(true);
            options[2].side = OptionSide.Right;
            right.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = options[2].text;
        }
        else // all 4 sides
        {
            top.gameObject.SetActive(true);
            options[0].side = OptionSide.Top;
            top.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = options[0].text;

            left.gameObject.SetActive(true);
            options[1].side = OptionSide.Left;
            left.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = options[1].text;

            right.gameObject.SetActive(true);
            options[2].side = OptionSide.Right;
            right.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = options[2].text;

            bottom.gameObject.SetActive(true);
            options[3].side = OptionSide.Bottom;
            bottom.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = options[3].text;
        }

        if ( cheated )
        {
            ShowAnswer();
        }
    }

    void Update()
    {
        if (controls.MouseClicked() && currentOption != OptionSide.None)
        {
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

        Vector2 mouseInput = new Vector2
        (
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        UpdateMouse(tutorial_lock ? Vector2.zero : mouseInput);
    }
    
    private void UpdateMouse(Vector2 input)
    {
        mouseState += input * sensetivity * 0.1f;
        mouseState = Vector2.ClampMagnitude(mouseState, 1.0f);

        float angle = Mathf.Atan2(mouseState.y, mouseState.x);

        if (mouseState.magnitude > 0.5f)
        {
            if (angle > -Mathf.PI/4 && angle < Mathf.PI/4)
            {
                SetSelected(OptionSide.Right);
            }
            else if (angle > Mathf.PI/4 && angle < 3*Mathf.PI/4)
            {
                SetSelected(OptionSide.Top);
            }
            else if (angle < -Mathf.PI/4 && angle > -3*Mathf.PI/4 )
            {
                SetSelected(OptionSide.Bottom);
            }
            else
            {
                SetSelected(OptionSide.Left);
            }
        } 
        else
        {
            SetSelected(OptionSide.None);
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
                    currentMaterial = top.material;
                    top.material = selectedMaterial;
                }
                else
                {
                    top.material = currentMaterial;
                }
                break;
            case OptionSide.Bottom:
                if (state)
                {
                    currentMaterial = bottom.material;
                    bottom.material = selectedMaterial;
                }
                else
                {
                    bottom.material = currentMaterial;
                }
                break;
            case OptionSide.Right:
                if (state)
                {
                    currentMaterial = right.material;
                    right.material = selectedMaterial;
                }
                else
                {
                    right.material = currentMaterial;
                }
                break;
            case OptionSide.Left:
                if (state)
                {
                    currentMaterial = left.material;
                    left.material = selectedMaterial;
                }
                else
                {
                    left.material = currentMaterial;
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
                            bottom.material = correctMaterial;
                            break;
                        case OptionSide.Left:
                            left.material = correctMaterial;
                            break;
                        case OptionSide.Right:
                            right.material = correctMaterial;
                            break;
                        case OptionSide.Top:
                            top.material = correctMaterial;
                            break;
                    }
                }
            }
        }
    }
}
