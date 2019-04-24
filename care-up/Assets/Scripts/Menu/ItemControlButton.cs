using UnityEngine;

public class ItemControlButton : MonoBehaviour
{
    public GameUI.ItemControlButtonType buttonType;
    GameUI gameUI;

    void Start()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
    }

    private void OnEnable()
    {
        if(gameUI == null)
        {
            gameUI = GameObject.FindObjectOfType<GameUI>();
        }
        if (buttonType == GameUI.ItemControlButtonType.DropLeft)
        {
            if (gameUI.DropLeftBlink)
                GetComponent<Animator>().SetTrigger("BlinkOn");
        }
        else if (buttonType == GameUI.ItemControlButtonType.DropRight)
        {
            if (gameUI.DropRightBlink)
                GetComponent<Animator>().SetTrigger("BlinkOn");
        }
        else if(gameUI.buttonToBlink == buttonType)
        {
            GetComponent<Animator>().SetTrigger("BlinkOn");
        }
    }   
}
