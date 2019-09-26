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

    public PlayerAvatarData avatarData = new PlayerAvatarData();

    private GameObject maleChar;
    private GameObject femaleChar;

    private GameObject maleFace;
    private GameObject femaleFace;

    private List<Transform> maleHeads = new List<Transform>();
    private List<Transform> femaleHeads = new List<Transform>();
    private List<Transform> femaleBodies = new List<Transform>();
    private List<Transform> maleBodies = new List<Transform>();
    private List<Transform> maleGlasses = new List<Transform>();
    private List<Transform> femaleGlasses = new List<Transform>();

    Actions currentAction = Actions.Idle;
    Actions nextAction;

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
        foreach (Transform h in maleHeads)
        {
            h.gameObject.SetActive(maleHeads.IndexOf(h) == avatarData.headType && avatarData.gender == Gender.Male);
        }
        SetFace(true, avatarData.eyeType, avatarData.mouthType);
    }

    void UpdateFemaleHeads()
    {
        foreach (Transform h in femaleHeads)
        {
            h.gameObject.SetActive(femaleHeads.IndexOf(h) == avatarData.headType && avatarData.gender == Gender.Female);
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
        foreach (Transform b in femaleBodies)
        {
            b.gameObject.SetActive(femaleBodies.IndexOf(b) == avatarData.bodyType && avatarData.gender == Gender.Female);
        }
    }

    void UpdateMaleGlasses()
    {
        foreach (Transform g in maleGlasses)
        {
            g.gameObject.SetActive(maleGlasses.IndexOf(g) == avatarData.glassesType && avatarData.gender == Gender.Male);
        }
    }

    void UpdateFemaleGlasses()
    {
        foreach (Transform g in femaleGlasses)
        {
            g.gameObject.SetActive(femaleGlasses.IndexOf(g) == avatarData.glassesType && avatarData.gender == Gender.Female);
        }
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

        UpdateMaleGlasses();
        UpdateFemaleGlasses();
    }
}
