using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class Options : MonoBehaviour {

    private PlayerPrefsManager prefsManager;

    private Slider volumeSlider;
    private Dropdown qualityOptions;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        prefsManager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        if (prefsManager == null) Debug.LogError("No player prefs manager found");

        volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        volumeSlider.value = prefsManager.Volume;
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);

        qualityOptions = GameObject.Find("QualityDropdown").GetComponent<Dropdown>();
        List<string> options = new List<string>(QualitySettings.names);
        qualityOptions.AddOptions(options);
        qualityOptions.value = QualitySettings.GetQualityLevel();
    }

    public void OnVolumeChange(float value)
    {
        AudioListener.volume = value;
    }

    public void OnBackButton()
    {
        SaveOptions();
        SceneManager.LoadScene("Menu");
    }

    private void SaveOptions()
    {
        prefsManager.Volume = volumeSlider.value;
    }

    public void OnQualityChange()
    {
        QualitySettings.SetQualityLevel(qualityOptions.value, true);
    }

}