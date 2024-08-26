using System;
using System.Collections;
using UnityEngine;

namespace Composite.Core
{
    public abstract class AbstractController
    {
        private AbstractCompositionRoot compositionRoot;

        public AbstractCompositionRoot CompositionRoot { get => compositionRoot; set => compositionRoot = value; }

        public virtual void DeclareSignals() { }

        public virtual void SubscribeToSignals() { }

        public abstract void Initialize();

        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return compositionRoot.StartCoroutine(coroutine);
        }

        public void StopCoroutine(IEnumerator coroutine)
        {
            compositionRoot.StopCoroutine(coroutine);
        }

        public void DeclareSignal<T>()
        {
            compositionRoot.DeclareSignal<T>();
        }

        public void SubscribeToSignal<T>(Action callback)
        {
            compositionRoot.SubscribeToSignal<T>(callback);
        }

        public void SubscribeToSignal<T>(Action<T> callback)
        {
            compositionRoot.SubscribeToSignal<T>(T => callback?.Invoke(T));
        }

        public void TryFireSignal<T>()
        {
            compositionRoot.TryFireSignal<T>();
        }

        public void TryFireSignal<T>(T value)
        {
            compositionRoot.TryFireSignal(value);
        }

        public void UnsubscribeFromSignal<T>(Action callback)
        {
            compositionRoot.UnsubscribeFromSignal<T>(callback);
        }
    }
}