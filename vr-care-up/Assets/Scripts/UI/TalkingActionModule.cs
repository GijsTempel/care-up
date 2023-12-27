using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Security.Permissions;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class TalkingActionModule : MonoBehaviour
{
    [SerializeField] private GameObject selectionDialogPrefab;
    [SerializeField] private GameObject notifBubblePrefab;
    private GameObject selectionDialogueInstance = null;
    private GameObject notifBubbleInstance = null;

    private ActionManager actionManager = null;

    public Transform notifBubbleAnchor;

    public string dialogueXml;
    private List<SelectDialogue.DialogueOption> optionsList;
    private ActionExpectant actionExpectant;
    public static TalkingActionModule latestCaller = null;
    private PlayerScript player;
    private string currentPlayerWTG = "";
    private bool toShowDialogue = false;
    public ActionTrigger notifBubbleActionTrigger;
    public ActionModule_ActionTriggerIgniter actionTriggerIgniter;
    public bool debugElement = false;

    void Start()
    {
        actionExpectant = GetComponent<ActionExpectant>();
        player = GameObject.FindObjectOfType<PlayerScript>();
        actionManager = GameObject.FindAnyObjectByType<ActionManager>();
        optionsList = new List<SelectDialogue.DialogueOption>();

        if (dialogueXml != "")
            LoadDialogueOptions(dialogueXml);
        else
            LoadDialogueOptions("Greeting");

        if (notifBubbleInstance == null)
        {
            Transform head = GameObject.Find("HeadTriggerRaycast").transform;
            notifBubbleInstance = GameObject.Instantiate<GameObject>(notifBubblePrefab, notifBubbleAnchor);
            notifBubbleInstance.transform.localRotation = Quaternion.identity;
            notifBubbleInstance.SetActive(false);
            UnityEngine.UI.Button button = 
                notifBubbleInstance.GetComponentInChildren<UnityEngine.UI.Button>();
            
            if (button != null)
            {
                button.onClick.AddListener(delegate { Debug.Log("delegateButtonClickTest"); this.ShowChatOptions(); });
                if (notifBubbleActionTrigger != null)
                    button.onClick.AddListener(delegate { notifBubbleActionTrigger.AttemptTrigger(); } );
            }

            HoveringRayButtonCollider hoverButton =
                notifBubbleInstance.GetComponentInChildren<HoveringRayButtonCollider>();
            if (hoverButton != null)
            {
                hoverButton.triggerButton.AddListener(delegate { Debug.Log("delegateButtonClickTest"); this.ShowChatOptions(); });
                if (notifBubbleActionTrigger != null)
                    hoverButton.triggerButton.AddListener(delegate { notifBubbleActionTrigger.AttemptTrigger(); } );
            }

            //---------------------------------------------
            selectionDialogueInstance = Object.Instantiate(selectionDialogPrefab, notifBubbleAnchor) as GameObject;
            selectionDialogueInstance.name = "SelectionDialogueTalk";
            selectionDialogueInstance.transform.localPosition = new Vector3();
            selectionDialogueInstance.transform.localRotation = Quaternion.identity;
            SelectDialogue dialogue = selectionDialogueInstance.GetComponent<SelectDialogue>();
            dialogue.actionType = ActionManager.ActionType.PersonTalk;
            dialogue.actionTriggerIgniter = actionTriggerIgniter;
            dialogue.AddOptions(optionsList);
        }
    }

    void ShowChatOptions(bool toShow = true)
    {
        toShowDialogue = true;
    }

    void Update()
    {
        if (player == null)
            player = GameObject.FindObjectOfType<PlayerScript>();

        if (player.currentWTGName != currentPlayerWTG)
        {
            toShowDialogue = false;
        }

        UpdateNotifBubble();
        currentPlayerWTG = player.currentWTGName;
    }


    void UpdateNotifBubble()
    {

        if (actionExpectant.isCurrentAction)
        {
            notifBubbleInstance.SetActive(!toShowDialogue);
            if (selectionDialogueInstance != null)
                selectionDialogueInstance.SetActive(toShowDialogue);
        }
        else
        {
            notifBubbleInstance.SetActive(false);
            if (selectionDialogueInstance != null)
                selectionDialogueInstance.SetActive(false);
        }
    }

    protected void LoadDialogueOptions(string filename)
    {
        optionsList.Clear();

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/PersonDialogues/" + filename);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList xmlOptions = xmlFile.FirstChild.NextSibling.ChildNodes;

        int count = 0;
        foreach (XmlNode xmlOption in xmlOptions)
        {
            string description = xmlOption.Attributes["text"].Value;
            string topic = xmlOption.Attributes["topic"] != null ? xmlOption.Attributes["topic"].Value : "";
            string audio = xmlOption.Attributes["audio"] != null ? xmlOption.Attributes["audio"].Value : "";

            //if (count < 3) // 3 options max, 4 is Close.
            //{
                SelectDialogue.DialogueOption option = new SelectDialogue.DialogueOption(description, Blank, topic, audio);
                optionsList.Add(option);
                ++count;
            //}
            //else
            //{
            //    break;
            //}
        }

        // for leave option
        //optionsList.Add(new SelectDialogue.DialogueOption("Verlaten", DialoqueTalk, "CM_Leave", ""));
    }

    private void Blank(string s, List<SelectDialogue.DialogueOption> dialogueOption = null, string question = null, string audio = "") { }

}
