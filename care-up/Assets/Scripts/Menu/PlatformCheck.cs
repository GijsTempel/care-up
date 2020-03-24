using UnityEngine;

public class PlatformCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject IOSPanel;

    [SerializeField]
    private GameObject generalPanel;


    private void Start()
    {
#if UNITY_ANROID
        if (generalPanel != null)
        {
            generalPanel.SetActive(false);
        }

        if (IOSPanel != null)
        {
            IOSPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("IOS login register panel is missing.");
        }

        return;
#endif

        if (IOSPanel != null)
        {
            IOSPanel.SetActive(false);
        }
    }

}
