using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndButtonRemover : MonoBehaviour {

    [SerializeField] private GameObject canvas;

    public void ButtonClick () {
        canvas.SetActive(false);
    }
}
