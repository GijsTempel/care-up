using System.Collections;
using System.Collections.Generic;
using MBS;
using UnityEngine;
using UnityEngine.UI;


public class MasterPasswordControl : MonoBehaviour
{
    int openPassPanelClickCount = 0;
    public GameObject passwordPopup;
    public Image passPanelImage;
    public InputField passInput;

    long buttonPressTime = 0;
    

    public void OpenPassButtonClicked(bool forceClose = false)
    {
        long currentTimeMil = new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeMilliseconds();
        if ((currentTimeMil - buttonPressTime) > 1000)
            openPassPanelClickCount = 0;
        buttonPressTime = currentTimeMil;
        
        if (passwordPopup.activeSelf || forceClose)
        {
            openPassPanelClickCount = 0;
            passwordPopup.SetActive(false);
        }
        else
        {
            openPassPanelClickCount++;
            if (openPassPanelClickCount >= 3)
            {
                openPassPanelClickCount = 0;
                passwordPopup.SetActive(true);
            }
        }
        passPanelImage.enabled = passwordPopup.activeSelf;

    }

    public void EnterPass()
    {
        string passEntered = passInput.text;
        if (PlayerPrefsManager.CheckMasterPassword(passEntered))
        {
            PlayerPrefsManager.EnableMasterMode(true);
            GetComponent<Animator>().SetTrigger("correct");
            UpdateDebugElements();
        }
        else
        {
            PlayerPrefsManager.EnableMasterMode(false);
            GetComponent<Animator>().SetTrigger("incorrect");
            UpdateDebugElements();
        }
    }

    void UpdateDebugElements()
    {
        WUUGLoginGUI wUUGLoginGUI = GameObject.FindObjectOfType<WUUGLoginGUI>();
        if (wUUGLoginGUI != null)
            wUUGLoginGUI.UpdateDebugOptionsPanelVis();
        OptionsAutoPlayToggle optionsAutoPlayToggle = GameObject.FindObjectOfType<OptionsAutoPlayToggle>();
        if (optionsAutoPlayToggle != null)
            optionsAutoPlayToggle.UpdateVisability();
    }

    public void DisableMasterMode()
    {
        PlayerPrefsManager.EnableMasterMode(false);
        OpenPassButtonClicked(true);
        UpdateDebugElements();
    }

}
