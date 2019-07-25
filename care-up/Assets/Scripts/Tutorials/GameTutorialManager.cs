using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStep
{
    public string Title { get; set; }

    public string Description { get; set; }

    public object TutorialImage { get; set; }
}

public class GameTutorialManager : MonoBehaviour
{
    private Text title;
    private Text description;
    private Image image;
    private GameObject previousButton;
    private Button nextButton;

    private List<TutorialStep> tutorialSteps;
    private object[] tutorialImages;
    private int index;

    private void Start()
    {        
        LoadInfo();
    }

    private void LoadInfo()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Xml/TutorialSteps");
        XmlDocument xmlFile = new XmlDocument();

        xmlFile.LoadXml(textAsset.text);

        XmlNodeList steps = xmlFile.FirstChild.NextSibling.ChildNodes;

        tutorialImages = Resources.LoadAll("Resources/Sprites/TutorialImages");

        int i = 0;

        foreach (XmlNode step in steps)
        {
            tutorialSteps.Add(new TutorialStep
            {
                Title = step.Attributes["title"].Value,
                Description = step.Attributes["description"].Value,
                TutorialImage = tutorialImages[i]
            });

            i++;
        }
    }
   
    private void InitilizeObjects()
    {
        previousButton = transform.Find("TutorialCanvas/Canvas/Background/NavigationButtons/PreviousStep").gameObject;       
    }

    public void NextStep()
    {
        ++index;

        if (index >= tutorialSteps.Count)
        {
            index = 0;
        }

        UpdateTutorialStep();
    }

    public void PreviousStep()
    {
        --index;

        if (index < 0)
        {
            index = tutorialSteps.Count - 1;
        }

        UpdateTutorialStep();
    }

    private void UpdateTutorialStep()
    {
        title.text = tutorialSteps[index].Title;
        description.text = tutorialSteps[index].Description;
        image = (Image)tutorialSteps[index].TutorialImage;
    }
}
