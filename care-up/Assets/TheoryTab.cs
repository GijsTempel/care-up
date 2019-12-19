using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheoryTab : MonoBehaviour
{
    public Text Title;
    public Text Descr;

    public void Show(bool value)
    {
        print("FSAFASD");
        gameObject.SetActive(value);
    }

    public void ShowTheory(string title_text, string descr_text, string buttonText = "")
    {
        Show(true);
        Title.text = title_text;
        Descr.text = descr_text;
    }
}
