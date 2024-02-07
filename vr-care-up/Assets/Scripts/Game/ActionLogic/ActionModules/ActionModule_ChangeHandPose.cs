using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModule_ChangeHandPose : MonoBehaviour
{
    public enum ChangingHand {Left, Right}
    public ChangingHand changingHand = ChangingHand.Left;
    public string grabHandPoseObjectName = "";
    public string controlObjectName = "";
    public string handPoseDataObjectName = "";


    void DoChangePose(GrabHandPose grabHandPose, HandPoseData newHandPoseData)
    {
        if (changingHand == ChangingHand.Left)
            grabHandPose.leftHandPose = newHandPoseData;
        else
            grabHandPose.righHandPose = newHandPoseData;
    }


    public void Execute()
    {
        GameObject grabHandPoseObject = GameObject.Find(grabHandPoseObjectName);
        if (grabHandPoseObject == null)
            return;
        if (grabHandPoseObject.GetComponent<GrabHandPose>() == null)
            return;

        GameObject newHandPoseDataObject = null;
        if (controlObjectName == "" || controlObjectName == "-")
        {
            newHandPoseDataObject = GameObject.Find(handPoseDataObjectName);
        }
        else
        {
            GameObject controlObject = GameObject.Find(controlObjectName);
            if (controlObject == null)
                return;
            if (controlObject.GetComponent<ShowHideObjects>() == null)
                return;
            newHandPoseDataObject = controlObject.GetComponent<ShowHideObjects>().GetObjectByName(handPoseDataObjectName);
        }

        if (newHandPoseDataObject == null)
            return;
        if (newHandPoseDataObject.GetComponent<HandPoseData>() == null)
            return;
        DoChangePose(grabHandPoseObject.GetComponent<GrabHandPose>(), newHandPoseDataObject.GetComponent<HandPoseData>());
    }
}
