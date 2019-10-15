using System.Collections.Generic;
using UnityEngine;

public class ShopTabsTest : MonoBehaviour
{
    public List<ShopTabButton> buttons;
    public List<GameObject> tabs;

    public void SwitchTab(int value)
    {
        foreach (ShopTabButton button in buttons)
        {
            button.SetState(false);
        }

        buttons[value].SetState(true);

        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
        tabs[value].SetActive(true);
    }   
}
