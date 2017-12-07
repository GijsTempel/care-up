using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace LoginProAsset
{
    public class LoginPro_Report : MonoBehaviour
    {
        public InputField MessageField;

        //public UIAnimation_Alert MessageToShowOnResult;
        //public UIAnimation AnimationToLaunchOnResult;
      //  public UIAnimation AnimationHideWindow;
       // public UIAnimation AnimationToStopOnResult;

        public PlaceUIElement ReportWindow;
        public PlaceUIElement ReportButton;

        public LoginPro_ShowReport ShowReport;

        void Start()
        {
            if (this.MessageField == null)
                Debug.LogError("Please set the messageField to LoginPro_Report.");
        }

        public void Report()
        {
            StartCoroutine(LaunchReport());
        }

        private IEnumerator LaunchReport()
        {
            // Check if the report has been taken
            if (LoginPro_ShowReport.ScreenshotTaken == "")
            {
                Debug.LogError("No screenshot had been captured when opening the report menu.");
//                if (this.AnimationToStopOnResult != null)
  //                  this.AnimationToStopOnResult.Stop();
 //               this.MessageToShowOnResult.Show("No screenshot has been taken.", 3);
                yield break;
            }

            // Send report
            LoginPro.Manager.Report(MessageField.text, LoginPro_ShowReport.ScreenshotTaken, Success, Error);
        }

        public void Error(string errorMessage)
        {
            /*
            // Stop animation
            if (this.AnimationToStopOnResult != null)
                this.AnimationToStopOnResult.Stop();
            if (this.AnimationHideWindow != null)
                this.AnimationHideWindow.Launch();*/


            // Show the error
            errorMessage = errorMessage.Replace("ERROR: ", "Report failed: ");
            Debug.LogWarning(errorMessage);

            // Show message on error
           /* if (this.MessageToShowOnResult != null)
                this.MessageToShowOnResult.Show(errorMessage, 5);*/
        }

        public void Success(string[] datas)
        {
            /*
            // Stop animation
            if (this.AnimationToStopOnResult != null)
                this.AnimationToStopOnResult.Stop();
            if (this.AnimationHideWindow != null)
                this.AnimationHideWindow.Launch();

            // Show message on success
            if (this.MessageToShowOnResult != null)
                this.MessageToShowOnResult.Show("Abuse reported, an administrator will study the case.", 5);
            */
        }
    }
}