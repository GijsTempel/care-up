using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectDialogue : MonoBehaviour {
    
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

    private bool destroy = true;

    private Renderer top;
    private Renderer bottom;
    private Renderer right;
    private Renderer left;

    private Material selectedMaterial;
    private Material defaultMaterial;

    private static CameraMode cameraMode;

    void Start()
    {
        if (cameraMode == null)
        {
            cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
            if (cameraMode == null) Debug.LogError("No camera mode found");
        }
    }

    public void Init(bool selfDestroy = true)
    {
        selectedMaterial = Resources.Load<Material>("Materials/Object Material");
        defaultMaterial = Resources.Load<Material>("Materials/Floor Material");
      
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
        options.Clear();

        destroy = selfDestroy;
    }

    public void AddOptions(List<DialogueOption> list)
    {
        foreach(DialogueOption item in list )
        {
            options.Add(item);
        }

        InitOptions();
    }

    private void InitOptions()
    {
        if (options.Count == 0)
        {
            Debug.LogError("0 options inited.");
            return;
        }

        if (options.Count == 1)
        {
            top.gameObject.SetActive(true);
            options[0].side = OptionSide.Top;
            top.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = options[0].text;
        }
        else if (options.Count == 2)
        {
            left.gameObject.SetActive(true);
            options[0].side = OptionSide.Left;
            left.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = options[0].text;

            right.gameObject.SetActive(true);
            options[1].side = OptionSide.Right;
            right.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = options[1].text;
        }
        else if (options.Count == 3)
        {
            top.gameObject.SetActive(true);
            options[0].side = OptionSide.Top;
            top.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = options[0].text;

            left.gameObject.SetActive(true);
            options[1].side = OptionSide.Left;
            left.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = options[1].text;

            right.gameObject.SetActive(true);
            options[2].side = OptionSide.Right;
            right.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = options[2].text;
        }
        else
        {
            top.gameObject.SetActive(true);
            options[0].side = OptionSide.Top;
            top.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = options[0].text;

            left.gameObject.SetActive(true);
            options[1].side = OptionSide.Left;
            left.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = options[1].text;

            right.gameObject.SetActive(true);
            options[2].side = OptionSide.Right;
            right.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = options[2].text;

            bottom.gameObject.SetActive(true);
            options[3].side = OptionSide.Bottom;
            bottom.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = options[3].text;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentOption != OptionSide.None)
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

        UpdateMouse(mouseInput);
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
                top.material = state ? selectedMaterial : defaultMaterial;
                break;
            case OptionSide.Bottom:
                bottom.material = state ? selectedMaterial : defaultMaterial;
                break;
            case OptionSide.Right:
                right.material = state ? selectedMaterial : defaultMaterial;
                break;
            case OptionSide.Left:
                left.material = state ? selectedMaterial : defaultMaterial;
                break;
            default:
                break;
        }
    }
}
