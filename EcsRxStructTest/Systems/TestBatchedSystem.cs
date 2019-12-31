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
    [CollectionAffinity(2)]
    public class TestBatchedSystem : BatchedSystem<StructComponentA, StructComponentB>
    {
        public TestBatchedSystem(IComponentDatabase componentDatabase, IComponentTypeLookup componentTypeLookup, IBatchBuilderFactory batchBuilderFactory, IThreadHandler threadHandler) : base(componentDatabase, componentTypeLookup, batchBuilderFactory, threadHandler)
        {
        }

        protected override void Process(int entityId, ref StructComponentA component1, ref StructComponentB component2)
        {
            component1.ExecuteCount += 1;
        }

        protected override IObservable<bool> ReactWhen()
        {
            return Observable.Delay(Observable.Interval(TimeSpan.Zero).Select(x => true), TimeSpan.FromSeconds(1));
        }
    }

    [CollectionAffinity(2)]
    public class ReportTestBatchedSystem : BatchedSystem<StructComponentA, StructComponentB>
    {
        public ReportTestBatchedSystem(IComponentDatabase componentDatabase, IComponentTypeLookup componentTypeLookup, IBatchBuilderFactory batchBuilderFactory, IThreadHandler threadHandler) : base(componentDatabase, componentTypeLookup, batchBuilderFactory, threadHandler)
        {
        }

        protected override void Process(int entityId, ref StructComponentA component1, ref StructComponentB component2)
        {
            Console.WriteLine($"Str {entityId} {component1.ExecuteCount}");
        }

        protected override IObservable<bool> ReactWhen()
        {
            return Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => true);
        }
    }
}

