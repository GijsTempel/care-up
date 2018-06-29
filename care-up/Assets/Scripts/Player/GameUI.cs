using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
	GameObject Player;
	public Animator Blink;
	public Animator IPadBlink;
	bool BlinkState = false;
	public bool testValue;
	GameObject donePanel;
	GameObject closeButton;
	GameObject closeDialog;
    
    

	public void MoveBack()
	{
		Player.GetComponent<PlayerScript>().MoveBackButton();
	}

	public void OpenRobotUI()
    {
		RobotManager.UIElementsState[0] = false;
        Player.GetComponent<PlayerScript>().OpenRobotUI();
    }

	public void ToggleUsingOnMode()
    {
		Player.GetComponent<PlayerScript>().ToggleUsingOnMode(false);
    }

	public void CloseButtonPressed(bool value)
	{
		closeDialog.SetActive(value);
		closeButton.SetActive(!value);
	}

	public void CloseGame()
	{
		bl_SceneLoaderUtils.GetLoader.LoadLevel("Menu");
	}


	public void ButtonBlink(bool ToBlink)
	{
		if (BlinkState == ToBlink)
			return;
		BlinkState = ToBlink;
        if (transform.Find("Extra").gameObject.activeSelf)
        {
            Blink.SetTrigger("BlinkOnes");
			BlinkState = false;
        }
        else if (ToBlink)
        {
            Blink.SetTrigger("BlinkStart");
			RobotManager.UIElementsState[1] = true;
        }
        else
        {
            Blink.SetTrigger("BlinkStop");
			RobotManager.UIElementsState[1] = false;
        }
	}

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		closeButton = transform.Find("CloseBtn").gameObject;
		closeDialog = transform.Find("CloseDialog").gameObject;
		closeDialog.SetActive(false);
		donePanel = transform.Find("DonePanel").gameObject;
		donePanel.SetActive(false);

		//Debug.Log(Application.isEditor);
	}

	public void ShowDonePanel(bool value)
	{
		donePanel.SetActive(value);
	}
    
	public void EndScene()
    {
        GameObject.Find("Preferences").GetComponent<EndScoreManager>().LoadEndScoreScene();
    }
	
	// Update is called once per frame
	void Update () {
		testValue = RobotManager.UIElementsState[0];
	}
}
