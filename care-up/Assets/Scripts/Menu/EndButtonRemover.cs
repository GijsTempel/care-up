using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class EndButtonRemover : MonoBehaviour {

    [SerializeField] private GameObject canvas;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonClick () {
        canvas.SetActive(false);
    }
}
