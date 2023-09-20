using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TalkSelectionOptions : MonoBehaviour
{
    public List<Button> optionButtons = new List<Button>();

    public void TalkActionButtonClicked(int buttonIndex)
    {
        Button currentButton = optionButtons[buttonIndex];
        if (currentButton.GetComponent<ActionTrigger>() != null)
        {
            currentButton.GetComponent<ActionTrigger>().GetComponentInChildren<ActionCollider>().RayTriggerAction();
        }
    }

    public void AddOptions(List<SelectDialogue.DialogueOption> optionList)
    {
        if (optionList.Count == 0)
        {
            Debug.LogError("0 options inited.");
            return;
        }
        
        for (int i = 0; i < 4; i++)
        {
            if (i < optionList.Count)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = optionList[i].text;
                optionButtons[i].GetComponent<ActionTrigger>().LeftActionManagerObject = optionList[i].attribute;
            }
            else
                optionButtons[i].gameObject.SetActive(false);
        }

    }

}
