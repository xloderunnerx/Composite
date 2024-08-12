#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
public class FeatureGeneratorWindow : EditorWindow
{
    private string featureName;
    private bool generateModel;
    private bool generateView;
    private bool generateController;
    private bool generateConfiguration;

    [MenuItem("Tools/Feature Generator")]
    public static void ShowWindow()
    {
        GetWindow<FeatureGeneratorWindow>("Feature Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate New Feature", EditorStyles.boldLabel);

        featureName = EditorGUILayout.TextField("Feature Name", featureName);
        generateModel = EditorGUILayout.Toggle("Generate Model", generateModel);
        generateView = EditorGUILayout.Toggle("Generate View", generateView);
        generateController = EditorGUILayout.Toggle("Generate Controller", generateController);
        generateConfiguration = EditorGUILayout.Toggle("Generate Configuration", generateConfiguration);

        if (GUILayout.Button("Generate Feature"))
        {
            GenerateFeature();
        }
    }

    private void GenerateFeature()
    {
        if (string.IsNullOrEmpty(featureName))
        {
            Debug.LogError("Feature name cannot be empty!");
            return;
        }

        var directoryPath = $"{Application.dataPath}/Scripts/App/Features/{featureName}";
        if (Directory.Exists(directoryPath))
        {
            Debug.LogError("Feature already exists!");
            return;
        }

        Directory.CreateDirectory(directoryPath);
        GenerateFeatureScript(directoryPath);

        if (generateModel) GenerateModel(directoryPath);
        if (generateView) GenerateView(directoryPath);
        if (generateController) GenerateController(directoryPath);
        if (generateConfiguration) GenerateConfiguration(directoryPath);

        AssetDatabase.Refresh();
        Debug.Log($"Feature {featureName} generated successfully.");
    }

    private void GenerateFeatureScript(string directoryPath)
    {
        var featurePath = $"{directoryPath}/{featureName}Feature.cs";
        using (var streamWriter = new StreamWriter(featurePath))
        {
            streamWriter.Write(
                $"using Composite.Core;" +
                $"\n" +
                $"\n" +
                $"namespace App.Features.{featureName}\n" +
                $"{{" +
                $"\n" +
                $"\tpublic class {featureName}Feature : AbstractFeature" +
                $"\n" +
                $"\t{{\n" +
                $"\t\tpublic override void InstallBindings()\n" +
                $"\t\t{{" +
                $"\n" +
                (generateModel ? $"\t\t\tCompositionRoot.Bind<{featureName}Model>();\n" : $"") +
                (generateView ? $"\t\t\tCompositionRoot.BindFromHierarchy<{featureName}View>();\n" : $"") +
                (generateController ? $"\t\t\tCompositionRoot.BindController<{featureName}Controller>();\n" : $"") +
                $"\t\t}}\n" +
                $"\n" +
                (generateConfiguration ? $"\t\tpublic override bool IsEnabled()\n\t\t{{\n\t\t\treturn CompositionRoot.GetInstance<{featureName}Configuration>().isEnabled;\n\t\t}}\n" : $"") +
                $"\t}}" +
                $"\n" +
                $"}}");
        }
    }

    private void GenerateController(string featureDirectoryPath)
    {
        var directoryPath = $"{featureDirectoryPath}/Controllers";
        Directory.CreateDirectory(directoryPath);
        var controllerPath = $"{directoryPath}/{featureName}Controller.cs";
        using (var streamWriter = new StreamWriter(controllerPath))
        {
            streamWriter.Write(
                $"using Composite.Core;" +
                $"\n" +
                $"\n" +
                $"namespace App.Features.{featureName}\n" +
                $"{{" +
                $"\n" +
                $"\tpublic class {featureName}Controller : AbstractController" +
                $"\n" +
                $"\t{{\n" +
                $"\t\tpublic override void Initialize()\n" +
                $"\t\t{{\n" +
                $"\t\t}}\n" +
                $"\t}}" +
                $"\n" +
                $"}}");
        }
    }

    private void GenerateConfiguration(string featureDirectoryPath)
    {
        var directoryPath = $"{featureDirectoryPath}/Configurations";
        Directory.CreateDirectory(directoryPath);
        var configurationPath = $"{directoryPath}/{featureName}Configuration.cs";
        using (var streamWriter = new StreamWriter(configurationPath))
        {
            streamWriter.Write(
                $"using UnityEngine;" +
                $"\n" +
                $"\n" +
                $"namespace App.Features.{featureName}\n" +
                $"{{" +
                $"\n" +
                $"\t[CreateAssetMenu(menuName = \"Configuration/Features/{featureName}/{featureName}Configuration\", fileName = \"{featureName}Configuration\")]" +
                $"\n" +
                $"\tpublic class {featureName}Configuration : AbstractConfiguration" +
                $"\n" +
                $"\t{{\n" +
                $"\t}}" +
                $"\n" +
                $"}}");
        }
    }

    private void GenerateView(string featureDirectoryPath)
    {
        var directoryPath = $"{featureDirectoryPath}/Views";
        Directory.CreateDirectory(directoryPath);
        var viewPath = $"{directoryPath}/{featureName}View.cs";
        using (var streamWriter = new StreamWriter(viewPath))
        {
            streamWriter.Write(
                $"using Composite.Core;" +
                $"\n" +
                $"using UnityEngine;" +
                $"\n" +
                $"\n" +
                $"namespace App.Features.{featureName}\n" +
                $"{{" +
                $"\n" +
                $"\tpublic class {featureName}View : AbstractView" +
                $"\n" +
                $"\t{{\n" +
                $"\t}}" +
                $"\n" +
                $"}}");
        }
    }

    private void GenerateModel(string featureDirectoryPath)
    {
        var directoryPath = $"{featureDirectoryPath}/Model";
        Directory.CreateDirectory(directoryPath);
        var modelPath = $"{directoryPath}/{featureName}Model.cs";
        using (var streamWriter = new StreamWriter(modelPath))
        {
            streamWriter.Write(
                $"namespace App.Features.{featureName}\n" +
                $"{{" +
                $"\n" +
                $"\tpublic class {featureName}Model" +
                $"\n" +
                $"\t{{\n" +
                $"\t}}" +
                $"\n" +
                $"}}");
        }
    }
}
#endif