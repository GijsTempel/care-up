using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsInventory : MonoBehaviour
{
    PlayerScript player;
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
    }

    public bool LeftHandEmpty()
    {
        if (player != null && player.GetObjectInHand(true) != null)
            return false;
        return true;
    }

    public bool RightHandEmpty()
    {
        if (player != null && player.GetObjectInHand(false) != null)
            return false;
        return true;
    }



    public ObjectDataHolder LeftHandObject()
    {
        return GetHandObjectDataHolder(true);
    }

    public ObjectDataHolder RightHandObject()
    {
        return GetHandObjectDataHolder(false);
    }

    private ObjectDataHolder GetHandObjectDataHolder(bool isLeft = true)
    {
        if (player != null && player.GetObjectInHand(isLeft) != null)
        {
            if (player.GetObjectInHand(isLeft).GetComponent<ObjectDataHolder>() != null)
                return player.GetObjectInHand(isLeft).GetComponent<ObjectDataHolder>();
        }
        return null;
    }

}
