using UnityEngine;
using UnityEditor;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;


[InitializeOnLoad]
class FinishCompiling
{

    static bool initialized = false;
    static bool compileFinished = false;
    static string lastSaveString = "null";
    static float SaveInterval = 1800f; 	//(every 30 minutes)




    static FinishCompiling()
    {
        if (!initialized)
        {
            lastSaveString = EditorPrefs.GetString("LastSave", "null");
            if (lastSaveString == "null")
            {
                EditorPrefs.SetString("LastSave", System.DateTime.Now.ToString());
            }
            initialized = true;
        }
        EditorApplication.update += new EditorApplication.CallbackFunction(CheckUpdate);
    }


    static void CheckUpdate()
    {

        //Stop the Player if scripts start compiling
        if (EditorApplication.isCompiling)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                Debug.LogWarning("Playback Stopped For Compilation");
            }
            compileFinished = false;

        }
        else
        {
            if (!compileFinished)
            {
                compileFinished = true;
                System.DateTime lastSaveDT = System.DateTime.Now;
                System.DateTime.TryParse(lastSaveString, out lastSaveDT);

                if (System.DateTime.Now.Subtract(lastSaveDT).TotalSeconds > SaveInterval)
                {
                    //ScriptSaver();
                    RecursiveCopy();
                    lastSaveString = EditorPrefs.GetString("LastSave", "null");
                    compileFinished = true;
                    EditorPrefs.SetString("LastSave", System.DateTime.Now.ToString());
                }

            }
        }
    }


    public static void ScriptSaver() { RecursiveCopy(); }





    #region Methods
    /// <summary>
    /// Copy all files with appropriate directory structure.
    /// 
    /// Modified by JAC from blog post referenced below.
    /// </summary>
    /// <see cref="http://www.codeproject.com/Tips/278248/Recursively-Copy-folder-contents-to-another-in-Csh"/>
    public static void RecursiveCopy()
    {
        string SaveDir = string.Format(Directory.GetParent(Application.dataPath).FullName + "/ScriptSaves/ScriptBackup_{0:yyyy.MM.dd-HH.mm.ss}", System.DateTime.Now);
		RecursiveCopy(@"Assets/", SaveDir + "/");

		string ZipFolderName = SaveDir + ".zip";
		FastZip fastZip = new FastZip();
		bool recurse = true;
		string filter = null; 
		fastZip.CreateZip(ZipFolderName, SaveDir + "/", recurse, filter);

		Directory.Delete(SaveDir + "/",true);

        Debug.LogWarning(":::SCRIPT BACKUP:::  -  " + SaveDir);
        EditorApplication.Beep();
    }



	
	public static bool RecursiveCopy(string SourcePath, string DestinationPath)
	{
		SourcePath = SourcePath.EndsWith(@"/") ? SourcePath : SourcePath + @"/";
        DestinationPath = DestinationPath.EndsWith(@"/") ? DestinationPath : DestinationPath + @"/";

        try
        {
            if (Directory.Exists(SourcePath))
            {
                if (Directory.Exists(DestinationPath) == false)
                {
                    Directory.CreateDirectory(DestinationPath);
                }

                foreach (string OneFile in Directory.GetFiles(SourcePath))
                {
                    FileInfo fileInfo = new FileInfo(OneFile);
                    if ((fileInfo.Name.EndsWith(".cs") || fileInfo.Name.EndsWith(".js")) || fileInfo.Name.EndsWith(".unity"))
                    {
                        fileInfo.CopyTo(string.Format(@"{0}/{1}", DestinationPath, fileInfo.Name), true);
                    }
                }

                foreach (string OneDir in Directory.GetDirectories(SourcePath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(OneDir);
                    if (RecursiveCopy(OneDir, DestinationPath + directoryInfo.Name) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log("Recursive Copy Exception: " + ex.Message);
            return false;
        }
    }
    #endregion
}

