using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> characters;

    [SerializeField]
    private GameObject previousButton, nextButton;

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

        //GameObject.Find("w_char").SetActive(false);

        SetCharacters(characters);
        previousButton?.GetComponent<Button>().onClick.AddListener(PreviousStep);
        nextButton?.GetComponent<Button>().onClick.AddListener(NextStep);
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
            SetCurrentItem(ref index);
            index++;
        }
    }

    private void SetCurrentItem(ref int index)
    {
        if (index < 0)
            index = parameters.Count - 1;

        else if (index >= parameters.Count)
            index = 0;

        print(index);

        gender = parameters[index].gender == "Female" ? CharacterCreationScene.CharGender.Female : CharacterCreationScene.CharGender.Male;
        characterCreation.SetCurrent(gender, parameters[index].headType, parameters[index].bodyType, parameters[index].glassesType);
    }   
}

