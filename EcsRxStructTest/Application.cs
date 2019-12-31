using EcsRx.Infrastructure;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Ninject;
using EcsRx.Extensions;
using System.Collections.Generic;
using System.Text;
using EcsRx.Plugins.Batching;
using EcsRxStructTest.Components;
using EcsRx.Components.Lookups;
using System.Reactive.Linq;
using System;

namespace EcsRxStructTest
{
    public class Application : EcsRxApplication
    {
        IDependencyContainer _container;

        public Application(IDependencyContainer container) : base()
        {
            _container = container;
        }

        public override IDependencyContainer Container => _container;

        protected override void StartSystems()
        {
            //base.StartSystems();
            this.StartAllBoundSystems();
        }

        protected override void LoadPlugins()
        {
            base.LoadPlugins();
            RegisterPlugin(new BatchPlugin());
        }


        protected override void ApplicationStarted()
        {
            var collection = EntityCollectionManager.GetCollection();

            var componentTypeLookup = _container.Resolve<IComponentTypeLookup>();
            int structComponentAId = componentTypeLookup.GetComponentType<StructComponentA>();
            int structComponentBId = componentTypeLookup.GetComponentType<StructComponentB>();

            
            //Will not crash
            for (int i = 0; i <= Math.Max(1, Environment.ProcessorCount); i++)
            {
                var e = collection.CreateEntity();
                e.AddComponent<RefComponentA>();
                e.AddComponent<RefComponentB>();
            }

            //Will crash after some time
            for (int i = 0; i <= Math.Max(1, Environment.ProcessorCount); i++)
            {
                var e = collection.CreateEntity();
                e.AddComponent<StructComponentA>(structComponentAId);
                e.AddComponent<StructComponentB>(structComponentBId);
            }
        }
    }
}

