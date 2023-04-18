using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetStateButton : MonoBehaviour
{
    public void CopyToClipboard()
    {
        var textEditor = new TextEditor();
        string str = transform.Find("Text").GetComponent<Text>().text;
        textEditor.text = str;
        textEditor.SelectAll();
        textEditor.Copy();
    }
    
}
