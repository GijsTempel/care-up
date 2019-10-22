using System.Collections.Generic;
using UnityEngine;
using CareUpAvatar;

public class PlayerAvatar : MonoBehaviour
{
    public UVWarp m_leftEyeObject;
    public UVWarp m_rightEyeObject;
    public UVWarp m_mouthObject;
    public UVWarp f_leftEyeObject;
    public UVWarp f_rightEyeObject;
    public UVWarp f_mouthObject;
    public Transform hatAnchor;
    public Transform glassesAnchor;

    public GameObject CurrentHat;
    public GameObject CurrentGlasses;

    HatsPositioningDB.HatInfo hatOffsetInfo = new HatsPositioningDB.HatInfo();

    int maxGlasses = 7;

    public PlayerAvatarData avatarData = new PlayerAvatarData();

    private GameObject maleChar;
    private GameObject femaleChar;

    private GameObject maleFace;
    private GameObject femaleFace;

    private List<Transform> maleHads = new List<Transform>();
    private List<Transform> femaleHads = new List<Transform>();
    private List<Transform> femaleBodies = new List<Transform>();
    private List<Transform> maleBodies = new List<Transform>();
    private List<Transform> maleGlasses = new List<Transform>();
    private List<Transform> femaleGlasses = new List<Transform>();

    Actions currentAction = Actions.Idle;
    Actions nextAction;

    public void SetHatExclusion()
    {
        if (avatarData.hat != "")
        {
            hatOffsetInfo.excluded = true;
        }
    }

    public HatsPositioningDB.HatInfo GetHatOffsetInfo()
    {
        if (avatarData.hat == "")
            return null;
        
        HatsPositioningDB.HatInfo info = new HatsPositioningDB.HatInfo();
        info.name = avatarData.hat;
        info.position = hatOffsetInfo.position;
        info.rotation = hatOffsetInfo.rotation;
        info.scale = hatOffsetInfo.scale;
        info.excluded = hatOffsetInfo.excluded;
        return info;
    }

    public int GetMaxHeadNum(Gender g)
    {
        int maxHead = maleHads.Count - 1;
        if (g == Gender.Female)
        {
            maxHead = femaleHads.Count - 1;
        }
        return maxHead;
    }

    public int GetMaxBodyNum()
    {
        int maxBody = maleBodies.Count - 1;
        if (avatarData.gender == Gender.Female)
        {
            maxBody = femaleBodies.Count - 1;
        }
        return maxBody;
    }

    public void SetHatOffset(Vector3 pos, Vector3 rot, float scl)
    {
        hatOffsetInfo.position = pos;
        hatOffsetInfo.rotation = rot;
        hatOffsetInfo.scale = scl;
        UpdateHatOffset();
    }

    private void UpdateHatOffset()
    {
        if (CurrentHat != null)
        {
            CurrentHat.transform.localPosition = hatOffsetInfo.position;
            CurrentHat.transform.localRotation = Quaternion.Euler(hatOffsetInfo.rotation);
            float s = hatOffsetInfo.scale;
            CurrentHat.transform.localScale = new Vector3(s, s, s);
        }
    }

    public int GetMaxGlassesNum()
    {
        return maxGlasses;
    }

    public bool LoadNewGlasses(int gIndex)
    {
        if (CurrentGlasses != null)
        {
            Destroy(CurrentGlasses);
            CurrentGlasses = null;
        }
        avatarData.glassesType = gIndex;
        Object glassesPrefab = Resources.Load<GameObject>("NecessaryPrefabs/Shop_Items/gl_" + gIndex.ToString());
        if (glassesPrefab != null)
        {
            GameObject newGlasses = Instantiate(glassesPrefab, glassesAnchor, true) as GameObject;
            newGlasses.transform.position = glassesAnchor.position;
            newGlasses.transform.rotation = glassesAnchor.rotation;
            newGlasses.transform.localScale = glassesAnchor.localScale;

            CurrentGlasses = newGlasses;
            return true;
        }
        return false;
    }

    public void LoadNewHat(string hatName)
    {
        if (CurrentHat != null)
        {
            Destroy(CurrentHat);
            CurrentHat = null;
        }
        avatarData.hat = hatName;
        Object hatPrefab = Resources.Load<GameObject>("NecessaryPrefabs/Shop_Items/" + hatName);
        if (hatPrefab != null)
        {
            PlayerPrefsManager pref = GameObject.FindObjectOfType<PlayerPrefsManager>();
            GameObject newHat = Instantiate(hatPrefab, hatAnchor, true) as GameObject;
            newHat.transform.position = hatAnchor.position;
            newHat.transform.rotation = hatAnchor.rotation;
            newHat.transform.localScale = hatAnchor.localScale;

            hatOffsetInfo.position = new Vector3();
            hatOffsetInfo.rotation = new Vector3();           
            hatOffsetInfo.scale = 1f;
            hatOffsetInfo.excluded = false;
            if (pref != null)
            {
                HatsPositioningDB.HatInfo hatInfo = pref.hatsPositioning.GetHatInfo(avatarData.GetHatOffsetIndex(), avatarData.hat);
                if (hatInfo != null)
                {
                    hatOffsetInfo.position = hatInfo.position;
                    hatOffsetInfo.rotation = hatInfo.rotation;
                    hatOffsetInfo.scale = hatInfo.scale;
                    hatOffsetInfo.excluded = hatInfo.excluded;
                }
            }
            CurrentHat = newHat;
            UpdateHatOffset();
        }
    }

