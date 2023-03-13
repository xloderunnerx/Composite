using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Composite.Core
{
    public abstract class AbstractCompositionRoot : MonoBehaviour
    {
        [Inject] private DiContainer container;
        private SignalBus signalBus;
        [SerializeField] protected List<AbstractConfiguration> configurations;
        private readonly List<AbstractFeature> features = new List<AbstractFeature>();
        private readonly List<AbstractController> controllers = new List<AbstractController>();
        private readonly List<AbstractView> views = new List<AbstractView>();

        protected DiContainer Container { get => container; private set => container = value; }

        public void BindSignalBus()
        {
            SignalBusInstaller.Install(container);
            signalBus = container.Resolve<SignalBus>();
        }

        public void Bind<T>()
        {
            container.Bind<T>().AsSingle().NonLazy();
        }

        public void BindConfigurations()
        {
            container.BindInstances(configurations.ToArray());
        }

        public void BindFeature<T>() where T : AbstractFeature, new()
        {
            var feature = new T();
            feature.CompositionRoot = this;
            if (!feature.IsEnabled())
                return;
            feature.InstallBindings();
            features.Add(feature);
        }

        public void BindController<T>() where T : AbstractController
        {
            container.Bind<T>().AsSingle().NonLazy();
            var controller = container.Resolve<T>();
            controller.CompositionRoot = this;
            controllers.Add(controller);
        }

        public void BindFromHierarchy<T>() where T : AbstractView
        {
            var instance = GetComponentInChildren<T>(true);
            container.BindInstance(instance);
            views.Add(instance);
        }

        public void InitializeControllers()
        {
            controllers.ForEach(controller => controller.Initialize());
        }

        public void UpdateControllers()
        {
            controllers.Where(controller => controller is IUpdatable).ToList().ForEach(controller => (controller as IUpdatable).Update());
        }

        public T GetInstance<T>() => container.Resolve<T>();

        public void DeclareSignals()
        {
            controllers.ForEach(controller => controller.DeclareSignals());
        }

        public void DeclareSignal<T>()
        {
            signalBus.DeclareSignal<T>();
        }

        public void SubscribeToSignals()
        {
            controllers.ForEach(controller => controller.SubscribeToSignals());
        }

        public void SubscribeToSignal<T>(Action callback)
        {
            signalBus.Subscribe<T>(callback);
        }

        public void SubscribeToSignal<T>(Action<T> callback)
        {
            signalBus.Subscribe(callback);
        }

        public void TryFireSignal<T>()
        {
            signalBus.TryFire<T>();
        }
        public void TryFireSignal<T>(T value)
        {
            signalBus.TryFire(value);
        }

        public void UnsubscribeFromSignal<T>(Action callback)
        {
            signalBus.Unsubscribe<T>(callback);
        }
    }
}