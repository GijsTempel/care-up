using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attaching this script to a gameObject will 
/// </summary>
public class HandsTrackingTouchHandler : MonoBehaviour
{
    public GameObject linkToFireTo = null;

    private void Start()
    {
        // if target not specified, consider THIS as target
        linkToFireTo = (linkToFireTo == null ? this.gameObject : linkToFireTo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "RightHandIndex3_end")
        {
            UnityEngine.UI.Button targetButton = linkToFireTo.GetComponent<UnityEngine.UI.Button>();
            if (targetButton != null)
            {
                // test later whether exit is "upwards"
                targetButton.onClick.Invoke();
                Debug.Log("AYO INVOKE " + linkToFireTo.name + " " + Time.time);
            }
        }
    }
}