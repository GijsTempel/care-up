using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CAScreenKeyboard : MonoBehaviour
{
    public GameObject CA_SKBPanel;
    public InputField SKBInput;
    public Text descrTest;
    public GameObject visibilityToggle;
    public Button ShowKeyboardButton;

    public int keyboardMode = 0;
    public Sprite CAKeyboardOpenIcon;
    public Sprite CAKeyboardCloseIcon;

    CAKeyboard_Key shiftButtonA = null;
    CAKeyboard_Key shiftButtonB = null;
    CAKeyboard_Key fnButton = null;
    CAKeyboard_Key capsButton = null;

    bool shiftPressed = false;
    bool capsPressed = false;
    bool fnPressed = false;
    int caretposition;
    int selectionAPoint = 0;
    int selectionBPoint = 0;

    [SerializeField] private GameObject loginPasswordVisibilityOn = default;

    [SerializeField] private GameObject loginPasswordVisibilityOff = default;
    InputField linkedInputField = null;
    void Start()
    {
        foreach (CAKeyboard_Key b in GameObject.FindObjectsOfType<CAKeyboard_Key>())
        {
            if (b.buttonType == CAKeyboard_Key.ButtonType.shift)
            {
                if (shiftButtonA == null)
                {
                    shiftButtonA = b;
                }
                else
                {
                    shiftButtonB = b;
                }
            }
            else if (b.buttonType == CAKeyboard_Key.ButtonType.fn)
            {
                fnButton = b;
            }
            else if (b.buttonType == CAKeyboard_Key.ButtonType.caps)
            {
                capsButton = b;
            }
        }
        CA_SKBPanel.SetActive(false);
        if (PlayerPrefsManager.CAKeyboardVisible)
        {
            ShowCAKeyboard(true);
        }
    }

    void ShowCAKeyboard(bool toShow)
    {
        if (toShow)
        {
            GetComponent<Animator>().SetTrigger("Open");
            ShowKeyboardButton.GetComponent<Image>().sprite = CAKeyboardCloseIcon;
            PlayerPrefsManager.CAKeyboardVisible = true;
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Close");
            ShowKeyboardButton.GetComponent<Image>().sprite = CAKeyboardOpenIcon;
            PlayerPrefsManager.CAKeyboardVisible = false;

        }
    }
    public void ShowCAKeyboardButtonClicked()
    {
        ShowCAKeyboard(!PlayerPrefsManager.CAKeyboardVisible);
    }

    void Update()
    {
        if (SKBInput.isFocused)
        {
            caretposition = SKBInput.caretPosition;
            selectionAPoint = SKBInput.selectionAnchorPosition;
            selectionBPoint = SKBInput.selectionFocusPosition;
        }
    }

    public void KeyPressed(string _character)
    {
        if (_character.Length == 0)
            return;

        string _text = EraseSelectedSegment();
        if (_text == null)
            _text = SKBInput.text;
        else
            caretposition -= 1;
        SKBInput.text = _text.Insert(caretposition, _character);
        caretposition += _character.Length;
        if (shiftPressed && !capsPressed)
        {
            shiftPressed = false;
            shiftButtonA.HighlightKey(shiftPressed);
            shiftButtonB.HighlightKey(shiftPressed);
            keyboardMode = 0;
            UpdateAllKeys();
        }
        SKBInput.ActivateInputField();
        StartCoroutine(SetCursorPos());
    }


    void UpdateAllKeys()
    {
        foreach (CAKeyboard_Key b in GameObject.FindObjectsOfType<CAKeyboard_Key>())
        {
            b.UpdateView();
        }
    }

    string EraseSelectedSegment()
    {
        int rng = 1;
        if (selectionAPoint != selectionBPoint && SKBInput.text.Length > 0)
        {
            int a = selectionAPoint + 1;
            if (selectionBPoint < a)
                a = selectionBPoint + 1;
            rng = Mathf.Abs(selectionBPoint - selectionAPoint);
            caretposition = a;
            SKBInput.text = SKBInput.text.Remove(caretposition - 1, rng);
            return SKBInput.text;
        }
        return null;
    }

    public void FunctionKeyPressed(CAKeyboard_Key.ButtonType buttonType)
    {
        if (buttonType == CAKeyboard_Key.ButtonType.enter)
            EnterValue();

        if (buttonType == CAKeyboard_Key.ButtonType.backspace)
        {
            if (true)//caretposition > 0 && caretposition <= SKBInput.text.Length)
            {
                if (EraseSelectedSegment() == null && SKBInput.text.Length > 0 && caretposition > 0)
                    SKBInput.text = SKBInput.text.Remove(caretposition - 1, 1);
                caretposition -= 1;
                if (caretposition < 0)
                    caretposition = 0;
                SKBInput.ActivateInputField();
                StartCoroutine(SetCursorPos());
            }
        }
        else if (buttonType == CAKeyboard_Key.ButtonType.shift)
        {
            shiftPressed = !shiftPressed;
            capsPressed = false;
            fnPressed = false;
            shiftButtonA.HighlightKey(shiftPressed);
            shiftButtonB.HighlightKey(shiftPressed);
            fnButton.HighlightKey(fnPressed);
            capsButton.HighlightKey(capsPressed);
            if (shiftPressed)
                keyboardMode = 1;
            else
                keyboardMode = 0;
            UpdateAllKeys();
        }
        else if (buttonType == CAKeyboard_Key.ButtonType.fn)
        {
            shiftPressed = false;
            capsPressed = false;
            fnPressed = !fnPressed;
            shiftButtonA.HighlightKey(shiftPressed);
            shiftButtonB.HighlightKey(shiftPressed);
            fnButton.HighlightKey(fnPressed);
            capsButton.HighlightKey(capsPressed);
            if (fnPressed)
                keyboardMode = 2;
            else
                keyboardMode = 0;
            UpdateAllKeys();
        }
        else if (buttonType == CAKeyboard_Key.ButtonType.caps)
        {
            capsPressed = !capsPressed;
            shiftPressed = false;
            fnPressed = false;
            shiftButtonA.HighlightKey(shiftPressed);
            shiftButtonB.HighlightKey(shiftPressed);
            fnButton.HighlightKey(fnPressed);
            capsButton.HighlightKey(capsPressed);
            if (capsPressed)
                keyboardMode = 1;
            else
                keyboardMode = 0;
            UpdateAllKeys();
        }
        else if (buttonType == CAKeyboard_Key.ButtonType.space)
            KeyPressed(" ");
        else if (buttonType == CAKeyboard_Key.ButtonType.left)
            MoveCaretPosition(-1);
        else if (buttonType == CAKeyboard_Key.ButtonType.right)
            MoveCaretPosition(1);

    }

    void MoveCaretPosition(int dir)
    {
        caretposition = Mathf.Clamp(caretposition + dir, 0, SKBInput.text.Length);
        SKBInput.caretPosition = caretposition;
        SKBInput.ActivateInputField();
        StartCoroutine(SetCursorPos());
    }

    private IEnumerator SetCursorPos()
    {
        yield return new WaitForEndOfFrame();
        SKBInput.caretPosition = caretposition;
        SKBInput.ForceLabelUpdate();
    }

    public void UpdateKeyboardMode(int _mode = 0)
    {

    }

    public void InputChanged()
    {
        if (linkedInputField != null)
        {
            linkedInputField.text = SKBInput.text;
        }
    }

    public void Popup(InputField _input, bool isPassword, string description = "")
    {
        visibilityToggle.SetActive(isPassword);
        CA_SKBPanel.gameObject.SetActive(true);
        linkedInputField = _input;
        SKBInput.text = linkedInputField.text;
        descrTest.text = description;
        SKBInput.contentType = linkedInputField.contentType;
        SKBInput.ActivateInputField();

    }

    public void EnterValue()
    {
        linkedInputField.text = SKBInput.text;
        linkedInputField = null;
        CA_SKBPanel.gameObject.SetActive(false);
    }

    public void OnTogglePasswordVisibility()
    {
        bool passwordVisible = !(SKBInput.contentType == InputField.ContentType.Password);

        SKBInput.contentType = passwordVisible ? InputField.ContentType.Password : InputField.ContentType.Standard;
        linkedInputField.contentType = passwordVisible ? InputField.ContentType.Password : InputField.ContentType.Standard;
        loginPasswordVisibilityOn.SetActive(passwordVisible);
        loginPasswordVisibilityOff.SetActive(!passwordVisible);
        SKBInput.ForceLabelUpdate();
        linkedInputField.ForceLabelUpdate();
    }
}
