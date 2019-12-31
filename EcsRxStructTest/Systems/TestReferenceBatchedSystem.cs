using EcsRx.Attributes;
using EcsRx.Collections;
using EcsRx.Components.Database;
using EcsRx.Components.Lookups;
using EcsRx.Plugins.Batching.Factories;
using EcsRx.Plugins.Batching.Systems;
using EcsRx.Threading;
using EcsRxStructTest.Components;
using System;
using System.Reactive.Linq;

namespace EcsRxStructTest.Systems
{
    [CollectionAffinity(1)]
    public class TestReferenceBatchedSystem : ReferenceBatchedSystem<RefComponentA, RefComponentB>
    {
        public TestReferenceBatchedSystem(IComponentDatabase componentDatabase, IComponentTypeLookup componentTypeLookup, IReferenceBatchBuilderFactory batchBuilderFactory, IThreadHandler threadHandler) : base(componentDatabase, componentTypeLookup, batchBuilderFactory, threadHandler)
        {
        }

        protected override void Process(int entityId, RefComponentA component1, RefComponentB component2)
        {
            component1.ExecuteCount += 1;
        }

        protected override IObservable<bool> ReactWhen()
        {
            return Observable.Delay(Observable.Interval(TimeSpan.Zero).Select(x => true), TimeSpan.FromSeconds(1));
        }
    }

    [CollectionAffinity(1)]
    public class ReportTestReferenceBatchedSystem : ReferenceBatchedSystem<RefComponentA, RefComponentB>
    {
        public ReportTestReferenceBatchedSystem(IComponentDatabase componentDatabase, IComponentTypeLookup componentTypeLookup, IReferenceBatchBuilderFactory batchBuilderFactory, IThreadHandler threadHandler) : base(componentDatabase, componentTypeLookup, batchBuilderFactory, threadHandler)
        {
        }

        protected override void Process(int entityId, RefComponentA component1, RefComponentB component2)
        {
            Console.WriteLine($"Ref {entityId} {component1.ExecuteCount}");
        }

        protected override IObservable<bool> ReactWhen()
        {
            return Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => true);
        }
    }

}

