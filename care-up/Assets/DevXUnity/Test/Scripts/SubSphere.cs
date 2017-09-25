﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubSphere : MonoBehaviour {

    public float speed = 1f;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        SubUpdate();

    }

    void SubUpdate()
    {
        this.transform.RotateAround(gameObject.transform.parent.transform.position, new Vector3(0, 1f, 0), speed*15 * Time.smoothDeltaTime);
        this.transform.Rotate(0, 15*Time.smoothDeltaTime, 0);
    }
}
