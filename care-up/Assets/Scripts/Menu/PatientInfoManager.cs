using UnityEngine;
using UnityEngine.UI;

public class PatientInfoManager : MonoBehaviour
{
    private Transform infoTabs;
    private GameObject prescriptionButton;
    private GameObject patientButton;

    public void SetTabActive(string tabName)
    {
        if (tabName == "QuizTab")
        {
            SetInteractability(false);
        }

        for (int i = 0; i < infoTabs.childCount; i++)
        {
            if (infoTabs.GetChild(i).name == tabName)
                infoTabs.GetChild(i).gameObject.SetActive(true);
            else
                infoTabs.GetChild(i).gameObject.SetActive(false);
        }       
    }

    public void SetInteractability(bool value)
    {
        prescriptionButton.GetComponent<Button>().interactable = value;
        patientButton.GetComponent<Button>().interactable = value;
    }

    private void Start()
    {
        infoTabs = GameObject.Find("UI/IpadPanel/PatientInfoTabs/Info").transform;

        prescriptionButton = GameObject.Find("Prescription");
        patientButton = GameObject.Find("Patient");

        prescriptionButton.GetComponent<Button>().onClick.AddListener(OnPrescriptionFormClick);
        patientButton.GetComponent<Button>().onClick.AddListener(OnPatientRecordsClick);

        GameObject.Find("Record").GetComponent<Button>().onClick.AddListener(OnFinishClick);
    }

    private void OnFinishClick()
    {
        PlayerPrefsManager prefsManager = GameObject.FindObjectOfType<PlayerPrefsManager>();

        if (prefsManager != null && !prefsManager.practiceMode)
        {
            FindObjectOfType<GameUI>().OpenDonePanelYesNo();
        }
        else
        {
            FindObjectOfType<ActionManager>().OnUseAction("PaperAndPen");
        }

        FindObjectOfType<PlayerScript>().CloseRobotUI();
    }

    private void OnPrescriptionFormClick()
    {
        FindObjectOfType<PlayerScript>().OpenRobotUI();
        SetTabActive("PrescriptionTab");
        FindObjectOfType<ActionManager>().OnExamineAction("PrescriptionForm", "good");
    }

    private void OnPatientRecordsClick()
    {
        FindObjectOfType<PlayerScript>().OpenRobotUI();
        SetTabActive("RecordsTab");
        FindObjectOfType<ActionManager>().OnExamineAction("PatientRecords", "good");
    }
}
