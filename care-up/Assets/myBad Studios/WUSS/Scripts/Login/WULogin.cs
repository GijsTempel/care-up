using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

namespace MBS
{
    public enum WULStates { Dummy, LoginChallenge, AccountMenu, RegisterAccount, ValidateLoginStatus, LoginMenu, Logout, PasswordReset, PasswordChange, FetchAccountDetails, UpdateAccountDetails, AccountInfo, Count }
    public enum WULActions { DoLogin, SubmitRegistration, VerifyLogin, Logout, PasswordReset, PasswordChange, FetchAccountDetails, UpdateAccountDetails, FetchUserEmail, FetchAvailableGameInfo, CreateNewGame, TrustedLogin }
    public enum WULGravatarTypes { MysteryMan, Identicon, Monsterid, Wavatar, Retro, Blank }

    static public class WULogin
    {

        static public bool on_Login_Success = true;
        static public bool on_Registration_Success = true;
        static public bool on_Registration = false;
        static public bool ChangeAllToWhite = false;
        static public bool EmailTheSame = false;
        static public bool UsernameTheSame = false;
        static public bool UsernameNotValid = false;
        static public bool UserNotWithEmail = false;
        static public bool UserNotFound = false;
        static public bool characterCreated = false;

        static public bool justLoggedOff = false;

        //static private int FirstLogin = 1;
        //static private int SecondLogin = 2;

        #region RESPONSE DELEGATES
        static public Action<CML>
            onRegistered,
            onReset,
            onLoggedIn,
            onLoggedOut,
            onAccountInfoReceived,
            onInfoUpdated,
            onPasswordChanged,
            onGameListFetched,
            onNewGameCreated;

        static public Action<CMLData>
            onLoginFailed,
            onLogoutFailed,
            onRegistrationFailed,
            onResetFailed,
            onAccountInfoFetchFailed,
            onInfoUpdateFail,
            onPasswordChangeFail,
            onGameListFetchFailed,
            onNewGameCreationFailed;

        static public Action<Texture2D>
            __InternalOnProfileImageReceived;

        static public Action<Texture2D>
            onGravatarTextureFetched;

        static public Action<Sprite>
            onGravatarSpriteFetched;

        static public Action
            onResumeGame;
        #endregion

        #region STATIC PUBLIC FIELDS & PROPERTIES
        static WULoginPrefs __data => WULoginPrefs.Instance;

        static public CMLData fetched_info = null;
        static public Texture2D user_gravatar;
        static public Sprite user_gravatar_sprite;
        static public int UID { get { return (null == fetched_info) ? 0 : fetched_info.Int("uid"); } set { if (null == fetched_info) return; fetched_info.Seti("uid", value); } }
        static public string display_name { get { return (null == fetched_info) ? "" : fetched_info.String("display_name"); } set { if (null == fetched_info) return; fetched_info.Set("display_name", value); } }
        static public string nickname { get { return (null == fetched_info) ? "" : fetched_info.String("nickname"); } set { if (null == fetched_info) return; fetched_info.Set("nickname", value); } }
        static public string username { get { return (null == fetched_info) ? "" : fetched_info.String("user_login"); } set { if (null == fetched_info) return; fetched_info.Set("user_login", value); } }
        static public string email { get { return (null == fetched_info) ? "" : fetched_info.String("user_email"); } set { if (null == fetched_info) return; fetched_info.Set("user_email", value); } }
        static public string website { get { return (null == fetched_info) ? "" : fetched_info.String("user_url"); } set { if (null == fetched_info) return; fetched_info.Set("user_url", value); } }
        static public string registration_date { get { return (null == fetched_info) ? "" : fetched_info.String("user_registered"); } set { if (null == fetched_info) return; fetched_info.Set("user_registered", value); } }
        static public string roles { get { return (null == fetched_info) ? "" : fetched_info.String("roles"); } set { if (null == fetched_info) return; fetched_info.Set("roles", value); } }
        static public bool logged_in = false;

        public const string GamesListPrefName = "WUSSGAMESLIST";
        static public CML AvailableGames;

#if WUSKU
        static public bool RequireSerialForLogin { get; private set; }
        static public bool HasSerial { get { return (null == fetched_info) ? false : fetched_info.Bool("registered"); } set { fetched_info?.Seti("registered", value ? 1 : 0); } }
        static public string SerialNumber { get { return (null == fetched_info) ? "" : fetched_info.String("serial"); } set { fetched_info?.Set("serial", value); } }
#endif

#if WUS
        static public int highscore { get { return (null == fetched_info) ? 0 : fetched_info.Int(HighScoresField); } set { if (null == fetched_info) return; fetched_info.Seti(HighScoresField, value); } }
        static string HighScoresField => $"{WPServer.GameID}_HighScore";
#endif

#if WUM
        static public string[] CurrencyNames { get; private set; }
        static public string CurrencyString(string currency_name) => $"{WPServer.GameID}_currency_{currency_name}";
        static public string CurrencyString(int index) => index < __data.Currencies.Length ? __data.Currencies[index] : string.Empty;

