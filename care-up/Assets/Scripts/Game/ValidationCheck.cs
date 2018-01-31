using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace LoginProAsset
{
public class ValidationCheck : MonoBehaviour
	{
		
		private LoadingScreen loadingScreen;
		private void Start()
		{
			if (GameObject.Find("Preferences") != null)
			{
				loadingScreen = GameObject.Find("Preferences").GetComponent<LoadingScreen>();
				if (loadingScreen == null) Debug.LogError("No loading screen found");
			}
			else
			{
				Debug.LogWarning("No 'preferences' found. Game needs to be started from first scene");
			}
		}
	// Update is called once per frame
	void Update () {
		
	}
	//Get player data from server.
	public void GetFromServer()
	{
		// Here datas are null since we don't want to SEND anything (but we want something)
		LoginPro.Manager.ExecuteOnServer("GetData",Startgame, null);
	}
	//check if code is correct and set to true in database. 
		public void Startgame(string[] datas)
		{
			if (datas [2]=="True") 
			{
				//Popup.Show ("Login succesvol", 5);
				Debug.Log ("code klopt");
                GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set("Login succesvol");
                SceneManager.LoadScene("Menu");

			} 
			else 
			{
                GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set("Je hebt geen gevalideerd product");
                Debug.Log("code klopt Niet");
            }
		}
}
}