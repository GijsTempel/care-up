using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

[ExecuteInEditMode]
public class FileListGenerator : MonoBehaviour {


    //public string serverFileListURL = "";
    public string fullGameExeName = "";
    public string gameBuildPath = "";
    string localFileListPath = "";
    public bool openFileListOnComplete;

    public enum OperatingSystem
    {
        Windows,
        Mac,
        Linux
    }

    public OperatingSystem buildOperatingSystem;

    public void AttemptFileListGeneration()
    {
        if(gameBuildPath == "" || fullGameExeName == "")
        {
            UnityEngine.Debug.Log("Verify Game Build Path and Full Exe Name (ex. Game.exe)");
            return;
        }

        ThreadStart threadStart = delegate
        {
            GenerateFileList();
        };

        new Thread(threadStart).Start();

    }

    void GenerateFileList()
    {
        localFileListPath = Path.Combine(gameBuildPath, "fileList.txt");

        string[] _AllFiles = Directory.GetFiles(gameBuildPath, "*", SearchOption.AllDirectories);

        TextWriter tw = new StreamWriter(localFileListPath, false);

        string _exePath = System.IO.Path.Combine(gameBuildPath, fullGameExeName);

        if(buildOperatingSystem == OperatingSystem.Mac)
            _exePath = System.IO.Path.Combine(gameBuildPath, fullGameExeName + @".app/Contents/MacOS/" + fullGameExeName);

        using (var md5 = MD5.Create())
        {
            UnityEngine.Debug.Log("Exepath is " + _exePath);
            using (var stream = File.OpenRead(_exePath))
            {
                string _md5 = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                tw.WriteLine(_md5);
            }
        }


        foreach (string s in _AllFiles)
        {
            string t = s.Replace(gameBuildPath + @"\", null);

            //Add Exceptions if you have items in build output folder that you do not want in final. Uncomment if statement and add your exceptions.
            //Example !t.StartsWith(@"Logs\") && !t.EndsWith("Thumbs.db")
            if (!t.Contains("fileList.txt") && !t.Contains("output_log.txt"))
            {

                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(s))
                    {
                        string _md5 = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                        tw.WriteLine(t + "\t" + _md5);
                    }
                }
            }
        }

        tw.Close();

        UnityEngine.Debug.Log("File List Created in Build Output Folder");

        if (openFileListOnComplete)
            Process.Start(localFileListPath);

    }

}
