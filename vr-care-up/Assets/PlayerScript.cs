using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public string currentWTGName = "";
    private GameObject leftHandObject;
    private GameObject rightHandObject;
    
    public void SetObjectInHand(GameObject obj, bool isRightHand = true, bool isPickUp = true)
    {
        string objName = "";
        if (obj != null)
            objName = obj.name;

        if (isRightHand)
        {
            if (isPickUp)
            {
                rightHandObject = obj;
            }
            else
            {
                if (rightHandObject == obj)
                {
                    rightHandObject = null;
                    objName = "";
                }
            }
            Debug.Log("@RightHandObj:" + objName);
        }
        else
        {
            if (isPickUp)
            {
                leftHandObject = obj;
            }
            else
            {
                if (leftHandObject == obj)
                {
                    leftHandObject = null;
                    objName = "";
                }
            }

            Debug.Log("@LeftHandObj:" + objName);
        }
    }

    public void UpdateWalkToGroup(string WTGName)
    {
        currentWTGName = WTGName;
    }

}
