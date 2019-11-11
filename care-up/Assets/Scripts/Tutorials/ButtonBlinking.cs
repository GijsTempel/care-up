using UnityEngine;

public class ButtonBlinking : MonoBehaviour
{
    public GameUI.ItemControlButtonType buttonType;
    PlayerPrefsManager prefs;

    GameUI gameUI;

    public void Start()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
        prefs = GameObject.FindObjectOfType<PlayerPrefsManager>();
    }

    public void OnEnable()
    {       
        prefs = GameObject.FindObjectOfType<PlayerPrefsManager>();
        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        GetComponent<Animator>().ResetTrigger("BlinkStart");
        GetComponent<Animator>().ResetTrigger("BlinkStop");
        GetComponent<Animator>().ResetTrigger("BlinkOnes");

        if (prefs != null)
            if (!prefs.practiceMode)
                return;        

        if (gameUI == null)
            gameUI = GameObject.FindObjectOfType<GameUI>();

        bool toBlink = false;

        if (buttonType == GameUI.ItemControlButtonType.Ipad)
        {
            if (gameUI.prescriptionButtonBlink || gameUI.recordsButtonBlink || gameUI.paperAndPenButtonblink)
                toBlink = true;
        }
        else if (buttonType == GameUI.ItemControlButtonType.Records)
        {
            if (gameUI.recordsButtonBlink)
                toBlink = true;
        }
        else if (buttonType == GameUI.ItemControlButtonType.Prescription)
        {
            if (gameUI.prescriptionButtonBlink)
                toBlink = true;
        }
        else if (buttonType == GameUI.ItemControlButtonType.General)
        {
            if (gameUI.prescriptionButtonBlink || gameUI.recordsButtonBlink)
                toBlink = true;
        }
        else if (buttonType == GameUI.ItemControlButtonType.PaperAndPen)
        {
            if (gameUI.paperAndPenButtonblink)
                toBlink = true;
        }
        else if (buttonType == GameUI.ItemControlButtonType.RecordsBack)
        {
            if (!gameUI.recordsButtonBlink && (gameUI.prescriptionButtonBlink || gameUI.paperAndPenButtonblink))
                toBlink = true;
        }
        else if (buttonType == GameUI.ItemControlButtonType.PrescriptionBack)
        {
            if (!gameUI.prescriptionButtonBlink && (gameUI.paperAndPenButtonblink || gameUI.recordsButtonBlink))
                toBlink = true;
        }
        else if (buttonType == GameUI.ItemControlButtonType.GeneralBack)
        {
            if (gameUI.paperAndPenButtonblink && !(gameUI.recordsButtonBlink || gameUI.prescriptionButtonBlink))
                toBlink = true;
        }
        else if (buttonType == GameUI.ItemControlButtonType.MessageTabBack)
        {
            if (gameUI.paperAndPenButtonblink || gameUI.recordsButtonBlink || gameUI.prescriptionButtonBlink)
                toBlink = true;
        }
        else if (buttonType == GameUI.ItemControlButtonType.Close)
        {
            if (!gameUI.recordsButtonBlink && !gameUI.prescriptionButtonBlink && !gameUI.paperAndPenButtonblink &&
                GameObject.Find("QuizDynamicCanvas") == null)
                toBlink = true;
        }

        if(prefs != null)
        {
            if (toBlink && prefs.practiceMode)
            {
                if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Blink"))
                {
                    GetComponent<Animator>().SetTrigger("BlinkStart");
                }
            }
            else
            {
                if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Blink"))
                {
                    GetComponent<Animator>().SetTrigger("BlinkStop");
                }
            }
        }      
    }

    public void StartBlinking()
    {
        GetComponent<Animator>().SetTrigger("BlinkStart");
    }

    public void StopBlinking()
    {
        if (GetComponent<Animator>() != null && GetComponent<Animator>().isActiveAndEnabled)
            GetComponent<Animator>().SetTrigger("BlinkStop");
    }

    public void JoystickStopBlinking()
    {
        GameObject.Find("JoystickKnob").GetComponent<Animator>().SetTrigger("BlinkStop");
        GameObject.Find("JoystickBackground").GetComponent<Animator>().SetTrigger("BlinkStop");
    }
}
