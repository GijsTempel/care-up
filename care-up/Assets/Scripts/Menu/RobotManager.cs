using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour {

    public bool top = true;

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

    public void TriggerUI(bool value)
    {
        // play some kind of UI animation?
        UI_object.SetActive(value);

        if (!value)
        {
            GameObject.FindObjectOfType<PlayerScript>().ResetUIHover();
        }
    }

    private void UpdateTriggerPosition()
    {
        float x = Screen.width - 182.9f;
        float y = Screen.height * (top ? 0.63f : 0.15f);
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
            (eyeL.parent.localPosition.x - eyeL.localPosition.x - eyeLA.parent.localPosition.x - eyeLA.localPosition.x - 0.1772721f) * 2,
            (eyeLA.parent.localPosition.y - eyeLA.localPosition.y - eyeL.parent.localPosition.y - eyeL.localPosition.y - 0.1221102f) * 2
            );

        eyeRMat.mainTextureOffset = new Vector2(
            (eyeR.parent.localPosition.x - eyeR.localPosition.x - eyeRA.parent.localPosition.x - eyeRA.localPosition.x + 0.1772721f) * 2,
            (eyeRA.parent.localPosition.y - eyeRA.localPosition.y - eyeR.parent.localPosition.y - eyeR.localPosition.y - 0.1221102f) * 2
            );
        mouthMat.mainTextureOffset = new Vector2(
            (mouth.localPosition.x - mouthA.localPosition.x) * 2f,
            (mouth.localPosition.y - mouthA.localPosition.y) * 2f * 1.11900882674f
            );
 
        
          
} 
}
	 