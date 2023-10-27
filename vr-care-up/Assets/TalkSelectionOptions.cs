using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TalkSelectionOptions : MonoBehaviour
{
    public List<Button> optionButtons = new List<Button>();
    public ActionExpectant actionExpectant;
    public GameObject line2;
    public VerticalLayoutGroup answerPanelVerticalLayoutGroup;

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
                optionButtons[i].transform.Find("Text").GetComponent<Text>().text = optionList[i].text;
                optionButtons[i].GetComponent<ActionTrigger>().LeftActionManagerObject = optionList[i].attribute;
                if (optionList[i].attribute != "")
                    optionButtons[i].onClick.AddListener(TriggerExpectant);
            }
            else
                optionButtons[i].gameObject.SetActive(false);
        }
        if (line2 != null)
            line2.SetActive(optionList.Count > 2);
        if (optionList.Count <= 2)
        {
            answerPanelVerticalLayoutGroup.padding.top = 50;
            answerPanelVerticalLayoutGroup.padding.bottom = 50;
        }
        else
        {
            answerPanelVerticalLayoutGroup.padding.top = 0;
            answerPanelVerticalLayoutGroup.padding.bottom = 0;
        }
    }

    public void TriggerExpectant()
    {
        if (actionExpectant != null)
        {
            Debug.Log("@TriggerExpectant():" + Random.Range(0, 99999).ToString());
            actionExpectant.TryExecuteAction();
        }
    }

}
