using System;
using System.IO;
using UnityEngine;

namespace LoginProAsset
{
    /// <summary>
    /// This class allows you to understand how sending a file can be done
    /// Remember to set the image you want to send as "Advanced" -> Read/Write enabled
    /// Then setting an accepted format ARG32, RGB32... (Unity will tell you)
    /// 
    /// IMPORTANT :
    /// It's possible to send a picture but it's also possible to send ANY file
    /// Just remember that for text file Unity wants the file to have the .txt extension
    /// And your file must be in a folder called 'Resources'
    /// The resulting file (get from the server) will be (in Unity editor otherwise it's in the build folder) found at :
    /// C:\Users\YOUR_USER\AppData\LocalLow\YOUR_COMPANY\YOUR_PRODUCT
    /// </summary>
    public class LoginPro_SendAndGetFiles : MonoBehaviour
    {
        // This is just here to show a popup when datas are sent (or error occured)
       // public UIAnimation_Alert Popup;


        /// <summary>
        /// Send datas to the server
        /// Call methodForSuccess if data sent correctly
        /// Call methodForError if data not sent
        /// </summary>
        public void SendAndGetFilesToServer()
        {
            // The path of the buffer file
            Texture2D image = Resources.Load("PictureOnDisk") as Texture2D;
            byte[] bytes = image.EncodeToPNG();
            string fileToSend = Convert.ToBase64String(bytes);

            // Information to send to the server (encrypted with RSA)
            string[] datas = new string[3]; // <- CAUTION TO THE SIZE OF THE ARRAY (It's the number of data you want to send)
            datas[0] = "Hi server, save this picture please.";
            datas[1] = "Another data for example.";
            datas[2] = fileToSend;
            LoginPro.Manager.ExecuteOnServer("SaveFile", SendToServer_Success, SendToServer_Error, datas);
        }
        public void SendToServer_Success(string[] datas)
        {
            Debug.Log("Success! The server answered : " + datas[0]);

            byte[] fileBytes = Convert.FromBase64String(datas[1]);
            File.WriteAllBytes(Application.persistentDataPath + "/PictureFromServer.png", fileBytes);

            //if (this.Popup != null)
              //  this.Popup.Show("Success! Picture 'PictureFromServer.png' has been created.", 3);
            //else
               // Debug.LogWarning("Popup is not set in LoginPro_SendAndGetFiles.");
        }
        public void SendToServer_Error(string errorMessage)
        {
            Debug.LogError(errorMessage);
          //  this.Popup.Show("Error : " + errorMessage, 3);
        }
    }
}