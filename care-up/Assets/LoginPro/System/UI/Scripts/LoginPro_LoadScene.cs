using UnityEngine;

namespace LoginProAsset
{
    public class LoginPro_LoadScene : MonoBehaviour
    {
        public string SceneToLoad;

        /// <summary>
        /// Load scene with the scene object
        /// </summary>
        public void LoadScene()
        {
            LoginPro_Security.Load(SceneToLoad);
        }
    }
}