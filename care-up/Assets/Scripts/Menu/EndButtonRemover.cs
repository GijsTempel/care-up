using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndButtonRemover : MonoBehaviour {

    public GameObject goToMenuButton;

    [SerializeField]
    private GameObject SecondPanel;

    [SerializeField]
    private GameObject StartPanel;

    public void ButtonClick () {
        goToMenuButton.SetActive(false);
    }

    public void OnFurtherButtonClick() {
        StartPanel.SetActive (false);

        SecondPanel.SetActive (true);
    }
}
