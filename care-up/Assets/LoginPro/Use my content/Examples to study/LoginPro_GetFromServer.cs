using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    /// <summary>
    /// This class allow you to understand how to get datas from the server :
    /// 
    /// It's very easy to do :
    /// 1 - Call : LoginPro.Manager.ExecuteOnServer([ACTION_NAME], [METHOD_FOR_SUCCESS], [METHOD_FOR_ERROR], [DATAS]);
    /// 
    /// 2 - Create a PHP script in the folder [LoginPro_Server/Game/Includes/Actions] where you get the array you sent
    ///     (In this script you can do what you want, save in the database, get information, ...)
    ///     
    /// 3 - Open the script [LoginPro_Server/Game/Server.php] -> At the end add the name of the newly created action (and its path)
    /// 
    /// 4 - Upload the 2 PHP script you've just changed -> That's all :)
    /// 
    /// 
    /// All information will be automatically encrypted on network, all your datas and players' accounts are safe.
    /// 
    /// 
    /// Note : Read this file and the PHP script [LoginPro_Server/Game/Includes/Actions/GetData.php]
    /// You will see it's very easy to understand
    /// </summary>
    public class LoginPro_GetFromServer : MonoBehaviour
    {
        // All the datas we want to get from the server
        public Text Data1;
        public Text Data2;
        public Text Data3;


        // This is just here to show a popup when datas are sent (or error occured)
        public UIAnimation_Alert Popup;

        /// <summary>
        /// Get datas from the server
        /// Call methodForSuccess if data sent correctly
        /// Call methodForError if data not sent
        /// </summary>
        public void GetFromServer()
        {
            // Here datas are null since we don't want to SEND anything (but we want something)
            LoginPro.Manager.ExecuteOnServer("GetData", GetFromServer_Success, GetFromServer_Error, null);
        }
        public void GetFromServer_Success(string[] datas)
        {
            Debug.Log("Success! The server answered : " + datas[0]);
            Popup.Show("Success! The server answered : " + datas[0], 3);
            Data1.text = datas[1];
            Data2.text = datas[2];
            Data3.text = datas[3];
        }
        public void GetFromServer_Error(string errorMessage)
        {
            Debug.LogError(errorMessage);
            Popup.Show("Error : " + errorMessage, 5);
        }
    }
}