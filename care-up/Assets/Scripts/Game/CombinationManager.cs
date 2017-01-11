using System;
using System.Xml;
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

    public string combinationListName;
    private List<Combination> combinationList = new List<Combination>();

    private ActionManager actionManager;

    void Start()
    {
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/Combinations/" + combinationListName);
        XmlDocument xmlFile = new XmlDocument();
        xmlFile.LoadXml(textAsset.text);
        XmlNodeList combinations = xmlFile.FirstChild.NextSibling.ChildNodes; 

        foreach (XmlNode c in combinations)
        {
            Combination combination = new Combination();
            combination.leftInput = c.Attributes["leftInput"].Value;
            combination.rightInput = c.Attributes["rightInput"].Value;
            combination.leftResult = c.Attributes["leftResult"].Value;
            combination.rightResult = c.Attributes["rightResult"].Value;
            combinationList.Add(combination);
        }
    }

    public bool Combine(string leftInput, string rightInput, out string leftResult, out string rightResult)
    {
        leftResult = "";
        rightResult = "";

        bool found = false;

        foreach (Combination c in combinationList)
        {
            if (leftInput == c.leftInput && rightInput == c.rightInput)
            {
                leftResult = c.leftResult;
                rightResult = c.rightResult;
                found = true;
            }
            else if (leftInput == c.rightInput && rightInput == c.leftInput)
            {
                leftResult = c.rightResult;
                rightResult = c.leftResult;
                found = true;
            }
        }

        
        if (found)
        {
            actionManager.OnCombineAction(leftInput, rightInput);
        }

        return found;
    }

}