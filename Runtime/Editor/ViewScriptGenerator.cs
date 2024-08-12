#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
public static class ViewScriptGenerator
{

    [MenuItem("Assets/Create/CustomView C# Script", false, 80)]
    public static void CreateCustomScript()
    {
        string path = GetSelectedPathOrFallback();

        string scriptName = "NewCustomView.cs";
        string filePath = AssetDatabase.GenerateUniqueAssetPath($"{path}/{scriptName}");

        string template = GetScriptTemplate("NewCustomView");

        System.IO.File.WriteAllText(filePath, template);
        AssetDatabase.Refresh();

        Object asset = AssetDatabase.LoadAssetAtPath<Object>(filePath);
        Selection.activeObject = asset;
    }

    private static string GetScriptTemplate(string className)
    {
        return
            $@"using UnityEngine;
using Composite.Core;

public class {className} : AbstractView
{{
    
}}
";
    }

    private static string GetSelectedPathOrFallback()
    {
        string path = "Assets";
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
            {
                path = System.IO.Path.GetDirectoryName(path);
                break;
            }
        }

        return path;
    }
}
#endif
