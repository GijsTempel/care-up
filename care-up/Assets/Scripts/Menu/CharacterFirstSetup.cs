using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterFirstSetup : MonoBehaviour
{
    public Text BigNumberHolder;
    public Text FullName;
    int currentTab = 0;
    public List<GameObject> tabs;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool CheckFirstTab()
    {
        bool check = true;
        if (BigNumberHolder.text == "")
        {
            BigNumberHolder.transform.GetComponentInParent<Animator>().SetTrigger("red");
            check = false;
        } 

        if (FullName.text == "")
        {
            FullName.transform.GetComponentInParent<Animator>().SetTrigger("red");
            check = false;
        } 
        return check;
    }

    public void SetTab(int tab)
    {
        bool check = true;
        if (currentTab == 0)
        {
            check = CheckFirstTab();
        }

        if (check && tab >= 0 && tab < tabs.Count)
        {
            foreach(GameObject t in tabs)
            {
                t.SetActive(false);
            }
            tabs[tab].SetActive(true);
            currentTab = tab;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
