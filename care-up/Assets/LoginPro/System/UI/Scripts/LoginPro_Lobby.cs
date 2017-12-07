using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace LoginProAsset
{
    public class LoginPro_Lobby : MonoBehaviour
    {
       // public UIAnimation_Alert MessageToShowOnResult;

        public Object playerRowPrefab;
        public GameObject PlayersGrid;

        public string SceneToLoadOnStart;
        public string SceneToLoadOnLeave;

        public Text ChatMessages;
        public InputField inputMessage;

        // List of servers
        private List<LoginProAsset.Player> players { get; set; }

        private int fpsCounter;

        /// <summary>
        /// A timer set to refresh the lobby every minutes
        /// </summary>
        void FixedUpdate()
        {
            if (LoginPro.Session.LoggedIn && LoginPro.Session.serverJoinedId != "")
            {
                // Update menu only once in a while (every seconds)
                if (this.fpsCounter < 0)
                {
                    this.fpsCounter = 600;
                    this.GetPlayers();
                    this.GetChat();
                }
                this.fpsCounter--;
            }
            else
            {
                // Leave without any check
                this.leave(null);
            }
        }


        ///////////////////////////////// - Refresh lobby players - ///////////////////////////////////////////
        private void GetPlayers()
        {
            // Get players list from server
            if (LoginPro.Manager != null)
                LoginPro.Manager.ExecuteOnServer("GetLobbyPlayers", getPlayersSuccess, getPlayersError, null);
            else
                Debug.Log("Get players : Manager is null !!");
        }

        private void getPlayersSuccess(string[] serverDatas)
        {
            // Clear the players list
            this.players = new List<LoginProAsset.Player>();

            // Clear the server list
            Transform playersGridTransform = PlayersGrid.transform;
            foreach (Transform child in playersGridTransform)
            {
                Destroy(child.gameObject);
            }

            LoginPro.Session.iAmServerHost = serverDatas[0] == LoginPro.Session.Username;

            int numberOfDataReceivedPerPlayer = 3;  // A player is constituted of 2 datas

            int dataLength = serverDatas.Length;    // Get the length of the array received
            for (int i = 1; i + (numberOfDataReceivedPerPlayer - 1) < dataLength; i += numberOfDataReceivedPerPlayer)  // As long as there is datas -> create a player row
            {
                string playerId = serverDatas[i];
                string playerName = serverDatas[i + 1];
                string playerScore = serverDatas[i + 2];
                bool isServerHost = playerName == serverDatas[0];

                // Create the player row
                GameObject playerRow = (GameObject)Instantiate(playerRowPrefab);

                // If it's the host : color in red
                if (isServerHost)
                    playerRow.GetComponent<Image>().color = new Color(1, 0, 0.16f);

                // Get buttons of the row to display information
                GameObject PlayerId = playerRow.transform.Find("PlayerId").gameObject;
                GameObject PlayerName = playerRow.transform.Find("PlayerName").gameObject;
                GameObject PlayerScore = playerRow.transform.Find("PlayerScore").gameObject;
                GameObject KickButton = playerRow.transform.Find("KickButton").gameObject;

                // Check if all gameObject are found in the prefab : otherwise show some errors
                if (PlayerId == null) Debug.LogError("The PlayerRow prefab must contain a PlayerId child.");
                if (PlayerName == null) Debug.LogError("The PlayerRow prefab must contain a PlayerName child.");
                if (PlayerScore == null) Debug.LogError("The PlayerRow prefab must contain a PlayerScore child.");
                if (KickButton == null) Debug.LogError("The PlayerRow prefab must contain a KickButton child.");

                // Attach action on button clicks
                if (LoginPro.Session.iAmServerHost && !isServerHost)
                {
                    Button kickButton = KickButton.GetComponent<Button>();
                    kickButton.onClick.AddListener(() => this.kick(playerId));
                    kickButton.transform.localScale = Vector3.one;
                }

                // Affect values to it
                PlayerId.transform.GetChild(0).GetComponent<Text>().text = playerId;
                PlayerName.transform.GetChild(0).GetComponent<Text>().text = playerName;
                PlayerScore.transform.GetChild(0).GetComponent<Text>().text = playerScore;

                // Insert it in the report list
                LoginProAsset.Player player = new LoginProAsset.Player(playerId, playerName, playerScore);
                playerRow.transform.SetParent(PlayersGrid.transform);
                players.Add(player);
            }
        }
        private void getPlayersError(string errorMessage)
        {
            Debug.Log(errorMessage);
          //  if (MessageToShowOnResult != null)
             //   MessageToShowOnResult.Show(errorMessage, 3);
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set(errorMessage);
            // Leave the lobby
            this.Leave();
        }

        ///////////////////////////////// - Send chat message - ///////////////////////////////////////////
        public void SendChatMessage()
        {
            // If there is no reference to Chat
            if (inputMessage == null)
            {
                Debug.LogWarning("LobbyWindow : no reference to Chat.");
                return;
            }

            string[] datas = new string[1];
            datas[0] = inputMessage.text;
            inputMessage.text = "";

            // Send message
            if (LoginPro.Manager != null)
                LoginPro.Manager.ExecuteOnServer("SendChatMessage", getChatSuccess, sendMessageError, datas);
            else
                Debug.Log("Send message : Manager is null !!");
        }
        private void sendMessageError(string errorMessage)
        {
            Debug.Log(errorMessage);
           // if (MessageToShowOnResult != null)
               // MessageToShowOnResult.Show(errorMessage, 3);
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set(errorMessage);
        }

        ///////////////////////////////// - Get chat - ///////////////////////////////////////////
        public void GetChat()
        {
            // Send message
            if (LoginPro.Manager != null)
                LoginPro.Manager.ExecuteOnServer("GetChat", getChatSuccess, sendMessageError, null);
            else
                Debug.Log("Get chat : Manager is null !!");
        }
        private void getChatSuccess(string[] serverDatas)
        {
            // Clear chat
            ChatMessages.text = "";

            int numberOfDataReceivedPerPlayer = 3;  // A message is constituted of 3 datas

            int dataLength = serverDatas.Length;    // Get the length of the array received
            for (int i = 1; i + (numberOfDataReceivedPerPlayer - 1) < dataLength; i += numberOfDataReceivedPerPlayer)
            {
                string messageDate = serverDatas[i];
                string playerName = serverDatas[i + 1];
                string message = serverDatas[i + 2];

                ChatMessages.text += string.Format("[{0}] {1} : {2}\n", messageDate, playerName, message);
            }
        }
        private void getChatError(string errorMessage)
        {
            Debug.Log(errorMessage);
            // if (MessageToShowOnResult != null)
            //  MessageToShowOnResult.Show(errorMessage, 3);
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set(errorMessage);
        }

        ///////////////////////////////// - Kick a player - ///////////////////////////////////////////
        private void kick(string playerId)
        {
            string[] datas = new string[1];
            datas[0] = playerId;

            // Get players list from server
            if (LoginPro.Manager != null)
                LoginPro.Manager.ExecuteOnServer("KickPlayer", getPlayersSuccess, kickError, datas);
            else
                Debug.Log("Kick player : Manager is null !!");
        }
        private void kickError(string errorMessage)
        {
            Debug.Log(errorMessage);
            //if (MessageToShowOnResult != null)
           //     MessageToShowOnResult.Show(errorMessage, 3);
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set(errorMessage);
        }


        ///////////////////////////////// - Leave the lobby - ///////////////////////////////////////////
        public void Leave()
        {
            // Get players list from server
            if (LoginPro.Manager != null)
                LoginPro.Manager.ExecuteOnServer("LeaveLobby", leave, leaveError, null);
            else
                Debug.Log("Leave lobby : Manager is null !!");
        }
        private void leave(string[] serverDatas)
        {
            // Clear the server id
            LoginPro.Session.serverJoinedId = "";

            // Load the servers scene
            LoginPro_Security.Load(this.SceneToLoadOnLeave);
        }
        private void leaveError(string errorMessage)
        {
            Debug.Log(errorMessage);
            //if (MessageToShowOnResult != null)
            //    MessageToShowOnResult.Show(errorMessage, 3);
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set(errorMessage);

            // Leave the lobby
            this.leave(null);
        }
    }
}