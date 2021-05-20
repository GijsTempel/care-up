using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public bool top = true;
    //private GameObject Game_UI;
    private GameObject UI_object;
    public static bool[] UIElementsState = { false, false };

    private static Transform notification;
    //private static int notificationCount = 0;

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

    void Start()
    {
        instance = this;
        //Game_UI = GameObject.FindObjectOfType<GameUI>().gameObject;

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

        UI_object = GameObject.Find("PatientInfoTabs/Info");

        // UI_trigger = GameObject.Find("RobotUITrigger").gameObject;
        // UI_trigger.SetActive(true);

        // notification = UI_trigger.transform.Find("Notification");
        //  notification.gameObject.SetActive(false);

        //if (GameObject.FindObjectOfType<TutorialManager>() != null &&
        //    GameObject.FindObjectOfType<Tutorial_UI>() == null &&
        //    GameObject.FindObjectOfType<Tutorial_Theory>() == null)
        //{
        //    SetUITriggerActive(false);
        //}

        // reset counter
        //notificationCount = 0;
    }

    void Update()
    {
        UpdateFaceAnimations();
    }

    public void TriggerUI(bool value)
    {
        UI_object.SetActive(value);
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

    public void ToggleCloseBtn(bool value)
    {
        GameObject.Find("PatientInfoTabs").transform.Find("TopBarUI/CloseBtn").gameObject.SetActive(value);
    }

    public static void SetUITriggerActive(bool value)
    {
        //UI_trigger.SetActive(value);
        GameObject.FindObjectOfType<GameUI>().UpdateWalkToGroupUI(value);
    }

    //public static void SetNotification(int n)
    //   {
    //	if (n > notificationCount)
    //	{
    //           if (UI_trigger.GetComponent<Animator>() != null && UI_trigger.GetComponent<Animator>().isActiveAndEnabled)
    //               UI_trigger.GetComponent<Animator>().SetTrigger("BlinkWithoutHint");
    //           UIElementsState[0] = true;
    //	}

    //       notificationCount = n;

    //       if (n > 0)
    //       {
    //           notification.gameObject.SetActive(true);
    //           notification.Find("Text").GetComponent<Text>().text = n.ToString();
    //       }
    //       else
    //       {
    //           notification.gameObject.SetActive(false);
    //       }

    //       RobotUIMessageTab.SetNotification(n);
    //   }

    //public static int NotificationNumber
    //{
    //    get { return notificationCount; }
    //}
}
