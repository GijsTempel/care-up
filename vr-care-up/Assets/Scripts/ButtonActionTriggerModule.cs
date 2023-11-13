using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActionTriggerModule : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private String buttonText;

    private GameObject buttonInstance = null;

    public Transform buttonAnchor;
    public ActionTrigger actionTrigger;
    public ActionExpectant actionExpectant;

    // Start is called before the first frame update
    void Start()
    {
        if (buttonInstance == null)
        {
            buttonInstance = GameObject.Instantiate<GameObject>(buttonPrefab, buttonAnchor);
            buttonInstance.transform.localRotation = Quaternion.identity;
            buttonInstance.GetComponent<ButtonActionTrigger>().actionTrigger = actionTrigger;
            buttonInstance.GetComponent<ButtonActionTrigger>().SetText(buttonText);
            buttonInstance.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        buttonInstance.SetActive(actionExpectant.isCurrentAction);
    }
}
