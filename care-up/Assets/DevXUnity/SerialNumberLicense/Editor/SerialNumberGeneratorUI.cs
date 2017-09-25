using System;
using UnityEngine;


using UnityEditor;
using System.Collections.Generic;

namespace DevXUnityTools.Plugins
{
    /// <summary>
    /// License manager UI
    /// </summary>
    public class SerialNumberGeneratorUI : EditorWindow
    {

        #region Window/Open DevXUnityTools-Licenses
        /// <summary>
        /// Show License Generator Tab page
        /// </summary>
        [MenuItem("Window/DevXUnityTools-SerialNumbers")]
        internal static void LicenseGeneratorShow()
        {

            var window = (SerialNumberGeneratorUI)EditorWindow.GetWindow(typeof(SerialNumberGeneratorUI));
            

            window.titleContent = new GUIContent("SerialNumbers");
            window.minSize = new Vector2(800, 600);
            window.Show(true);
        }
        #endregion

        // filter valiables
        string email;
        string hardwareID;

        bool is_expiration;
        int expirationDays;
        string comment;

        Vector2 scroll_pos;
        
        
        /// <summary>
        /// Main GUI
        /// </summary>
        void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("DevXUnity - Serial number generator Ver 1.0");

            EditorGUILayout.LabelField("htttp://en.unity3d.netobf.com");
            EditorGUILayout.LabelField("For additional protection it is necessary to use obfuscator (DevXUnity-ObfuscatorPro/Base)");

            GUILayout.Space(10);



            GUILayout.BeginVertical(table_row_style);
            {
                hardwareID = EditorGUILayout.TextField("HardwareID", hardwareID);
                email = EditorGUILayout.TextField("eMail", email);
                is_expiration = EditorGUILayout.ToggleLeft("Date restrictions", is_expiration);
                if (is_expiration)
                {
                    expirationDays = EditorGUILayout.IntField("Expires through (days)", expirationDays);
                    if (expirationDays < 1)
                        expirationDays = 1;
                    if (expirationDays > 365)
                        expirationDays = 365;
                }
                comment = EditorGUILayout.TextField("Comment", comment);
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(new GUIContent("Create serial number", FindTexture("TextWrite16")), tool_bar_button_style))
                {
                    if (string.IsNullOrEmpty(hardwareID)==false
                        || is_expiration)
                    {
                        //
                        // Make license by HardwareID and save in folder
                        //
                        DateTime? exp_date = null;
                        if (is_expiration)
                            exp_date = DateTime.Now.AddDays(expirationDays);
                        SerialNumberGeneratorTools.MakeLicense(hardwareID, exp_date, comment + "", email+"");
                        //hardwareID = "";
                        //comment = "";
                        //is_expiration = false;
                        SerialNumberGeneratorTools.LicenseList = null;
                        GUI.FocusControl("");
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Warning", "You must specify the 'HardwareID; or (and) 'Date restrictions'", "ok");


                    }
                }

                if (GUILayout.Button(new GUIContent("Clear filter", FindTexture("New16")), tool_bar_button_style))
                {
                        email = "";
                    hardwareID = "";
                        comment = "";
                        is_expiration = false;
                        GUI.FocusControl("");
                }
                if (GUILayout.Button(new GUIContent("Update list", FindTexture("Update16")), tool_bar_button_style))
                {
                      SerialNumberGeneratorTools.LicenseList = null;
                      GUI.FocusControl("");
                }

