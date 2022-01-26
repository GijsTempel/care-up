using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheoryTab : MonoBehaviour
{
    public Text Title;
    public Text Descr;
    Button ContinueButton;

    public void Show(bool value)
    {
        gameObject.SetActive(value);
        if (value && PlayerPrefsManager.simulatePlayerActions)
            Invoke("AutoContinue", 5f);
    }

    public void ShowTheory(string title_text, string descr_text, string buttonText = "")
    {
        Show(true);
        Title.text = title_text;
        Descr.text = descr_text;
    }

    void AutoContinue()
    {
        ContinueButton = transform.Find("panel/quizElements/Continue").GetComponent<Button>();
        if (ContinueButton != null)
            if (ContinueButton.gameObject.activeSelf)
                ContinueButton.onClick.Invoke();
    }
}
