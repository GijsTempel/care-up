using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class PickableObject : MonoBehaviour
{
    bool isKinematic = false;
    private PlayerScript player;
    public HandPoseData pickedBy = null;
    private Transform transformToFallow;

    public void Drop()
    {
        transformToFallow = null;
        if (gameObject.GetComponent<Rigidbody>() != null)
            gameObject.GetComponent<Rigidbody>().isKinematic = isKinematic;
}
    
    public bool PickUp(Transform handTransform)
    {
        transformToFallow = handTransform;
        Debug.Log("@ ## " + name + ":" + Random.Range(0, 9999).ToString());
        if (gameObject.GetComponent<Rigidbody>() != null)
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        return true;
    }

    private void Update()
    {
        if (transformToFallow != null)
        {
            transform.position = transformToFallow.position;
            transform.rotation = transformToFallow.rotation;
        }

    }
    private void Start()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
            isKinematic = gameObject.GetComponent<Rigidbody>().isKinematic;
        player = GameObject.FindObjectOfType<PlayerScript>();
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(PickupAction);
        grabInteractable.selectExited.AddListener(DropAction);
    }

    private void PickupAction(BaseInteractionEventArgs arg)
    {
        if (player == null)
            return;
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandPoseData handData = arg.interactorObject.transform.GetComponentInChildren<HandPoseData>();
            bool isRightHand = (handData.handType == HandPoseData.HandModelType.Right);
            player.SetObjectInHand(gameObject, isRightHand);
            pickedBy = handData;

        }
    }

    private void DropAction(BaseInteractionEventArgs arg)
    {
        if (player == null)
            return;
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandPoseData handData = arg.interactorObject.transform.GetComponentInChildren<HandPoseData>();
            bool isRightHand = (handData.handType == HandPoseData.HandModelType.Right);
            player.SetObjectInHand(gameObject, isRightHand, false);
            pickedBy = null;

        }
    }

}
