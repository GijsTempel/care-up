using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using CareUpAvatar;

public class CharacterCreationScene : MonoBehaviour
{

    public struct FaceData
    {
        public int eyeType;
        public int mouthType;
    };

    List<FaceData> maleFaceData = new List<FaceData>();
    List<FaceData> femaleFaceData = new List<FaceData>();

    public Gender gender;
    public int headType;
    public int bodyType;
    public int glassesType;
    public GameObject AvatarObject;

    public  GameObject maleChar;
    private GameObject femaleChar;

    private GameObject maleFace;
    private GameObject femaleFace;

    private List<Transform> maleHeads = new List<Transform>();
    private List<Transform> femaleHeads = new List<Transform>();
    private List<Transform> femaleBodies = new List<Transform>();
    private List<Transform> maleBodies = new List<Transform>();
    private List<Transform> maleGlasses = new List<Transform>();
    private List<Transform> femaleGlasses = new List<Transform>();
    
    private Image maleBtn;
    private Image femaleBtn;

    public GameObject inputNameField;
    public GameObject inputBIGfield;

    public UVWarp m_leftEyeObject;
    public UVWarp m_rightEyeObject;
    public UVWarp m_mouthObject;

    public UVWarp f_leftEyeObject;
    public UVWarp f_rightEyeObject;
    public UVWarp f_mouthObject;

    private void Start()
    {       
        if (SceneManager.GetActiveScene().name == "MainMenu")
            return;
        if (GameObject.Find("CharacterCustomization/Canvas/Image/InfoHolder/CharacterPanel/GenderButtonsHolder/MaleBtn") != null)
        {
            maleBtn = GameObject.Find("CharacterCustomization/Canvas/Image/InfoHolder/CharacterPanel/GenderButtonsHolder/MaleBtn").GetComponent<Image>();
            femaleBtn = GameObject.Find("CharacterCustomization/Canvas/Image/InfoHolder/CharacterPanel/GenderButtonsHolder/FemaleBtn").GetComponent<Image>();
        }
        Initialize(GameObject.Find("w_char"));

        // set up initial info
        PlayerPrefsManager manager = GameObject.FindObjectOfType<PlayerPrefsManager>();
        if (manager != null && CharacterInfo.sex != "")
        {
            SetCurrent(
                CharacterInfo.sex == "Female" ? Gender.Female : Gender.Male,
                CharacterInfo.headType, CharacterInfo.bodyType, CharacterInfo.glassesType);
        }
        else
        {
            SetCurrent(Gender.Female, 0, 0, -1);
        }

        if (manager != null)
        {
            if (inputNameField != null)
                inputNameField.GetComponent<InputField>().text = manager.fullPlayerName;
            if (inputBIGfield != null)
                inputBIGfield.GetComponent<InputField>().text = DatabaseManager.FetchField("AccountStats", "BIG_number");
        }
    }

