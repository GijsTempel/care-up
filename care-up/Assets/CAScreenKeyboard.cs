using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CAScreenKeyboard : MonoBehaviour
{
    public GameObject CA_SKBPanel;
    public InputField SKBInput;
    public Text descrTest;
    public GameObject visibilityToggle;
    [SerializeField] private GameObject loginPasswordVisibilityOn = default;

    [SerializeField] private GameObject loginPasswordVisibilityOff = default;
    InputField linkedInputField = null;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InputChanged()
    {
        if (linkedInputField != null)
        {
            linkedInputField.text = SKBInput.text;
        }
    }
    public void Popup(InputField _input, bool isPassword, string description = "")
    {
        visibilityToggle.SetActive(isPassword);
        CA_SKBPanel.gameObject.SetActive(true);
        linkedInputField = _input;
        SKBInput.text = linkedInputField.text;
        descrTest.text = description;
        SKBInput.contentType = linkedInputField.contentType;
        SKBInput.Select();
        SKBInput.ActivateInputField();
        //fields.login_password.contentType = passwordVisible ? InputField.ContentType.Standard : InputField.ContentType.Password;

    }

    public void EnterValue()
    {
        linkedInputField.text = SKBInput.text;
        linkedInputField = null;
        CA_SKBPanel.gameObject.SetActive(false);
    }

    public void OnTogglePasswordVisibility()
    {
        bool passwordVisible = !(SKBInput.contentType == InputField.ContentType.Password);

        SKBInput.contentType = passwordVisible ? InputField.ContentType.Password : InputField.ContentType.Standard;
        linkedInputField.contentType = passwordVisible ? InputField.ContentType.Password : InputField.ContentType.Standard;
        loginPasswordVisibilityOn.SetActive(passwordVisible);
        loginPasswordVisibilityOff.SetActive(!passwordVisible);
        SKBInput.ForceLabelUpdate();
        linkedInputField.ForceLabelUpdate();
    }
}
