using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI : MonoBehaviour {
	GameObject Player;


	public void MoveBack()
	{
		Player.GetComponent<PlayerScript>().MoveBackButton();
	}



	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
