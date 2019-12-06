using UnityEngine;

public class ItemControlButton : MonoBehaviour
{
    public GameUI.ItemControlButtonType buttonType;
    GameUI gameUI;
    PlayerPrefsManager prefs;

    void Start()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
        prefs = GameObject.FindObjectOfType<PlayerPrefsManager>();
        
        print(GetComponentInParent<Canvas>().name);
        Vector3 vec = new Vector3();
        // RectTransformUtility.WorldToScreenPoint(Camera.ma(GetComponentInParent<RectTransform>(),
        //     transform.position, Camera.main, out vec);
        print(GetComponent<RectTransform>().position);
        // RectTransformUtility.PixelAdjustPoint
        print(vec);
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
        else if (gameUI.buttonToBlink == buttonType && !directionActive)
        {
            {
                toBlink = true;
            }
        }
        if (!toBlink)
        {
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ItemBlink"))
                GetComponent<Animator>().SetTrigger("BlinkOff");
        }
        else if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ItemBlink"))
            GetComponent<Animator>().SetTrigger("BlinkOn");
    }

    private void OnEnable()
    {
        prefs = GameObject.FindObjectOfType<PlayerPrefsManager>();
        UpdateBlinkState();
    }
}
