using UnityEngine;
using UnityEngine.UI;


public class ItemControlButton : MonoBehaviour
{
    public GameUI.ItemControlButtonType buttonType;
    GameUI gameUI;
    PlayerPrefsManager prefs;

    void Start()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
        prefs = GameObject.FindObjectOfType<PlayerPrefsManager>();
    }

    public void UpdateBlinkState()
    {
        if (prefs != null)
            if (!prefs.practiceMode)
                return;
        GetComponent<Animator>().ResetTrigger("BlinkOn");
        GetComponent<Animator>().ResetTrigger("BlinkOff");
        bool toBlink = false;
        if (gameUI == null)
            gameUI = GameObject.FindObjectOfType<GameUI>();
        bool directionActive = gameUI.moveButtonToBlink != GameUI.ItemControlButtonType.None;

        if (buttonType == GameUI.ItemControlButtonType.DropLeft)
        {
            if (gameUI.DropLeftBlink)
            {
                toBlink = true;
            }
        }
        else if (buttonType == GameUI.ItemControlButtonType.DropRight)
        {
            if (gameUI.DropRightBlink)
            {
                toBlink = true;
            }
        }
        else if (buttonType == GameUI.ItemControlButtonType.MoveLeft || buttonType == GameUI.ItemControlButtonType.MoveRight)
        {
            if (gameUI.moveButtonToBlink == buttonType)
            {
                toBlink = true;
            }
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
        else if (buttonType == GameUI.ItemControlButtonType.PaperAndPen)
        {
            if (gameUI.paperAndPenButtonblink && !gameUI.LevelEnded)
                toBlink = true;
        }
        else if (gameUI.buttonToBlink == buttonType && !directionActive)
        {
            {
                toBlink = true;
            }
        }
        if (!toBlink)
        {
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ItemBlink") 
                || GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Blink"))
                    GetComponent<Animator>().SetTrigger("BlinkOff");
            //GetComponent<Button>().onClick.Invoke();
        }
        else if (!(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ItemBlink") 
            || GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Blink")))
        {
            //GameObject.FindObjectOfType<AnimatedFingerHint>().MoveTo(GetComponent<RectTransform>().position);
            GetComponent<Animator>().SetTrigger("BlinkOn");
        }
    }

    public void StopBlinking()
    {
        GetComponent<Animator>().SetTrigger("BlinkOff");
    }

    private void OnEnable()
    {
        prefs = GameObject.FindObjectOfType<PlayerPrefsManager>();
        UpdateBlinkState();
    }
}
