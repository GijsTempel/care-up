using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStarter : MonoBehaviour {
    public List<string> triggers;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

#if UNITY_EDITOR
    [ContextMenu("Start Action")]
    public void StartAction()
    {
        int j = -999;
        
        print(j);
        Animator player = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
        if (player != null)
        {
            foreach (string t in triggers)
            {
                if (t != "")
                {
                    string[] strArr = t.Split(char.Parse(" "));
                    int.TryParse(strArr[0], out j);
                    if (j != 0)
                    {
                        string intName = "";
                        for (int i = 1; i < strArr.Length; i++)
                        {
                            if (i != 1)
                                intName += " ";
                            intName += strArr[i];
                        }
                        print(intName);
                        player.SetInteger(intName, j);
                    }
                    else
                    {
                        player.SetTrigger(t);
                    }
                }
            }
        }
    }
#endif
}

