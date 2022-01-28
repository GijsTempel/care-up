using System.Xml;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using CareUp.Actions;

/// <summary>
/// Handles saving/loading game state.
/// </summary>
public class SaveLoadManager : MonoBehaviour {

    private const string savePath = "/Xml/SaveFiles/";

    private bool needLoad = false;
    private string needLoadName;

    void Start()
    {
        Directory.CreateDirectory(Application.dataPath + savePath);
        SceneManager.sceneLoaded += OnLoaded;
    }


    private void Update()
    {
        //disable debug consol to record video
        if (PlayerPrefsManager.videoRecordingMode)
            Debug.developerConsoleVisible = false;
    }

    private void OnLoaded(Scene s, LoadSceneMode m)
    {
        if (needLoad)
        {
            needLoad = false;
            Load(needLoadName);
        }
    }

    public void SetNeedLoad(string filename = "continue")
    {
        needLoad = true;
        needLoadName = filename;
    }

    /// <summary>
    /// Saves info from scene at current state.
    /// </summary>
    /// <param name="filename">Filename of savefile.</param>
    public void Save(string filename = "continue")
    {
        XmlDocument doc = new XmlDocument();
        XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.AppendChild(docNode);

        XmlNode save = doc.CreateElement("Save");
        doc.AppendChild(save);

        { //scene
            XmlNode sceneNode = doc.CreateElement("scene");
            save.AppendChild(sceneNode);

            XmlAttribute sceneName = doc.CreateAttribute("name");
            sceneName.Value = SceneManager.GetActiveScene().name;
            sceneNode.Attributes.Append(sceneName);
        } //end scene

        { //player
            GameObject player = GameObject.Find("Player");
            XmlNode playerNode = doc.CreateElement("player");
            save.AppendChild(playerNode);

            { // position
                XmlNode positionNode = doc.CreateElement("position");

                XmlAttribute x = doc.CreateAttribute("x");
                x.Value = player.transform.position.x.ToString();
                positionNode.Attributes.Append(x);

                XmlAttribute y = doc.CreateAttribute("y");
                y.Value = player.transform.position.y.ToString();
                positionNode.Attributes.Append(y);

                XmlAttribute z = doc.CreateAttribute("z");
                z.Value = player.transform.position.z.ToString();
                positionNode.Attributes.Append(z);

                playerNode.AppendChild(positionNode);
            }

            { // rotation
                XmlNode rotationNode = doc.CreateElement("rotation");

                XmlAttribute x = doc.CreateAttribute("x");
                x.Value = player.transform.localRotation.x.ToString();
                rotationNode.Attributes.Append(x);

                XmlAttribute y = doc.CreateAttribute("y");
                y.Value = player.transform.localRotation.y.ToString();
                rotationNode.Attributes.Append(y);

                XmlAttribute z = doc.CreateAttribute("z");
                z.Value = player.transform.localRotation.z.ToString();
                rotationNode.Attributes.Append(z);

                XmlAttribute w = doc.CreateAttribute("w");
                w.Value = player.transform.localRotation.w.ToString();
                rotationNode.Attributes.Append(w);

                playerNode.AppendChild(rotationNode);
            }
        } //end player

        { // objects
            GameObject intObjects = GameObject.Find("Interactable Objects");
            HandsInventory inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
            XmlNode objectsNode = doc.CreateElement("objects");
            save.AppendChild(objectsNode);

            foreach (Transform child in intObjects.transform)
            {
                XmlNode objectNode = doc.CreateElement("object");
                objectsNode.AppendChild(objectNode);

                XmlAttribute name = doc.CreateAttribute("name");
                name.Value = child.name;
                objectNode.Attributes.Append(name);

                if (child.gameObject == inv.LeftHandObject)
                {
                    XmlAttribute hand = doc.CreateAttribute("hand");
                    hand.Value = "left";
                    objectNode.Attributes.Append(hand);
                }
                else if (child.gameObject == inv.RightHandObject)
                {
                    XmlAttribute hand = doc.CreateAttribute("hand");
                    hand.Value = "right";
                    objectNode.Attributes.Append(hand);
                }
                
                ExaminableObject exam = child.GetComponent<ExaminableObject>();
                if (exam != null)
                {
                    XmlAttribute examState = doc.CreateAttribute("state");
                    examState.Value = exam.state;
                    objectNode.Attributes.Append(examState);
                }

                { // position
                    XmlNode positionNode = doc.CreateElement("position");

                    XmlAttribute x = doc.CreateAttribute("x");
                    x.Value = child.position.x.ToString();
                    positionNode.Attributes.Append(x);

                    XmlAttribute y = doc.CreateAttribute("y");
                    y.Value = child.position.y.ToString();
                    positionNode.Attributes.Append(y);

                    XmlAttribute z = doc.CreateAttribute("z");
                    z.Value = child.position.z.ToString();
                    positionNode.Attributes.Append(z);

                    objectNode.AppendChild(positionNode);
                }

                { // rotation
                    XmlNode rotationNode = doc.CreateElement("rotation");

                    XmlAttribute x = doc.CreateAttribute("x");
                    x.Value = child.localRotation.x.ToString();
                    rotationNode.Attributes.Append(x);

                    XmlAttribute y = doc.CreateAttribute("y");
                    y.Value = child.localRotation.y.ToString();
                    rotationNode.Attributes.Append(y);

                    XmlAttribute z = doc.CreateAttribute("z");
                    z.Value = child.localRotation.z.ToString();
                    rotationNode.Attributes.Append(z);

                    XmlAttribute w = doc.CreateAttribute("w");
                    w.Value = child.localRotation.w.ToString();
                    rotationNode.Attributes.Append(w);

                    objectNode.AppendChild(rotationNode);
                }
            }
        } //end objects

        ActionManager actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        { //actions
            XmlNode scenario = doc.CreateElement("scenario");

            XmlAttribute currentAction = doc.CreateAttribute("current");
            currentAction.Value = actionManager.CurrentActionIndex.ToString();
            scenario.Attributes.Append(currentAction);

            XmlAttribute pointsValue = doc.CreateAttribute("points");
            pointsValue.Value = ActionManager.Points.ToString();
            scenario.Attributes.Append(pointsValue);

            save.AppendChild(scenario);

            List<Action> actionList = actionManager.ActionList;
 
            foreach (Action item in actionList)
            {
                XmlNode actionNode = doc.CreateElement("action");
                scenario.AppendChild(actionNode);

                XmlAttribute actionStatus = doc.CreateAttribute("status");
                actionStatus.Value = item.matched.ToString();
                actionNode.Attributes.Append(actionStatus);
            }
        } //end actions

        { // timer
            GameTimer timer = GameObject.Find("GameLogic").GetComponent<GameTimer>();
            XmlNode timeNode = doc.CreateElement("timer");
            save.AppendChild(timeNode);

            XmlAttribute timeValue = doc.CreateAttribute("time");
            timeValue.Value = timer.CurrentTime.ToString();
            timeNode.Attributes.Append(timeValue);
        } //end timer
        
        doc.Save(Application.dataPath + savePath + filename);
    }

