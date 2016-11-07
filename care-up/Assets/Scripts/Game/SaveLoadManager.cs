using System.Xml;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SaveLoadManager : MonoBehaviour {

    private const string savePath = "Assets/Resources/Xml/SaveFiles/";

    private bool needLoad = false;
    private string needLoadName;

	void Start()
    {
        SceneManager.sceneLoaded += OnLoaded;
    }

    private void OnLoaded(Scene s, LoadSceneMode m)
    {
        if (needLoad)
        {
            needLoad = false;
            Load(needLoadName);
        }
    }

    public void SetNeedLoad(string filename = "continue.xml")
    {
        needLoad = true;
        needLoadName = filename;
    }

    public void Save(string filename = "continue.xml")
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
            XmlNode objectsNode = doc.CreateElement("objects");
            save.AppendChild(objectsNode);

            foreach (Transform child in intObjects.transform)
            {
                XmlNode objectNode = doc.CreateElement("object");
                objectsNode.AppendChild(objectNode);

                XmlAttribute name = doc.CreateAttribute("name");
                name.Value = child.name;
                objectNode.Attributes.Append(name);

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

        { //points
            XmlNode pointsNode = doc.CreateElement("points");
            save.AppendChild(pointsNode);

            XmlAttribute pointsValue = doc.CreateAttribute("points");
            pointsValue.Value = actionManager.Points.ToString();
            pointsNode.Attributes.Append(pointsValue);
        } //emd points

        { // timer
            GameTimer timer = GameObject.Find("GameLogic").GetComponent<GameTimer>();
            XmlNode timeNode = doc.CreateElement("timer");
            save.AppendChild(timeNode);

            XmlAttribute timeValue = doc.CreateAttribute("time");
            timeValue.Value = timer.CurrentTime.ToString();
            timeNode.Attributes.Append(timeValue);
        } //end timer
        
        doc.Save(savePath + filename);
    }

    public void LoadLevel(string filename = "continue.xml")
    {
        SetNeedLoad(filename);
        XmlDocument doc = new XmlDocument();
        doc.Load(savePath + filename);

        XmlNode docNode = doc.FirstChild;
        XmlNode save = docNode.NextSibling;
        
        XmlNode sceneNode = save.FirstChild;
        SceneManager.LoadScene(sceneNode.Attributes["name"].Value);
    }

    private void Load(string filename)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(savePath + filename);

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
        foreach (Transform child in interactableObjects.transform)
        {
            Destroy(child.gameObject);
        }

        XmlNodeList objectList = objectsNode.ChildNodes;
        foreach (XmlNode item in objectList)
        {
            string name = item.Attributes["name"].Value;

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
        //end scenario

        //points
        XmlNode points = scenario.NextSibling;
        actionManager.Points = int.Parse(points.Attributes["points"].Value);
        //end points

        //timer
        XmlNode timer = points.NextSibling;
        GameObject.Find("GameLogic").GetComponent<GameTimer>().CurrentTime =
            float.Parse(timer.Attributes["time"].Value);
        //end timer
    }
}