                if (GUILayout.Button(new GUIContent("Open license directory", FindTexture("Folder16")), tool_bar_button_style))
                {
                    System.Diagnostics.Process.Start(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), SerialNumberGeneratorTools.BasePath));
                }

                if (GUILayout.Button(new GUIContent("Re-generate as base keys", FindTexture("Build16")), tool_bar_button_style))
                {
                    //
                    // Re-generate simple keys
                    //
                    SerialNumberGeneratorTools.UpdateKeys(re_create:true);
                     GUI.FocusControl("");
                    AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
                }
                if (GUILayout.Button(new GUIContent("Re-generate as SDA keys", FindTexture("Build16")), tool_bar_button_style))
                {
                    //
                    // Re-generate DSA keys
                    //
                    SerialNumberGeneratorTools.UpdateKeys(re_create: true, as_dsa:true);
                    GUI.FocusControl("");
                    AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
                }
            }
            GUILayout.EndHorizontal();


           

            GUILayout.Space(10);

            scroll_pos=GUILayout.BeginScrollView(scroll_pos);

            //
            // Header
            //
            GUILayout.BeginVertical(table_header_style);
            GUILayout.BeginHorizontal(table_header_style2);
            {
                GUILayout.Label("Create date", EditorStyles.boldLabel, GUILayout.Width(150));
                GUILayout.Label("Name (HardwareID)", EditorStyles.boldLabel, GUILayout.Width(250));
                GUILayout.Label("Serial number", EditorStyles.boldLabel, GUILayout.Width(200));
                GUILayout.Label("eMail", EditorStyles.boldLabel, GUILayout.Width(150));
                GUILayout.Label("Comment", EditorStyles.boldLabel, GUILayout.Width(200));
                GUILayout.Space(5);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();


            //
            // Get all license items
            //
            var rows = SerialNumberGeneratorTools.GetLicenseList();
            if (rows.Count == 0)
            {
                GUILayout.BeginVertical(table_row_style);
                EditorGUILayout.LabelField("(empty list)");
                GUILayout.EndVertical();
            }
            else
            {


                int ind = 0;
                foreach (var row in rows)
                {
                    if (string.IsNullOrEmpty(hardwareID) == false)
                    {
                        if (row.name != null && row.name.IndexOf(hardwareID, StringComparison.InvariantCultureIgnoreCase) < 0)
                            continue;
                    }

                    if (string.IsNullOrEmpty(email) == false)
                    {
                        if (row.eMail != null && row.eMail.IndexOf(email, StringComparison.InvariantCultureIgnoreCase) < 0)
                            continue;
                    }

                    ind++;

                    //
                    // License info row
                    //
                    GUILayout.BeginVertical(table_row_style);

                    GUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(new GUIContent(row.create_date.ToString("yyyy.MM.dd HH:mm:ss"), FindTexture("Activate16")), EditorStyles.label, GUILayout.Width(150));
                    EditorGUILayout.TextArea(row.name, EditorStyles.label, GUILayout.Width(250));
                    EditorGUILayout.TextArea(row.LicenseContent, GUILayout.Width(200));
                    EditorGUILayout.TextArea(row.eMail, EditorStyles.label, GUILayout.Width(150));
                    EditorGUILayout.TextArea(row.comment, EditorStyles.label, GUILayout.Width(200));

                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();

                }
            }
            GUILayout.EndScrollView();

        }

        //
        // Styles:
        //
        

        #region table_header_style
        static GUIStyle _table_header_style;
        internal static GUIStyle table_header_style
        {
            get
            {
                if (_table_header_style == null)
                {


                    _table_header_style = new GUIStyle(EditorStyles.helpBox);
                    _table_header_style.margin = new RectOffset(1, 1, 1, 1);
                    _table_header_style.padding = new RectOffset(0, 0, 0, 0);
                }
                return _table_header_style;
            }
        }
        #endregion

        #region table_header_style2
        static GUIStyle _table_header_style2;
        internal static GUIStyle table_header_style2
        {
            get
            {
                if (_table_header_style2 == null)
                {
                    _table_header_style2 = new GUIStyle();//EditorStyles.helpBox);

                    _table_header_style2.padding = new RectOffset(1, 1, 4, 4);
                    //_table_header_style2.margin = new RectOffset(0, 0, 0, 0);
                    _table_header_style2.margin = new RectOffset(1, 1, 1, 1);

                    if (true)
                    {
                        var texture = new Texture2D(1, 1);
                        texture.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 0.15f));
                        texture.Apply();

                        _table_header_style2.normal.background = texture;
                        _table_header_style2.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }

                    if (true)
                    {
                        var texture = new Texture2D(1, 1);
                        texture.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 0.14f));
                        texture.Apply();

                        _table_header_style2.hover.background = texture;
                    }

                }
                return _table_header_style2;
            }
        }
        #endregion


        #region table_row_style
        static GUIStyle _table_row_style;
        internal static GUIStyle table_row_style
        {
            get
            {
                if (_table_row_style == null)
                {
                    //_table_row_style = new GUIStyle(EditorStyles.toolbarButton);
                    //_table_row_style.fixedHeight = 25;

                    _table_row_style = new GUIStyle(EditorStyles.helpBox);
                    //_table_row_style.fixedHeight = 26;
                    _table_row_style.margin = new RectOffset(1, 1, 1, 1);
                    _table_row_style.padding = new RectOffset(1, 1, 4, 4);
                }
                return _table_row_style;
            }
        }
        #endregion

        #region tool_bar_button_style
        static GUIStyle _tool_bar_button_style;
        internal static GUIStyle tool_bar_button_style
        {
            get
            {
                if (_tool_bar_button_style == null)
                {
                    _tool_bar_button_style = new GUIStyle(EditorStyles.toolbarButton);// EditorStyles.textField);

                    _tool_bar_button_style.fixedHeight = 22;
                }
                return _tool_bar_button_style;
            }
        }
        #endregion



        #region FindTexture
        static internal Dictionary<string, Texture2D> cache_textures = new Dictionary<string, Texture2D>();
        static internal Texture2D FindTexture(string name)
        {
            if (cache_textures == null)
                cache_textures = new Dictionary<string, Texture2D>();

            if (cache_textures.ContainsKey(name) && cache_textures[name] != null)
                return cache_textures[name];

            Texture2D text = Resources.Load<Texture2D>(name);
            if (text == null)
            {
                cache_textures[name] = new Texture2D(1, 1);
                return null;
            }

            cache_textures[name] = text;

            return text;
        }
        #endregion
    }

}
