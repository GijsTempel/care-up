using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI : MonoBehaviour {
	GameObject Player;
	public Animator Blink;
	bool BlinkState = false;

	public void MoveBack()
	{
		Player.GetComponent<PlayerScript>().MoveBackButton();
	}

	public void OpenRobotUI()
    {
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
        }
        else
        {
            Blink.SetTrigger("BlinkStop");
        }
	}

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
