using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace LoginProAsset
{
    public class LoginPro_GetServers : MonoBehaviour
    {
       // public UIAnimation_Alert MessageToShowOnResult;

        public Object serverRowPrefab;
        public GameObject ServersGrid;

        public string SceneToLoadOnJoin;
        public string SceneToLoadOnCreate;

        // List of servers
        private List<Server> servers { get; set; }

        // Menus
        public string LoginScene;

        public void Start()
        {
            // Verify we did logged in
            if (!LoginPro.Session.LoggedIn)
            {
                LoginPro_Security.Load(LoginScene);  // The connection is not established, go back to login menu
            }

            // Join directly the server you were connected to
            if (LoginPro.Session.serverJoinedId != "")
                Join(LoginPro.Session.serverJoinedId);
            else
                this.Refresh();
        }

        ///////////////////////////////// - Refresh servers - ///////////////////////////////////////////
        private void Refresh()
        {
            // Clear the servers list
            servers = new List<Server>();

            // Clear the server list
            Transform serversGridTransform = ServersGrid.transform;
            foreach (Transform child in serversGridTransform)
            {
                Destroy(child.gameObject);
            }

            // Get servers list from server
            if (LoginPro.Manager != null)
                LoginPro.Manager.ExecuteOnServer("GetServers", getServersSuccess, getServersError, null);
            else
                Debug.Log("Get servers : Manager is null !!");
        }

        private void getServersSuccess(string[] serverDatas)
        {
            int numberOfDataReceivedPerServer = 2;  // A server is constituted of 2 datas

            int dataLength = serverDatas.Length;    // Get the length of the array received
            for (int i = 1; i + (numberOfDataReceivedPerServer - 1) <= dataLength; i += numberOfDataReceivedPerServer)  // As long as there is datas -> create a server row
            {
                string serverId = serverDatas[i];
                string serverName = serverDatas[i + 1];

                // Create the server row
                GameObject serverRow = (GameObject)Instantiate(serverRowPrefab);

                // Get buttons of the row to display information
                GameObject ServerId = serverRow.transform.Find("ServerId").gameObject;
                GameObject ServerName = serverRow.transform.Find("ServerName").gameObject;
                GameObject ServerJoinButton = serverRow.transform.Find("JoinButton").gameObject;

                // Check if all gameObject are found in the prefab : otherwise show some errors
                if (ServerId == null) Debug.LogError("The ServerRow prefab must contain a ServerId child.");
                if (ServerName == null) Debug.LogError("The ServerRow prefab must contain a ServerName child.");
                if (ServerJoinButton == null) Debug.LogError("The ServerRow prefab must contain a ServerJoinButton child.");

                // Attach action on button clicks
                Button joinButton = ServerJoinButton.GetComponent<Button>();
                joinButton.onClick.AddListener(() => this.Join(serverId));

                // Affect values to it
                ServerId.transform.GetChild(0).GetComponent<Text>().text = serverId;
                ServerName.transform.GetChild(0).GetComponent<Text>().text = serverName;

                // Insert it in the report list
                Server server = new Server(serverId, serverName);
                serverRow.transform.SetParent(ServersGrid.transform);
                servers.Add(server);
            }
        }
        private void getServersError(string errorMessage)
        {
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set(errorMessage);
            Debug.Log(errorMessage);
            //if (MessageToShowOnResult != null)
            //    MessageToShowOnResult.Show(errorMessage, 3);
        }

        ///////////////////////////////// - Join the selected lobby - ///////////////////////////////////////////
        public void Join(string serverIdToJoin)
        {
            string[] datas = new string[1];
            datas[0] = serverIdToJoin;

            // Get players list from server
            if (LoginPro.Manager != null)
                LoginPro.Manager.ExecuteOnServer("JoinLobby", joinLobbySuccess, joinLobbyError, datas);
            else
                Debug.Log("Join lobby : Manager is null !!");
        }
        private void joinLobbySuccess(string[] serverDatas)
        {
         //   if (MessageToShowOnResult != null)
              //  MessageToShowOnResult.Show(serverDatas[0].Replace("ERROR:", ""), 3);

            // Save the lobby I just joined
            LoginPro.Session.serverJoinedId = serverDatas[1];
            LoginPro.Session.iAmServerHost = serverDatas[2] == LoginPro.Session.Username;

            // Load the server joining scene
            LoginPro_Security.Load(this.SceneToLoadOnJoin);
        }
        private void joinLobbyError(string errorMessage)
        {
            Debug.Log(errorMessage);
           // if (MessageToShowOnResult != null)
               // MessageToShowOnResult.Show(errorMessage, 3);

            LoginPro.Session.serverJoinedId = "";
        }

        ///////////////////////////////// - Create a new lobby - ///////////////////////////////////////////
        public void CreateLobby()
        {
            string[] datas = new string[2];
            datas[0] = "MyNewServer";
            datas[1] = "1";

            // Create a new lobby and become host of it
            if (LoginPro.Manager != null)
                LoginPro.Manager.ExecuteOnServer("CreateLobby", createLobbySuccess, createLobbyError, datas);
            else
                Debug.Log("Create lobby : Manager is null !!");
        }
        private void createLobbySuccess(string[] serverDatas)
        {
          //  if (MessageToShowOnResult != null)
             //   MessageToShowOnResult.Show(serverDatas[0], 3);

            // Save the created lobby id
            LoginPro.Session.serverJoinedId = serverDatas[1];
            LoginPro.Session.iAmServerHost = true;

            // Load the server joining scene
            LoginPro_Security.Load(this.SceneToLoadOnCreate);
        }
        private void createLobbyError(string errorMessage)
        {
            Debug.Log(errorMessage);
            // if (MessageToShowOnResult != null)
            //  MessageToShowOnResult.Show(errorMessage, 3);
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set(errorMessage);
        }
    }
}