    private void Awake()
    {
        femaleChar = transform.Find("female").gameObject;
        maleChar = transform.Find("male").gameObject;

        femaleFace = transform.Find("Bone/Pelvis/Spine/Neck/Head/HEAD_CONTAINER/face/f_face").gameObject;
        maleFace = transform.Find("Bone/Pelvis/Spine/Neck/Head/HEAD_CONTAINER/face/m_face").gameObject;

        // bodies
        femaleChar.transform.Find("f_body").GetComponentsInChildren<Transform>(true, femaleBodies);
        maleChar.transform.Find("m_body").GetComponentsInChildren<Transform>(true, maleBodies);

        femaleBodies.RemoveAt(0);
        maleBodies.RemoveAt(0);

        // heads
        femaleChar.transform.Find("f_head").GetComponentsInChildren<Transform>(true, femaleHads);
        maleChar.transform.Find("m_head").GetComponentsInChildren<Transform>(true, maleHads);

        // trim first ones cuz it's "HEAD_CONTAINER"
        femaleHads.RemoveAt(0);
        maleHads.RemoveAt(0);

        //trim three last ones cuz those are glasses
        //femaleChar.transform.Find("f_glasses").GetComponentsInChildren<Transform>(true, femaleGlasses);
        //maleChar.transform.Find("m_glasses").GetComponentsInChildren<Transform>(true, maleGlasses);
    }

    // Start is called before the first frame update
    void Start()
    {
        //femaleGlasses.RemoveAt(0);
        //maleGlasses.RemoveAt(0);

        avatarData.gender = Gender.Female;
        avatarData.headType = 4;
        avatarData.glassesType = 0;
        avatarData.bodyType = 7;
        avatarData.eyeType = 4;

        UpdateCharacter();

        Animator anim = GetComponent<Animator>();
        anim.Play(anim.GetCurrentAnimatorStateInfo(0).fullPathHash,0, Random.Range(0f,1f));
    }

    public void JumpToNextAnimation()
    {
        SetAnimationAction(nextAction, true, true);
    }
    //public void ShiftAnimation(float f)
    //{
    //    Animator anim = GetComponent<Animator>();
    //    anim.CrossFade(anim.GetCurrentAnimatorStateInfo(0).fullPathHash, f);
    //}

    public void SetAnimationAction(Actions action, bool force = false, bool immed = true)
    {
        if (!immed)
        {
            float randTimeout = Random.Range(0.1f, 2f);
            nextAction = action;
            Invoke("JumpToNextAnimation", randTimeout);
        }
        else
        {
            if (action != currentAction || force)
            {
                Animator anim = GetComponent<Animator>();
                switch (action)
                {
                    case Actions.Idle:
                        anim.SetTrigger("idle" + Random.Range(1, 3).ToString());
                        break;
                    case Actions.Dance:
                        anim.SetTrigger("dance" + Random.Range(1, 3).ToString());
                        break;
                    case Actions.Sad:
                        anim.SetTrigger("sad" + Random.Range(1, 3).ToString());
                        break;
                    case Actions.Posing:
                        anim.SetTrigger("posing" + Random.Range(1, 3).ToString());
                        break;
                }
                currentAction = action;
            }
        }
    }

    void SetFace(bool isMale, int eyes, int mouth)
    {
        UVWarp leftEyeObject = f_leftEyeObject;
        UVWarp rightEyeObject = f_rightEyeObject;
        UVWarp mouthObject = f_mouthObject;

        if (isMale)
        {
            leftEyeObject = m_leftEyeObject;
            rightEyeObject = m_rightEyeObject;
            mouthObject = m_mouthObject;
        }

        float gridStep = 0.125f;
        float eyeTypeOffset = eyes * gridStep;
        leftEyeObject.offset.x = eyeTypeOffset;
        rightEyeObject.offset.x = eyeTypeOffset;
        float mouthTypeOffset = mouth * gridStep;
        mouthObject.offset.x = mouthTypeOffset;
    }

    void UpdateMaleHeads()
    {
        foreach (Transform h in maleHads)
        {
            h.gameObject.SetActive(maleHads.IndexOf(h) == avatarData.headType && avatarData.gender == Gender.Male);
        }
        SetFace(true, avatarData.eyeType, avatarData.mouthType);
    }

    void UpdateFemaleHeads()
    {
        foreach (Transform h in femaleHads)
        {
            h.gameObject.SetActive(femaleHads.IndexOf(h) == avatarData.headType && avatarData.gender == Gender.Female);
        }
        SetFace(false, avatarData.eyeType, avatarData.mouthType);
    }

    void UpdateMaleBodies()
    {
        foreach (Transform b in maleBodies)
        {
            b.gameObject.SetActive(maleBodies.IndexOf(b) == avatarData.bodyType && avatarData.gender == Gender.Male);
        }
    }

    void UpdateFemaleBodies()
    {
        int bIndex = avatarData.bodyType;
        if (bIndex > 1000000)
            bIndex -= 1000000;
        foreach (Transform b in femaleBodies)
        {
            b.gameObject.SetActive(femaleBodies.IndexOf(b) == bIndex && avatarData.gender == Gender.Female);
        }
    }

    void UpdateGlasses()
    {
        LoadNewGlasses(avatarData.glassesType);

        //foreach (Transform g in maleGlasses)
        //{
        //    g.gameObject.SetActive(maleGlasses.IndexOf(g) == avatarData.glassesType && avatarData.gender == Gender.Male);
        //}
    }

    public void UpdateCharacter()
    {
        maleChar.SetActive(avatarData.gender == Gender.Male);
        maleFace.SetActive(avatarData.gender == Gender.Male);
        femaleChar.SetActive(avatarData.gender == Gender.Female);
        femaleFace.SetActive(avatarData.gender == Gender.Female);

        UpdateMaleHeads();
        UpdateFemaleHeads();

        UpdateMaleBodies();
        UpdateFemaleBodies();

        UpdateGlasses();
        LoadNewHat(avatarData.hat);
    }
}
