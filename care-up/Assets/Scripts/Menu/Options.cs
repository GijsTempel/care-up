using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class Options : MonoBehaviour {

    private PlayerPrefsManager prefsManager;

    private Slider volumeSlider;
    private Dropdown qualityOptions;
    private Dropdown resolutionOptions;

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
        List<string> qOptions = new List<string>(QualitySettings.names);
        qualityOptions.AddOptions(qOptions);
        qualityOptions.value = QualitySettings.GetQualityLevel();

        resolutionOptions = GameObject.Find("ResolutionDropdown").GetComponent<Dropdown>();
        Resolution[] resolutions = Screen.resolutions;
        int currentResolution = 0;
        for (; currentResolution < resolutions.Length; ++currentResolution)
            if (resolutions[currentResolution].width == Screen.currentResolution.width 
                && resolutions[currentResolution].height == Screen.currentResolution.height)
                break;
        List<string> rOptions = new List<string>();
        foreach( Resolution r in resolutions )
            rOptions.Add(r.ToString());
        resolutionOptions.AddOptions(rOptions);
        resolutionOptions.value = currentResolution;
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

    public void OnResolutionChange()
    {
        Resolution[] resolutions = Screen.resolutions;
        Screen.SetResolution(resolutions[resolutionOptions.value].width, 
            resolutions[resolutionOptions.value].height, Screen.fullScreen);
    }
}