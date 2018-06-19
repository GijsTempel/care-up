using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimatorStateUpdater : MonoBehaviour {

	public int ElementID = 0;

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable()
    {
        Debug.Log("PrintOnEnable: script was enabled");
		if (RobotManager.UIElementsState[ElementID])
		{
			transform.GetComponent<Animator>().SetTrigger("BlinkStart");
		}

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