    public void LoadLevel(string filename = "continue")
    {
        SetNeedLoad(filename);
        
        XmlDocument doc = new XmlDocument();
        doc.Load(Application.dataPath + savePath + filename);

        XmlNode docNode = doc.FirstChild;
        XmlNode save = docNode.NextSibling;
        
        XmlNode sceneNode = save.FirstChild;
        GameObject.Find("Preferences").GetComponent<LoadingScreen>().LoadLevel(sceneNode.Attributes["name"].Value);
        //SceneManager.LoadScene(sceneNode.Attributes["name"].Value);
    }

    /// <summary>
    /// Loads and sets everything in scene.
    /// </summary>
    /// <param name="filename">Filename of savefile</param>
    private void Load(string filename)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Application.dataPath + savePath + filename);

        XmlNode docNode = doc.FirstChild;
        XmlNode save = docNode.NextSibling;

        //scene 
        XmlNode sceneNode = save.FirstChild;
        if (sceneNode.Attributes["name"].Value != SceneManager.GetActiveScene().name)
        {
            Debug.LogError("Savefile is for another scene.");
            Debug.Log(sceneNode.Attributes["name"].Value + " != " + SceneManager.GetActiveScene().name);
            return;
        }
        //end scene

        //Player
        XmlNode playerNode = sceneNode.NextSibling;
        GameObject player = GameObject.Find("Player");
        {
            XmlNode playerPosition = playerNode.FirstChild;
            float posX = float.Parse(playerPosition.Attributes["x"].Value);
            float posY = float.Parse(playerPosition.Attributes["y"].Value);
            float posZ = float.Parse(playerPosition.Attributes["z"].Value);
            player.transform.position = new Vector3(posX, posY, posZ);
        
            XmlNode playerRotation = playerPosition.NextSibling;
            float rotX = float.Parse(playerRotation.Attributes["x"].Value);
            float rotY = float.Parse(playerRotation.Attributes["y"].Value);
            float rotZ = float.Parse(playerRotation.Attributes["z"].Value);
            float rotW = float.Parse(playerRotation.Attributes["w"].Value);
            player.transform.localRotation = new Quaternion(rotX, rotY, rotZ, rotW);
        }
        //end player

        //Objects
        XmlNode objectsNode = playerNode.NextSibling;
        GameObject interactableObjects = GameObject.Find("Interactable Objects");
        HandsInventory inventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        foreach (Transform child in interactableObjects.transform)
        {
            Destroy(child.gameObject);
        }

        XmlNodeList objectList = objectsNode.ChildNodes;
        foreach (XmlNode item in objectList)
        {
            string name = item.Attributes["name"].Value;
            string hand = item.Attributes["hand"] != null ? item.Attributes["hand"].Value : "none";
            string examState = item.Attributes["state"] != null ? item.Attributes["state"].Value : "";

            XmlNode positionNode = item.FirstChild;
            float posX = float.Parse(positionNode.Attributes["x"].Value);
            float posY = float.Parse(positionNode.Attributes["y"].Value);
            float posZ = float.Parse(positionNode.Attributes["z"].Value);

            XmlNode rotationNode = positionNode.NextSibling;
            float rotX = float.Parse(rotationNode.Attributes["x"].Value);
            float rotY = float.Parse(rotationNode.Attributes["y"].Value);
            float rotZ = float.Parse(rotationNode.Attributes["z"].Value);
            float rotW = float.Parse(rotationNode.Attributes["w"].Value);
            
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs\\" + name),
                new Vector3(posX, posY, posZ), new Quaternion(rotX, rotY, rotZ, rotW),
                interactableObjects.transform) as GameObject;
            obj.name = name; // prefent 'clone' in the name
            ExaminableObject exam = obj.GetComponent<ExaminableObject>();
            if (exam != null)
            {
                exam.state = examState;
            }

            inventory.PickItem(obj.GetComponent<PickableObject>(), hand);
        }
        //end objects

        //scenario
        XmlNode scenario = objectsNode.NextSibling;
        XmlNodeList actionNodes = scenario.ChildNodes;
        List<bool> statusList = new List<bool>();
        foreach(XmlNode node in actionNodes)
        {
            statusList.Add(bool.Parse(node.Attributes["status"].Value));
        }

        ActionManager actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        actionManager.SetActionStatus(statusList);

        actionManager.CurrentActionIndex = int.Parse(scenario.Attributes["current"].Value);
        ActionManager.Points = int.Parse(scenario.Attributes["points"].Value);
        //end scenario

        //timer
        XmlNode timer = scenario.NextSibling;
        GameObject.Find("GameLogic").GetComponent<GameTimer>().CurrentTime =
            float.Parse(timer.Attributes["time"].Value);
        //end timer
    }
}
