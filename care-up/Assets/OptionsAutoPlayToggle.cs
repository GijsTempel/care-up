using UnityEngine;
using UnityEngine.UI;


public class OptionsAutoPlayToggle : MonoBehaviour
{
    Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle = transform.Find("AutoPlayToggle2").GetComponent<Toggle>();
        toggle.isOn = PlayerPrefsManager.simulatePlayerActions;

#if !(UNITY_EDITOR || DEVELOPMENT_BUILD)
        gameObject.SetActive(false);
#endif
    }

    public void ValueChanged()
    {
        PlayerPrefsManager.simulatePlayerActions = toggle.isOn;
    }
}
