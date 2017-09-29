using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    /// <summary>
    /// This class allow you to understand how to send datas to the server :
    /// 
    /// It's very easy to do :
    /// 1 - Create an array (datas) with the datas you want to send to the server (Data1, Data2, Data3, Data4, Coins, Score, ...)
    /// 
    /// 2 - Call : LoginPro.Manager.ExecuteOnServer([ACTION_NAME], [METHOD_FOR_SUCCESS], [METHOD_FOR_ERROR], [DATAS]);
    /// 
    /// 3 - Create a PHP script in the folder [LoginPro_Server/Game/Includes/Actions] where you get the array you sent
    ///     (In this script you can do what you want, save in the database, get information, ...)
    ///     
    /// 4 - Open the script [LoginPro_Server/Game/Server.php] -> At the end add the name of the newly created action (and its path)
    /// 
    /// 5 - Upload the 2 PHP script you've just changed -> That's all :)
    /// 
    /// 
    /// All information will be automatically encrypted on network, all your datas and players' accounts are safe.
    /// 
    /// 
    /// Note : Read this file and the PHP script [LoginPro_Server/Game/Includes/Actions/SendData.php]
    /// You will see it's very easy to understand
    /// </summary>
    public class LoginPro_SendToServer : MonoBehaviour
    {
        // All the datas we want to send to the server
        public InputField Data1;
        public InputField Data2;
        public InputField Data3;


        // This is just here to show a popup when datas are sent (or error occured)
        public UIAnimation_Alert Popup;


        /// <summary>
        /// Send datas to the server
        /// Call methodForSuccess if data sent correctly
        /// Call methodForError if data not sent
        /// </summary>
        public void SendToServer()
        {
            // Information to send to the server (encrypted with RSA)
            string[] datas = new string[3]; // <- CAUTION TO THE SIZE OF THE ARRAY (It's the number of data you want to send)
            datas[0] = Data1.text;
            datas[1] = Data2.text;
            datas[2] = Data3.text;
            LoginPro.Manager.ExecuteOnServer("SendData", SendToServer_Success, SendToServer_Error, datas);
        }
        public void SendToServer_Success(string[] datas)
        {
            Debug.Log("Success! The server answered : " + datas[0]);
            Popup.Show("Success! The server answered : " + datas[0], 3);
        }
        public void SendToServer_Error(string errorMessage)
        {
            Debug.LogError(errorMessage);
            Popup.Show("Error : " + errorMessage, 5);
        }
    }
}