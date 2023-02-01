using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    GameObject _PlayerLookAtPoint; 
    // Start is called before the first frame update
    void Start()
    {
        _PlayerLookAtPoint = GameObject.Find("PlayerLookAtPoint");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.LookAt(_PlayerLookAtPoint.transform);
    }
}
