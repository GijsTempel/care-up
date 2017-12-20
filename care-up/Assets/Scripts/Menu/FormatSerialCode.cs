using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

[RequireComponent(typeof(InputField))]
public class FormatSerialCode : MonoBehaviour {

    private InputField input;

    private void Start()
    {
        input = GetComponent<InputField>();
    }

    public void Format()
    {
        input.text = Regex.Replace(input.text, @"\s+", string.Empty);
    }
}
