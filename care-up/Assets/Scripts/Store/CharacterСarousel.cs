using System.Collections.Generic;
using UnityEngine;
using CareUpAvatar;

public class CharacterСarousel : MonoBehaviour
{
    public int CurrentCharacter = 1;
    public CharacterPanelManager panelManager;
    public List<GameObject> platforms;

    private float turnAngle = 0;
    private int behindMarker = 3;
    private int nextTurnDir = 0;
    private int turnDir = 0;
    private int targetTurnPosition = -1;
    private float defaultTurnSpeed = 90f;

    static int current;

    private PlayerPrefsManager pref;

    public List<PlayerAvatar> avatars = new List<PlayerAvatar>();
    public List<GameObject> checkMarks = new List<GameObject>();

    public void UpdateSelected(PlayerAvatarData aData)
    {
        int currentMarker = GetCurrentMarker();
        avatars[currentMarker].avatarData = aData;
        avatars[currentMarker].UpdateCharacter();
        //GameObject.FindObjectOfType<LoadCharacterScene>().LoadCharacter(avatars[GetCurrentMarker()]);
    }

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

        if (pref != null)
        {
            TurnToPosition(pref.CarouselPosition);
        }
    }

    public void Scroll(int dir)
    {
        int nextChar = CurrentCharacter + dir;
        if (nextChar >= 0 && nextChar < PlayerPrefsManager.storeManager.CharacterItems.Count)
            nextTurnDir = dir;
        targetTurnPosition = -1;

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
        pref = GameObject.FindObjectOfType<PlayerPrefsManager>();
        // checkMarks.Clear();
        // foreach (GameObject platform in platforms)
        // {
        //     checkMarks.Add(platform.transform.Find("checkMark").gameObject);
        // }
        // foreach (GameObject platform in platforms)
        // {
        //     avatars.Add(platform.transform.Find("PlayerAvatar").GetComponent<PlayerAvatar>());
        // }

        Invoke("Initialize", 0.1f);
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

    public void TurnToPosition(int value)
    {
        targetTurnPosition = value;
    }

    private void Update()
    {
        float turnSpeed = defaultTurnSpeed;
        if (targetTurnPosition != -1)
        {
            if (targetTurnPosition == CurrentCharacter)
                targetTurnPosition = -1;
            else
            {
                if (targetTurnPosition < CurrentCharacter)
                    nextTurnDir = -1;
                else if (targetTurnPosition > CurrentCharacter)
                    nextTurnDir = 1;
                //turnSpeed = defaultTurnSpeed * 2;
            }
        }
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
            float nextAngle = (turnAngle + (turnSpeed * turnDir)) % 360;
            if (nextAngle < 0)
                nextAngle = 360 + nextAngle;
            if (Mathf.Abs(rot.y - nextAngle) < (turnSpeed / 6))
            {
                rot.y = nextAngle;
                turnDir = 0;
                turnAngle = nextAngle;
                if (pref != null)
                    pref.CarouselPosition = CurrentCharacter;
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
