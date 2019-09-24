using System.Collections.Generic;
using UnityEngine;
using CareUpAvatar;

public class CharacterСarrousel : MonoBehaviour
{
    int nextTurnDir = 0;
    int turnDir = 0;
    [HideInInspector]
    public static int CurrentCharacter { get; set; } = 1;

    public CharacterPanelManager panelManager;
    private float turnAngle = 0;
    private int behindMarker = 3;
    private List<PlayerAvatar> avatars = new List<PlayerAvatar>();
    public List<GameObject> platforms;
    private List<GameObject> checkMarks = new List<GameObject>();

    public void Initialize()
    {
        int current = CurrentCharacter - 1;
        foreach (PlayerAvatar avatar in avatars)
        {
            PlayerAvatarData playerAvatarData = GetAvatarData(current);
            if (playerAvatarData != null)
            {
                avatar.avatarData = playerAvatarData;
                avatar.UpdateCharacter();

                bool purchased = PlayerPrefsManager.storeManager.CharacterItems[current].purchased;
                checkMarks[current].SetActive(purchased);

                //panelManager.SetStoreInfo(behindMarker, current);
            }
            avatar.SetAnimationAction(Actions.Idle, true);
            current++;
        }
        panelManager.SetStoreInfo(CurrentCharacter);
    }

    public void Scroll(int dir)
    {
        int nextChar = CurrentCharacter + dir;
        if (nextChar >= 0 && nextChar < PlayerPrefsManager.storeManager.CharacterItems.Count)
            nextTurnDir = dir;
        // enabled = true;
    }

    public int GetCurrentMarker()
    {
        int currentMarker = behindMarker - 2;
        if (currentMarker < 0)
            currentMarker = 4 + currentMarker;
        return currentMarker;
    }

    private void Start()
    {
        foreach (GameObject platform in platforms)
        {
            checkMarks.Add(platform.transform.Find("checkMark").gameObject);
        }
        foreach (GameObject platform in platforms)
        {
            avatars.Add(platform.transform.Find("PlayerAvatar").GetComponent<PlayerAvatar>());
        }

        Invoke("Initialize", 0.01f);
    }

    private PlayerAvatarData GetAvatarData(int index)
    {
        if (index >= 0 && index < (PlayerPrefsManager.storeManager.CharacterItems.Count))
        {
            //panelManager.SetStoreInfo(behindMarker, index);
            return PlayerPrefsManager.storeManager.CharacterItems[index].playerAvatar;
        }
        return null;
    }

    private void Update()
    {
        // if (turnDir == 0 && nextTurnDir == 0)
        //     enabled = false;
        if (turnDir == 0 && nextTurnDir != 0)
        {
            turnDir = nextTurnDir;
            nextTurnDir = 0;
            CurrentCharacter += turnDir;
            int index = CurrentCharacter + 1;

            PlayerAvatarData playerAvatar = GetAvatarData(index);
            if (turnDir < 0)
            {
                index = CurrentCharacter - 1;
                playerAvatar = GetAvatarData(index);
            }
            if (playerAvatar != null)
            {
                avatars[behindMarker].avatarData = playerAvatar;
                avatars[behindMarker].UpdateCharacter();
                bool purchased = PlayerPrefsManager.storeManager.CharacterItems[index].purchased;
                checkMarks[behindMarker].SetActive(purchased);
            }
            panelManager.SetStoreInfo(CurrentCharacter);
            platforms[behindMarker].SetActive(playerAvatar != null);

            behindMarker += turnDir;

            if (behindMarker > 3)
                behindMarker = 0;
            else if (behindMarker < 0)
                behindMarker = 3;

            foreach (PlayerAvatar a in avatars)
            {
                a.SetAnimationAction(Actions.Idle, false);
            }
        }
        if (turnDir != 0)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            float nextAngle = (turnAngle + (90f * turnDir)) % 360;
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
                    avatars[currentMarker].SetAnimationAction(Actions.Posing);
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
