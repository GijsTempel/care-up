using UnityEngine;
using UnityEngine.UI;

public class ApplePaymentsUI : MonoBehaviour
{
    [TextArea]
    public string AppleOnlyText;

#if UNITY_IOS
    private void Start()
    {
        gameObject.GetComponent<Text>().text = AppleOnlyText;
    }
#endif
}
