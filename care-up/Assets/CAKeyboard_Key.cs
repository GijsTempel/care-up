using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CAKeyboard_Key : MonoBehaviour
{
    public string baseChar;
    public string shiftChar;
    public string fnChar;
    int mode = 0;
    CAScreenKeyboard _CAScreenKeyboard;
    Text _text;
    // Start is called before the first frame update
    void Start()
    {
        _CAScreenKeyboard = GameObject.FindObjectOfType<CAScreenKeyboard>();
        _text = transform.Find("Text").GetComponent<Text>();
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


    void UpdateView()
    {
        _text.text = GetCurrentChar();
    }
}
