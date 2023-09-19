using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TalkSelectionOptions : MonoBehaviour
{
    public List<Button> optionButtons = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    public void CorrectDialogue()
    {

    }

    public void IncorrectDialogue()
    {

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
            if (i < optionButtons.Count)
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
