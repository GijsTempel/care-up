using System;
using System.Xml;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles all combinations in scene.
/// </summary>
public class CombinationManager : MonoBehaviour {

    [Serializable]
    public class Combination
    {
        public string leftInput;
        public string rightInput;
        public string leftResult;
        public string rightResult;
        public bool allowMultiple = false;
    };

    public string combinationListName;
    private List<Combination> combinationList = new List<Combination>();

    private ActionManager actionManager;

    /// <summary>
    /// Instantly loads xml file and stores all possible combination for the scene.
    /// </summary>
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
            combination.allowMultiple = false;
            if (c.Attributes["allowMultiple"] != null)
                combination.allowMultiple = c.Attributes["allowMultiple"].Value == "true";
            combinationList.Add(combination);
        }
    }

    /// <summary>
    /// Handles combine. Checks if Combination possible for the scene.
    /// </summary>
    /// <param name="leftInput">Current object in left hand</param>
    /// <param name="rightInput">Current object in right hand</param>
    /// <param name="leftResult">If combine performed - object that should be in left hand</param>
    /// <param name="rightResult">If combine performed - object that should be in right hand</param>
    /// <returns>True if combine performed</returns>
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

        return found;
    }

    public bool CombineMultiple(string leftInput, string rightInput)
    {
        foreach (Combination c in combinationList)
        {
            if (leftInput == c.leftInput && rightInput == c.rightInput && c.allowMultiple)
                return true;
            if (leftInput == c.rightInput && rightInput == c.leftInput && c.allowMultiple)
                return true;
        }
        return false;
    }

}