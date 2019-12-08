using UnityEngine;
using UnityEditor;
using System.IO;

namespace UnityEditorExtension {
  public class ScriptTemplate : UnityEditor.AssetModificationProcessor {
    static void OnWillCreateAsset(string assetName) {
      string name = removeLastMatch(assetName, ".meta");
      if (System.IO.Path.GetExtension(name) == ".cs") createScriptTemplate(name);
    }

    static void createScriptTemplate(string scriptName) {
      string projectPath = removeLastMatch(Application.dataPath, "/Assets");
      string rootNamespace = getRootNamespace(projectPath);
      if (rootNamespace.Equals(string.Empty)) {
        Debug.Log("This project doesn't have root namespace.");
        return;
      }
      if (!System.IO.File.Exists(scriptName)) return;
      string[] script = File.ReadAllLines(scriptName);
      script[4] = "namespace " + rootNamespace + "\n" + "{" + "\n" + "    " + script[4];
      for (int i = 5; i < 18; i++) {
        script[i] = "    " + script[i];
      }
      File.WriteAllLines(scriptName, script);
      File.AppendAllText(scriptName, "}");
    }

    static string getRootNamespace(string projectPath) {
      string editorSettingsPath = "ProjectSettings/EditorSettings.asset";
      if (!System.IO.File.Exists(System.IO.Path.Combine(projectPath, editorSettingsPath))) return string.Empty;
      SerializedObject editorSettings = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(editorSettingsPath)[0]);
      string namespaceProperty = "m_ProjectGenerationRootNamespace";
      string rootNamespace = editorSettings.FindProperty(namespaceProperty).stringValue;
      return rootNamespace;
    }

    static string removeLastMatch(string str, string unwantedStr) {
      return str.Remove(str.LastIndexOf(unwantedStr), unwantedStr.Length);
    }
  }
}
