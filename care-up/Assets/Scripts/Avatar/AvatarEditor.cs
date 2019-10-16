using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUpAvatar;
using UnityEngine.UI;
using System.IO;

public class AvatarEditor : MonoBehaviour
{
    public enum Selections
    {
        Hats,
    };
    bool hatOffsetLock = false;

    PlayerPrefsManager pref;
    public PlayerAvatar MainAvatar;
    public InputField HeadInput;
    public InputField BodyInput;
    public InputField GenderInput;
    public InputField GlassesInput;
    public InputField HatInput;
    Scrollbar posOffset_x;
    Scrollbar posOffset_y;
    Scrollbar posOffset_z;
    Scrollbar rotOffset_x;
    Scrollbar rotOffset_y;
    Scrollbar rotOffset_z;
    Scrollbar sclOffset;
    Scrollbar camRotScrool;
    public Transform camPivot;

    Vector3 HatOffsetPos = new Vector3();
    Vector3 HatOffsetRot = new Vector3();
    float HatOffsetScale = 1f;

    Selections currentSelection = new Selections();
    List<string> Hats = new List<string>
            { "None" };

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

    public void HatOffsetChanged()
    {
        if (!hatOffsetLock)
        {

            float px = (posOffset_x.value - 0.5f) * 2f;
            float py = (posOffset_y.value - 0.5f) * 2f;
            float pz = (posOffset_z.value - 0.5f) * 2f;

            float rx = (rotOffset_x.value - 0.5f) * 360f;
            float ry = (rotOffset_y.value - 0.5f) * 360f;
            float rz = (rotOffset_z.value - 0.5f) * 360f;
            float s = sclOffset.value * 2f;
            HatOffsetPos = new Vector3(px, py, pz);
            HatOffsetRot = new Vector3(rx, ry, rz);
            HatOffsetScale = s;

            MainAvatar.SetHatOffset(HatOffsetPos, Quaternion.Euler(HatOffsetRot), HatOffsetScale);
        }
    }

    public void GoToMainMenu()
    {
        bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
    }

    public void SetAvatarAction(int act)
    {
        MainAvatar.SetAnimationAction((Actions)act, true);
    }

    public void CameraControllerChanged()
    {
        float y_rot = (camRotScrool.value - 0.5f) * 2f * 180f;
        Vector3 currentCamRot = camPivot.eulerAngles;
        currentCamRot.y = y_rot;
        camPivot.localRotation = Quaternion.Euler(currentCamRot.x, y_rot, currentCamRot.z);
    }

    void UpdateHatOffsetControl()
    {
        hatOffsetLock = true;
        HatsPositioningDB.HatInfo info = MainAvatar.GetHatOffsetInfo();
        if (info != null)
        {
            HatOffsetPos = info.position;
            HatOffsetRot = info.rotation;
            HatOffsetScale = info.scale;
            posOffset_x.value = HatOffsetPos.x / 2 + 0.5f;
            posOffset_y.value = HatOffsetPos.y / 2 + 0.5f;
            posOffset_y.value = HatOffsetPos.y / 2 + 0.5f;

            rotOffset_x.value = HatOffsetRot.y / 360f + 0.5f;
            rotOffset_y.value = HatOffsetRot.y / 360f + 0.5f;
            rotOffset_z.value = HatOffsetRot.z / 360f + 0.5f;
            sclOffset.value = HatOffsetScale / 2;
        }
        else
        {
            ResetHatOffsetValue(-1);
        }
        hatOffsetLock = false;
    }

    public void ResetHatOffsetValue(int value)
    {
        if (value == -1)
        {
            posOffset_x.value = 0.5f;
            posOffset_y.value = 0.5f;
            posOffset_z.value = 0.5f;
            rotOffset_x.value = 0.5f;
            rotOffset_y.value = 0.5f;
            rotOffset_z.value = 0.5f;
            sclOffset.value = 0.5f;
        }
        else
        {
            switch (value)
            {
                case 0:
                    posOffset_x.value = 0.5f;
                    break;
                case 1:
                    posOffset_y.value = 0.5f;
                    break;
                case 2:
                    posOffset_z.value = 0.5f;
                    break;
                case 3:
                    rotOffset_x.value = 0.5f;
                    break;
                case 4:
                    rotOffset_y.value = 0.5f;
                    break;
                case 5:
                    rotOffset_z.value = 0.5f;
                    break;
                case 6:
                    sclOffset.value = 0.5f;
                    break;
            }
        }
        HatOffsetChanged();
    }

    public void ApplyCurentHead()
    {
        if (pref != null && MainAvatar.avatarData.hat != "")
        {
            HatOffsetChanged();
            HatsPositioningDB.HatInfo info = new HatsPositioningDB.HatInfo();
            info.name = MainAvatar.avatarData.hat;
            info.position = HatOffsetPos;
            info.rotation = HatOffsetRot;
            info.scale = HatOffsetScale;

            pref.hatsPositioning.UpdateHatInfo(MainAvatar.avatarData.GetHatOffsetIndex(), info);
            pref.hatsPositioning.SaveInfoToXml();
        }
    }

