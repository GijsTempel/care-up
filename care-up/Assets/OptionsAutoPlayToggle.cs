using UnityEngine;
using UnityEngine.UI;


public class OptionsAutoPlayToggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Toggle>().isOn = PlayerPrefsManager.simulatePlayerActions;

#if !(UNITY_EDITOR || DEVELOPMENT_BUILD)
        gameObject.SetActive(false);
#endif
    }

    public void ValueChanged()
    {
        PlayerPrefsManager.simulatePlayerActions = GetComponent<Toggle>().isOn;
    }
}
