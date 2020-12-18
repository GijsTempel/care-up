using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

namespace MBS
{
    public enum WPServerState { None, Contacting }

    public class WPServer : MonoBehaviour
    {

        static WPServer _instance;
        static public WPServer Instance
        {
            get
            {
                if ( null == _instance )
                    _instance = FindObjectOfType<WPServer>();
                if ( null == _instance )
                {
                    GameObject go = new GameObject( "WPServer" );
                    _instance = go.AddComponent<WPServer>();
                }
                return _instance;
            }
        }

        public void setUseOnlineURL(bool value)
        {
            use_online_url = value;
            if (value)
                WPServer.GameID = 2269;
            else
                WPServer.GameID = 2269;
        }

        public class WPServerErrorException : Exception { public WPServerErrorException( string message ) : base( message ) { } }

        public enum eWussServerContactType { GET, POST }

        static public WPServerState ServerState => server_state;
        static public WPServerState server_state = WPServerState.None;

        static void SetServerState( WPServerState value, bool update_arc = true )
        {
            if ( value == WPServerState.Contacting )
                ARC += update_arc ? 1 : 0;
            else
                if ( ARC > 0 )
                ARC -= update_arc ? 1 : 0;
            if ( server_state == value )
                return;

            if ( value == WPServerState.Contacting || ARC == 0 )
            {
                server_state = value;
                if ( Application.isPlaying )
                    OnServerStateChange?.Invoke( value );
            }
        }

        static int ARC = 0;

        static public Action<WPServerState> OnServerStateChange;

        [SerializeField] eWussServerContactType post_method = default(eWussServerContactType);
        //[SerializeField, HideInInspector, WPGameId] int game_id = 2269;
        [SerializeField] bool use_online_url = true;
        [SerializeField]
        string
        online_url = "http://www.mysite.com",
        offline_url = "http://localhost";

        static public int GameID = 2269;// => Instance.game_id;

        public eWussServerContactType PostMethod => post_method;

        public string SelectedURL => ( use_online_url ? online_url : offline_url ).Trim();
        string URL( string filename ) => $"{SelectedURL}/wp-content/plugins/{filename}";

        [Header ("Optional")]
        public string security;

        [SerializeField] bool
            do_not_destroy_this = false,
            do_not_destroy_canvas = false,
            print_response_headers = false,
            show_on_screen_errors = false;

        void Awake()
        {
            if ( null != _instance && _instance != this )
            {
                Destroy( this );
                return;
            }
            if ( do_not_destroy_this )
                DontDestroyOnLoad( gameObject );

            if ( do_not_destroy_canvas )
            {
                Canvas c = GetComponent<Canvas>();
                if ( null == c )
                    c = GetComponentInParent<Canvas>();
                if ( null != c )
                    DontDestroyOnLoad( c.gameObject );
            }
            if ( null == _instance )
                _instance = this;
        }

        static public void ContactServer(
            object action,
            string filepath,
            string wuss_kit,
            CMLData data = null,
            Action<CML> response = null,
            Action<CMLData> failedresponse = null
        ) => ContactServer( action.ToString(), filepath, wuss_kit, data, response, failedresponse );

        static public void ContactServer(
            string action,
            string filepath,
            string wuss_kit,
            CMLData data = null,
            Action<CML> response = null,
            Action<CMLData> failedresponse = null
        ) => Instance.StartCoroutine( CallServer( action, filepath, wuss_kit, data, response, failedresponse ) );

        
        public class JsonHelper
        {
            public static T[] getJsonArray<T>(string json)
            {
                string newJson = "{ \"array\": " + json + "}";
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
                return wrapper.array;
            }
        
            [System.Serializable]
            private class Wrapper<T>
            {
                public T[] array = default(T[]);
            }
        }

        static public void RequestPurchases(
            int UserID) => Instance.StartCoroutine( RequestScenePurchases( UserID ) );

        static public void _CheckBundleVersion() => Instance.StartCoroutine(CheckBundleVersion());


