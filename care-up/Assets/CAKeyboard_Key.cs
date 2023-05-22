using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CAKeyboard_Key : MonoBehaviour
{
    public string baseChar;
    public string shiftChar;
    public string fnChar;
    Color baseColor;
    Color hlColor = Color.yellow;
    //int mode = 0; value never used
    CAScreenKeyboard _CAScreenKeyboard;
    Text _text;
    public enum ButtonType
    {
        normal,
        shift,
        caps,
        fn,
        tab,
        backspace,
        enter,
        space,
        left,
        right
    };
    public ButtonType buttonType = ButtonType.normal;
    // Start is called before the first frame update
    void Start()
    {
        baseColor = GetComponent<Image>().color;
        Button b = gameObject.GetComponent<Button>();
        b.onClick.AddListener(delegate () { KeyClicked(); });
        _CAScreenKeyboard = GameObject.FindObjectOfType<CAScreenKeyboard>();
        _text = transform.Find("xText").GetComponent<Text>();
        UpdateView();
        if (shiftChar == "")
            shiftChar = baseChar.ToUpper();
    }

    public void KeyClicked()
    {
        if (buttonType == ButtonType.normal)
        {
            _CAScreenKeyboard.KeyPressed(GetCurrentChar());
        }
        else
        {
            _CAScreenKeyboard.FunctionKeyPressed(buttonType);
        }
    }

  
    public void HighlightKey(bool toHighlight = false)
    {
        if (toHighlight)
        {
            GetComponent<Image>().color = hlColor;
        }
        else
        {
            GetComponent<Image>().color = baseColor;
        }
    }

    string GetCurrentChar()
    {
        string character = baseChar;
        switch (_CAScreenKeyboard.keyboardMode)
        {
            case 1:
                character = shiftChar;
                break;
            case 2:
                character = fnChar;
                break;
            default:
                character = baseChar;
                break;
        }
        return character;
    }


    public void UpdateView()
    {
        if (buttonType == ButtonType.normal)
        {
            _text.text = GetCurrentChar();
        }
        else
        {
            _text.text = System.Enum.GetName(typeof(ButtonType), buttonType).ToUpper();
            if (buttonType == ButtonType.left)
            {
                _text.text = "\u2190";
            }
            else if (buttonType == ButtonType.right)
            {
                _text.text = "\u2192";
            }
        }
    }
}
