using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    public class LoginPro_Friends : MonoBehaviour
    {
        public InputField UsernameToSearch;

        // UI components
        public UnityEngine.Object friendRowPrefab = null;

        // Friends
        private List<Friend> friends { get; set; }
        public GameObject FriendsGrid;
        public PlaceUIElement FriendsButton;

        void Start()
        {
            // Get UI components
            if (friendRowPrefab == null)
                friendRowPrefab = Resources.Load("FriendRow");
        }

        void OnGUI()
        {
            // Hide report if not logged in
            if (!LoginPro.Session.LoggedIn)
            {
                this.FriendsButton.transform.localScale = Vector3.zero;
            }
            else
            {
                this.FriendsButton.transform.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// Send friend request
        /// Call methodForSuccess if request sent
        /// Call methodForError if request failed
        /// </summary>
        public void SendFriendRequest()
        {
            if (!LoginPro.Session.LoggedIn)
            {
                SendFriendRequestError("You must login before calling this action.");
                return;
            }

            string username = UsernameToSearch.text;
            // If datas are empty : no need to send the form
            if (string.IsNullOrEmpty(username))
            {
                SendFriendRequestError("Please fill all the fields to modify your account.");
                return;
            }
            if (username.Length < 3)
            {
                SendFriendRequestError("Username must be at least 3 characters long.");
                return;
            }

            // Information to send to the server
            string[] datas = new string[1];
            datas[0] = username;
            LoginPro.Manager.ExecuteOnServer("SendFriendRequest", SendFriendRequestSuccess, SendFriendRequestError, datas);
        }

        public void SendFriendRequestError(string errorMessage)
        {
            // Show the error
            Debug.LogWarning(errorMessage);

        }

        public void SendFriendRequestSuccess(string[] datas)
        {
            // Show message on success
        }

        /// <summary>
        /// Get friends list
        /// Call methodForSuccess if list received
        /// Call methodForError if list not received
        /// </summary>
        public void GetFriends()
        {
            // Clear the friend list
            this.friends = new List<Friend>();

            // Clear the friend list of game objects
            Transform friendsGridTransform = FriendsGrid.transform;
            foreach (Transform child in friendsGridTransform)
            {
                Destroy(child.gameObject);
            }

            if (!LoginPro.Session.LoggedIn)
            {
                GetFriendsError("You must login before calling this action.");
                return;
            }

            // Get all my friends (and friend requests)
            LoginPro.Manager.ExecuteOnServer("GetFriends", GetFriendsSuccess, GetFriendsError, null);
        }

        public void GetFriendsError(string errorMessage)
        {
            // Show the error
            Debug.LogWarning(errorMessage);

            // Show message on error
        }

        public void GetFriendsSuccess(string[] serverDatas)
        {
            // Clear friends grid
            this.ClearFriendList();

            // Show message on success

            Color notConnectedColor = Color.grey;

            int numberOfDataReceivedPerFriend = 5;  // A friend is constituted of 3 datas

            int dataLength = serverDatas.Length;    // Get the length of the array received

            for (int i = 1; i + numberOfDataReceivedPerFriend <= dataLength; i += numberOfDataReceivedPerFriend)   // As long as there is datas -> create a friend row
            {
                string friendId = serverDatas[i];
                string friendName = serverDatas[i + 1];
                bool isConnected = serverDatas[i + 2] == "True";
                string status = serverDatas[i + 3];
                bool isMyDemand = serverDatas[i + 4] == "True";

                // Don't show refused requests or my request not responded
                if (status == "Refused" || (status == "Pending" && isMyDemand))
                    continue;

                // Create the friend row
                GameObject friendRow = (GameObject)Instantiate(this.friendRowPrefab);

                if (!isConnected)
                    friendRow.GetComponent<Image>().color = notConnectedColor;

                // Get buttons of the row to display information
                GameObject friendIdGameObject = friendRow.transform.Find("FriendId").gameObject;
                GameObject friendNameGameObject = friendRow.transform.Find("Name").gameObject;
                GameObject acceptGameObject = friendRow.transform.Find("Accept").gameObject;
                GameObject refuseGameObject = friendRow.transform.Find("Refuse").gameObject;

                // Check if all gameObject are found in the prefab : otherwise show some errors
                if (friendIdGameObject == null) Debug.LogError("The ReportRow prefab must contain a FriendId child.");
                if (friendNameGameObject == null) Debug.LogError("The ReportRow prefab must contain a Name child.");
                if (acceptGameObject == null) Debug.LogError("The ReportRow prefab must contain a Accept child.");
                if (refuseGameObject == null) Debug.LogError("The ReportRow prefab must contain a Refuse child.");

                Button AcceptButton = acceptGameObject.GetComponent<Button>();
                Button RefuseButton = refuseGameObject.GetComponent<Button>();

                // If the friend request is not responded yet : ask for an answer
                if (status == "Pending")
                {
                    // Attach action on button clicks
                    AcceptButton.onClick.AddListener(() => this.UpdateFriendRequest(friendId, true));
                    RefuseButton.onClick.AddListener(() => this.UpdateFriendRequest(friendId, false));
                }
                else
                {
                    // Otherwise : hide the 2 buttons
                    AcceptButton.transform.localScale = Vector3.zero;
                    RefuseButton.transform.localScale = Vector3.zero;
                }

                // Affect values to it
                friendIdGameObject.transform.GetChild(0).GetComponent<Text>().text = friendId;
                friendNameGameObject.transform.GetChild(0).GetComponent<Text>().text = friendName;

                // Insert it in the report list
                Friend friend = new Friend(friendId, name, status);
                friendRow.transform.SetParent(FriendsGrid.transform);
                friends.Add(friend);
            }
        }

        /// <summary>
        /// Update friend request
        /// Call methodForSuccess if updated
        /// Call methodForError if failed
        /// </summary>
        public void UpdateFriendRequest(string friendId, bool accepted)
        {
            if (!LoginPro.Session.LoggedIn)
            {
                UpdateFriendRequestError("You must login before calling this action.");
                return;
            }

            // Information to send to the server
            string[] datas = new string[2];
            datas[0] = friendId;
            datas[1] = accepted.ToString();
            LoginPro.Manager.ExecuteOnServer("UpdateFriendRequest", UpdateFriendRequestSuccess, UpdateFriendRequestError, datas);
        }
        public void UpdateFriendRequestError(string errorMessage)
        {
            // Show the error
            Debug.LogWarning(errorMessage);

        }

        public void UpdateFriendRequestSuccess(string[] serverDatas)
        {
            // Show new friend list
            this.GetFriendsSuccess(serverDatas);
        }

        /// <summary>
        /// Clear the list of friend shown on screen
        /// </summary>
        public void ClearFriendList()
        {
            // Clear the friends list
            this.friends = new List<Friend>();

            // Clear the friend list
            Transform friendsGridTransform = FriendsGrid.transform;
            foreach (Transform child in friendsGridTransform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}