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
    public string walkToGroupName = "";
    private List<SelectDialogue.DialogueOption> optionsList;
    public ActionExpectant actionExpectant;
    public static TalkingActionModule latestCaller = null;
    private PlayerScript player;
    private string currentPlayerWTG = "";
    private bool toShowDialogue = false;

    void Start()
    {
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
            UnityEngine.UI.Button button = notifBubbleInstance.GetComponentInChildren<UnityEngine.UI.Button>();
            if (button != null)
                button.onClick.AddListener(delegate { Debug.Log("delegateButtonClickTest"); this.ShowChatOptions(); });


            //---------------------------------------------
            selectionDialogueInstance = Object.Instantiate(selectionDialogPrefab, notifBubbleAnchor) as GameObject;
            selectionDialogueInstance.transform.localPosition = new Vector3();
            selectionDialogueInstance.transform.localRotation = Quaternion.identity;

            TalkSelectionOptions dialogue = selectionDialogueInstance.GetComponent<TalkSelectionOptions>();
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
        // Debug.Log("@pcWTG == wtgName: " + (player.currentWTGName == walkToGroupName).ToString());
        if (player.currentWTGName == walkToGroupName && actionExpectant.isCurrentAction)
        {
            notifBubbleInstance.SetActive(!toShowDialogue);
            selectionDialogueInstance.SetActive(toShowDialogue);
        }
        else
        {
            notifBubbleInstance.SetActive(false);
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
