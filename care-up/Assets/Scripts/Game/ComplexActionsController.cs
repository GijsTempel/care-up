using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexActionsController : MonoBehaviour {
    public List<GameObject> objects;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SetTrigger(string trigger)
    {
        switch (trigger)
        {
            case "Patient Wash Hands":
                print("+++++++++++++++++++++++Patient Wash Hands");
                GameObject.Find("woman_patient3").GetComponent<Animator>().SetTrigger("wash");
                break;

        }
    }
}
