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
    private GameObject QuizPanel;

    [SerializeField]
    private GameObject SendScorePanel;

    [SerializeField]
    private GameObject CertificatePanel;

    public void ButtonClick () {
        goToMenuButton.SetActive (false);
    }

    public void OnToQuizClick () {
        StepPanel.GetComponent<Animator> ().SetBool ("pop", false);
        StepPanel.SetActive (false);

        QuizPanel.SetActive (true);
        QuizPanel.GetComponent<Animator> ().SetBool ("pop", true);
    }
    public void OnToScoreClick()
    {
        QuizPanel.GetComponent<Animator>().SetBool("pop", false);
        QuizPanel.SetActive(false);

        ScorePanel.SetActive(true);
        ScorePanel.GetComponent<Animator>().SetBool("pop", true);
    }


    public void OnBackToQuizClick () {
        ScorePanel.GetComponent<Animator>().SetBool ("pop", false);
        ScorePanel.SetActive (false);

        QuizPanel.SetActive (true);
        QuizPanel.GetComponent<Animator>().SetBool ("pop", true);
    }

    public void OnBackToStepsClick()
    {
        QuizPanel.GetComponent<Animator>().SetBool("pop", false);
        QuizPanel.SetActive(false);

        StepPanel.SetActive(true);
        StepPanel.GetComponent<Animator>().SetBool("pop", true);
    }
    public void OnNextButton ()
    {
        ScorePanel.GetComponent<Animator>().SetBool("pop", false);
        ScorePanel.SetActive(false);

        CertificatePanel.SetActive(true);
        CertificatePanel.GetComponent<Animator>().SetBool("pop", true);
    }
    public void OnBackToScoreButton()
    {
        ScorePanel.GetComponent<Animator>().SetBool("pop", true);
        ScorePanel.SetActive(true);

        CertificatePanel.SetActive(false);
        CertificatePanel.GetComponent<Animator>().SetBool("pop", false);
    }
    public void OnSendScoreButton ()
    {
        CertificatePanel.GetComponent<Animator>().SetBool("pop", false);
        CertificatePanel.SetActive(false);

        SendScorePanel.SetActive(true);
        SendScorePanel.GetComponent<Animator>().SetBool("pop", true);
    }

    public void OnBackToCertificate()
    {
        CertificatePanel.SetActive(true);
        CertificatePanel.GetComponent<Animator>().SetBool("pop", true);

        SendScorePanel.SetActive(false);
        SendScorePanel.GetComponent<Animator>().SetBool("pop", false);
    }
}
