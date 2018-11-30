using UnityEngine;

namespace MBS
{
    [CreateAssetMenu (fileName ="WULOGINLocalData", menuName = "WULogin Localisation Data", order = 50)]
    public class WULLocalisation : MBSLocalisationBase
    {
        #region shared fields
        [Header ("Shared text")]
        [SerializeField]
        protected string cancel_button = "Cancel";
        [SerializeField]
        protected
        string
            change_button = "Change",
            username_label = "Username",
            password_label = "Password",
            username_sample_text = "username",
            password_sample_text = "password",
            email = "Email",
            email_sample_text = "mybad@me.com";
        #endregion

        #region scoring
        //This is not enough to warrant an entire class so I am adding it here instead
        [Header("High scores")]
        [SerializeField] string high_scores = "High scores";

        public string ScoringHeader => high_scores;
        public string ScoringShow => high_scores;
        public string ScoringHide => cancel_button;
        #endregion 

        #region login screen
        [Header( "Login screen" )]
        [SerializeField] string login_header = "Account Login";
        [SerializeField] string login_remember_me = "Remember me";
        [SerializeField] string login_confirm_button = "Login";

        public string LoginHeader => login_header;
        public string LoginUsername => username_label;
        public string LoginUsernameSample => username_sample_text;
        public string LoginPass => password_label;
        public string LoginPassSample => password_sample_text;
        public string LoginRemember => login_remember_me;
        public string LoginCancel => cancel_button;
        public string LoginAccept => login_confirm_button;
        #endregion

        #region main menu
        [Header( "Main menu" )]
        [SerializeField] string login_button = "Login";
        [SerializeField]
        string
//            sign_up_button = "Register",
            pass_reset_button = "Password Reset";

        public string MainLogin => login_button;
        public string MainRegister => register_button;
        public string MainReset => pass_reset_button;
        #endregion

        #region registration screen
        [Header( "Registration screen" )]
        [SerializeField] string register_header = "Create new account";
        [SerializeField]
        string
            register_verify = "Verify password",
            register_verify_sample_text = "verify password",
            register_button = "Register";

        public string RegisterHeader => register_header;
        public string RegisterUsername => username_label;
        public string RegisterUsernameSample => username_sample_text;
        public string RegisterPass => password_label;
        public string RegisterPassSample => password_sample_text;
        public string RegisterVerify => register_verify;
        public string RegisterVerifySample => register_verify_sample_text;
        public string RegisterEmail => email;
        public string RegisterEmailSample => email_sample_text;
        public string RegisterCancel => cancel_button;
        public string RegisterAccept => register_button;
        #endregion

        #region post login menu
        [Header( "Post login menu" )]
        [SerializeField] string resume_button = "Resume game...";
        [SerializeField]
        string
            play_button = "Play",
            start_button = "Start",
            retry_button = "Play again",
            my_details_button = "My details...",
            change_password_button = "Change password...",
            logout_button = "Log out";

        public string PostResume => resume_button;
        public string PostPlay => play_button;
        public string PostStart => start_button;
        public string PostRetry => retry_button;
        public string PostDetails => my_details_button;
        public string PostPass => change_password_button;
        public string PostLogout => logout_button;
        #endregion

        #region localization
        [Header( "Localization screen" )]
        [SerializeField] string localize_accept_button = "Confirm";
        [SerializeField] string localize_cancel_button = "Cancel";

        public string LocalizeConfirm => localize_accept_button;
        public string LocalizeCancel => localize_cancel_button;
        #endregion

        #region password reset
        [Header( "Password reset" )]
        [SerializeField] string pass_reset_header = "Reset Password";
        [SerializeField] string pass_reset_email = "or Email";

        public string ResetHeader => pass_reset_header;
        public string ResetUsername => username_label;
        public string ResetUsernameSample => username_sample_text;
        public string ResetEmail => pass_reset_email;
        public string ResetEmailSample => email_sample_text;
        public string ResetCancel => cancel_button;
        public string ResetAccept => change_button;
        #endregion

        #region password change
        [Header( "Password change" )]
        [SerializeField] string pass_change_header = "Change Password";
        [SerializeField]
        string
            pass_change_old = "Old password",
            pass_change_new = "New password",
            pass_change_verify = "Verify new",
            pass_change_old_sample_text = "old password",
            pass_change_new_sample_text = "new password",
            pass_change_verify_sample_text = "new password";

