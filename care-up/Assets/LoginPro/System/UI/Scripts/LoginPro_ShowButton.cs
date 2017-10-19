using UnityEngine;

namespace LoginProAsset
{
    public class LoginPro_ShowButton : MonoBehaviour
    {
        void OnGUI()
        {
            // Hide button if not logged in
            this.transform.localScale = LoginPro.Session.LoggedIn ? Vector3.one : Vector3.zero;
        }
    }
}