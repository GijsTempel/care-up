using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attaching this script to a gameObject will 
/// </summary>
public class HandsTrackingTouchHandler : MonoBehaviour
{
    public GameObject linkToFireTo = null;

    private float timer = 0f;

    private void Start()
    {
        // if target not specified, consider THIS as target
        linkToFireTo = (linkToFireTo == null ? this.gameObject : linkToFireTo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "RightHandIndex3_end")
        {
            timer = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "RightHandIndex3_end")
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                UnityEngine.UI.Button targetButton = linkToFireTo.GetComponent<UnityEngine.UI.Button>();
                if (targetButton != null)
                {
                    // later implement proper touch, stick to the position on triggerEnter, unstick and fire function on Exit
                    // for now, lazy check whether it was an intentional touch: touch needs to be longer then .2f
                    targetButton.onClick.Invoke();
                }

                // lazy way of making sure it fires once without new variables
                timer = Mathf.NegativeInfinity;
            }
        }
    }
}