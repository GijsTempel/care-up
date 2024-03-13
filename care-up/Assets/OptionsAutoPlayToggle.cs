using UnityEngine;
using UnityEngine.UI;


public class OptionsAutoPlayToggle : MonoBehaviour
{
    public Toggle toggle;
    public Toggle recordingToggle;
    public Toggle recordingWithTextToggle;

    // Start is called before the first frame update
    void Start()
    {
        toggle.isOn = PlayerPrefsManager.simulatePlayerActions;
        recordingToggle.isOn = PlayerPrefsManager.videoRecordingMode;

        UpdateVisability();
    }

    public void UpdateVisability()
    {
        bool devMode = PlayerPrefsManager.GetDevMode();

        recordingToggle.transform.parent.gameObject.SetActive(devMode);
        gameObject.SetActive(devMode);
    }

    public void ValueChanged()
    {
        PlayerPrefsManager.simulatePlayerActions = toggle.isOn;
    }

    public void RecordingModeChanged()
    {
        PlayerPrefsManager.videoRecordingMode = recordingToggle.isOn;
    }
    public void RecordingWithTextModeChanged()
    {
        PlayerPrefsManager.videoRecordingWithTextMode = recordingWithTextToggle.isOn;
    }
}
