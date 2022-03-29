using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SceneGroupButton : MonoBehaviour
{
    public Text sceneGroupTitle;
    public Image iconImage;
    public Text numText;
    int groupID;
    string groupTitle;
    string groupIconName;
    int _numberOfScenes = 0;
    SceneGroupMenu sceneGroupMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetButtonData(int _id, string _title, string _iconName, int _num)
    {
        sceneGroupTitle.text = _title;
        _numberOfScenes = _num;
        groupID = _id;
        string numStr = "";
        if (_num > 0)
            numStr = _num.ToString();
        numText.text = numStr;
        if (_iconName != "")
        {
            iconImage.sprite = Resources.Load("Sprites/nUI/groupIcons/" + _iconName, typeof(Sprite)) as Sprite;
        }
        else
        {
            iconImage.sprite = Resources.Load("Sprites/nUI/groupIcons/empty", typeof(Sprite)) as Sprite;
        }
    }

    public void ButtonClicked()
    {
        if (sceneGroupMenu == null)
        {
            sceneGroupMenu = GameObject.FindObjectOfType<SceneGroupMenu>();
        }
        sceneGroupMenu.OpenGroup(groupID);
    }
}
