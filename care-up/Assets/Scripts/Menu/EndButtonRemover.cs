using UnityEngine;

public class EndButtonRemover : MonoBehaviour {

    int endScoreShown = 0;
    public GameObject goToMenuButton;

    public GameObject ResultInfoHolder;
    [SerializeField]
    private GameObject ScorePanel = null;

    [SerializeField]
    private GameObject StepPanel = null;

    [SerializeField]
    private GameObject QuizPanel = null;

    [SerializeField]
    private GameObject SendScorePanel = null;

    [SerializeField]
    private GameObject CertificatePanel = null;

    public void ButtonClick () {
        // goToMenuButton.SetActive (false);
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
        if (endScoreShown == 0)
        {
            int value = GameObject.FindObjectOfType<EndScoreManager>().percent;   
            int coins = GameObject.FindObjectOfType<EndScoreManager>().rewardCoins;         
            EndScoreRadial endScoreRadial = GameObject.FindObjectOfType<EndScoreRadial>();
            if (endScoreRadial != null)
                GameObject.FindObjectOfType<EndScoreRadial>().StartAnimation(value, coins);
        }
        endScoreShown++;
    }

    public void ShowResultInfoHolder()
    {
        ResultInfoHolder.SetActive(true);
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
