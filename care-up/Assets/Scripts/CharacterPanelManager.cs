using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> characters;

    [SerializeField]
    private GameObject previousButton, nextButton, buyButton, adjustButton;

    private CharacterCreationScene characterCreation;
    private CharacterCreationScene.CharGender gender;

    private List<CharacterItem> parameters = PlayerPrefsManager.storeManager.CharacterItems;

    private int index = 0;

    private void Start()
    {
        if (characterCreation == null)
            characterCreation = GameObject.FindObjectOfType<CharacterCreationScene>();

        foreach (GameObject gameObject in characters)
        {
            gameObject.SetActive(true);
        }

        SetCharacters(characters);
        previousButton?.GetComponent<Button>().onClick.AddListener(PreviousStep);
        nextButton?.GetComponent<Button>().onClick.AddListener(NextStep);

        buyButton?.GetComponent<Button>().onClick.AddListener(BuyCharacter);
    }

    private void NextStep()
    {
        index -= 2;
        SetCharacters(characters);
    }

    private void PreviousStep()
    {
        index -= 4;
        SetCharacters(characters);
    }

    public void SetCharacters(List<GameObject> items)
    {
        foreach (GameObject item in items)
        {
            characterCreation.Initialize(item);

            (bool purchased, int price) = SetCurrentItem(ref index);

            item.transform.parent.Find("Checkmark").gameObject.SetActive(price > 120); //temporary

            if (item.transform.parent.name == "CenterGuy")
            {
                buyButton.transform.GetChild(0).GetComponent<Text>().text = price.ToString();

                if (price > 120)
                    adjustButton.SetActive(true);
                else
                    adjustButton.SetActive(false);
            }

            index++;
        }
    }

    private (bool purchased, int price) SetCurrentItem(ref int index)
    {
        if (index < 0)
            index = parameters.Count - 1;

        else if (index >= parameters.Count)
            index = 0;

        gender = parameters[index].gender == "Female" ? CharacterCreationScene.CharGender.Female : CharacterCreationScene.CharGender.Male;
        characterCreation.SetCurrent(gender, parameters[index].headType, parameters[index].bodyType, parameters[index].glassesType);

        return (parameters[index].purchased, parameters[index].price);
    }

    private void BuyCharacter()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (i == 1)
            {
                characters[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                characters[i].GetComponent<Animator>().SetTrigger("dance1");
                adjustButton.SetActive(true);
            }
            else
                characters[i].transform.parent.gameObject.SetActive(false);
        }
    }

    // To do:

    // Save to database on adjustbutton click
    // Set guys active when animation ends
    // Set guys local scale to normal
    //
}

