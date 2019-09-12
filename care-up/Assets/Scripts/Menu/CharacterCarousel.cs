using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCarousel : MonoBehaviour
{
    public List<TextMesh> Labels;
    public List<GameObject> Platforms;

    string[] ll = {"A","B","C","D","E","F","G"};
    int behindMarker = 3;
    public float turnAngle = 0;
    bool turnTrigger = false;
    int turnDir = 0;
    int nextTurnDir = 0;
    int currentChar = 1;

    void Start()
    {
        int cc = currentChar - 1;
        foreach(TextMesh t in Labels)
        {
            t.text = GetLabel(cc);
            cc++;
        }
    }

    string GetLabel(int num)
    {
        if (num >= 0 && num < (ll.Length))
        {
            return ll[num];
        }
        return "";
    }

    public void Turn(int dir)
    {   int nextChar = currentChar + dir;
        if (nextChar >= 0 && nextChar < ll.Length-1)
            nextTurnDir = dir;
    }


    void Update()
    {
        if (turnDir == 0 && nextTurnDir != 0)
        {
            turnDir = nextTurnDir;
            nextTurnDir = 0;

            currentChar += turnDir;
            string label = GetLabel(currentChar + 2);
            if (turnDir < 0)
                label = GetLabel(currentChar - 1);
            Labels[behindMarker].text = label;
            Platforms[behindMarker].SetActive(label != "");

            behindMarker += turnDir;
            if (behindMarker > 3)
                behindMarker = 0;
            else if (behindMarker < 0)
                behindMarker = 3;
    
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
            }
            else
            {
                rot.y += turnDir * 5f;
            }
            transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
    }
}
