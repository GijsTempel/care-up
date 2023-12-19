using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAirFlowVolume : MonoBehaviour
{
    public GameObject boneToControl;
    private float volumeValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        volumeValue = boneToControl.transform.localPosition.y / 0.2f;
        gameObject.GetComponent<AudioSource>().volume = volumeValue;
        Debug.Log("Bone position value: " + boneToControl.transform.localPosition.y.ToString()) ;
        Debug.Log("Volume value: " + volumeValue.ToString()); ;
    }
}
