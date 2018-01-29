using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
		public InputField enteredSerial;
        public InputField Data3;



        // This is just here to show a popup when datas are sent (or error occured)
        //public UIAnimation_Alert Popup;

        public string Serial = "";
        public bool ValidProduct = false;

        void Start()
        {


            // Store the serial number in playerprefs so the user does not have to write them everytime.
            string storedSerial = PlayerPrefs.GetString("SerialKey");
            if(storedSerial != string.Empty)
             {
                ValidProduct = true;
                //Serial was valid we can continue on to the game menu/level
               
             }

         
          
       }

        /// <summary>
        /// Send datas to the server
        /// Call methodForSuccess if data sent correctly
        /// Call methodForError if data not sent
        /// </summary>
        public void SendToServer()
        {
            string[] data = new string[1];
            data[0] = enteredSerial.text;

            LoginPro.Manager.ExecuteOnServer("SetSerial", SetSerialSuccess, SetSerialError, data);
        }
		
        public void SendToServer_Error(string errorMessage)
        {
            Debug.LogError(errorMessage);
            //Popup.Show("Error : " + errorMessage, 5);
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set(errorMessage);
        }
        //Check if code is correct

        public void SetSerialSuccess(string[] datas)
        {
            Serial = enteredSerial.text;
            ValidProduct = true;
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set("Succes, je protocol is nu beschikbaar start het spel om je protocol te spelen.");

            if (GameObject.Find("Preferences") != null)
            {
                GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().CheckSerial();
            }
        }

        //Check if code is incorrect
        public void SetSerialError(string msg)
        {
            //Popup.Show ("Helaas, de code is incorrect.", 5);
            GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set("Helaas, de code klopt niet. Probeer het opnieuw");
            //topText = "Helaas, de code klopt niet. Probeer het opnieuw";
            Debug.Log("code invalide");
        }
			

        }
}