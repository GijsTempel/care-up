using UnityEngine;

/// <summary>
/// This class allows us to manage the third tutorial scene
/// </summary>
namespace LoginProAsset
{
    public class SendFilesOnServer : MonoBehaviour
    {
        public UnityEngine.Object nextSceneToLoad;

        public void NextInstallationStep()
        {
            LoginPro_Security.Load(nextSceneToLoad.name);
        }
    }
}
