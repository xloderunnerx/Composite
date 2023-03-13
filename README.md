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
            CompositionRoot.BindFromHierarchy<HelloWorldView>(); // View binding from scene hierarchy.
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
```
public class HelloWorldController : AbstractController, IUpdatable
{
	private HelloWorldView view;
    private HelloWorldConfiguration configuration;

    public HelloWorldController(HelloWorldView view, HelloWorldConfiguration configuration) // Constructor inject.
    {
        this.configuration = configuration;
        this.view = view;
    }

    public override void Initialize()
    {
        Debug.Log(configuration.helloWorld); // Prints "Hello Wolrld!" once from configuration.
        view.SetHelloWorldText(configuration.helloWorld) // Set "Hello Wolrld!" to View once from configuration
    }

    public void Update()
    {
       	Debug.Log(configuration.helloWorld); // Prints "Hello Wolrld!" each frame from configuration.
    }
}

```
### Abstract View
AbstractView is a class for all scene related objects.
```
public class HelloWorldView : AbstractView
{
	[SerializeField] private TextMeshProUGUI helloWorldText;

	public void SetHelloWorldText(string value) => helloWorldText.text = value;
}
```
### Abstract Configuration
AbstractConfiguration is a class inherited from ScriptableObject. Should be set to Composition Root configurations field in case to be binded. Should contain all predefined settings for feature.
```
[CreateAssetMenu(menuName = "Configuration/Features/HelloWorld/HelloWorldConfiguration", fileName = "HelloWorldConfiguration")]
public class HelloWorldConfiguration : AbstractConfiguration
{
	public string helloWorld;
}
```

## Signal Bus
Controllers can communicate with each other by SignalBus.
Signal is a class that may or may not contain parametrs as fields.
```
public class HelloWorldSignal
{
	public string helloWorld;
}
```
or
```
public class HelloWorldSignal
{
}
```
### Signal Declaration
Signals should be declared in DeclareSignals method of Controllers
```
public class HelloWorldController : AbstractController
{
	private HelloWorldView view;
    private HelloWorldConfiguration configuration;

    public HelloWorldController(HelloWorldView view, HelloWorldConfiguration configuration) // Constructor inject.
    {
        this.configuration = configuration;
        this.view = view;
    }

    public override void Initialize()
    {
        
    }

	public override void DeclareSignals() // Signal Declaration.
    {
		DeclareSignal<HelloWorldSignal>();
	}

	public override void SubscribeToSignals()
	{
		SubscribeToSignal<HelloWorldSignal>(signal => Debug.Log(signal.));
	}
}
```
### Signal Subscription
Controllers should subscribe to Signals in SubscribeToSignals method of Controllers.
```
public class HelloWorldController : AbstractController
{
	private HelloWorldView view;
    private HelloWorldConfiguration configuration;

    public HelloWorldController(HelloWorldView view, HelloWorldConfiguration configuration) // Constructor inject.
    {
        this.configuration = configuration;
        this.view = view;
    }

    public override void Initialize()
    {
        
    }

	public override void DeclareSignals() // Signal Declaration.
    {
		DeclareSignal<HelloWorldSignal>();
	}

	public override void SubscribeToSignals()
	{
		SubscribeToSignal<HelloWorldSignal>(signal => Debug.Log(signal.helloWorld)); // Subscription that prints Signal parametr.
	}
}
```
### Firing Signals
```
public class HelloWorldController : AbstractController
{
	private HelloWorldView view;
    private HelloWorldConfiguration configuration;

    public HelloWorldController(HelloWorldView view, HelloWorldConfiguration configuration) // Constructor inject.
    {
        this.configuration = configuration;
        this.view = view;
    }

    public override void Initialize()
    {
        TryFireSignal(new HelloWorldSignal(configuration.helloWorld)); // Firing Signal with parametr from Configuration.
    }

	public override void DeclareSignals() // Signal Declaration.
    {
		DeclareSignal<HelloWorldSignal>();
	}

	public override void SubscribeToSignals()
	{
		SubscribeToSignal<HelloWorldSignal>(signal => Debug.Log(signal.helloWorld)); // Subscription that prints Signal parametr.
	}
}
```