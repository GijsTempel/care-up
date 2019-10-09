using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUpAvatar;
using UnityEngine.UI;

public class AvatarEditor : MonoBehaviour
{
    public PlayerAvatar MainAvatar;
    public InputField HeadInput;
    public InputField BodyInput;
    public InputField GenderInput;
    public InputField GlassesInput;

    public GameObject Tools;
    public GameObject Selector;
    Transform SelectorContent;

    public List<GameObject> Tabs;
    public List<Button> TabsButtons;

    int currentTab = 0;

    int currentHead = 0;
    int currentBody = 0;
    int currentGlasses = -1;

    Gender currentGender = Gender.Female;

    // Start is called before the first frame update
    void Start()
    {
        SelectorContent = Selector.transform.Find("Scroll View/Viewpor/Content");
        Invoke("Initialize", 0.1f);
    }

    public void ShowSelector(bool value)
    {
        Tools.SetActive(!value);
        Selector.SetActive(value);
    }

    public void SelectElement(string element)
    {
        print(element);
        ShowSelector(false);
    }

    public void SetTab(int value)
    {
        foreach(GameObject t in Tabs)
            t.SetActive(false);
        Tabs[value].SetActive(true);
        foreach (Button b in TabsButtons)
            b.interactable = true;
        TabsButtons[value].interactable = false;
    }

    public void Initialize()
    {
        ChangeGender(0);
        ChangeHead(0);
        ChangeBody(0);
        ChangeGlasses(0);
        SetTab(0);
    }

    public void ChangeGender(int value)
    {
        if (value != 0)
        {
            if (currentGender == Gender.Female)
                currentGender = Gender.Male;
            else
                currentGender = Gender.Female;
        }
        MainAvatar.avatarData.gender = currentGender;
        MainAvatar.UpdateCharacter();
        GenderInput.text = currentGender.ToString();
    }

    public void ChangeHead(int value)
    {
        int nextHead = currentHead + value;
        if (nextHead < 0)
            nextHead = 0;
        if (nextHead > MainAvatar.GetMaxHeadNum())
            nextHead = MainAvatar.GetMaxHeadNum();
        currentHead = nextHead;
        HeadInput.text = currentHead.ToString();
        MainAvatar.avatarData.headType = currentHead;
        MainAvatar.UpdateCharacter();
    }

    public void ChangeGlasses(int value)
    {
        int nextGlasses = currentGlasses + value;
        if (nextGlasses < -1)
            nextGlasses = -1;
        if (nextGlasses > MainAvatar.GetMaxGlassesNum())
            nextGlasses = MainAvatar.GetMaxGlassesNum();
        currentGlasses = nextGlasses;
        GlassesInput.text = currentGlasses.ToString();
        MainAvatar.avatarData.glassesType = currentGlasses;
        MainAvatar.UpdateCharacter();
    }


    public void HeadInputChanged()
    {
        currentHead = int.Parse(HeadInput.text);
        ChangeHead(0);
    }

    public void GlassesInputChanged()
    {
        currentGlasses = int.Parse(GlassesInput.text);
        ChangeGlasses(0);
    }

    public void ChangeBody(int value)
    {
        int nextBody = currentBody + value;
        if (nextBody < 0)
            nextBody = 0;
        if (nextBody > MainAvatar.GetMaxBodyNum())
            nextBody = MainAvatar.GetMaxBodyNum();
        currentBody = nextBody;
        BodyInput.text = currentBody.ToString();
        MainAvatar.avatarData.bodyType = currentBody;
        MainAvatar.UpdateCharacter();
    }


    public void SetEyes(int value)
    {
        MainAvatar.avatarData.eyeType = value;
        MainAvatar.UpdateCharacter();
    }

    public void SetMouth(int value)
    {
        MainAvatar.avatarData.mouthType = value;
        MainAvatar.UpdateCharacter();
    }

    public void BodyInputChanged()
    {
        currentBody = int.Parse(BodyInput.text);
        ChangeBody(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
