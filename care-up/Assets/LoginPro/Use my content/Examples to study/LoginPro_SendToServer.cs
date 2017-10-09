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
        public InputField Data2;
        public InputField Data3;

        public GameObject validated;
        // This is just here to show a popup when datas are sent (or error occured)
        public UIAnimation_Alert Popup;

        public string Serial = "";
        public bool ValidProduct = false;
        // Which of the base keys to compare in the check
        public byte CheckKey = 0;


        // This is the most important part.
        // Make sure your base keys are unique
        // choose random keys here and when you want to make a
        // 2.x release change the keys so the 1.x keys are no longer valid
        public uint[] MyBaseKeys = { 4, 16, 12, 64 };

        void Start()
        {


            // Init Guardian
            Guardian.Init(MyBaseKeys);
            // Number of letters between each dash in the serial
            Guardian.Spacing = 6;

            // Generate A Test Key
            //Serial = Guardian.Generate("Care-Up");

            // Store the serial number in playerprefs so the user does not have to write them everytime.
            string storedSerial = PlayerPrefs.GetString("SerialKey");
            if(storedSerial != string.Empty)
             {
                ValidProduct = true;
                //Serial was valid we can continue on to the game menu/level
                validated.SetActive(true);
            }

            var MySerialNumbers = Guardian.Generate(3, new System.Random(3));
            foreach (var serial in MySerialNumbers)
            {

                Debug.Log(serial.Value);
            }
          
       }

        /// <summary>
        /// Send datas to the server
        /// Call methodForSuccess if data sent correctly
        /// Call methodForError if data not sent
        /// </summary>
        public void SendToServer()
        {
			// Information to send to the server (encrypted with RSA)
			string[] datas = new string[2]; // <- CAUTION TO THE SIZE OF THE ARRAY (It's the number of data you want to send)
			datas[0] = Data1.text;
	
			Serial = Data1.text;
            if (Guardian.ValidateKey(Serial, CheckKey, MyBaseKeys[CheckKey]))
            {
                ValidProduct = true;
				datas[1] = ValidProduct.ToString();
				LoginPro.Manager.ExecuteOnServer("SendData", SendToServer_Success, SendToServer_Error, datas);
			
               // SceneManager.LoadScene("Menu");
                // Store the key so when we load up next time we dont have to enter serial again.
                //PlayerPrefs.SetString("SerialKey", Serial);
            }
            else {
				Popup.Show ("Helaas, de code is incorrect.", 5);
                //topText = "Helaas, de code klopt niet. Probeer het opnieuw";
                Debug.Log("code invalide");
            }

            Debug.Log(datas[0]);
			Debug.Log(datas[1]);

        }
		//Check if code is correct
        public void SendToServer_Success(string[] datas)
        {
            
            Debug.Log("Success! The server answered : " + datas[0]);
			if (ValidProduct) 
			{
				Popup.Show ("Succes, code correct. Je kunt het spel starten.", 5);
                validated.SetActive(true);

			} 
        }
        public void SendToServer_Error(string errorMessage)
        {
            Debug.LogError(errorMessage);
            Popup.Show("Error : " + errorMessage, 5);
        }
			

        }
}