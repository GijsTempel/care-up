using System.Collections.Generic;
using UnityEngine;
using CareUpAvatar;

public class CharacterCarousel : MonoBehaviour
{
    public List<GameObject> platforms;
    List<PlayerAvatar> avatars = new List<PlayerAvatar>();

    //List<PlayerAvatarData> avatarsData = new List<PlayerAvatarData>();
    public float turnAngle = 0;

    private int behindMarker = 3;
    private int turnDir = 0;
    private int nextTurnDir = 0;
    private int currentChar = 1;

    public void Initialize()
    {
        int cc = currentChar - 1;
        foreach (PlayerAvatar a in avatars)
        {
            PlayerAvatarData d = GetAvatarData(cc);
            if (d != null)
            {
                a.avatarData = d;
                a.UpdateCharacter();
            }
            a.SetAnimationAction(Actions.Idle, true);
            cc++;
        }
    }

    public int GetCurrentMarker()
    {
        int currentMarker = behindMarker - 2;
        if (currentMarker < 0)
            currentMarker = 4 + currentMarker;
        print(behindMarker.ToString() + "  " + currentMarker);
        return currentMarker;
    }

    void Start()
    {
        foreach (GameObject p in platforms)
        {
            avatars.Add(p.transform.Find("PlayerAvatar").GetComponent<PlayerAvatar>());
        }

        Invoke("Initialize", 0.01f);
    }

    PlayerAvatarData GetAvatarData(int n)
    {
        if (n >= 0 && n < (PlayerPrefsManager.storeManager.avatarsData.Count))
        {
            return PlayerPrefsManager.storeManager.avatarsData[n];
        }
        return null;
    }

    public void Turn(int dir)
    {   int nextChar = currentChar + dir;
        if (nextChar >= 0 && nextChar < PlayerPrefsManager.storeManager.avatarsData.Count)
            nextTurnDir = dir;
    }

    void Update()
    {
        if (turnDir == 0 && nextTurnDir != 0)
        {
            turnDir = nextTurnDir;
            nextTurnDir = 0;

            currentChar += turnDir;
            PlayerAvatarData d = GetAvatarData(currentChar + 1);
            if (turnDir < 0)
            {
                d = GetAvatarData(currentChar - 1);
            }
            if (d != null)
            {
                avatars[behindMarker].avatarData = d;
                avatars[behindMarker].UpdateCharacter();
            }

            platforms[behindMarker].SetActive(d != null);

            behindMarker += turnDir;
            if (behindMarker > 3)
                behindMarker = 0;
            else if (behindMarker < 0)
                behindMarker = 3;
            
            foreach(PlayerAvatar a in avatars)
            {
                a.SetAnimationAction(Actions.Idle);
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
                    avatars[currentMarker].SetAnimationAction(Actions.Dance);
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