        static public IEnumerator CheckBundleVersion()
        {
            string url = CAServer.BundleAddress + "/" + Application.version + "/version.txt";
            Debug.Log(url);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {

                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {

                }
                else if (webRequest.downloadHandler.text != "")
                {
                    int bundleVersion = 0;
                    int.TryParse(webRequest.downloadHandler.text, out bundleVersion);
                    if (bundleVersion != 0)
                    {
                        PlayerPrefsManager playerPrefsManager = GameObject.FindObjectOfType<PlayerPrefsManager>();
                        if (playerPrefsManager != null)
                        {
                            if (bundleVersion != playerPrefsManager.BundleVersion)
                            {
                                Caching.ClearCache();
                            }
                            playerPrefsManager.BundleVersion = bundleVersion;
                        }
                    }
                }
                else
                {

                }

            }
        }

        static public IEnumerator RequestScenePurchases(int UserID)
        {
            string url = Instance.SelectedURL + "/request_purchases.php?user_id=" + UserID.ToString();
#if UNITY_WEBGL
            url = "https://leren.careup.online/request_purchases.php?user_id=" + UserID.ToString();
#endif
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                string[] pages = url.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else if (webRequest.downloadHandler.text != "")
                {
                    Debug.Log(webRequest.downloadHandler.text);
                    PlayerPrefsManager.ClearSKU();
                    PlayerPrefsManager.PurchasedScetesData[] sceteStoreData; 
                    sceteStoreData = JsonHelper.getJsonArray<PlayerPrefsManager.PurchasedScetesData>(webRequest.downloadHandler.text); 
                    foreach(PlayerPrefsManager.PurchasedScetesData ssd in sceteStoreData)
                        PlayerPrefsManager.AddSKU(ssd.product_name);
                }
                else
                    PlayerPrefsManager.ClearSKU();

                if (GameObject.FindObjectOfType<LevelSelectionScene_UI>() != null)
                    GameObject.FindObjectOfType<LevelSelectionScene_UI>().RefrashSceneSelectionButtons();
            }
        }

        static public IEnumerator CallServer( string action, string filepath, string wuss_kit, CMLData data, Action<CML> response, Action<CMLData> failedresponse )
        {
            CMLData error = new CMLData();
            bool wait_for_response = !( null == response && null == failedresponse );

            data = _prepareData( data, action, wuss_kit );
            WWWForm f = _prepareForm( data );
            string get = _GET_Url( data );

            SetServerState( WPServerState.Contacting, wait_for_response );

            string _uri = Instance.URL( filepath ) + ( Instance.post_method == eWussServerContactType.GET ? get : string.Empty );
            UnityWebRequest w = Instance.post_method == eWussServerContactType.GET ? UnityWebRequest.Get( _uri ) : UnityWebRequest.Post( _uri, f );
            
            switch ( Application.platform )
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WebGLPlayer:
                    break;

                default:
                    Dictionary<string, string> headers = WUCookie.Cookie;
                    foreach ( var kvp in headers )
                        w.SetRequestHeader( kvp.Key, kvp.Value );
                    break;
            }
            
            yield return w.SendWebRequest();

            SetServerState( WPServerState.None, wait_for_response );