    void SetFace(bool isMale, int eyes, int mouth)
    {
        UVWarp leftEyeObject = f_leftEyeObject;
        UVWarp rightEyeObject = f_rightEyeObject;
        UVWarp mouthObject = f_mouthObject;
        if(isMale)
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

    FaceData GetFaceDate()
    {
        FaceData faceData = new FaceData();
        List<FaceData> faceDataList = maleFaceData;
        if (gender == Gender.Female)
            faceDataList = femaleFaceData;
        if(faceDataList.Count > headType)
        {
            faceData = faceDataList[headType];
        }
        return faceData;
    }

    void ReadFaceDate()
    {
        maleFaceData = ReadFaceDateFile("MaleFaces");
        femaleFaceData = ReadFaceDateFile("FemaleFaces");
    }

    List<FaceData> ReadFaceDateFile(string fileName)
    {
        TextAsset maleData = (TextAsset)Resources.Load("XML/" + fileName);
        string[] lines = maleData.text.Split("\n"[0]);
        List<FaceData> faceDate = new List<FaceData>();
        foreach (string l in lines)
        {
            FaceData f = new FaceData();
            f.eyeType = 0;
            f.mouthType = 0;

            string[] lineData = l.Split(',');

            if (lineData.Length > 1)
            {
                int.TryParse(lineData[0], out f.eyeType);
                int.TryParse(lineData[1], out f.mouthType);
            }
            faceDate.Add(f);
        }
        return faceDate;
    }

    public void Initialize(GameObject gameObject)
    {
        AvatarObject = gameObject; 
        femaleChar = AvatarObject.transform.Find("female").gameObject;
        maleChar = AvatarObject.transform.Find("male").gameObject;

        ReadFaceDate();
        femaleFace = AvatarObject.transform.Find("Bone/Pelvis/Spine/Neck/Head/HEAD_CONTAINER/face/f_face").gameObject;
        maleFace = AvatarObject.transform.Find("Bone/Pelvis/Spine/Neck/Head/HEAD_CONTAINER/face/m_face").gameObject;

        // bodies
        femaleChar.transform.Find("f_body").GetComponentsInChildren<Transform>(true, femaleBodies);
        maleChar.transform.Find("m_body").GetComponentsInChildren<Transform>(true, maleBodies);

        femaleBodies.RemoveAt(0);
        maleBodies.RemoveAt(0);

        // filter bodies lists, leave only correct ones (thx unity3d)
        //femaleBodies = femaleBodies.Where(b => b.name.Contains("f_body_")).ToList();
        //maleBodies = maleBodies.Where(b => b.name.Contains("Body_")).ToList();

        // heads
        femaleChar.transform.Find("f_head").GetComponentsInChildren<Transform>(true, femaleHeads);
        maleChar.transform.Find("m_head").GetComponentsInChildren<Transform>(true, maleHeads);

        // trim first ones cuz it's "HEAD_CONTAINER"
        femaleHeads.RemoveAt(0);
        maleHeads.RemoveAt(0);

        //trim three last ones cuz those are glasses
        femaleChar.transform.Find("f_glasses").GetComponentsInChildren<Transform>(true, femaleGlasses);
        maleChar.transform.Find("m_glasses").GetComponentsInChildren<Transform>(true, maleGlasses);

        femaleGlasses.RemoveAt(0);
        maleGlasses.RemoveAt(0);

        //femaleGlasses = femaleHeads.GetRange(femaleHeads.Count - 3, 3);
        //femaleHeads.RemoveRange(femaleHeads.Count - 3, 3);
        //maleGlasses = maleHeads.GetRange(maleHeads.Count - 3, 3);
        //maleHeads.RemoveRange(maleHeads.Count - 3, 3);
    }  

    void UpdateMaleHeads()
    {
        foreach (Transform h in maleHeads)
        {
            h.gameObject.SetActive(maleHeads.IndexOf(h) == headType && gender == Gender.Male);
        }
        FaceData faceData = GetFaceDate();
        
        SetFace(true, faceData.eyeType, faceData.mouthType);
    }

    void UpdateFemaleHeads()
    {
        foreach (Transform h in femaleHeads)
        {
            h.gameObject.SetActive(femaleHeads.IndexOf(h) == headType && gender == Gender.Female);
        }
        FaceData faceData = GetFaceDate();
        SetFace(false, faceData.eyeType, faceData.mouthType);
    }

    void UpdateMaleBodies()
    {
        foreach (Transform b in maleBodies)
        {
            b.gameObject.SetActive(maleBodies.IndexOf(b) == bodyType && gender == Gender.Male);
        }
    }

    void UpdateFemaleBodies()
    {
        foreach (Transform b in femaleBodies)
        {
            b.gameObject.SetActive(femaleBodies.IndexOf(b) == bodyType && gender == Gender.Female);
        }
    }

    void UpdateMaleGlasses()
    {
        foreach (Transform g in maleGlasses)
        {
            g.gameObject.SetActive(maleGlasses.IndexOf(g) == glassesType && gender == Gender.Male);
        }
    }

    void UpdateFemaleGlasses()
    {
        foreach (Transform g in femaleGlasses)
        {
            g.gameObject.SetActive(femaleGlasses.IndexOf(g) == glassesType && gender == Gender.Female);
        }
    }

    void UpdateActiveObjects()
    {
        maleChar.SetActive(gender == Gender.Male);
        maleFace.SetActive(gender == Gender.Male);
        femaleChar.SetActive(gender == Gender.Female);
        femaleFace.SetActive(gender == Gender.Female);

        UpdateMaleHeads();
        UpdateFemaleHeads();

        UpdateMaleBodies();
        UpdateFemaleBodies();

        UpdateMaleGlasses();
        UpdateFemaleGlasses();
    }

    /// <summary>
    /// Currently no check for out of bounds, plz care
    /// </summary>
    /// <param name="glasses">-1 for None</param>
    public void SetCurrent(Gender g, int head, int body, int glasses)
    {
        gender = g;
        headType = head;
        bodyType = body;
        glassesType = glasses;
        
        UpdateActiveObjects();

        // updating gender buttons
        //maleBtn.color = (gender == CharGender.Female) ? Color.white : Color.green;
        //femaleBtn.color = (gender == CharGender.Female) ? Color.green : Color.white;
    }

    public void PreviousGender()
    {
        // using "if" for now in case i'll add saving last body+head of different gender instead of just resetting
        if (gender == Gender.Female)
        {
            gender = Gender.Male;
        }
        else
        {
            gender = Gender.Female;
        }

        // but for now we're just resetting
        bodyType = 0;
        headType = 0;

        UpdateActiveObjects();
    }

    public void NextGender()
    {
        // now that i think, same as previous function
        // at least while we have only 2 genders
        PreviousGender();
    }

    public void PreviousHead()
    {
        --headType; // -1
        if (headType < 0)
        {
            if (gender == Gender.Female)
            {
                headType = femaleHeads.Count - 1;
            }
            else
            {
                headType = maleHeads.Count - 1;
            }
        }

        if (gender == Gender.Female)
        {
            UpdateFemaleHeads();
        }
        else
        {
            UpdateMaleHeads();
        }
    }

    public void NextHead()
    {
        ++headType;
        if (gender == Gender.Female)
        {
            if (headType >= femaleHeads.Count)
            {
                headType = 0;
            }
            UpdateFemaleHeads();
        }
        else
        {
            if (headType >= maleHeads.Count)
            {
                headType = 0;
            }
            UpdateMaleHeads();
        }
    }

    public void PreviousBody()
    {
        --bodyType;
        if (bodyType < 0)
        {
            if (gender == Gender.Female)
            {
                bodyType = femaleBodies.Count - 1;
            }
            else
            {
                bodyType = maleBodies.Count - 1;
            }
        }

        if (gender == Gender.Female)
        {
            UpdateFemaleBodies();
        }
        else
        {
            UpdateMaleBodies();
        }
    }

    public void NextBody()
    {
        ++bodyType;
        if (gender == Gender.Female)
        {
            if (bodyType >= femaleBodies.Count)
            {
                bodyType = 0;
            }
            UpdateFemaleBodies();
        }
        else
        {
            if (bodyType >= maleBodies.Count)
            {
                bodyType = 0;
            }
            UpdateMaleBodies();
        }
    }

    public void PreviousGlasses()
    {
        --glassesType;
        if (glassesType < -1) // -1 means none, allow that one
        {
            // currently hardcoded 3 types of glasses
            glassesType = 2; // 2 is index of #3, 0 is index of #1
        }

        if (gender == Gender.Female)
        {
            UpdateFemaleGlasses();
        }
        else
        {
            UpdateMaleGlasses();
        }
    }

    public void NextGlasses()
    {
        ++glassesType;

        if (glassesType > 2)
        {
            glassesType = -1;
        }

        if (gender == Gender.Female)
        {
            UpdateFemaleGlasses();
        }
        else
        {
            UpdateMaleGlasses();
        }
    }

    public void MaleBtn()
    {
        SetCurrent(Gender.Male, 0, 0, -1);

        //maleBtn.color = Color.green;
        //femaleBtn.color = Color.white;
    }

    public void FemaleBtn()
    {
        SetCurrent(Gender.Female, 0, 0, -1);

        //maleBtn.color = Color.white;
        //femaleBtn.color = Color.green;
    }

    public void Save()
    {
        bool check = true;

        // check if name is filled
        if (inputNameField.GetComponent<InputField>().text == "")
        {
            inputNameField.GetComponent<Image>().color = Color.red;
            check = false;
        } 

        // check if BIG number is filled
        /*if (inputBIGfield.GetComponent<InputField>().text == "")
        {
            inputBIGfield.GetComponent<Image>().color = Color.red;
            check = false;
        }*/

        if (check)
        { 
            CharacterInfo.SetCharacterCharacteristicsWU( 
                ((gender == Gender.Female) ? "Female" : "Male"),
                headType, bodyType, glassesType);

            // save full name
            PlayerPrefsManager.SetFullName(inputNameField.GetComponent<InputField>().text);
            // save big number
            PlayerPrefsManager.SetBIGNumber(inputBIGfield.GetComponent<InputField>().text);

            // set new character scene to be seen and saved info
            DatabaseManager.UpdateField("AccountStats", "CharSceneV2", "true");

            if (DatabaseManager.FetchField("AccountStats", "TutorialCompleted") == "true")
            {
                bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
            }
            else
            {
                bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Tutorial", "scenes_tutorial");

                // bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Tutorial");
            }
        }
    }
    
    public void ShowCharacter()
    {        
        Initialize(GameObject.Find("w_char"));
        headType = CharacterInfo.headType;
        bodyType = CharacterInfo.bodyType;
        glassesType = CharacterInfo.glassesType;

        switch (CharacterInfo.sex)
        {
            case "Female":
                {
                    gender = Gender.Female;

                    if (maleChar != null)
                        maleChar.SetActive(false);
                    if (maleFace != null)
                        maleFace.SetActive(false);
                    femaleFace.SetActive(true);
                    UpdateFemaleHeads();
                    UpdateFemaleBodies();
                    UpdateFemaleGlasses();

                    break;
                }
            case "Male":
                {
                    gender = Gender.Male;
                    if(femaleChar != null)
                        femaleChar.SetActive(false);
                    if(femaleFace != null)
                        femaleFace.SetActive(false);
                    maleFace.SetActive(true);
                    UpdateMaleHeads();
                    UpdateMaleBodies();
                    UpdateMaleGlasses();
                    break;
                }
        }
    }

    public void FullNameFieldCleanColor()
    {
        inputNameField.GetComponent<Image>().color = Color.white;
    }

    public void BIGFieldCleanColor()
    {
        inputBIGfield.GetComponent<Image>().color = Color.white;
        inputBIGfield.GetComponent<InputField>().text =
            Regex.Replace(inputBIGfield.GetComponent<InputField>().text, "[^.0-9]", "");
    }
}
