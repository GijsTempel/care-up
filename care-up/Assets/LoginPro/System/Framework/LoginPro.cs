namespace LoginProAsset
{
    public static class LoginPro
    {
        // The Login manager to communicate securely with the server
        public static LoginPro_Manager Manager;

        // Session singleton (no need to initialize it it's made at first call)
        private static LoginPro_Session session = null;
        public static LoginPro_Session Session
        {
            get
            {
                if (LoginPro.session == null)
                    LoginPro.session = new LoginPro_Session();
                return LoginPro.session;
            }
        }
    }
}