            try
            {
                if ( !string.IsNullOrEmpty( w.error ) )
                    throw new WPServerErrorException( w.error );

                HandleResponse( action, data, w, error, response, failedresponse );
            }
            catch ( WPServerErrorException e )
            {
                WULogin.ChangeAllToWhite = true;
                error.Set ("message", $"WPServer error: {e.Message}" );

                if ( Instance.show_on_screen_errors )
                    StatusMessage.Message = e.Message;

                if (e.Message == "Failed to create account: Sorry, dit e-mailadres is al in gebruik!") {
                    WULogin.EmailTheSame = true;
                    WULogin.on_Registration_Success = false;
                }
                if (e.Message == "Username is not valid. Please select another")
                {
                    WULogin.UsernameNotValid = true;
                    WULogin.on_Registration_Success = false;
                }

                if (e.Message == "Username already in use") {
                    WULogin.UsernameTheSame = true;
                    WULogin.on_Registration_Success = false;
                }

                if (e.Message == "incorrect password") {
                    WULogin.on_Login_Success = false;
                }

                if (e.Message == "No user found matching asdf@outlook.com") {
                    WULogin.UserNotWithEmail = true;
                }

                if (e.Message == "No user found with username or email matching asdf") {
                    WULogin.UserNotWithEmail = true;
                }

                //if (Instance.show_on_screen_errors)
                failedresponse?.Invoke (error);

                    if (e.Message == "Kan de server niet bereiken. Neem contact op via support@careup.nl")
                    GameObject.Find("NoInternet").GetComponent<Animator>().SetTrigger("pop");
                Debug.LogWarning ($"{e.Message}\n{w.downloadHandler.text}" );
            }
        }

        static CMLData _prepareData( CMLData data, string action, string wuss_kit )
        {
            if ( null == data )
                data = new CMLData();

            if ( data.String( "gid" ) == string.Empty || data.Int( "gid" ) < 0 )
                data.Seti( "gid", GameID );

            data.Seti( "unity", 1 );
            data.Set( "action", action );
            data.Set( "wuss", wuss_kit );

            if ( !string.IsNullOrEmpty( Instance.security ) )
            {
                string token = Instance.security.Trim();
                if ( !string.IsNullOrEmpty( Instance.security ) )
                {
                    foreach ( string s in data.Keys )
                        token += data [s];
                    data.Set( "token", Encoder.MD5( token ) );
                }
            }

            return data;
        }

        static WWWForm _prepareForm( CMLData data )
        {
            WWWForm f = new WWWForm();

            if ( Instance.post_method == eWussServerContactType.POST )
                foreach ( string s in data.Keys )
                    f.AddField( s, data.String( s ) );

            return f;
        }

        static string _GET_Url( CMLData data )
        {
            string get = string.Empty;
            if ( Instance.post_method == eWussServerContactType.GET )
            {
                foreach ( string s in data.Keys )
                    get += "&" + s + "=" + data.String( s );

                get = "?" + get.Substring( 1 );
            }
            return get;
        }

        static void HandleResponse( string action, CMLData data, UnityWebRequest w, CMLData error, Action<CML> response, Action<CMLData> failedresponse )
        {
            string result_string = string.Empty;
            int
                datastart = w.downloadHandler.text.IndexOf( "<CML>" ) + 5,
                dataend = w.downloadHandler.text.IndexOf( "</CML>" );

            if ( datastart < 5 || dataend < datastart )
                throw new WPServerErrorException( "No data returned from the server" );

            result_string = w.downloadHandler.text.Substring( datastart, dataend - datastart );

            int warning_index = result_string.IndexOf( "<br />" );
            if ( warning_index > 0 )
                throw new WPServerErrorException( $"Excessive server response: {result_string.Substring( warning_index + 6 )}" );

            if ( result_string.Trim() != string.Empty )
                result_string = Encoder.Base64Decode( result_string );

            CML results = new CML();
            results.Parse( result_string );

            if ( Instance.print_response_headers )
            {
                var response_headers = w.GetResponseHeaders();
                foreach ( var x in response_headers )
                    Debug.Log( x.Key + " = " + x.Value + " : " + x.GetType() );
            }
            if ( action == WULActions.DoLogin.ToString() || action == WULActions.VerifyLogin.ToString() || action == WULActions.TrustedLogin.ToString() )
            {
                WUCookie.ExtractCookie( w );
            }
            else
            {
                if ( action == WULActions.Logout.ToString() )
                {
                    WUCookie.ClearCookie();
                    WUCookie.StoreCookie();
                }
            }

            if ( results.Count == 0 )
                throw new WPServerErrorException( "No data returned from the server" );

#if WUM
       // WUMoney.TestForTapJoyAwards( results );
#endif
            CMLData any_errors = results.NodeWithField( "success", "false" );
            if ( null != any_errors )
            {
                if ( action != WULActions.VerifyLogin.ToString() && null != failedresponse )
                    throw new WPServerErrorException( any_errors.String( "message" ) );
                else
                    return;
            }

            //if there were no errors, pass the results along to the response delegate, if any
            if ( action == WULActions.FetchUserEmail.ToString() )
            {
                int i = results.GetFirstNodeOfTypei( WULogin.LOGINConstant );
                results [i].Set( "gravatar_type", data.String( "gravatar_type" ) );
            }
            response?.Invoke( results );
        }
    }
}