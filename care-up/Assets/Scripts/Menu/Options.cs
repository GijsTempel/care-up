using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    private PlayerPrefsManager prefsManager;

    private Slider volumeSlider;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        prefsManager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        if (prefsManager == null) Debug.LogError("No player prefs manager found");

        volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        volumeSlider.value = prefsManager.Volume;
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);
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

}