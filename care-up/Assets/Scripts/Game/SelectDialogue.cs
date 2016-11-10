using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectDialogue : MonoBehaviour {
    
    public class DialogueOption
    {
        public delegate void OptionAction(string debugMSG);

        public string text;
        public OptionAction function;
        public OptionSide side;

        public DialogueOption(string txt, OptionAction func)
        {
            text = txt;
            function = func;
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

    private Renderer top;
    private Renderer bottom;
    private Renderer right;
    private Renderer left;

    private Material selectedMaterial;
    private Material defaultMaterial;

    void Start()
    {
        transform.position = Camera.main.transform.position 
            + Camera.main.transform.forward * 3.0f;
        transform.rotation = Camera.main.transform.rotation;

        selectedMaterial = Resources.Load<Material>("Materials/Object Material");
        defaultMaterial = Resources.Load<Material>("Materials/Blue Material");

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

        AddOptions();
        InitOptions();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnOptionClick(string debugMSG)
    {
        Debug.Log(debugMSG + " clicked");
    }

    public void AddOptions()
    {
        // comment out to test 1-2-3-4 options
        // if more then 4 - it will take 1st four as options
        options.Add(new DialogueOption("Option 1", OnOptionClick));
        options.Add(new DialogueOption("Option 2", OnOptionClick));
        options.Add(new DialogueOption("Option 3", OnOptionClick));
        //options.Add(new DialogueOption("Option 4", OnOptionClick));
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
            top.transform.GetChild(0).GetComponent<TextMesh>().text = options[0].text;
        }
        else if (options.Count == 2)
        {
            left.gameObject.SetActive(true);
            options[0].side = OptionSide.Left;
            left.transform.GetChild(0).GetComponent<TextMesh>().text = options[0].text;

            right.gameObject.SetActive(true);
            options[1].side = OptionSide.Right;
            right.transform.GetChild(0).GetComponent<TextMesh>().text = options[1].text;
        }
        else if (options.Count == 3)
        {
            top.gameObject.SetActive(true);
            options[0].side = OptionSide.Top;
            top.transform.GetChild(0).GetComponent<TextMesh>().text = options[0].text;

            left.gameObject.SetActive(true);
            options[1].side = OptionSide.Left;
            left.transform.GetChild(0).GetComponent<TextMesh>().text = options[1].text;

            right.gameObject.SetActive(true);
            options[2].side = OptionSide.Right;
            right.transform.GetChild(0).GetComponent<TextMesh>().text = options[2].text;
        }
        else
        {
            top.gameObject.SetActive(true);
            options[0].side = OptionSide.Top;
            top.transform.GetChild(0).GetComponent<TextMesh>().text = options[0].text;

            left.gameObject.SetActive(true);
            options[1].side = OptionSide.Left;
            left.transform.GetChild(0).GetComponent<TextMesh>().text = options[1].text;

            right.gameObject.SetActive(true);
            options[2].side = OptionSide.Right;
            right.transform.GetChild(0).GetComponent<TextMesh>().text = options[2].text;

            bottom.gameObject.SetActive(true);
            options[3].side = OptionSide.Bottom;
            bottom.transform.GetChild(0).GetComponent<TextMesh>().text = options[3].text;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0) && currentOption != OptionSide.None)
        {
            DialogueOption option = options.Find(x => x.side == currentOption);
            if ( option != null )
            {
                option.function(option.text);
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
