using System;

/// <summary>
/// User session is the class where all user options are saved
/// </summary>
namespace LoginProAsset
{
    public class LoginPro_Session
    {
        public bool LoggedIn = false;                               // Is the user logged in or not ?
        public string Username = "";                                // The username used for connection
        public string Password = "";                                // The password used for connection
        public string Mail = "";                                    // The mail of the account
        public string RegistrationDate = "";                        // The date of the registration
        public DateTime CurrentConnectionDate = DateTime.Now;       // The date of the current connection
        public string PreviousConnectionDate = "";                  // The date of the previous connection
        public double MinutesPlayed = 0;                            // The number of minutes played
        public string TimePlayedText = "0 hours and 0 minutes";     // The (in hours ad minutes) played

        // Multiplayer
        public string serverJoinedId = "";                          // The id of the server the player is connected to
        public bool iAmServerHost = false;                          // Flag to indicate if the player is the host of the lobby

        public string Session_id = "";                              // The SID sent to the server to reload session information concerning the user (like session_token, AES keys, ...)
        public string Session_token = "";                           // The session 'secret key' checked both sides to make sure the communication has been made between the real user and the real server (generated everytime a session is started)

        // User role (add some more in LoginPro_UserRole if you want to)
        public LoginPro_UserRole Role = LoginPro_UserRole.Player;   // The role of the user (Player, Admin, GameMaster, ...)

        // Security
        public string PublicModulus = "";                           // RSA keys (public only)
        public string PublicExponent = "";                          // RSA exponent (read from the public certificate too)
        public string AES_Key = "";                                 // The AES key of the session
        public string AES_IV = "";                                  // The AES initial vector of the session

        public LoginPro_Session()
        {
            this.ClearSession();
        }

        // Reinitialization method
        public void ClearSession()
        {
            this.LoggedIn = false;
            this.Username = "";
            this.Password = "";
            this.Mail = "";
            this.RegistrationDate = "";
            this.CurrentConnectionDate = DateTime.Now;
            this.PreviousConnectionDate = "";
            this.MinutesPlayed = 0;
            this.TimePlayedText = "0 hours and 0 minutes";
            this.Session_id = "";
            this.Session_token = "";
            this.Role = LoginPro_UserRole.Player;
            this.PublicModulus = "";
            this.PublicExponent = "";
            this.AES_Key = "";
            this.AES_IV = "";
        }
    }
}