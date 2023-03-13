using System.Collections.Generic;
namespace Composite.Core
{
    public abstract class AbstractFeature
    {
        private AbstractCompositionRoot compositionRoot;

        public AbstractCompositionRoot CompositionRoot { get => compositionRoot; set => compositionRoot = value; }

        public abstract void InstallBindings();

        public virtual bool IsEnabled() => true;
    }
}