        static public int Cash(int index) => Cash(index >= CurrencyNames.Length ? string.Empty : CurrencyNames[index]);
        static public int Cash(string currency_name)
        {
            currency_name = currency_name.ToLower().Trim();
            return fetched_info?.Int(CurrencyString(currency_name)) ?? 0;
        }
#endif

        #endregion

        #region PRIVATE CODE
        const string filepath = "wuss_login/unity_functions.php";
        public const string LOGINConstant = "LOGIN";
        static string FieldsToFetch
        {
            get
            {
                string result = "";
                if (null == __data)
                    return "user_id,user_email";

                foreach (string meta in __data.MetaInfo)
                    if (meta.Trim() != string.Empty)
                        result += $",{meta}";

#if WUM
                List<string> cur = new List<string>();
                for (int i = 0; i < __data.Currencies.Length; i++)
                {
                    __data.Currencies[i] = __data.Currencies[i].ToLower().Trim();
                    if (!cur.Contains(__data.Currencies[i]))
                    {
                        cur.Add(__data.Currencies[i]);
                        result += $",{CurrencyString(__data.Currencies[i])}";
                    }
                }
                if (!cur.Contains("points"))
                {
                    result += $",{WPServer.GameID}_currency_points";
                    cur.Insert(0, CurrencyString("points"));
                }
                CurrencyNames = cur.ToArray();
#endif

#if WUS
                if (__data.FetchHighscore && result.IndexOf(HighScoresField) < 0)
                    result += $",{HighScoresField}";
#endif

#if WUSKU
                if (__data.CheckForSerial)
                    result += $",SERIALCHECK";
                if (__data.FetchSerial)
                    result += $",SERIALFETCH";
                RequireSerialForLogin = __data.RequireSerial;
                SerialNumber = string.Empty;
#endif

                result += ",user_id";
                result += ",user_email";
                if (__data.FetchUsername)
                    result += ",user_login";
                if (__data.FetchDisplayName)
                    result += ",display_name";
                if (__data.FetchURL)
                    result += ",user_url";
                if (__data.FetchRegistration)
                    result += ",user_registered";
                if (__data.FetchRoles)
                    result += ",roles";
                if (result[0] == ',')
                    result = result.Substring(1);
                return result;
            }
        }

        #endregion

        static public void RegisterAccount(CMLData fields) => WPServer.ContactServer(WULActions.SubmitRegistration, filepath, LOGINConstant, fields, onRegistered, onRegistrationFailed);
        static public void ResetPassword(CMLData fields) => WPServer.ContactServer(WULActions.PasswordReset, filepath, LOGINConstant, fields, onReset, onResetFailed);
        static public void ChangePassword(CMLData fields) => WPServer.ContactServer(WULActions.PasswordChange, filepath, LOGINConstant, fields, onPasswordChanged, onPasswordChangeFail);
        static public void LogOut() { justLoggedOff = true; WPServer.ContactServer(WULActions.Logout, filepath, LOGINConstant, null, __onLogOutSuccess, onLogoutFailed ); }
        static public void FetchPersonalInfo() => WPServer.ContactServer( WULActions.FetchAccountDetails, filepath, LOGINConstant, null, onAccountInfoReceived, onAccountInfoFetchFailed );
        static public void FetchAvailableGameInfo() => WPServer.ContactServer( WULActions.FetchAvailableGameInfo, filepath, LOGINConstant, null, __onGameListFetched, __onGameListFetchFailed );

        static public void AttemptAutoLogin()
        {
            // stop goddamn autologging after recent logOff
            if (justLoggedOff)
                return;

            CMLData data = new CMLData();
            data.Set( "wul_fields", FieldsToFetch );
            WPServer.ContactServer( WULActions.VerifyLogin, filepath, LOGINConstant, data, __onLoginSuccess );
        }

        static public void AttemptToLogin( CMLData fields )
        {
            // WTF UNITY
            UnityWebRequest.ClearCookieCache();

            WUCookie.ClearCookie();
            WUCookie.StoreCookie();
            fields.Set( "wul_fields", FieldsToFetch );
            WPServer.ContactServer( WULActions.DoLogin, filepath, LOGINConstant, fields, __onLoginSuccess, onLoginFailed );
        }
        


        static public void AttemptTrustedLogin( CMLData fields )
        {
            WUCookie.ClearCookie();
            WUCookie.StoreCookie();
            fields.Set( "wul_fields", FieldsToFetch );
            WPServer.ContactServer( WULActions.TrustedLogin, filepath, LOGINConstant, fields, __onLoginSuccess, onLoginFailed );
        }

