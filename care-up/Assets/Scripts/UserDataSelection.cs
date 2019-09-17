using System.Collections.Generic;
using UnityEngine;
using CareUpAvatar;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class UserDataSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject inputNameField, inputBIGfield = default;

    [SerializeField]
    private GameObject textInfoPanel, avatarPanel = default;

    [SerializeField]
    private List<GameObject> avatarsButtons = default;

    [SerializeField]
    private List<PlayerAvatar> avatars;

    private void InitializeAvatars()
    {
        for (int i = 0; i < avatars.Count; i++)
        {
            PlayerAvatarData playerAvatarData = PlayerPrefsManager.storeManager.CharacterItems[i].playerAvatar;

            if (playerAvatarData != null)
            {
                avatars[i].avatarData = playerAvatarData;
                avatars[i].UpdateCharacter();
            }
            avatars[i].SetAnimationAction(Actions.Idle, true);
        }
    }

    public void Next()
    {
        SaveInfo();
        textInfoPanel.SetActive(false);
        avatarPanel.SetActive(true);

        InitializeAvatars();
    }

    private void Start()
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

    public void ChooseAvatar()
    {
        PlayerAvatar selectedAvatar;

        for (int i = 0; i < avatarsButtons.Count; i++)
        {
            if (avatarsButtons[i] == EventSystem.current.currentSelectedGameObject)
            {
                selectedAvatar = avatars[i];
                avatarsButtons[i].GetComponent<CanvasGroup>().alpha = 1;
            }
            else
                avatarsButtons[i].GetComponent<CanvasGroup>().alpha = 0;
        }          
    }

    public void SaveInfo()
    {
        bool check = true;

        if (inputNameField.GetComponent<InputField>().text == "")
        {
            inputNameField.GetComponent<Image>().color = Color.red;
            check = false;
        }

        if (check)
        {
            PlayerPrefsManager.SetFullName(inputNameField.GetComponent<InputField>().text);
            PlayerPrefsManager.SetBIGNumber(inputBIGfield.GetComponent<InputField>().text);
        }
    }

    public void FullNameFieldCleanColor()
    {
        inputNameField.GetComponent<Image>().color = Color.white;
    }

    public void BIGFieldCleanColor()
    {
        inputBIGfield.GetComponent<Image>().color = Color.white;
        inputBIGfield.GetComponent<InputField>().text =
            Regex.Replace(inputBIGfield.GetComponent<InputField>().text, "[^.0-9]", "");
    }
}
