using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUpAvatar;

public class PlayerAvatar : MonoBehaviour
{
    public PlayerAvatarData avatarData = new PlayerAvatarData();

    private List<Transform> maleHeads = new List<Transform>();
    private List<Transform> femaleHeads = new List<Transform>();
    private List<Transform> femaleBodies = new List<Transform>();
    private List<Transform> maleBodies = new List<Transform>();
    private List<Transform> maleGlasses = new List<Transform>();
    private List<Transform> femaleGlasses = new List<Transform>();

    private GameObject maleChar;
    private GameObject femaleChar;

    private GameObject maleFace;
    private GameObject femaleFace;

    public UVWarp m_leftEyeObject;
    public UVWarp m_rightEyeObject;
    public UVWarp m_mouthObject;
    public UVWarp f_leftEyeObject;
    public UVWarp f_rightEyeObject;
    public UVWarp f_mouthObject;

    PlayerAvatarData.Actions currentAction = PlayerAvatarData.Actions.Idle;

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
        femaleChar.transform.Find("f_head").GetComponentsInChildren<Transform>(true, femaleHeads);
        maleChar.transform.Find("m_head").GetComponentsInChildren<Transform>(true, maleHeads);

        // trim first ones cuz it's "HEAD_CONTAINER"
        femaleHeads.RemoveAt(0);
        maleHeads.RemoveAt(0);

        //trim three last ones cuz those are glasses
        femaleChar.transform.Find("f_glasses").GetComponentsInChildren<Transform>(true, femaleGlasses);
        maleChar.transform.Find("m_glasses").GetComponentsInChildren<Transform>(true, maleGlasses);
            }
    // Start is called before the first frame update
    void Start()
    {
        femaleGlasses.RemoveAt(0);
        maleGlasses.RemoveAt(0);


        avatarData.gender = PlayerAvatarData.Gender.Female;
        avatarData.headType = 4;
        avatarData.glassesType = 0;
        avatarData.bodyType = 7;
        UpdateCharacter();

    }

    public void SetAnimationAction(PlayerAvatarData.Actions action, bool force = false)
    {
        if (action != currentAction || force)
        {
            Animator anim = GetComponent<Animator>();
            switch (action)
            {
                case PlayerAvatarData.Actions.Idle:
                    anim.SetTrigger("idle" + Random.Range(1,3).ToString());
                    break;
                case PlayerAvatarData.Actions.Dance:
                    anim.SetTrigger("dance" + Random.Range(1,3).ToString());
                    break;
                case PlayerAvatarData.Actions.Sad:
                    anim.SetTrigger("sad" + Random.Range(1,3).ToString());
                    break;
            }
            currentAction = action;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        foreach (Transform h in maleHeads)
        {
            h.gameObject.SetActive(maleHeads.IndexOf(h) == avatarData.headType && avatarData.gender == PlayerAvatarData.Gender.Male);
        }
        SetFace(true, avatarData.eyeType, avatarData.mouthType);
    }

    void UpdateFemaleHeads()
    {
        foreach (Transform h in femaleHeads)
        {
            h.gameObject.SetActive(femaleHeads.IndexOf(h) == avatarData.headType && avatarData.gender == PlayerAvatarData.Gender.Female);
        }
        SetFace(true, avatarData.eyeType, avatarData.mouthType);
    }
    void UpdateMaleBodies()
    {
        foreach (Transform b in maleBodies)
        {
            b.gameObject.SetActive(maleBodies.IndexOf(b) == avatarData.bodyType && avatarData.gender == PlayerAvatarData.Gender.Male);
        }
    }

    void UpdateFemaleBodies()
    {
        foreach (Transform b in femaleBodies)
        {
            b.gameObject.SetActive(femaleBodies.IndexOf(b) == avatarData.bodyType && avatarData.gender == PlayerAvatarData.Gender.Female);
        }
    }

    void UpdateMaleGlasses()
    {
        foreach (Transform g in maleGlasses)
        {
            g.gameObject.SetActive(maleGlasses.IndexOf(g) == avatarData.glassesType && avatarData.gender == PlayerAvatarData.Gender.Male);
        }
    }

    void UpdateFemaleGlasses()
    {
        foreach (Transform g in femaleGlasses)
        {
            g.gameObject.SetActive(femaleGlasses.IndexOf(g) == avatarData.glassesType && avatarData.gender == PlayerAvatarData.Gender.Female);
        }
    }

    public void UpdateCharacter()
    {
        maleChar.SetActive(avatarData.gender == PlayerAvatarData.Gender.Male);
        maleFace.SetActive(avatarData.gender == PlayerAvatarData.Gender.Male);
        femaleChar.SetActive(avatarData.gender == PlayerAvatarData.Gender.Female);
        femaleFace.SetActive(avatarData.gender == PlayerAvatarData.Gender.Female);

        UpdateMaleHeads();
        UpdateFemaleHeads();

        UpdateMaleBodies();
        UpdateFemaleBodies();

        UpdateMaleGlasses();
        UpdateFemaleGlasses();

        //UpdateMaleFace();
        //UpdateFemaleFace();
    }
}
