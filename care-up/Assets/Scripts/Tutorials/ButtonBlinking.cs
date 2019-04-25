using UnityEngine;

public class ButtonBlinking : MonoBehaviour
{
    public GameUI.ItemControlButtonType buttonType;

    GameUI gameUI;

    public void UpdateButtonState()
    {
        GetComponent<Animator>().ResetTrigger("BlinkStart");
        GetComponent<Animator>().ResetTrigger("BlinkStop");
        GetComponent<Animator>().ResetTrigger("BlinkOnes");

        if (gameUI == null)
            gameUI = GameObject.FindObjectOfType<GameUI>();

        bool toBlink = false;

        if (buttonType == GameUI.ItemControlButtonType.Ipad)
        {
            if (gameUI.prescriptionButtonBlink || gameUI.recordsButtonBlink)
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

        if (toBlink)
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
                GetComponent<Animator>().ResetTrigger("BlinkStop");
            }
        }
    }

    public void OnEnable()
    {
        UpdateButtonState();
    }
    public void Start()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
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
