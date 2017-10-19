using UnityEngine;

namespace LoginProAsset
{
    public class LoginPro_CreateTables : MonoBehaviour
    {
        public UIAnimation AnimationToPlayOnSuccess;
        public UIAnimation AnimationToStopOnSuccess;

        public void Launch()
        {
            LoginPro.Manager.CreateTables(Success, Error);
        }

        public void Error(string errorMessage)
        {
            Debug.Log("Tables creation failed.");
            Debug.LogError(errorMessage);
        }

        public void Success(string[] datas)
        {
            Debug.Log("Tables created.");
        }
    }
}