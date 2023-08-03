using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CAInput : MonoBehaviour
{
    InputField field;
    public bool isPassword = false;
    // Start is called before the first frame update
    void Start()
    {
        field = gameObject.GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (field.isFocused)
        {
            CAScreenKeyboard keyboard = GameObject.FindObjectOfType<CAScreenKeyboard>();
            if (keyboard != null)
            {
                string placeholderText = "";
                if (field.placeholder != null)
                {
                    placeholderText = field.placeholder.GetComponent<Text>().text;

                }
                keyboard.Popup(field, isPassword, placeholderText);
            }
        }
    }
}