        public string ChangeHeader => pass_change_header;
        public string ChangeOld => pass_change_old;
        public string ChangeOldSample => pass_change_old_sample_text;
        public string ChangeNew => pass_change_new;
        public string ChangeNewSample => pass_change_new_sample_text;
        public string ChangeVerify => pass_change_verify;
        public string ChangeVerifySample => pass_change_verify_sample_text;
        public string ChangeCancel => cancel_button;
        public string ChangeAccept => change_button;
        #endregion

        #region personal info
        [Header( "Personal info" )]
        [SerializeField] string personal_info_header = "My account info";
        [SerializeField]
        string
            personal_tab1_button = "Names",
            personal_tab2_button = "IM",
            personal_tab3_button = "Misc",
            personal_name = "First name",
            personal_surname = "Surname",
            personal_display_name = "Display name",
            personal_nickname = "Nickname",
            personal_aol = "AIM",
            personal_yim = "Yim",
            personal_jabber = "Jabber",
            personal_url = "Website URL",
            personal_bio = "About me",
            personal_name_sample_text = "John",
            personal_surname_sample_text = "Doe",
            personal_display_name_sample_text = "Maverick",
            personal_nickname_sample_text = "Warlord711",
            personal_aol_sample_text = "mybad@me.com",
            personal_yim_sample_text = "yim",
            personal_jabber_sample_text = "Jabber93",
            personal_url_sample_text = "http://www.mybadstudios.com",
            personal_bio_sample_text = "About me...",
            personal_update_button = "Update";

        public string BioHeader => personal_info_header;
        public string BioTab0 => personal_tab1_button;
        public string BioTab1 => personal_tab2_button;
        public string BioTab2 => personal_tab3_button;
        public string BioName => personal_name;
        public string BioNameSample => personal_name_sample_text;
        public string BioSurname => personal_surname;
        public string BioSurnameSample => personal_surname_sample_text;
        public string BioDisplay => personal_display_name;
        public string BioDisplaySample => personal_display_name_sample_text;
        public string BioNick => personal_nickname;
        public string BioNickSample => personal_nickname_sample_text;
        public string BioJabber => personal_jabber;
        public string BioJabberSample => personal_jabber_sample_text;
        public string BioYim => personal_yim;
        public string BioYimSample => personal_yim_sample_text;
        public string BioAim => personal_aol;
        public string BioAimSample => personal_aol_sample_text;
        public string BioEmail => email;
        public string BioEmailSample => email_sample_text;
        public string BioWebsite => personal_url;
        public string BioWebsiteSample => personal_url_sample_text;
        public string BioBio => personal_bio;
        public string BioBioSample => personal_bio_sample_text;
        public string BioCancel => cancel_button;
        public string BioAccept => personal_update_button;
        #endregion

        #region serial registration
        [Header( "Serial number" )]
        [SerializeField]
        string serial_header = "Register game";
        [SerializeField]
        string
            serial_label = "Serial number",
            serial_register = "Register",
            serial_buy = "Purchase",
            purchase_success_header = "Registration successful",
            purchase_success_message = "Your game is now fully licensed";

        public string SerialHeader => serial_header;
        public string SerialLabel => serial_label;
        public string SerialRegister => serial_register;
        public string SerialBuy => serial_buy;
        public string SerialCancel => cancel_button;
        public string RegistrationSuccessHeader => purchase_success_header;
        public string RegistrationSuccessMessage => purchase_success_message;

        #endregion

        #region errors
        [Header ("Error Messages")]
        [SerializeField]
        string invalid_email = "Please check email address: Invalid email format detected";

        [SerializeField]
        string
            all_fields_required = "All fields are required...",
            failed_verification = "Password mismatch",
            email_required = "Email is a required field",
            provide_current_password = "Please provide your current password",
            provide_new_password = "Please provide a new password",
            need_email_or_username = "Please enter either your username or your email to continue";

        public string InvalidEmail => invalid_email;
        public string AllFieldsRequired => all_fields_required;
        public string FailedVerification => failed_verification;
        public string EmailRequired => email_required;
        public string ProvideCurrentPassword => provide_current_password;
        public string ProvideNewPassword => provide_new_password;
        public string NeedEmailOrUsername => need_email_or_username;
        #endregion

    }
}