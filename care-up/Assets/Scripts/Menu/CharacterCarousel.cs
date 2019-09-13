using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using CareUpAvatar;

public class CharacterCarousel : MonoBehaviour
{
    public List<GameObject> Platforms;
    List<PlayerAvatar> Avatars = new List<PlayerAvatar>();
    List<PlayerAvatarData> avatarsData = new List<PlayerAvatarData>();

    int behindMarker = 3;
    public float turnAngle = 0;
    int turnDir = 0;
    int nextTurnDir = 0;
    int currentChar = 1;

    public void Initialize()
    {
        int cc = currentChar - 1;
        foreach (PlayerAvatar a in Avatars)
        {
            PlayerAvatarData d = GetAvaData(cc);
            if (d != null)
            {
                a.avatarData = GetAvaData(cc);
                a.UpdateCharacter();
            }
            a.SetAnimationAction(PlayerAvatarData.Actions.Idle, true);
            cc++;
        }
    }

    public int GetCurrentMarker()
    {
        int currentMarker = behindMarker - 2;
        if (currentMarker < 0)
            currentMarker = 4 + currentMarker;
        return currentMarker;
    }

    void Start()
    {
        foreach (GameObject p in Platforms)
        {
            Avatars.Add(p.transform.Find("PlayerAvatar").GetComponent<PlayerAvatar>());
        }

        string characterStoreXml = "CharacterStore";
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/" + characterStoreXml);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);

        XmlNodeList xmlCharacterList = xmlFile.FirstChild.NextSibling.ChildNodes;

        foreach (XmlNode xmlSceneNode in xmlCharacterList)
        {
            PlayerAvatarData ava = new PlayerAvatarData();

            ava.gender = PlayerAvatarData.Gender.Male;
            string gender = xmlSceneNode.Attributes["gender"].Value;
            if (gender == "Female")
                ava.gender = PlayerAvatarData.Gender.Female;

            int.TryParse(xmlSceneNode.Attributes["glassesType"].Value, out int glassesType);
            ava.glassesType = glassesType;
            int.TryParse(xmlSceneNode.Attributes["bodyType"].Value, out int bodyType);
            ava.bodyType = bodyType;
            int.TryParse(xmlSceneNode.Attributes["headType"].Value, out int headType);
            ava.headType = headType;
            int.TryParse(xmlSceneNode.Attributes["mouth"].Value, out int mouthType);
            ava.mouthType = mouthType;
            int.TryParse(xmlSceneNode.Attributes["eye"].Value, out int eyeType);
            ava.eyeType = eyeType;

            avatarsData.Add(ava);
        }
        Initialize();
    }

    PlayerAvatarData GetAvaData(int n)
    {
        if (n >= 0 && n < (avatarsData.Count))
        {
            return avatarsData[n];
        }
        return null;
    }

    public void Turn(int dir)
    {   int nextChar = currentChar + dir;
        if (nextChar >= 0 && nextChar < avatarsData.Count)
            nextTurnDir = dir;
    }

    void Update()
    {
        if (turnDir == 0 && nextTurnDir != 0)
        {
            turnDir = nextTurnDir;
            nextTurnDir = 0;

            currentChar += turnDir;
            PlayerAvatarData d = GetAvaData(currentChar + 1);
            if (turnDir < 0)
            {
                d = GetAvaData(currentChar - 1);
            }
            if (d != null)
            {
                Avatars[behindMarker].avatarData = d;
                Avatars[behindMarker].UpdateCharacter();
            }

            Platforms[behindMarker].SetActive(d != null);

            behindMarker += turnDir;
            if (behindMarker > 3)
                behindMarker = 0;
            else if (behindMarker < 0)
                behindMarker = 3;
            
            foreach(PlayerAvatar a in Avatars)
            {
                a.SetAnimationAction(PlayerAvatarData.Actions.Idle);
            }
    
        }
        if (turnDir != 0)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            float nextAngle = (turnAngle + (90f * turnDir))%360;
            if (nextAngle < 0)
                nextAngle = 360 + nextAngle;
            if (Mathf.Abs(rot.y - nextAngle) < 15f)
            {
                rot.y = nextAngle;
                turnDir = 0;
                turnAngle = nextAngle;
                if (nextTurnDir == 0)
                {
                    int currentMarker = GetCurrentMarker();
                    Avatars[currentMarker].SetAnimationAction(PlayerAvatarData.Actions.Dance);
                }
            }
            else
            {
                rot.y += turnDir * 300f * Time.deltaTime;
            }
            transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
    }
}
