using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using System;
public class Activation : MonoBehaviour {
	
	public Rect windowRect = new Rect(20, 20, 360, 75);
	public string Serial = "";
	public bool ValidProduct = false;
	public string topText = "Registreer product door uw activatiecode in te voeren.";
    // Which of the base keys to compare in the check
    public byte CheckKey = 0;
	// This is the most important part.
	// Make sure your base keys are unique
	// choose random keys here and when you want to make a
	// 2.x release change the keys so the 1.x keys are no longer valid
	public uint[] MyBaseKeys = { 4, 16, 12, 64 };
	


	void Start () {

  
        // Init Guardian
        Guardian.Init(MyBaseKeys);
		// Number of letters between each dash in the serial
        Guardian.Spacing = 6;
		
		// Center Window
		windowRect.x = (Screen.width/2)-(windowRect.width/2);
		windowRect.y = (Screen.height/2)-(windowRect.height/2);
		
		// Generate A Test Key
		//Serial = Guardian.Generate("Care-Up");
    
        // Store the serial number in playerprefs so the user does not have to write them everytime.
        string storedSerial = PlayerPrefs.GetString("SerialKey");
       if(storedSerial != string.Empty)
        {
		   ValidProduct = true;
           //Serial was valid we can continue on to the game menu/level
            SceneManager.LoadScene("Menu");
        }
       
        var MySerialNumbers = Guardian.Generate(3, new System.Random(3));
        foreach (var serial in MySerialNumbers)
        {
            WriteToLogFile(serial.Value);
            Debug.Log(serial.Value);
        }

       
    }
	void WriteToLogFile (string message)
    {
        using (System.IO.StreamWriter logfile = new System.IO.StreamWriter(@"C:\Users\Gijs Tempel.000\Desktop\Keycodes\CDkeys.txt"))
        {
            logfile.Write(message);
        }
    }
    void OnGUI() {
        windowRect = GUI.Window(0, windowRect, DoMyWindow, topText);
    }

 
void DoMyWindow(int windowID) {
		Serial = GUILayout.TextField(Serial);
        if (GUILayout.Button("Accepteren")) {
			// Check if the serial input verfies with the basekey and checksum.

			if(Guardian.ValidateKey(Serial, CheckKey, MyBaseKeys[CheckKey])) {
			    ValidProduct = true;
                topText = "Inloggen Geslaagd";
                SceneManager.LoadScene("Menu");
                // Store the key so when we load up next time we dont have to enter serial again.
                PlayerPrefs.SetString("SerialKey", Serial);
            }
            else {
				topText = "Helaas, de code klopt niet. Probeer het opnieuw";
			}
		}        
    }
}
