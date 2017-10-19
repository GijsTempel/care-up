using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace LoginProAsset
{
    /// <summary>
    /// 
    /// Administration manager.
    /// 
    /// Useful to manage users accounts
    /// 
    /// </summary>
    public class LoginPro_Administration : MonoBehaviour
    {
        // UI components
        public UnityEngine.Object reportRowPrefab = null;

        public Text AlertField;
        public GameObject ReportsGrid;
        public GameObject DisplayMessage;
        public GameObject DisplayWindow;

        public InputField NewTitle;
        public InputField NewText;

        public InputField Username;
        public InputField Role;

        private InputField displayMessageField = null;
        private RectTransform displayMessageRect = null;

        private RawImage displayWindowImage = null;
        private RectTransform displayWindowRect = null;

        // List of reports
        private List<Report> reports { get; set; }

        // Menus
        public string PreviousScene;

        void Start()
        {
            // Verify we did logged in
            if (!LoginPro.Session.LoggedIn)
            {
                LoginPro_Security.Load(PreviousScene);  // The connection is not established, go back to login menu
            }

            // Get UI components
            if (reportRowPrefab == null)
                reportRowPrefab = Resources.Load("ReportRow");

            displayMessageField = DisplayMessage.GetComponentInChildren<InputField>();
            displayMessageRect = DisplayMessage.GetComponent<RectTransform>();
            displayWindowImage = DisplayWindow.GetComponentInChildren<RawImage>();
            displayWindowRect = DisplayWindow.GetComponent<RectTransform>();

            // Load screen data
            RefreshReports();
        }

        private void ShowScreenshotWindow()
        {
            displayWindowRect.localScale = Vector3.one;
        }
        private void ShowMessageWindow()
        {
            displayMessageRect.localScale = Vector3.one;
        }


        ///////////////////////////////// - CALLBACKS EVENTS on UI components - ///////////////////////////////////////////
        public void RefreshReportsLaunched()
        {
            RefreshReports();
        }
        public void HideScreenshotWindow()
        {
            displayWindowRect.localScale = Vector3.zero;
        }
        public void HideMessageWindow()
        {
            displayMessageRect.localScale = Vector3.zero;
        }
        public void ShowScreenshot(string reportId)
        {
            GetScreenshot(reportId);
        }
        private void DisplayScreenshot(string screenshotString)
        {
            Texture2D tex = new Texture2D(862, 415);

            byte[] screenshotBytes = Convert.FromBase64String(screenshotString);
            tex.LoadImage(screenshotBytes);
            displayWindowImage.texture = tex;
            ShowScreenshotWindow();
        }

        public void ShowMessage(string message)
        {
            displayMessageField.text = message;
            ShowMessageWindow();
        }

        public void SaveLaunched()
        {
            SaveReports();
        }

        public void SetRemoveFlag(string id, Image reportRowImage)
        {
            Color red = new Color(1, 0, 0.16f);
            Color grey = new Color(1, 1, 1, 0.3921f);

            foreach (Report r in reports)
            {
                if (r.Id == id)
                {
                    r.ToBeRemoved = !r.ToBeRemoved;
                    reportRowImage.color = r.ToBeRemoved ? red : grey;
                    break;
                }
            }
        }

        public void AddNewLaunched()
        {
            AddNew();
        }
        public void BanLaunched()
        {
            BanUser();
        }
        public void UnBanLaunched()
        {
            UnBanUser();
        }
        public void ChangeUserRoleLaunched()
        {
            ChangeUserRole();
        }



        ///////////////////////////////// - Refresh reports - ///////////////////////////////////////////
        private void RefreshReports()
        {
            // Clear the reports list
            reports = new List<Report>();

            // Clear the report list
            Transform reportsGridTransform = ReportsGrid.transform;
            foreach (Transform child in reportsGridTransform)
            {
                Destroy(child.gameObject);
            }

            // Get report list from server
            if (LoginPro.Manager != null)
                LoginPro.Manager.ReportsList(handleAdministrationSuccess, handleAdministrationError);
            else
                Debug.Log("ADMIN : Manager is null !!");
        }

        private void handleAdministrationSuccess(string[] serverDatas)
        {
            AlertField.text = serverDatas[0];       // Show server message

            int numberOfDataReceivedPerReport = 4;  // A report is constituted of 4 datas

            int dataLength = serverDatas.Length;    // Get the length of the array received
            for (int i = 1; i + numberOfDataReceivedPerReport <= dataLength; i += (numberOfDataReceivedPerReport + 1))  // As long as there is datas -> create a report row
            {
                string reportId = serverDatas[i];
                string reportDate = serverDatas[i + 1];
                string reporterUsername = serverDatas[i + 2];
                string message = serverDatas[i + 3];
                //string screenshot = serverDatas [i+4];
                string isDone = serverDatas[i + 4];

                // Create the report row
                GameObject reportRow = (GameObject)Instantiate(reportRowPrefab);

                // Get buttons of the row to display information
                GameObject ReportId = reportRow.transform.Find("ReportId").gameObject;
                GameObject ReportDate = reportRow.transform.Find("ReportDate").gameObject;
                GameObject Reporter = reportRow.transform.Find("Reporter").gameObject;
                GameObject ShowMessage = reportRow.transform.Find("ShowMessage").gameObject;
                GameObject ShowScreenshot = reportRow.transform.Find("ShowScreenshot").gameObject;
                GameObject IsDone = reportRow.transform.Find("IsDone").gameObject;
                GameObject Remove = reportRow.transform.Find("Remove").gameObject;

                // Check if all gameObject are found in the prefab : otherwise show some errors
                if (ReportId == null) Debug.LogError("The ReportRow prefab must contain a ReportId child.");
                if (ReportDate == null) Debug.LogError("The ReportRow prefab must contain a ReportDate child.");
                if (Reporter == null) Debug.LogError("The ReportRow prefab must contain a Reporter child.");
                if (ShowMessage == null) Debug.LogError("The ReportRow prefab must contain a ShowMessage child.");
                if (ShowScreenshot == null) Debug.LogError("The ReportRow prefab must contain a ShowScreenshot child.");
                if (IsDone == null) Debug.LogError("The ReportRow prefab must contain a IsDone child.");

                // Attach action on button clicks
                Button ShowMessageButton = ShowMessage.GetComponent<Button>();
                ShowMessageButton.onClick.AddListener(() => this.ShowMessage(message));

                Button ShowScreenshotButton = ShowScreenshot.GetComponent<Button>();
                ShowScreenshotButton.onClick.AddListener(() => this.ShowScreenshot(reportId));

                Button DeleteButton = Remove.GetComponent<Button>();
                DeleteButton.onClick.AddListener(() => this.SetRemoveFlag(reportId, reportRow.GetComponent<Image>()));

                // Affect values to it
                ReportId.transform.GetChild(0).GetComponent<Text>().text = reportId;
                ReportDate.transform.GetChild(0).GetComponent<Text>().text = reportDate;
                Reporter.transform.GetChild(0).GetComponent<Text>().text = reporterUsername;
                Toggle isDoneCheckbox = IsDone.GetComponent<Toggle>();
                isDoneCheckbox.isOn = isDone == "True";

                // Insert it in the report list
                Report report = new Report(reportId, isDoneCheckbox);
                reportRow.transform.SetParent(ReportsGrid.transform);
                reports.Add(report);
            }
        }
        private void handleAdministrationError(string errorMessage)
        {
            Debug.Log(errorMessage);
            AlertField.text = errorMessage; // Show the server's message
        }

        ///////////////////////////////// - Get screenshot - ///////////////////////////////////////////
        private void GetScreenshot(string reportId)
        {
            LoginPro.Manager.GetScreenshot(reportId, handleGetScreenshotSuccess, handleGetScreenshotError);
        }
        private void handleGetScreenshotError(string errorMessage)
        {
            Debug.Log(errorMessage);
            AlertField.text = errorMessage; // Show the server's message
        }
        private void handleGetScreenshotSuccess(string[] serverDatas)
        {
            AlertField.text = serverDatas[0];       // Show server message
            DisplayScreenshot(serverDatas[1]);      // Display the screenshot on screen
        }

        ///////////////////////////////// - Save reports - ///////////////////////////////////////////
        private void SaveReports()
        {
            string[] datas = new string[reports.Count * 3];

            int index = 0;
            foreach (Report r in reports)
            {
                datas[index] = r.Id;
                datas[index + 1] = r.IsDoneCheckbox.isOn.ToString();
                datas[index + 2] = r.ToBeRemoved.ToString();
                index += 3;
            }

            LoginPro.Manager.SaveReportsList(datas, handleSaveSuccess, handleSaveError);
        }
        private void handleSaveError(string errorMessage)
        {
            Debug.Log(errorMessage);
            AlertField.text = errorMessage;         // Show the server's message
        }
        private void handleSaveSuccess(string[] serverDatas)
        {
            AlertField.text = serverDatas[0];       // Show server message
            RefreshReports();                       // Refresh report list
        }

        ///////////////////////////////// - Add new - ///////////////////////////////////////////
        private void AddNew()
        {
            LoginPro.Manager.AddNew(NewTitle.text, NewText.text, handleAddNewSuccess, handleAddNewError);
        }
        private void handleAddNewError(string errorMessage)
        {
            Debug.Log(errorMessage);
            AlertField.text = errorMessage;         // Show the server's message
        }
        private void handleAddNewSuccess(string[] serverDatas)
        {
            AlertField.text = serverDatas[0];       // Show server message
        }

        ///////////////////////////////// - Ban user - ///////////////////////////////////////////
        private void BanUser()
        {
            LoginPro.Manager.BanUser(Username.text, handleBanSuccess, handleBanError);
        }
        private void handleBanError(string errorMessage)
        {
            Debug.Log(errorMessage);
            AlertField.text = errorMessage;         // Show the server's message
        }
        private void handleBanSuccess(string[] serverDatas)
        {
            AlertField.text = serverDatas[0];       // Show server message
        }

        ///////////////////////////////// - UnBan user - ///////////////////////////////////////////
        private void UnBanUser()
        {
            LoginPro.Manager.UnBanUser(Username.text, handleUnBanSuccess, handleUnBanError);
        }
        private void handleUnBanError(string errorMessage)
        {
            Debug.Log(errorMessage);
            AlertField.text = errorMessage;         // Show the server's message
        }
        private void handleUnBanSuccess(string[] serverDatas)
        {
            AlertField.text = serverDatas[0];       // Show server message
        }

        ///////////////////////////////// - UnBan user - ///////////////////////////////////////////
        private void ChangeUserRole()
        {
            LoginPro.Manager.ChangeUserRole(Username.text, Role.text, handleChangeUserRoleSuccess, handleChangeUserRoleError);
        }
        private void handleChangeUserRoleError(string errorMessage)
        {
            Debug.Log(errorMessage);
            AlertField.text = errorMessage;         // Show the server's message
        }
        private void handleChangeUserRoleSuccess(string[] serverDatas)
        {
            AlertField.text = serverDatas[0];       // Show server message
        }
    }
}