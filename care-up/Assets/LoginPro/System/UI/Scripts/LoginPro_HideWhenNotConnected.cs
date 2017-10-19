using UnityEngine;

namespace LoginProAsset
{
    [RequireComponent(typeof(RectTransform))]
    public class LoginPro_HideWhenNotConnected : MonoBehaviour
    {
        void FixedUpdate()
        {
            // Hide if not connected
            if (!LoginPro.Session.LoggedIn)
                this.transform.localScale = Vector3.zero;

            // Set to its original scale when connected
            else
                this.transform.localScale = Vector3.one;
        }
    }
}