using UnityEngine;
using UnityEngine.UI;

public class ApplePaymentsUI : MonoBehaviour
{
    [TextArea]
    public string AppleOnlyText;

    private void Start()
    {
#if UNITY_IOS
        gameObject.GetComponent<Text>().text = AppleOnlyText;
#endif

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            gameObject.GetComponent<Text>().text = AppleOnlyText;
        }
    }
}