        static public void FetchProfileImage( Action<Texture2D> response ) => FetchProfileImage( response, WULGravatarTypes.Identicon );
        static public void FetchProfileImage( Action<Texture2D> response, WULGravatarTypes gravatar_type )
        {
            __InternalOnProfileImageReceived = response;

            if ( null == __InternalOnProfileImageReceived )
                return;

            CMLData data = new CMLData();
            data.Set( "gravatar_type", gravatar_type.ToString() );
            WPServer.ContactServer( WULActions.FetchUserEmail, filepath, LOGINConstant, data, onProfileImageFetched );
        }

        static public void UpdatePersonalInfo( CMLData fields )
        {
            if ( fields.String( "descr" ) != string.Empty )
                fields.Set( "descr", Encoder.Base64Encode( fields.String( "descr" ) ) );
            WPServer.ContactServer( WULActions.UpdateAccountDetails, filepath, LOGINConstant, fields, onInfoUpdated, onInfoUpdateFail );
        }

        static public void StoreFetchedInfo( CML data )
        {
            if ( null != fetched_info )
                return;
            fetched_info = (CMLData)data.GetFirstNodeOfType( LOGINConstant ).Copy( CMLCopyMode.no_id );
            fetched_info.Remove( "success" );
        }

        static void __onLoginSuccess( CML data )
        {
            StoreFetchedInfo ( data );
            logged_in = true;
            user_gravatar = null;
            user_gravatar_sprite = null;

            onLoggedIn?.Invoke( data );
            if ( email != string.Empty && null != __data && __data.FetchGravatar )
                FetchProfileImage( __SetProfileImage, __data.GravatarType );
        }

        static void __SetProfileImage( Texture2D image )
        {
            user_gravatar = image;
            user_gravatar_sprite =
                Sprite.Create(
                    user_gravatar,
                    new Rect( 0, 0, user_gravatar.width, user_gravatar.height ),
                    new Vector2( 0.5f, 0.5f ),
                    100f );

            onGravatarTextureFetched?.Invoke( user_gravatar );
            onGravatarSpriteFetched?.Invoke( user_gravatar_sprite );
        }

        static public void __onLogOutSuccess( CML data )
        {
            on_Login_Success = true;
            user_gravatar = null;
            user_gravatar_sprite = null;
            logged_in = false;
            fetched_info = null;
            onLoggedOut?.Invoke( data );
        }

        static public void onProfileImageFetched( CML data )
        {
            CMLData response = data [0];

            string gravatar_type_name = response.String( "gravatar_type" );
            WULGravatarTypes type = WULGravatarTypes.Blank;
            for ( WULGravatarTypes t = WULGravatarTypes.MysteryMan; t < WULGravatarTypes.Blank; t++ )
            {
                if ( t.ToString().ToLower() == gravatar_type_name.ToLower() )
                    type = t;
            }
            __data.__internalOnProfileImageFetched( response.String( "gravatar" ), type );
        }

        static public void ContactGravatar( string gravatar, WULGravatarTypes gravatar_type = WULGravatarTypes.Identicon )
            => __data.ContactGravatar( gravatar, __InternalOnProfileImageReceived, gravatar_type );

        static public void LoadAvailableGames()
        {
            string saved = PlayerPrefs.GetString( GamesListPrefName, string.Empty );
            if ( string.IsNullOrEmpty( saved ) )
            {
                AvailableGames = new CML();
                AvailableGames.AddNode( "GAMES" );
                AvailableGames.Save( GamesListPrefName );
            }
            else
            {
                AvailableGames = new CML();
                AvailableGames.Parse( saved );
            }
        }

        static public void CreateNewGame( string game_name, string description, string short_description, CMLData other = null )
        {
            if ( null == other )
                other = new CMLData();
            other.Set( "post_title", game_name );
            other.Set( "post_content", Encoder.Base64Encode( description ) );
            other.Set( "post_excerpt", Encoder.Base64Encode( short_description ) );
            WPServer.ContactServer( WULActions.CreateNewGame, filepath, LOGINConstant, other, __onNewGameCreated, onNewGameCreationFailed );
        }

        static public void __onNewGameCreated( CML response )
        {
            AvailableGames = response;
            AvailableGames.Save( GamesListPrefName );
            onNewGameCreated?.Invoke( response );
        }

        static public void __onGameListFetched( CML response )
        {
            AvailableGames = response;
            AvailableGames.Save( GamesListPrefName );
            onGameListFetched?.Invoke( response );
        }
        static public void __onGameListFetchFailed( CMLData response )
        {
            AvailableGames = new CML();
            AvailableGames.AddNode( "LOGIN" );
            onGameListFetchFailed?.Invoke( response );
        }

    }
}
