﻿using UnityEngine;
using UnityEngine.UI;
using MBS;

public class UMP_Options : MonoBehaviour
{
    public Animator Anim;
    public string OptionAnimation;

    public Animator AnimCN;
    public string CNAnimation;
    [Space(7)]
    public Text PlayerNameText = null;

    public Text QualityText = null;
    private int CurrentQuality = 0;

    public Text AntiStropicText = null;
    private int CurrentAS = 0;

    public Text AntiAliasingText = null;
    private int CurrentAA = 0;
    private string[] AAOptions = new string[] { "X0","X2","X4","X8"};

    public Text vSyncText = null;
    private int CurrentVSC = 0;
    private string[] VSCOptions = new string[] { "Don't Sync", "Every V Blank", "Every Second V Blank" };

    public Text blendWeightsText = null;
    private int CurrentBW = 0;

    public Text ResolutionText;
    private int CurrentRS = 0;

    public Slider VolumenSlider = null;

    public Toggle FullScreenToggle = null;
    public bool SaveFullcreen = true;
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mas"></param>
    public void GameQuality(bool mas)
    {
        if (mas)
        {
            CurrentQuality = (CurrentQuality + 1) % QualitySettings.names.Length;
        }
        else
        {
            if (CurrentQuality != 0)
            {
                CurrentQuality = (CurrentQuality - 1) % QualitySettings.names.Length;
            }
            else
            {
                CurrentQuality = (QualitySettings.names.Length - 1);
            }
        }
        QualityText.text = QualitySettings.names[CurrentQuality];
        QualitySettings.SetQualityLevel(CurrentQuality);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    public void AntiStropic(bool b)
    {
        if (b) { CurrentAS = (CurrentAS + 1) % 3; } else { if (CurrentAS != 0) { CurrentAS = (CurrentAS - 1) % 3; } else { CurrentAS = 2; } }

        switch (CurrentAS)
        {
            case 0 :
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                AntiStropicText.text = AnisotropicFiltering.Disable.ToString();
                break;
            case 1:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                AntiStropicText.text = AnisotropicFiltering.Enable.ToString();
                break;
            case 2:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                AntiStropicText.text = AnisotropicFiltering.ForceEnable.ToString();
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    public void AntiAliasing(bool b)
    {
        CurrentAA = (b) ? (CurrentAA + 1) % 4 : (CurrentAA != 0) ? (CurrentAA - 1) % 4 : CurrentAA = 3;
        AntiAliasingText.text = AAOptions[CurrentAA];
        switch (CurrentAA)
        {
            case 0:
                QualitySettings.antiAliasing = 0;               
                break;
            case 1:
                QualitySettings.antiAliasing = 2;
                break;
            case 2:
                QualitySettings.antiAliasing = 4;
                break;
            case 3:
                QualitySettings.antiAliasing = 8;
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    public void VSyncCount(bool b)
    {
        CurrentVSC = (b) ? (CurrentVSC + 1) % 3 : (CurrentVSC != 0) ? (CurrentVSC - 1) % 3 : CurrentVSC = 2;
        vSyncText.text = VSCOptions[CurrentVSC];
        switch (CurrentVSC)
        {
            case 0:
                QualitySettings.vSyncCount = 0;
                break;
            case 1:
                QualitySettings.vSyncCount = 1;
                break;
            case 2:
                QualitySettings.vSyncCount = 2;
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    public void blendWeights(bool b)
    {
        CurrentBW = (b) ? (CurrentBW + 1) % 3 : (CurrentBW != 0) ? (CurrentBW - 1) % 3 : CurrentBW = 2;
        switch (CurrentBW)
        {
            case 0:
                QualitySettings.skinWeights = SkinWeights.OneBone;
                blendWeightsText.text = SkinWeights.OneBone.ToString();
                break;
            case 1:
                QualitySettings.skinWeights = SkinWeights.TwoBones;
                blendWeightsText.text = SkinWeights.TwoBones.ToString();
                break;
            case 2:
                QualitySettings.skinWeights = SkinWeights.FourBones;
                blendWeightsText.text = SkinWeights.FourBones.ToString();
                break;
        }
    }

    /// <summary>
    /// Change resolution of screen
    /// NOTE: this works only in build, not in Unity Editor.
    /// </summary>
    /// <param name="b"></param>
    public void Resolution(bool b)
    {
        CurrentRS = (b) ? (CurrentRS + 1) % Screen.resolutions.Length : (CurrentRS != 0) ? (CurrentRS - 1) % Screen.resolutions.Length : CurrentRS = (Screen.resolutions.Length - 1);
#if !UNITY_EDITOR
        ResolutionText.text = Screen.resolutions[CurrentRS].width + " X " + Screen.resolutions[CurrentRS].height;
#else
        ResolutionText.text = Screen.resolutions[0].width + " X " + Screen.resolutions[0].height;
#endif
    }

    /// <summary>
    /// Change volumen
    /// </summary>
    /// <param name="v">volumen</param>
    public void Volumen(float v) { AudioListener.volume = v; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="forward"></param>
    bool mopen = false;

    public void WndowAnimation(bool forward)
    {
        if (forward)
        {
            if (forward == mopen)
                return;

            Anim.SetBool("show", true);
        }
        else
        {
            Anim.SetBool("show", false);
        }
        mopen = forward;
    }
   
    bool mopencn = false;
    bool cnopem = false;

    public void CNAnimationWindow()
    {
        cnopem = !cnopem;
        if (cnopem)
        {
            if (cnopem == mopencn)
                return;

            if (!AnimCN.gameObject.activeSelf) { AnimCN.gameObject.SetActive(true); }

            AnimCN.SetBool("show", true);
        }
        else
        {
            AnimCN.SetBool("show", false);
        }
        mopencn = cnopem;
    }

    /// <summary>
    /// Saved Options
    /// </summary>
    public void Apply()
    {
        Toggle postProcessing = GameObject.Find("PostProcessingToggle").GetComponent<Toggle>();
        GameObject preferences = GameObject.Find("Preferences");
        if (preferences != null)
        {
            preferences.GetComponent<PlayerPrefsManager>().postProcessingEnabled = postProcessing.isOn;
            PlayerPrefs.SetInt("PostProcessing", (postProcessing.isOn ? 1 : 0));
            Debug.Log("PostProcessing is set to " + postProcessing.isOn);
            int QualityLevel = 6;
            if (postProcessing.isOn)
                QualityLevel = 7;
            QualitySettings.SetQualityLevel(QualityLevel, true);
        }
        //StatusMessage.Message = "Aanpassingen zijn opgeslagen";
    } 

    public void ChangeName(InputField field) { if (field == null || PlayerNameText == null) return; PlayerNameText.text = field.text; field.text = string.Empty; CNAnimationWindow(); }
}
   