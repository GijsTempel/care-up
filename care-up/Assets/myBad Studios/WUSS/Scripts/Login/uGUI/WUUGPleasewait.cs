using UnityEngine;
using UnityEngine.UI;

namespace MBS
{
    public class WUUGPleasewait : MonoBehaviour
    {

        [SerializeField] Image spinner = default(Image);
        [SerializeField] float speed = 300f;
        [SerializeField] WUUGLoginGUI gui = default(WUUGLoginGUI);

        void Awake()
        {
            WPServer.OnServerStateChange += OnServerStateChanged;
            gameObject.SetActive( false );
        }

        void OnDestroy() => WPServer.OnServerStateChange -= OnServerStateChanged;
        
        public void OnServerStateChanged(WPServerState state) => gameObject?.SetActive( state == WPServerState.Contacting && gui.active_state == WUUGLoginGUI.eWULUGUIState.Active );
        void Update() => spinner.transform.Rotate( 0f, 0f, -speed * Time.deltaTime );
    }
}