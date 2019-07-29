using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStep
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Sprite TutorialImage { get; set; }
}

public class GameTutorialManager : MonoBehaviour
{
    private Text title;
    private Text description;
    private Image image;
    private GameObject previousButton;
    private GameObject nextButton;
    private int index = 0;

    private Sprite[] tutorialImages;
    private List<TutorialStep> tutorialSteps;

    private void Awake()
    {
        LoadInfo();
    }

    private void Start()
    {
        Initialize();        
    }   

    private void Initialize()
    {
        title = GameObject.Find("TutorialCanvas/Canvas/Background/Title/Text").GetComponent<Text>();
        description = GameObject.Find("TutorialCanvas/Canvas/Background/Description/Text").GetComponent<Text>();
        image = GameObject.Find("TutorialCanvas/Canvas/Background/StepImage/Image").GetComponent<Image>();

        previousButton = GameObject.Find("TutorialCanvas/Canvas/Background/NavigationButtons/PreviousStep");
        nextButton = GameObject.Find("TutorialCanvas/Canvas/Background/NavigationButtons/NextStep");

        previousButton.GetComponent<Button>().onClick.AddListener(PreviousStep);
        nextButton.GetComponent<Button>().onClick.AddListener(NextStep);
    }

    private void LoadInfo()
    {
        tutorialSteps = new List<TutorialStep>();

        TextAsset textAsset = (TextAsset)Resources.Load("Xml/GameTutorial");
        XmlDocument xmlFile = new XmlDocument();

        xmlFile.LoadXml(textAsset.text);

        XmlNodeList steps = xmlFile.FirstChild.NextSibling.ChildNodes;

        tutorialImages = Resources.LoadAll<Sprite>("Sprites/TutorialImages");

        int i = 0;

        foreach (XmlNode step in steps)
        {
            if (i < tutorialImages.Length)
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
    }

    private void NextStep()
    {
        ++index;

        previousButton.SetActive(true);

        if (index == tutorialSteps.Count - 1)
        {
            nextButton.SetActive(false);
        }

        UpdateTutorialStep();
    }

    private void PreviousStep()
    {
        --index;

        nextButton.SetActive(true);

        if (index == 0)
        {
            previousButton.SetActive(false);
        }

        UpdateTutorialStep();
    }

    private void UpdateTutorialStep()
    {
        title.text = tutorialSteps[index].Title;
        description.text = tutorialSteps[index].Description;
        image.sprite = tutorialSteps[index].TutorialImage;
    }
}
