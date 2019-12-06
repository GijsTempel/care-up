using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
//using UnityEditor.Animations;

public class PlayerSpawn : MonoBehaviour
{
    public int sceneID = 0;
    public string quizName;
    public GameObject playerPrefab;
    //public UnityEditor.Animations.AnimatorController animationController = null;
    public Vector3 robotPosition;
    public Vector3 robotRotation;
    public WalkToGroup.GroupType momentaryJumpTo = WalkToGroup.GroupType.NotSet;

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
        GameObject UIPrefab = null;
        GameObject flyHelper = Instantiate(Resources.Load("NecessaryPrefabs/UI/flyHelper") as GameObject);
        flyHelper.name = "flyHeloper";
        if (GameObject.FindObjectOfType(typeof(GameUI)) == null)
        {
            UIPrefab = Instantiate(Resources.Load("NecessaryPrefabs/UI/UI") as GameObject);
            
            UIPrefab.name = "UI";
        }
        Vector3 r = transform.rotation.eulerAngles;

        GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.Euler(0, r.y, r.z));
        Transform camTransform = player.transform.Find("CinematicControl/Arms/Armature/Hips/Spine/Spine1/Spine2/Neck/Head/Camera");
        Vector3 co = camTransform.rotation.eulerAngles;
        co.x = r.x;
        camTransform.rotation = Quaternion.Euler(co);
        player.name = "Player";

        if (GetComponent<Animator>() != null)
        {
            if (GetComponent<Animator>().runtimeAnimatorController != null)
            {
                player.GetComponentInChildren<Animator>().runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
            }
        }
        if (player.GetComponentInChildren<Animator>().runtimeAnimatorController == null)
        {
            player.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/PlayerAnimationController");
        }

        player.GetComponentInChildren<Animator>().SetInteger("sceneID", sceneID);
        player.GetComponent<PlayerScript>().momentaryJumpTo = momentaryJumpTo;
        GameObject itemControls = Instantiate(Resources.Load("NecessaryPrefabs/UI/ItemControls") as GameObject,
            transform.position, transform.rotation);
        itemControls.name = "ItemControls";

        GameObject itemDescription = Instantiate(Resources.Load("NecessaryPrefabs/UI/ItemDescription") as GameObject,
            transform.position, transform.rotation);
        itemDescription.name = "ItemDescription";

        GameObject iPad = UIPrefab.transform.Find("PatientInfoTabs").gameObject;

        //GameObject iPad = Instantiate(Resources.Load("Prefabs/UI/IPad") as GameObject,
        //  transform.position, transform.rotation);
        //iPad.name = "ipad";

        IpadLoadXmlInfo(iPad.transform);

        //GameObject iPad = Instantiate(Resources.Load("Prefabs/ipad") as GameObject,
        //    transform.position + new Vector3(0, -100f, 0), transform.rotation);
        //iPad.name = "ipad";
        //IpadLoadXmlInfo(iPad.transform);

        GameObject robot = Instantiate(Resources.Load("NecessaryPrefabs/robot") as GameObject,
            robotPosition, Quaternion.Euler(robotRotation));
        robot.name = "robot";

        if (quizName != "")
        {
            GameObject.FindObjectOfType<QuizTab>().Init(quizName);
        }
        else
        {
            Debug.LogWarning("Quiz file name is blank.");
        }

        iPad.transform.GetChild(1).Find("CloseBtn").GetComponent<Button>().onClick.AddListener(
            player.GetComponent<PlayerScript>().CloseRobotUI);

        GameObject.FindObjectOfType<GameTimer>().SetTextObject(
            GameObject.Find("PatientInfoTabs").transform.Find("TopBarUI").Find("GeneralDynamicCanvas")
            .Find("Timer").Find("Time").GetComponent<Text>());

        GameTimer.FindObjectOfType<ActionManager>().SetUIObjects(
            GameObject.Find("TopBarUI").transform.Find("GeneralDynamicCanvas").Find("Points").Find("PointsText").GetComponent<Text>(),
            GameObject.Find("TopBarUI").transform.Find("GeneralDynamicCanvas").Find("Percentage").Find("PointsText").GetComponent<Text>());

        player.GetComponent<PlayerScript>().joystickObject = GameObject.Find("FingersJoystickPrefab");

        Destroy(gameObject);
    }

    public void IpadLoadXmlInfo(Transform ipad)
    {
        Transform robotUI = ipad.Find("Info");

        if (prescriptionXml != "")
        {
            Transform prescriptionPanel = robotUI.Find("PrescriptionTab/Panel");
            Transform secondPrescriptionPanel = robotUI.Find("PrescriptionTab/Panel (1)");

            TextAsset textAsset = (TextAsset)Resources.Load("Xml/IpadInfo/" + prescriptionXml);
            XmlDocument xmlFile = new XmlDocument();
            xmlFile.LoadXml(textAsset.text);

            XmlNodeList nodes = xmlFile.FirstChild.NextSibling.ChildNodes;

            foreach (XmlNode node in nodes)
            {
                if (prescriptionPanel.Find(node.Name) != null)
                    prescriptionPanel.Find(node.Name).GetComponent<Text>().text =
                        node.Attributes["value"].Value;

                if (secondPrescriptionPanel.Find(node.Name) != null)
                    secondPrescriptionPanel.Find(node.Name).GetComponent<Text>().text =
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
                if (patientRecordsXmlPanel.Find(node.Name) != null)
                    patientRecordsXmlPanel.Find(node.Name).GetComponent<Text>().text =
                        node.Attributes["value"].Value;
            }
        }
    }
}
