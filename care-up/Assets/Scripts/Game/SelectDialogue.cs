using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectDialogue : MonoBehaviour {
    
    public class DialogueOption
    {
        public GameObject option;
        public delegate void Function();
    };

    enum OptionSelected
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
    private OptionSelected currentOption = OptionSelected.None;

    private Renderer top;
    private Renderer bottom;
    private Renderer right;
    private Renderer left;

    private Material selectedMaterial;
    private Material defaultMaterial;

    void Start()
    {
        top = transform.FindChild("Top").GetComponent<Renderer>();
        bottom = transform.FindChild("Bottom").GetComponent<Renderer>();
        right = transform.FindChild("Right").GetComponent<Renderer>();
        left = transform.FindChild("Left").GetComponent<Renderer>();

        selectedMaterial = Resources.Load<Material>("Materials/Object Material");
        defaultMaterial = Resources.Load<Material>("Materials/Blue Material");

        top.material = defaultMaterial;
        bottom.material = defaultMaterial;
        right.material = defaultMaterial;
        left.material = defaultMaterial;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        Vector2 mouseInput = new Vector2
        (
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        UpdateMouse(mouseInput);

    }
    
    public void UpdateMouse(Vector2 input)
    {
        mouseState += input * sensetivity * 0.1f;
        mouseState = Vector2.ClampMagnitude(mouseState, 1.0f);

        float angle = Mathf.Atan2(mouseState.y, mouseState.x);

        if (mouseState.magnitude > 0.5f)
        {
            if (angle > -Mathf.PI/4 && angle < Mathf.PI/4)
            {
                SetSelected(OptionSelected.Right);
            }
            else if (angle > Mathf.PI/4 && angle < 3*Mathf.PI/4)
            {
                SetSelected(OptionSelected.Top);
            }
            else if (angle < -Mathf.PI/4 && angle > -3*Mathf.PI/4 )
            {
                SetSelected(OptionSelected.Bottom);
            }
            else
            {
                SetSelected(OptionSelected.Left);
            }
        } 
        else
        {
            SetSelected(OptionSelected.None);
        }
    }

    private void SetSelected(OptionSelected option)
    {
        if ( option != currentOption )
        {
            SetOptionTo(currentOption, false);
            SetOptionTo(option, true);
            currentOption = option;
        }
    }

    private void SetOptionTo(OptionSelected option, bool state)
    {
        switch (option)
        {
            case OptionSelected.Top:
                top.material = state ? selectedMaterial : defaultMaterial;
                break;
            case OptionSelected.Bottom:
                bottom.material = state ? selectedMaterial : defaultMaterial;
                break;
            case OptionSelected.Right:
                right.material = state ? selectedMaterial : defaultMaterial;
                break;
            case OptionSelected.Left:
                left.material = state ? selectedMaterial : defaultMaterial;
                break;
            default:
                break;
        }
    }
}
