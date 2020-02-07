using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoplayMenuPanel : MonoBehaviour
{
    public GameObject panel;
    // Start is called before the first frame update

    void OnEnable()
    {
        panel.SetActive(PlayerPrefsManager.simulatePlayerActions);
#if !(UNITY_EDITOR || DEVELOPMENT_BUILD)
        panel.SetActive(false);
#endif
    }
}
