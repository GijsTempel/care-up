using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using UnityEditor.Animations;

public class PlayerSpawn : MonoBehaviour {

    public string quizName;
    public GameObject playerPrefab;
    public UnityEditor.Animations.AnimatorController animationController = null;
    public Vector3 robotPosition;
    public Vector3 robotRotation;

    [System.Serializable]
    public struct InfoPair
    {
        public string buttonName;
        public string prefabName;
    };

    public string prescriptionXml;
    public string patientRecordsXml;

    public List<InfoPair> infoList = new List<InfoPair>();

    void Awake()
    {
		if (GameObject.FindObjectOfType(typeof(GameUI)) == null)
		{
            // never used
			//GameObject UIPrefab = 
            Instantiate(Resources.Load("Prefabs/UI/UI") as GameObject);
	    }      

        GameObject player = Instantiate(playerPrefab,
            transform.position, transform.rotation);
        player.name = "Player";
        if (animationController != null)
        {
            player.GetComponentInChildren<Animator>().runtimeAnimatorController = animationController;
        }

        if (player.GetComponentInChildren<Animator>().runtimeAnimatorController == null) {
            player.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/PlayerAnimationController");
        }
         
       
        GameObject itemControls = Instantiate(Resources.Load("Prefabs/UI/ItemControls") as GameObject,
            transform.position, transform.rotation);
        itemControls.name = "ItemControls";

        GameObject itemDescription = Instantiate(Resources.Load("Prefabs/UI/ItemDescription") as GameObject,
            transform.position, transform.rotation);
        itemDescription.name = "ItemDescription";

        GameObject iPad = Instantiate(Resources.Load("Prefabs/ipad") as GameObject,
            transform.position + new Vector3(0, -100f, 0), transform.rotation);
        iPad.name = "ipad";
        IpadLoadXmlInfo(iPad.transform);

        GameObject robot = Instantiate(Resources.Load("Prefabs/robot") as GameObject, 
            robotPosition, Quaternion.Euler(robotRotation));
        robot.name = "robot";

        RobotUITabInfo infotab = GameObject.FindObjectOfType<RobotUITabInfo>();
        RobotUIInfoButton[] buttons = infotab.transform.Find("InfoDynamicCanvas").Find("ItemList").GetComponentsInChildren<RobotUIInfoButton>();

        foreach (RobotUIInfoButton b in buttons)
        {
            b.gameObject.SetActive(false);
        }

        for (int i = 0; i < infoList.Count && i < buttons.Length; ++i)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].GetComponentInChildren<Text>().text = infoList[i].buttonName;
            buttons[i].Set(infoList[i].prefabName);
        }

        if (quizName != "")
        {
            GameObject.FindObjectOfType<QuizTab>().Init(quizName);
        }
        else
        {
            Debug.LogWarning("Quiz file name is blank.");
        }

        iPad.transform.GetChild(0).GetChild(0).GetChild(0).Find("CloseBtn").GetComponent<Button>().onClick.AddListener(
            player.GetComponent<PlayerScript>().CloseRobotUI);

        GameObject.FindObjectOfType<GameTimer>().SetTextObject(
            GameObject.Find("RobotUI").transform.Find("TopBarUI").Find("GeneralDynamicCanvas")
            .Find("Timer").Find("Time").GetComponent<Text>());

        GameTimer.FindObjectOfType<ActionManager>().SetUIObjects(
            GameObject.Find("TopBarUI").transform.Find("GeneralDynamicCanvas").Find("Points").Find("PointsText").GetComponent<Text>(),
            GameObject.Find("TopBarUI").transform.Find("GeneralDynamicCanvas").Find("Percentage").Find("PointsText").GetComponent<Text>());

        Destroy(gameObject);
    }

    public void IpadLoadXmlInfo(Transform ipad)
    {
        Transform robotUI = ipad.Find("UI (1)/RobotUI");

        if (prescriptionXml != "")
        {
            Transform prescriptionPanel = robotUI.Find("PrescriptionTab/Panel");

            TextAsset textAsset = (TextAsset)Resources.Load("Xml/IpadInfo/" + prescriptionXml);
            XmlDocument xmlFile = new XmlDocument();
            xmlFile.LoadXml(textAsset.text);

            XmlNodeList nodes = xmlFile.FirstChild.NextSibling.ChildNodes;
            
            foreach (XmlNode node in nodes)
            {
                prescriptionPanel.Find(node.Name).GetComponent<Text>().text =
                    node.Attributes["value"].Value;
            }
        }

        if (patientRecordsXml != "")
        {
            Transform patientRecordsXmlPanel = robotUI.Find("RecordsTab/Panel");

            TextAsset textAsset = (TextAsset)Resources.Load("Xml/IpadInfo/" + patientRecordsXml);
            XmlDocument xmlFile = new XmlDocument();
            xmlFile.LoadXml(textAsset.text);

            XmlNodeList nodes = xmlFile.FirstChild.NextSibling.ChildNodes;

            foreach (XmlNode node in nodes)
            {
                patientRecordsXmlPanel.Find(node.Name).GetComponent<Text>().text =
                    node.Attributes["value"].Value;
            }
        }
    }
}
