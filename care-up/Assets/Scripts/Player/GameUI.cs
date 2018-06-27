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
		donePanel = transform.Find("DonePanel").gameObject;
		GameObject doneButton = donePanel.transform.Find("Button").gameObject;
        GameObject sl = GameObject.Find("SceneLoader 1");
        doneButton.GetComponent<Button>().onClick.AddListener(delegate { sl.GetComponent<bl_SceneLoader>().LoadLevelButton("Menu"); });
		donePanel.active = false;

		//Debug.Log(Application.isEditor);
	}
	
	// Update is called once per frame
	void Update () {
		testValue = RobotManager.UIElementsState[0];
	}
}
