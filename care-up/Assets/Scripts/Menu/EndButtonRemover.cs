using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndButtonRemover : MonoBehaviour {

    public GameObject goToMenuButton;

    [SerializeField]
    private GameObject ScorePanel;

    [SerializeField]
    private GameObject StepPanel;

    [SerializeField]
    private GameObject SendScorePanel;

    public void ButtonClick () {
        goToMenuButton.SetActive (false);
    }

    public void OnFurtherButtonClick () {
        StepPanel.GetComponent<Animator> ().SetBool ("pop", false);
        StepPanel.SetActive (false);

        ScorePanel.SetActive (true);
        ScorePanel.GetComponent<Animator> ().SetBool ("pop", true);
    }

    public void OnBackButtonClick () {
        ScorePanel.GetComponent<Animator> ().SetBool ("pop", false);
        ScorePanel.SetActive (false);

        StepPanel.SetActive (true);
        StepPanel.GetComponent<Animator> ().SetBool ("pop", true);
    }

    public void OnNextButton ()
    {
        ScorePanel.GetComponent<Animator>().SetBool("pop", false);
        ScorePanel.SetActive(false);

        SendScorePanel.SetActive(true);
        SendScorePanel.GetComponent<Animator>().SetBool("pop", true);
    }
}
