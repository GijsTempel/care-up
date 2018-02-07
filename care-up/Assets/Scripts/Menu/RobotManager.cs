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
    
    private static Transform controlBone;

    private static Material eyeLMat;
    private static Material eyeRMat;
    private static Material mouthMat;
    
	void Start ()
    {
        instance = this;
        
        controlBone = transform.Find("robotArm").Find("main").Find("face").Find("mAnker");

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
        eyeLMat.mainTextureOffset = new Vector2(controlBone.localPosition.x / 4, 0.0f);
        eyeRMat.mainTextureOffset = new Vector2(controlBone.localPosition.x / 4, 0.0f);
        mouthMat.mainTextureOffset = new Vector2(controlBone.localPosition.x / 4, 0.0f);
    }
}
