using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour {

    private GameObject UI_object;
    private GameObject UI_trigger;

    private Vector3 initialPosition;
    private float timer = 0.0f;
    private int direction = 1;

    private static RobotManager instance;
    
    private static Transform eyeL;
    private static Transform eyeLA;
    private static Transform eyeR;
    private static Transform eyeRA;
    private static Transform mouth;
    private static Transform mouthA;

    private static Material eyeLMat;
    private static Material eyeRMat;
    private static Material mouthMat;
    
	void Start ()
    {
        instance = this;

        Transform face = transform.Find("robotArm").Find("main").Find("face");
        eyeL = face.Find("mEye").Find("eye.L");
        eyeR = face.Find("mEye").Find("eye.R");
        eyeLA = face.Find("mAnker").Find("anker.L");
        eyeRA = face.Find("mAnker").Find("anker.R");
        mouth = face.Find("mouth");
        mouthA = face.Find("mouthAnker");

        mouthMat = transform.Find("robot_mouth").GetComponent<Renderer>().material;
        eyeLMat = transform.Find("robot_eye.L").GetComponent<Renderer>().material;
        eyeRMat = transform.Find("robot_eye.R").GetComponent<Renderer>().material;

        UI_object = Camera.main.transform.Find("UI").Find("RobotUI").gameObject;
        UI_object.SetActive(false);

        UI_trigger = Camera.main.transform.Find("UI").Find("RobotUITrigger").gameObject;
        UI_trigger.SetActive(true);

        initialPosition = transform.localPosition;
	}
	
	void Update ()
    {
        UpdateTriggerPosition();
        UpdateFaceAnimations();
	}

    public void TriggerUI()
    {
        // play some kind of UI animation?
        UI_object.SetActive(!UI_object.activeSelf);
        // also play some kind of robot animation?
    }

    private void UpdateTriggerPosition()
    {
        float x = Screen.width - 182.9f;
        float y = Screen.height * 0.63f;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 4.0f));

        transform.LookAt(Camera.main.transform);
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            0.0f);

        Vector3 robotPosition = Camera.main.WorldToScreenPoint(transform.position);
        robotPosition += new Vector3(7.0f, 3.0f); // slight offset
        UI_trigger.transform.position = robotPosition;
    }

    public static void RobotCorrectAction()
    {
        instance.GetComponent<Animator>().SetTrigger("Yes");
    }

    public static void RobotWrongAction()
    {
        instance.GetComponent<Animator>().SetTrigger("No");
    }

    private void UpdateFaceAnimations()
    {
        eyeLMat.mainTextureOffset = new Vector2(
            ((eyeL.localPosition.x + eyeL.parent.localPosition.x) - (eyeLA.localPosition.x + eyeLA.parent.localPosition.x)) / 4,
            ((eyeL.localPosition.y + eyeL.parent.localPosition.y) - (eyeLA.localPosition.y + eyeLA.parent.localPosition.y)) / 4);
        eyeRMat.mainTextureOffset = new Vector2(
            ((eyeR.localPosition.x + eyeR.parent.localPosition.x) - (eyeRA.localPosition.x + eyeRA.parent.localPosition.x)) / 4,
            ((eyeR.localPosition.y + eyeR.parent.localPosition.y) - (eyeRA.localPosition.y + eyeRA.parent.localPosition.y)) / 4);
        mouthMat.mainTextureOffset = new Vector2(
            (mouth.localPosition.x - mouthA.localPosition.x) / 4,
            (mouth.localPosition.y - mouthA.localPosition.y) / 4);
    }
}
