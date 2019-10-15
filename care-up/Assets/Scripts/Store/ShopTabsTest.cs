using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTabsTest : MonoBehaviour
{
    public List<ShopTabButton> buttons;
    public List<GameObject> tabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
