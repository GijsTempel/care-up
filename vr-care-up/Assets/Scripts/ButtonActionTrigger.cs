using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActionTrigger : MonoBehaviour
{

    public ActionTrigger actionTrigger;
    public TextMeshProUGUI textMeshProUGUI;

    public void SetText(string text)
    {
        textMeshProUGUI.text = text;
    }

    public void ButtonPressed()
    {
        if (actionTrigger != null)
        {
            if (actionTrigger.AttemptTrigger())
            {
                Destroy(gameObject);
            }
        }
    }
}
