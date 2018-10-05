using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndButtonRemover : MonoBehaviour {

    public GameObject goToMenuButton;

    public void ButtonClick () {
        goToMenuButton.SetActive(false);
    }
}
