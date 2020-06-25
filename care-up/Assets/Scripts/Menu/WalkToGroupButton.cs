using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WalkToGroupButton : MonoBehaviour {

    WalkToGroup linkedWalkToGroup = null;
    Color ButtonColor = Color.white;
    bool mouse_over = false;
    //button components
    [HideInInspector]
    public GameObject bg_h;
    public GameObject _icon;
    GameObject finger = null;
    GameUI gameUI;
    PlayerScript ps;
    public bool SideButton = false;

    void Start()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
        ps = GameObject.FindObjectOfType<PlayerScript>();
        if (!SideButton)
        {
            finger = transform.Find("f").gameObject;
            //finger.SetActive(false);
        }
        ButtonColor = GetComponent<Image>().color;
        if (transform.Find("blur"))
        {
            Color b2 = ButtonColor;
            b2.a = 0.4f;
            //blur.SetActive(false);
            bg_h = transform.Find("bg_h").gameObject;
            bg_h.SetActive(false);
            _icon = transform.Find("icon").gameObject;
            if (!SideButton)
                _icon.GetComponent<Image>().sprite = Resources.Load("Sprites/WalkGroup_Icons/" + name, typeof(Sprite)) as Sprite;
        }
    }

    public void UpdateHint()
    {
        if (SideButton)
            return;
        if (gameUI == null)
            gameUI = GameObject.FindObjectOfType<GameUI>();
        if (finger == null)
            finger = transform.Find("f").gameObject;
        if (linkedWalkToGroup == null)
            return;
        finger.SetActive(false);
        foreach (string s in gameUI.reqPlaces)
        {
            print(s );
            if (s == linkedWalkToGroup.name)
                finger.SetActive(true);
        }
    }

    public void setWalkToGroup(WalkToGroup wtg)
    {
        linkedWalkToGroup = wtg;
        if (SideButton)
        {
            GameUI gameUI = GameObject.FindObjectOfType<GameUI>();
            if (_icon == null)
            {
                _icon = transform.Find("bg_h/icon").gameObject;
            }
            switch (wtg.WalkToGroupType)
            {
                case WalkToGroup.GroupType.WorkField:
                    _icon.GetComponent<Image>().sprite = Resources.Load("Sprites/WalkGroup_Icons/MoveWorkfield" , typeof(Sprite)) as Sprite;
                    break;
                case WalkToGroup.GroupType.Doctor:
                    _icon.GetComponent<Image>().sprite = Resources.Load("Sprites/WalkGroup_Icons/MoveCollegue", typeof(Sprite)) as Sprite;
                    break;
                case WalkToGroup.GroupType.Patient:
                    _icon.GetComponent<Image>().sprite = Resources.Load("Sprites/WalkGroup_Icons/Movepatient", typeof(Sprite)) as Sprite;
                    break;
                case WalkToGroup.GroupType.Sink:
                    _icon.GetComponent<Image>().sprite = Resources.Load("Sprites/WalkGroup_Icons/MoveSink", typeof(Sprite)) as Sprite;
                    break;
                case WalkToGroup.GroupType.NotSet: // let's use this when we're setting icon/color from gameObject
                    _icon.GetComponent<Image>().sprite = Resources.Load("Sprites/WalkGroup_Icons/" + wtg.name, typeof(Sprite)) as Sprite;
                    setColor(gameUI.WTGButtons[wtg.name].GetComponent<Image>().color);
                    break;
            }
        }
    }

    public void HighlightGroup(bool value)
    {
        if (linkedWalkToGroup != null)
        linkedWalkToGroup.ButtonHovered = value;
   
        HighlightButton(value);
    }

    public void HighlightButton(bool value)
    {
        mouse_over = value;
    }

    void OnEnable()
    {
        HighlightButton(false);
        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position = Input.mousePosition;
        List<RaycastResult> hits = new List<RaycastResult>();
        if (EventSystem.current != null)
        {
            EventSystem.current.RaycastAll(pe, hits);
            foreach (RaycastResult h in hits)
            {
                if (h.gameObject == gameObject)
                    HighlightButton(true);
            }
        }
    }

    public void MoveToGroup()
    {
        if (PlayerAnimationManager.IsLongAnimation())
            return;
        if (linkedWalkToGroup != null)
        {
            if (ps == null)
                ps = GameObject.FindObjectOfType<PlayerScript>();
            ps.WalkToGroup_(linkedWalkToGroup);
            linkedWalkToGroup.ButtonHovered = false;
            if (!SideButton)
                HighlightButton(false);
        }
    }
}
