using UnityEngine;
using UnityEngine.UI;

namespace MBS {
	abstract public class WUUGLoginLocalisation : MonoBehaviour {
	
        public const string localisation_pref_name = "wul_localisation";

        [Header("Localisation")]
        [SerializeField] protected GridLayoutGroup localisation_grid;
        [SerializeField] protected WULocalizationButton localisation_button_prefab;
        [SerializeField] protected WULocalisedText ltf; //localised text fields
        [SerializeField] protected Image localisation_button;

        protected WULLocalisation localisation => MBSLocalisationList.LoginLocal;
        protected int localization_change = -1;

        abstract public void ShowLoginMenuScreen();

        public void ConsiderNewLocalisation( int which )
        {
            localization_change = which;
            localisation_grid.BroadcastMessage( "SelectALanguage", which );
        }

        public void ChangeLocalization()
        {
            if ( localization_change != MBSLocalisationList.Login.Selected )
            {
                MBSLocalisationList.Login.SelectLocalisation( localization_change );
                DoLocalisation();
            }
            ShowLoginMenuScreen();
        }

        void SetLocalisedValue( Text field, string value )
        {
            if ( null != field )
                field.text = value;
        }

        virtual protected void DoLocalisation()
        {
            if ( null == localisation )
                return;

            if ( localization_change != MBSLocalisationList.Login.Selected )
                MBSLocalisationList.Login.SelectLocalisation( localization_change );

            WULLocalisation loc = localisation;
            if ( null != localisation_button )
                localisation_button.sprite = loc.LocalisationGraphic;
            PlayerPrefs.SetInt( localisation_pref_name, localization_change );

            SetLocalisedValue( ltf.main_login_btn, loc.MainLogin );
            SetLocalisedValue( ltf.main_register_btn, loc.MainRegister );
            SetLocalisedValue( ltf.main_reset_btn, loc.MainReset );
            SetLocalisedValue( ltf.post_resume_btn, loc.PostResume );
            SetLocalisedValue( ltf.post_details_btn, loc.PostDetails );
            SetLocalisedValue( ltf.post_pass_btn, loc.PostPass );
            SetLocalisedValue( ltf.post_logout_btn, loc.PostLogout );
            SetLocalisedValue( ltf.login_header, loc.LoginHeader );
            SetLocalisedValue( ltf.login_username_lbl, loc.LoginUsername );
            SetLocalisedValue( ltf.login_username_sample, loc.LoginUsernameSample );
            SetLocalisedValue( ltf.login_pass_lbl, loc.LoginPass );
            SetLocalisedValue( ltf.login_pass_sample, loc.LoginPassSample );
            SetLocalisedValue( ltf.login_remember_lbl, loc.LoginRemember );
            SetLocalisedValue( ltf.login_cancel_btn, loc.LoginCancel );
            SetLocalisedValue( ltf.login_accept_btn, loc.LoginAccept );
            SetLocalisedValue( ltf.register_header, loc.RegisterHeader );
            SetLocalisedValue( ltf.register_username_lbl, loc.RegisterUsername );
            SetLocalisedValue( ltf.register_username_sample, loc.RegisterUsernameSample );
            SetLocalisedValue( ltf.register_pass_lbl, loc.RegisterPass );
            SetLocalisedValue( ltf.register_pass_sample, loc.RegisterPassSample );
            SetLocalisedValue( ltf.register_verify_lbl, loc.RegisterVerify );
            SetLocalisedValue( ltf.register_verify_sample, loc.RegisterVerifySample );
            SetLocalisedValue( ltf.register_email_lbl, loc.RegisterEmail );
            SetLocalisedValue( ltf.register_email_sample, loc.RegisterEmailSample );
            SetLocalisedValue( ltf.register_cancel_btn, loc.RegisterCancel );
            SetLocalisedValue( ltf.register_accept_btn, loc.RegisterAccept );
            SetLocalisedValue( ltf.reset_header, loc.ResetHeader );
            SetLocalisedValue( ltf.reset_username_lbl, loc.ResetUsername );
            SetLocalisedValue( ltf.reset_username_sample, loc.ResetUsernameSample );
            SetLocalisedValue( ltf.reset_email_lbl, loc.ResetEmail );
            SetLocalisedValue( ltf.reset_email_sample, loc.ResetEmailSample );
            SetLocalisedValue( ltf.reset_cancel_btn, loc.ResetCancel );
            SetLocalisedValue( ltf.reset_accept_btn, loc.ResetAccept );
            SetLocalisedValue( ltf.change_header, loc.ChangeHeader );
            SetLocalisedValue( ltf.change_old_lbl, loc.ChangeOld );
            SetLocalisedValue( ltf.change_old_sample, loc.ChangeOldSample );
            SetLocalisedValue( ltf.change_new_lbl, loc.ChangeNew );
            SetLocalisedValue( ltf.change_new_sample, loc.ChangeNewSample );
            SetLocalisedValue( ltf.change_verify_lbl, loc.ChangeVerify );
            SetLocalisedValue( ltf.change_verify_sample, loc.ChangeVerifySample );
            SetLocalisedValue( ltf.change_cancel_btn, loc.ChangeCancel );
            SetLocalisedValue( ltf.change_accept_btn, loc.ChangeAccept );
            SetLocalisedValue( ltf.bio_header, loc.BioHeader );
            SetLocalisedValue( ltf.bio_tab0, loc.BioTab0 );
            SetLocalisedValue( ltf.bio_tab1, loc.BioTab1 );
            SetLocalisedValue( ltf.bio_tab2, loc.BioTab2 );
            SetLocalisedValue( ltf.bio_name_lbl, loc.BioName );
            SetLocalisedValue( ltf.bio_name_sample, loc.BioNameSample );
            SetLocalisedValue( ltf.bio_surname_lbl, loc.BioSurname );
            SetLocalisedValue( ltf.bio_surname_sample, loc.BioSurnameSample );
            SetLocalisedValue( ltf.bio_display_lbl, loc.BioDisplay );
            SetLocalisedValue( ltf.bio_display_sample, loc.BioDisplaySample );
            SetLocalisedValue( ltf.bio_nick_lbl, loc.BioNick );
            SetLocalisedValue( ltf.bio_nick_sample, loc.BioNickSample );
            SetLocalisedValue( ltf.bio_jabber_lbl, loc.BioJabber );
            SetLocalisedValue( ltf.bio_jabber_sample, loc.BioJabberSample );
            SetLocalisedValue( ltf.bio_yim_lbl, loc.BioYim );
            SetLocalisedValue( ltf.bio_yim_sample, loc.BioYimSample );
            SetLocalisedValue( ltf.bio_aim_lbl, loc.BioAim );
            SetLocalisedValue( ltf.bio_aim_sample, loc.BioAimSample );
            SetLocalisedValue( ltf.bio_email_lbl, loc.BioEmail );
            SetLocalisedValue( ltf.bio_email_sample, loc.BioEmailSample );
            SetLocalisedValue( ltf.bio_website_lbl, loc.BioWebsite );
            SetLocalisedValue( ltf.bio_website_sample, loc.BioWebsiteSample );
            SetLocalisedValue( ltf.bio_bio_lbl, loc.BioBio );
            SetLocalisedValue( ltf.bio_bio_sample, loc.BioBioSample );
            SetLocalisedValue( ltf.bio_cancel_btn, loc.BioCancel );
            SetLocalisedValue( ltf.bio_accept_btn, loc.BioAccept );
            SetLocalisedValue( ltf.locale_accept_btn, loc.LocalizeConfirm );
            SetLocalisedValue( ltf.locale_cancel_btn, loc.LocalizeCancel );
            SetLocalisedValue( ltf.scoring_header, loc.ScoringHeader );
            SetLocalisedValue( ltf.scoring_show, loc.ScoringShow );
            SetLocalisedValue( ltf.scoring_close, loc.ScoringHide );
            SetLocalisedValue( ltf.serial_buy, loc.SerialBuy );
            SetLocalisedValue( ltf.serial_cancel, loc.SerialCancel );
            SetLocalisedValue( ltf.serial_header, loc.SerialHeader );
            SetLocalisedValue( ltf.serial_label, loc.SerialLabel );
            SetLocalisedValue( ltf.serial_register, loc.SerialRegister );
            //SetLocalisedValue( ltf.shop_close, loc.RegisterCancel );
            //SetLocalisedValue( ltf.custom_close, loc.RegisterCancel );
        }

