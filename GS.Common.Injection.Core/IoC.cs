using Autofac;

namespace GS.Common.Injection.Core
{
    public static class IoC
    {
        private static IContainer _container;
        private static readonly object SyncRoot = new object();

        public static IContainer Container
        {
            get
            {
                if (_container == null)
                {
                    lock (SyncRoot)
                    {
                        _container = new ContainerBuilder().Build();
                        var containerUpdater = new ContainerBuilder();
                        containerUpdater.RegisterInstance(_container).As<IContainer>();
                        containerUpdater.Update(_container);
                    }
                }

                return _container;
            }
        }
        public static void ResetContainer()
        {
            lock (SyncRoot)
            {
                if (_container != null)
                {
                    _container.Dispose();
                    _container = null;
                }
            }
        }
    }
}
