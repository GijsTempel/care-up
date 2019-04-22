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
        if(gameUI.buttonToBlink == buttonType)
        {
            GetComponent<Animator>().SetTrigger("BlinkOn");
        }
    }   
}
