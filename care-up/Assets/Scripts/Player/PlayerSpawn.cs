using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class PlayerSpawn : MonoBehaviour
{
    public int sceneID = 0;
    public string quizName;
    public float playerCameraFOV = -1f;
    public GameObject playerPrefab;
    public Vector3 robotPosition;
    public Vector3 robotRotation;
    public WalkToGroup.GroupType momentaryJumpTo = WalkToGroup.GroupType.NotSet;
    public bool cameraOff = false;
    [System.Serializable]
    public struct InfoPair
    {
        public string buttonName;
        public string prefabName;
    };

    [System.Serializable]
    public struct RandomEventSetup
    {
        public string eventGroups;
        public string actionRange;
    };

    public string prescriptionXml;
    public string patientRecordsXml;

    public List<InfoPair> infoList = new List<InfoPair>();

    public List<string> randomEventFiles = new List<string>();

    public List<RandomEventSetup> randomEventSetup = new List<RandomEventSetup>();

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
        Transform camTransform = player.transform.Find("CinematicControl/Arms/Armature/Hips/Spine/Spine1/Spine2/Neck/Head/PlayerMainCamera");
        Vector3 co = camTransform.rotation.eulerAngles;
        if (playerCameraFOV > 0)
            camTransform.GetComponent<Camera>().fieldOfView = playerCameraFOV;
        co.x = r.x;
        camTransform.rotation = Quaternion.Euler(co);
        player.name = "Player";
        if (cameraOff)
        {
            camTransform.GetComponent<Camera>().enabled = false;
        }
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
        if (PlayerPrefsManager.videoRecordingMode)
        {
            itemDescription.GetComponent<CanvasGroup>().alpha = 0f;
        }
        GameObject iPad = UIPrefab.transform.Find("IpadPanel/PatientInfoTabs").gameObject;
        IpadLoadXmlInfo(iPad.transform);

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
        GameObject.FindObjectOfType<RandomEventTab>().Init(randomEventFiles);

        iPad.transform.GetChild(1).Find("CloseBtn").GetComponent<Button>().onClick.AddListener(
            player.GetComponent<PlayerScript>().CloseRobotUI);

        GameObject.FindObjectOfType<GameTimer>().SetTextObject(
            GameObject.Find("PatientInfoTabs").transform.Find("TopBarUI").Find("GeneralDynamicCanvas")
            .Find("Timer").Find("Time").GetComponent<Text>());     

        player.GetComponent<PlayerScript>().joystickObject = GameObject.Find("FingersJoystickPrefab");
        for (int i = 0; i < randomEventSetup.Count; i++)
        {
            List<string> eventGroups = new List<string>();
            List<int> actionsRange = new List<int>();
            if (randomEventSetup[i].eventGroups == "")
            {
                eventGroups.Add("");
            }
            else
            {
                eventGroups.AddRange(randomEventSetup[i].eventGroups.Split(','));
            }

            if (randomEventSetup[i].actionRange == "")
            {
                ActionManager.AddRandomEventData(-1, eventGroups);
            }
            else
            {
                List<string> rangeSections = new List<string>();
                rangeSections.AddRange(randomEventSetup[i].actionRange.Split(','));
                foreach(string rangeSection in rangeSections)
                {
                    if (rangeSection.Contains(":"))
                    {
                        int a = 0;
                        int b = 0;
                        string[] rangeStringValues = rangeSection.Split(':');
                        int.TryParse(rangeStringValues[0], out a);
                        int.TryParse(rangeStringValues[1], out b);
                        for (int j = a; j <= b; j++)
                            actionsRange.Add(j);
                    }
                    else
                    {
                        int _value = -1;
                        int.TryParse(rangeSection, out _value);
                        if (_value != -1)
                        {
                            actionsRange.Add(_value);
                        }
                    }
                }
            }
            if (actionsRange.Count > 0)
            {
                ActionManager.AddRandomEventData(actionsRange[Random.Range(0, actionsRange.Count)], eventGroups);
            }

        }
        ActionManager.FinalizeRandomEventData();

        Destroy(gameObject);
    }


    List<int> RangeFromString(string rangeData)
    {
        List<int> currentRange = new List<int>();
        if (rangeData == "")
        {
            currentRange.Add(-1);
        }


        return currentRange;
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
                string lineName = "P" + node.Name;
                if (prescriptionPanel.Find(lineName) != null)
                    
                    prescriptionPanel.Find(lineName).Find(node.Name).GetComponent<Text>().text =
                        node.Attributes["value"].Value;

                if (secondPrescriptionPanel.Find(lineName) != null)
                    secondPrescriptionPanel.Find(lineName).Find(node.Name).GetComponent<Text>().text =
                        node.Attributes["value"].Value;
            }
        }

        if (patientRecordsXml != "")
        {
            Transform patientRecordsXmlPanel = robotUI.Find("RecordsTab/Panel");
            Transform patientRecordsTab = robotUI.Find("RecordsTab");
            TextAsset textAsset = (TextAsset)Resources.Load("Xml/IpadInfo/" + patientRecordsXml);
            XmlDocument xmlFile = new XmlDocument();
            xmlFile.LoadXml(textAsset.text);

            XmlNodeList nodes = xmlFile.FirstChild.NextSibling.ChildNodes;

            foreach (XmlNode node in nodes)
            {
                string lineName = "P" + node.Name;
                if (patientRecordsXmlPanel.Find(lineName) != null)
                    patientRecordsXmlPanel.Find(lineName).Find(node.Name).GetComponent<Text>().text =
                        node.Attributes["value"].Value;
                else if(patientRecordsTab.Find(node.Name) != null)
                    patientRecordsTab.Find(node.Name).GetComponent<Text>().text =
                        node.Attributes["value"].Value;
            }
        }
    }
}