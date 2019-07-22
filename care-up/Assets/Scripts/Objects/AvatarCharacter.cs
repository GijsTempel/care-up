using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCharacter : MonoBehaviour
{
    public enum CharGender
    {
        Male,
        Female
    };

    public CharGender gender;
    public int headType;
    public int bodyType;
    public int glassesType;

    private GameObject maleChar;
    private GameObject femaleChar;
    private List<Transform> maleHeads = new List<Transform>();
    private List<Transform> femaleHeads = new List<Transform>();
    private List<Transform> femaleBodies = new List<Transform>();
    private List<Transform> maleBodies = new List<Transform>();
    private List<Transform> maleGlasses = new List<Transform>();
    private List<Transform> femaleGlasses = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        maleChar = transform.Find("male").gameObject;
        femaleChar = transform.Find("female").gameObject;

        femaleChar.transform.Find("f_head").GetComponentsInChildren<Transform>(true, femaleHeads);
        maleChar.transform.Find("m_head").GetComponentsInChildren<Transform>(true, maleHeads);
        foreach(Transform t in maleHeads)
        {
            print(t.name);
        }

        femaleHeads.RemoveAt(0);
        maleHeads.RemoveAt(0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
