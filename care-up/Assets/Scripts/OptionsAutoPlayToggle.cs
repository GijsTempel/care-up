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


#if !(UNITY_EDITOR || DEVELOPMENT_BUILD)
        recordingToggle.transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
#endif
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
