using UnityEngine;
using UnityEngine.UI;

namespace LoginProAsset
{
    [RequireComponent(typeof(Scrollbar))]
    public class LoginPro_ScrollBarInit : MonoBehaviour
    {
        public int InitValue = 1;

        private Scrollbar scrollbar;

        void Awake()
        {
            this.scrollbar = this.transform.GetComponent<Scrollbar>();
        }

        void Start()
        {
            this.scrollbar.value = InitValue;
        }
    }
}