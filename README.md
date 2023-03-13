# Composite
## Installation
Add ```"com.loderunner.composite": "https://github.com/xloderunnerx/Composite.git",``` to the package manifest of your project.
## Usage
Composite itself is an MVC framework. It provides basic MVC functionality for fast UI building.
### Abstract Composition Root
AbstractCompositionRoot is a class where all features should be placed and binded.
```
public class CompositionRoot : AbstractCompositionRoot
    {
        private void Awake()
        {
            BindConfigurations(); // Configuration binding from Configurations field.
            BindSignalBus(); // Signal Bus binding.
            BindFeatures(); // Features binding.
        }

        private void Start()
        {
            DeclareSignals(); // Declares all signals declared in controllers.
            SubscribeToSignals(); // Subscribes to all signals declared in previous step.
            InitializeControllers();
        }

        private void Update()
        {
            UpdateControllers(); // Update all Controllers that implementing IUpdatable interface.
        }

        public void BindFeatures()
        {
            // Here goes features...
            BindFeature<HelloWorldFeature>();
        }
    }
```
### Abstract Feature
AbstractFeature is a class that can include controller, view and other bindings. Features provides more flexible development process.
```
public class HelloWorldFeature : AbstractFeature
    {
        public override void InstallBindings() // Installing Controllers, Views and other classes.
        {
            CompositionRoot.Bind<HelloWorldModel>(); // Regular class binding.
            CompositionRoot.BindController<HelloWorldController>(); // Controller binding.   
        }

        public override bool IsEnabled() // Feature state check, if feature is disabled then it will not be binded.
        {
            return CompositionRoot.GetInstance<AuthConfiguration>().isEnabled; // Getting feature configuration instance from container.
        }
    }
```
### Abstract Controller
AbstractController is a class for logic.
### Abstract View
AbstractView is a class for all scene related objects.
### Abstract Configuration
AbstractConfiguration is a class inherited from ScriptableObject. Should be set to Composition Root configurations field in case to be binded. Should contain all predefined settings for feature.

