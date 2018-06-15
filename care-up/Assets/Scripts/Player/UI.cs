using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI : MonoBehaviour {
	GameObject Player;
	public Animator Blink;

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


	public void BlinkRed()
	{
		Blink.SetTrigger("BlinkRed");
	}

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