        [System.Serializable]
        public struct WULocalisedText
        {
            public Text
                main_login_btn,
                main_register_btn,
                main_reset_btn,
                post_resume_btn,
                post_details_btn,
                post_pass_btn,
                post_logout_btn,
                login_header,
                login_username_lbl,
                login_username_sample,
                login_pass_lbl,
                login_pass_sample,
                login_remember_lbl,
                login_cancel_btn,
                login_accept_btn,
                register_header,
                register_username_lbl,
                register_username_sample,
                register_pass_lbl,
                register_pass_sample,
                register_verify_lbl,
                register_verify_sample,
                register_email_lbl,
                register_email_sample,
                register_cancel_btn,
                register_accept_btn,
                reset_header,
                reset_username_lbl,
                reset_username_sample,
                reset_email_lbl,
                reset_email_sample,
                reset_cancel_btn,
                reset_accept_btn,
                change_header,
                change_old_lbl,
                change_old_sample,
                change_new_lbl,
                change_new_sample,
                change_verify_lbl,
                change_verify_sample,
                change_cancel_btn,
                change_accept_btn,
                bio_header,
                bio_tab0,
                bio_tab1,
                bio_tab2,
                bio_name_lbl,
                bio_name_sample,
                bio_surname_lbl,
                bio_surname_sample,
                bio_display_lbl,
                bio_display_sample,
                bio_nick_lbl,
                bio_nick_sample,
                bio_jabber_lbl,
                bio_jabber_sample,
                bio_yim_lbl,
                bio_yim_sample,
                bio_aim_lbl,
                bio_aim_sample,
                bio_email_lbl,
                bio_email_sample,
                bio_website_lbl,
                bio_website_sample,
                bio_bio_lbl,
                bio_bio_sample,
                bio_cancel_btn,
                bio_accept_btn,
                locale_accept_btn,
                locale_cancel_btn,
                scoring_header,
                scoring_show,
                scoring_close,
                shop_header,
                show_show,
                shop_close,
                custom_header,
                custom_show,
                custom_close,
                serial_header,
                serial_label,
                serial_register,
                serial_buy,
                serial_cancel
                ;
        }
    }
}
