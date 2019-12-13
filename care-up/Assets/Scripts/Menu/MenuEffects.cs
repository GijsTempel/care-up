using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEffects : MonoBehaviour
{
    public GameObject slider;
    public List<GameObject> MenuButtons;
    int currentSelected = -1;
    UMP_Manager mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu = GameObject.FindObjectOfType<UMP_Manager>();
        currentSelected = mainMenu.GetCurrentWindow();
        Invoke("ApplyMenuEffect", 0.02f);
    }

    private int GetButtonNumber()
    {
        int w = mainMenu.GetCurrentWindow();
        int result = -1;
        switch (w)
        {
            case 0:
                result = 0;
                break;
            case 6:
                result = 1;
                break;
            case 5:
                result = 2;
                break;
            case 10:
                result = 3;
                break;
            case 1:
                result = 4;
                break;
        }
        return result;
    }

    public void ApplyMenuEffect()
    {
        int nextSelected = GetButtonNumber();
        //if (currentSelected == nextSelected)
        //    return;
        float nextSliderPos = -900;
        if(currentSelected >= 0)
            MenuButtons[currentSelected].GetComponent<Animator>().SetTrigger("down");
        if (nextSelected >= 0)
        {
            MenuButtons[nextSelected].GetComponent<Animator>().SetTrigger("up");
            nextSliderPos = MenuButtons[nextSelected].transform.parent.GetComponent<RectTransform>().anchoredPosition.x;
        }

        Vector2 anch = slider.GetComponent<RectTransform>().anchoredPosition;
        anch.x = nextSliderPos;
        slider.GetComponent<RectTransform>().anchoredPosition = anch;
        currentSelected = nextSelected;
    }

}