    public void ApplyAllHeads()
    {
        if (pref != null && MainAvatar.avatarData.hat != "")
        {
            for (int i = 0; i < MainAvatar.GetMaxHeadNum(Gender.Male); i++)
            {
                HatsPositioningDB.HatInfo info = new HatsPositioningDB.HatInfo();
                info.name = MainAvatar.avatarData.hat;
                info.position = HatOffsetPos;
                info.rotation = HatOffsetRot;
                info.scale = HatOffsetScale;
                pref.hatsPositioning.UpdateHatInfo(i, info);
            }
            for (int i = 0; i < MainAvatar.GetMaxHeadNum(Gender.Female); i++)
            {
                HatsPositioningDB.HatInfo info = new HatsPositioningDB.HatInfo();
                info.name = MainAvatar.avatarData.hat;
                info.position = HatOffsetPos;
                info.rotation = HatOffsetRot;
                info.scale = HatOffsetScale;
                pref.hatsPositioning.UpdateHatInfo(i+1000000, info);
            }
        }
        pref.hatsPositioning.SaveInfoToXml();
    }

    public void SaveAllHats()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        pref = GameObject.FindObjectOfType<PlayerPrefsManager>();

        foreach (string file in System.IO.Directory.GetFiles("Assets\\Resources\\NecessaryPrefabs\\Shop_Items")) 
        {
            
            string[] _path = file.Split('\\');

            string item_name = (_path[_path.Length-1]);
            string[] DSV = item_name.Split('.');

            if (DSV[DSV.Length - 1] == "prefab")
            {
                item_name = item_name.Split('.')[0];
                if (item_name.Split('_')[0] == "hat")
                {
                    Hats.Add(item_name);
                }
            }
        }
        SelectorContent = Selector.transform.Find("Scroll View/Viewport/Content");
        posOffset_x = Tabs[0].transform.Find("PositionOffset/x/Scrollbar").GetComponent<Scrollbar>();
        posOffset_y = Tabs[0].transform.Find("PositionOffset/y/Scrollbar").GetComponent<Scrollbar>();
        posOffset_z = Tabs[0].transform.Find("PositionOffset/z/Scrollbar").GetComponent<Scrollbar>();
        rotOffset_x = Tabs[0].transform.Find("RotationOffset/x/Scrollbar").GetComponent<Scrollbar>();
        rotOffset_y = Tabs[0].transform.Find("RotationOffset/y/Scrollbar").GetComponent<Scrollbar>();
        rotOffset_z = Tabs[0].transform.Find("RotationOffset/z/Scrollbar").GetComponent<Scrollbar>();
        sclOffset   = Tabs[0].transform.Find("ScaleOffset/x/Scrollbar").GetComponent<Scrollbar>();
        camRotScrool = transform.Find("Canvas/GameObject/AvatarView/Scrollbar").GetComponent<Scrollbar>();

        Invoke("Initialize", 0.01f);
    }

    public void BuildHatSelector()
    {
        currentSelection = Selections.Hats;
        foreach (Transform child in SelectorContent) {
            GameObject.Destroy(child.gameObject);
        }
        foreach (string h in Hats)
        {
            GameObject _Button = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/SelectorButton"), SelectorContent);
            _Button.transform.Find("Text").GetComponent<Text>().text = h;
            _Button.GetComponent<Button>().onClick.AddListener(() => SelectElement(h));
            string category = "hat";
            Sprite sprite = Resources.Load("Sprites/StoreItemPreview/" + h, typeof(Sprite)) as Sprite;
            if (sprite != null)
            {
                _Button.transform.Find("Image").GetComponent<Image>().sprite = sprite;
            }
            else
            {
                _Button.transform.Find("Image").GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }
        }
        ShowSelector(true);
    }

    public void ShowSelector(bool value)
    {
        Tools.SetActive(!value);
        Selector.SetActive(value);
    }

    public void SelectElement(string element)
    {
        ShowSelector(false);
        if (currentSelection == Selections.Hats)
        {
            if(element == "None")
            {
                MainAvatar.avatarData.hat = "";
                HatInput.text = "";
            }
            else
            {
                MainAvatar.avatarData.hat = element;
                HatInput.text = element;
            }
            MainAvatar.UpdateCharacter();
            UpdateHatOffsetControl();
        }
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
        UpdateHatOffsetControl();
    }

    public void ChangeHead(int value)
    {
        int nextHead = currentHead + value;
        if (nextHead < 0)
            nextHead = 0;
        if (nextHead > MainAvatar.GetMaxHeadNum(MainAvatar.avatarData.gender))
            nextHead = MainAvatar.GetMaxHeadNum(MainAvatar.avatarData.gender);
        currentHead = nextHead;
        HeadInput.text = currentHead.ToString();
        MainAvatar.avatarData.headType = currentHead;
        MainAvatar.UpdateCharacter();
        UpdateHatOffsetControl();
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
