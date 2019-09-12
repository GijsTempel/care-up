using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCarousel : MonoBehaviour
{
    public List<TextMesh> Labels;
    string[] ll = {"A","B","C","D","E","F","G"};
    int behindMarker = 3;
    public float turnAngle = 0;
    bool turnTrigger = false;
    int turnDir = 0;
    int nextTurnDir = 0;
    public int currentChar = 0;

    void Start()
    {
        int cc = currentChar;
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
    {
        nextTurnDir = dir;
    }


    void Update()
    {
        if (turnDir == 0 && nextTurnDir != 0)
        {
            turnDir = nextTurnDir;
            nextTurnDir = 0;

            currentChar += turnDir;
            if (turnDir > 0)
            {
                Labels[behindMarker].text = GetLabel(currentChar + 2);
            }
            else
            {
                Labels[behindMarker].text = GetLabel(currentChar - 2);
                
            }
            behindMarker += turnDir;
            if (behindMarker > 3)
                behindMarker = 0;
            else if (behindMarker < 0)
                behindMarker = 3;
            print(behindMarker);
    
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
