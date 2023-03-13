using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Composite.Utils.FeatureGenerator
{
    [CreateAssetMenu(fileName = "FeatureGenerator", menuName = "Utils/FeatureGenerator")]
    public class FeatureGenerator : ScriptableObject
    {
        public string featureName;
        public bool generateModel;
        public bool generateView;
        public bool generateController;
        public bool generateConfiguration;
        public bool generateFeature;

        private void OnValidate()
        {
            GenerateFeature();
        }

        public void GenerateFeature()
        {
            if (!generateFeature)
                return;
            generateFeature = false;
            var directoryPath = $"{Application.dataPath}/Scripts/App/Features/{featureName}";
            if (Directory.Exists(directoryPath))
            {
                Debug.LogError("Feature already exists!");
                return;
            }
            Directory.CreateDirectory(directoryPath);
            var featurePath = $"{directoryPath}/{featureName}Feature.cs";
            File.Create(featurePath).Close();
            var streamWriter = new StreamWriter(featurePath);
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
            streamWriter.Close();
            if (generateController) GenerateController(directoryPath);
            if (generateConfiguration) GenerateConfiguration(directoryPath);
            if (generateView) GenerateView(directoryPath);
            if (generateModel) GenerateModel(directoryPath);
        }

        public void GenerateController(string featureDirectoryPath)
        {
            var directoryPath = $"{featureDirectoryPath}/Controllers";
            Directory.CreateDirectory(directoryPath);
            var controllerPath = $"{directoryPath}/{featureName}Controller.cs";
            File.Create(controllerPath).Close();
            var streamWriter = new StreamWriter(controllerPath);
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
            streamWriter.Close();
        }

        public void GenerateConfiguration(string featureDirectoryPath)
        {
            var directoryPath = $"{featureDirectoryPath}/Configurations";
            Directory.CreateDirectory(directoryPath);
            var configurationPath = $"{directoryPath}/{featureName}Configuration.cs";
            File.Create(configurationPath).Close();
            var streamWriter = new StreamWriter(configurationPath);
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
            streamWriter.Close();
        }

        public void GenerateView(string featureDirectoryPath)
        {
            var directoryPath = $"{featureDirectoryPath}/Views";
            Directory.CreateDirectory(directoryPath);
            var viewPath = $"{directoryPath}/{featureName}View.cs";
            File.Create(viewPath).Close();
            var streamWriter = new StreamWriter(viewPath);
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
            streamWriter.Close();
        }

        public void GenerateModel(string featureDirectoryPath)
        {
            var directoryPath = $"{featureDirectoryPath}/Model";
            Directory.CreateDirectory(directoryPath);
            var modelPath = $"{directoryPath}/{featureName}Model.cs";
            File.Create(modelPath).Close();
            var streamWriter = new StreamWriter(modelPath);
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
            streamWriter.Close();
        }
    }
}