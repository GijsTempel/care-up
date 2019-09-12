using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject inputNameField;
    public GameObject inputBIGfield;

    void Start()
    {
        PlayerPrefsManager manager = GameObject.FindObjectOfType<PlayerPrefsManager>();

        if (manager != null)
        {
            if (inputNameField != null)
                inputNameField.GetComponent<InputField>().text = manager.fullPlayerName;
            if (inputBIGfield != null)
                inputBIGfield.GetComponent<InputField>().text = DatabaseManager.FetchField("AccountStats", "BIG_number");
        }
    }

    public void Save()
    {
        bool check = true;

        // check if name is filled
        if (inputNameField.GetComponent<InputField>().text == "")
        {
            inputNameField.GetComponent<Image>().color = Color.red;
            check = false;
        }

        if (check)
        {
            CharacterInfo.SetCharacterCharacteristicsWU(
                ((gender == CharGender.Female) ? "Female" : "Male"),
                headType, bodyType, glassesType);

            // save full name
            PlayerPrefsManager.SetFullName(inputNameField.GetComponent<InputField>().text);
            // save big number
            PlayerPrefsManager.SetBIGNumber(inputBIGfield.GetComponent<InputField>().text);

            // set new character scene to be seen and saved info
            DatabaseManager.UpdateField("AccountStats", "CharSceneV2", "true");

            if (DatabaseManager.FetchField("AccountStats", "TutorialCompleted") == "true")
            {
                bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
            }
            else
            {
                bl_SceneLoaderUtils.GetLoader.LoadLevel("Scenes_Tutorial");
            }
        }
    }
}
