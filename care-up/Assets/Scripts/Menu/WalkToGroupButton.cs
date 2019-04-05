using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WalkToGroupButton : MonoBehaviour {

    WalkToGroup linkedWalkToGroup = null;
    Color ButtonColor = Color.white;

    //button components
    [HideInInspector]
    public GameObject blur;
    [HideInInspector]
    public GameObject bg;
    [HideInInspector]
    public GameObject bg_h;
    [HideInInspector]
    public GameObject _icon;

    void Start()
    {
        ButtonColor = GetComponent<Image>().color;
        if (transform.Find("blur"))
        {
            blur = transform.Find("blur").gameObject;
            Color b2 = ButtonColor;
            b2.a = 0.4f;
            blur.GetComponent<Image>().color = b2;
            blur.SetActive(false);
            bg = transform.Find("bg").gameObject;
            bg_h = transform.Find("bg_h").gameObject;
            bg_h.SetActive(false);
            _icon = transform.Find("icon").gameObject;
            _icon.GetComponent<Image>().sprite = Resources.Load("Sprites/WalkGroup_Icons/" + name, typeof(Sprite)) as Sprite;

        }
    }

    public void setWalkToGroup(WalkToGroup wtg)
    {
        linkedWalkToGroup = wtg;
    }


    public void HighlightGroup(bool value)
    {
        if (linkedWalkToGroup != null)
        linkedWalkToGroup.ButtonHovered = value;
   
        HighlightButton(value);
        print(name);
    }

    public void HighlightButton(bool value)
    {
        if (blur != null)
        {
            blur.SetActive(value);
            if (value)
                GetComponent<Image>().sprite = Resources.Load("Sprites/WalkGroup_Icons/button_ring", typeof(Sprite)) as Sprite;
            else
                GetComponent<Image>().sprite = Resources.Load("Sprites/WalkGroup_Icons/button_ring_small", typeof(Sprite)) as Sprite;
            bg_h.SetActive(value);
            bg.SetActive(!value);
        }

    }

    public void MoveToGroup()
    {
        if (linkedWalkToGroup != null)
        {
            GameObject.FindObjectOfType<PlayerScript>().WalkToGroup(linkedWalkToGroup);
            linkedWalkToGroup.ButtonHovered = false;
            HighlightButton(false);
        }
    }

}
