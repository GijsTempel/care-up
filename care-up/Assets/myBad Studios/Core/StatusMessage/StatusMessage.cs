namespace MBS
{
    using UnityEngine;
    public class StatusMessage : MonoBehaviour
    {
        static StatusMessage _instance;
        static public StatusMessage Instance
        {
            get
            {
                if ( null == _instance )
                {
                    StatusMessage [] objs = GameObject.FindObjectsOfType<StatusMessage>();
                    if ( null == objs || objs.Length == 0 )
                    {
                        GameObject o = new GameObject( "StatusMessage" );
                        _instance = o.AddComponent<StatusMessage>();
                    }
                    else
                    {
                        _instance = objs [0];
                        if ( objs.Length > 1 )
                        {
                            for ( int i = 1; i < objs.Length; i++ )
                            {
                                objs [i].enabled = false;
                            }
                        }
                    }
                }
                return _instance;
            }
        }

        static public string statusMessage = "";

        public GUISkin skin;
        private GUIStyle guiStyle = new GUIStyle(); //create a new variable
        static public string Message
        {
            get { return statusMessage; }
            set
            {
                TestExistance();
                statusMessage = value + "\n" + statusMessage;
                if ( Instance.IsInvoking( "ClearStatusMessage" ) )
                    Instance.CancelInvoke( "ClearStatusMessage" );
                Instance.Invoke( "ClearStatusMessage", 3 );
            }
        }

        static void TestExistance()
        {
            if ( null == Instance )
            {
                GameObject G = new GameObject( "StatusMessages" );
                G.AddComponent( typeof( StatusMessage ) );
            }
        }

        public void ClearStatusMessage()
        {
            if ( Instance.IsInvoking( "ClearStatusMessage" ) )
                Instance.CancelInvoke( "ClearStatusMessage" );
            statusMessage = string.Empty;
        }

        void OnGUI() => Draw();
        
        public void Draw()
        {
            if ( Message != string.Empty )
            {
                guiStyle.fontSize = 20;
                guiStyle.normal.textColor = Color.white;
                GUI.depth = 0;
                GUI.skin = skin;
                float width = Screen.width,
                      height = Screen.height;
                GUI.Box( new Rect( 10f, height - 65f, width - 20, 60f ), "" );
                GUI.Label( new Rect( 20f, height - 62f, width - 40, 57f ), Message, guiStyle );
            }
        }

        void Awake()
        {
            if ( null != Instance && this != Instance )
                DestroyImmediate( gameObject );

            _instance = this;
            DontDestroyOnLoad( gameObject );
        }
    }
}
