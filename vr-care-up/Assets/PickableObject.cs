using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class PickableObject : MonoBehaviour
{
    private PlayerScript player;
    private void Start()
    {
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
        }
    }

}
