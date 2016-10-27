using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombinationManager : MonoBehaviour {

    [Serializable]
    public class Combination
    {
        public string leftInput;
        public string rightInput;
        public string leftResult;
        public string rightResult;
    };

    public List<Combination> combinationList = new List<Combination>();

    private ActionManager actionManager;

    void Start()
    {
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
    }

    public bool Combine(string leftInput, string rightInput, out string leftResult, out string rightResult)
    {
        leftResult = "";
        rightResult = "";

        bool found = false;

        foreach( Combination c in combinationList )
        {
            if ( leftInput == c.leftInput && rightInput == c.rightInput )
            {
                leftResult = c.leftResult;
                rightResult = c.rightResult;
                found = true;
            }
            else if ( leftInput == c.rightInput && rightInput == c.leftInput )
            {
                leftResult = c.rightResult;
                rightResult = c.leftResult;
                found = true;
            }
        }

        // if found send msg to action manager
        actionManager.OnCombineAction(leftInput, rightInput);

        return found;
    }

}
