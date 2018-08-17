using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementsManuButton : MonoBehaviour
{
    public bool opened = false;
    public Color openedTitleColior;
    public Color closedTitleColior;
    public GameObject overlayObj;
    public int iconID = 0;
    public string title = "First Login";
    public string desc = "Injecteren intramusculair (loodrechttechniek)";

    Text titleText;
    Text descText;

    Image icon;
    private void Start()
    {
        titleText = transform.Find("achTitle").GetComponent<Text>();
        descText = transform.Find("achDesc").GetComponent<Text>();

        icon = transform.Find("achIcon").GetComponent<Image>();
        UpdateElement();
    }

    public void UpdateElement()
    {
        overlayObj.SetActive(!opened);
        if (opened)
            titleText.color = openedTitleColior;
        else
            titleText.color = closedTitleColior;

        titleText.text = title;
        
        descText.text = desc;
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/achivements");
        icon.sprite = sprites[iconID];
    }
}
