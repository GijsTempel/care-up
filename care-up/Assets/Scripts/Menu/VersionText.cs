using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VersionText : MonoBehaviour {

	void Start () {
        GetComponent<Text>().text = "Versie: " + Application.version;
    }

}
