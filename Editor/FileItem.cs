using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class FileItem
{
  [MenuItem("File/Restart")]
  static void RestartUnityEditor()
  {
    string shPath = Application.dataPath + "/Editor/restart_unity_editor.sh";

    string unityEditorPath = EditorApplication.applicationPath;
    string args = "-projectPath " + Application.dataPath.Replace("/Assets", "");
    string command = unityEditorPath + " " + args;

    Process.Start("/bin/bash", $"-c \"{shPath} {command}\"");
    EditorApplication.Exit(0);
  }
}
