using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour {
    Vector3 start_pos;
    Material mat;
    public GameObject blinkBone;

	// Use this for initialization
	void Start () {
        start_pos = blinkBone.transform.localPosition;
        mat = GetComponent<MeshRenderer>().material;

    }
	
	// Update is called once per frame
	void Update () {
        float value = (start_pos.y - blinkBone.transform.localPosition.y) * 10f;
        mat.color = new Color(0f,0f,0f, value);

    }
}
