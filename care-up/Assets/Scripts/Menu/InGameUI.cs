using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour {

    private Transform ui;
    private Transform main;
    private Transform options;

    private GameObject game;
    private Controls controls;
    private GameTimer timer;

    private RigidbodyFirstPersonController player;
    private Crosshair crosshair;
    private Animator animator;

    private bool playerState = false;
    private float animatorSpeed = 1f;

    PlayerPrefsManager prefsManager;
    Slider volumeSlider;

	void Start () {

        ui = transform.GetChild(0);
        main = ui.GetChild(0);
        options = ui.GetChild(1);

        game = GameObject.Find("GameLogic");
        if (game != null)
        {
            controls = game.GetComponent<Controls>();
            timer = game.GetComponent<GameTimer>();
        }

        player = GameObject.Find("Player").GetComponent<RigidbodyFirstPersonController>();
        crosshair = GameObject.Find("Player").GetComponent<Crosshair>();
        animator = player.transform.GetChild(0).GetChild(0).GetComponent<Animator>();

        if (GameObject.Find("Preferences"))
        {
            prefsManager = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        }

        volumeSlider = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<Slider>();

        if (prefsManager != null)
        {
            volumeSlider.value = prefsManager.Volume;

        }
    }
	
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }

	}

    public void Toggle()
    {
        if (ui.gameObject.activeSelf)
        {
            ui.gameObject.SetActive(false);
            if (game != null)
            {
                controls.keyPreferences.ToggleLock();
                timer.enabled = true;
            }

            player.enabled = playerState;
            crosshair.enabled = true;

            animator.speed = animatorSpeed;
            Time.timeScale = 1f;

            ToggleAllSounds(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            ui.gameObject.SetActive(true);
            if (game != null)
            {
                controls.keyPreferences.ToggleLock();
                timer.enabled = false;
            }

            playerState = player.enabled;
            player.enabled = false;
            crosshair.enabled = false;

            animatorSpeed = animator.speed;
            animator.speed = 0.0f;
            Time.timeScale = 0f;

            ToggleAllSounds(false);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OnContinueButtonClick()
    {
        Toggle();
    }

    public void OnSaveButtonClick()
    {
        GameObject.Find("Preferences").GetComponent<SaveLoadManager>().Save();
    }

    public void OnOptionsButtonClick()
    {
        main.gameObject.SetActive(false);
        options.gameObject.SetActive(true);
    }

    public void OnOptionsBackButtonClick()
    {
        main.gameObject.SetActive(true);
        options.gameObject.SetActive(false);
    }

    public void OnExitButtonClick()
    {
        GameObject.Find("Preferences").GetComponent<LoadingScreen>().LoadLevel("Menu");
    }

    public void ToggleAllSounds(bool value)
    {
        AudioSource [] audio = GameObject.FindObjectsOfType<AudioSource>();
        foreach ( AudioSource a in audio )
        {
            if ( value )
            {
                a.UnPause();
            }
            else
            {
                a.Pause();
            }
        }
    }

    public void OnVolumeSliderChange()
    {
        AudioListener.volume = volumeSlider.value;
        if (prefsManager != null)
        {
            prefsManager.Volume = volumeSlider.value;
        }
    }
}
