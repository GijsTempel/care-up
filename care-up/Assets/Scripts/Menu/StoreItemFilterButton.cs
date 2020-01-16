using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemFilterButton : MonoBehaviour
{
    public GameObject SelectedCircle;
    private bool state = false;
    public TabGroup.FilterModes FilterMode;

    TabGroup tabGroup;

    // Start is called before the first frame update
    void Start()
    {
        tabGroup = GameObject.FindObjectOfType<TabGroup>();
    }

    public bool GetState()
    {
        return state;
    }

    public void Pressed()
    {
        print("FFFFFFFFFFFFF");
        tabGroup.FilterItems(FilterMode);
    }

    public void Select (bool toSelect)
    {
        state = toSelect;
        SelectedCircle.SetActive(toSelect);
    }
}
