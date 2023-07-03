using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabs : MonoBehaviour
{
    public List<GameObject> tabs;

    public void ShowTab(int tabIndex)
    {
        if (tabIndex > tabs.Count || tabIndex < 0)
            return;
        foreach(GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
        tabs[tabIndex].SetActive(true);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
