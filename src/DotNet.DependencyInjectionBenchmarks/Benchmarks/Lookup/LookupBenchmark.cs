﻿using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;
using System.Linq;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Lookup
{
    [BenchmarkCategory("Lookup")]
    public class LookupBenchmark : BaseBenchmark
    {
        public static string Description =>
            @"This benchmark is designed to test the lookup performance of each container. One small object is resolved from the container along with {ExtraRegistrations} dummy registrations that are located at warmup time.";

        [Params(0, 50, 100, 500, 2000)]
        public int ExtraRegistrations { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            SetupContainerForLookup(CreateAutofacContainer());
            SetupContainerForLookup(CreateCastleWindsorContainer());
            SetupContainerForLookup(CreateDryIocContainer());
            SetupContainerForLookup(CreateGraceContainer());
            SetupContainerForLookup(CreateLightInjectContainer());
            SetupContainerForLookup(CreateMicrosoftDependencyInjectionContainer());
            SetupContainerForLookup(CreateNInjectContainer());
            SetupContainerForLookup(CreateSimpleInjectorContainer());
            SetupContainerForLookup(CreateStructureMapContainer());
        }

        #region Benchmarks

        [Benchmark]
        [BenchmarkCategory(nameof(Autofac))]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(CastleWindsor))]
        public void CastleWindsor()
        {
            ExecuteBenchmark(CastleWindsorContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(DryIoc))]
        public void DryIoc()
        {
            ExecuteBenchmark(DryIocContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(Grace))]
        public void Grace()
        {
            ExecuteBenchmark(GraceContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(LightInject))]
        public void LightInject()
        {
            ExecuteBenchmark(LightInjectContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(MicrosoftDependencyInjection))]
        public void MicrosoftDependencyInjection()
        {
            ExecuteBenchmark(MicrosoftDependencyInjectionContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(NInject))]
        public void NInject()
        {
            ExecuteBenchmark(NInjectContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(SimpleInjector))]
        public void SimpleInjector()
        {
            ExecuteBenchmark(SimpleInjectorContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(StructureMap))]
        public void StructureMap()
        {
            ExecuteBenchmark(StructureMapContainer);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve(typeof(ISmallObjectService));
        }

        #endregion

        #region Setup Container 

        public void SetupContainerForLookup(IContainer scope)
        {
            var allTypes = DummyClasses.GetTypes(ExtraRegistrations)
                .Select(t => new RegistrationDefinition { ExportType = t, ActivationType = t }).ToList();

            var definitions = SmallObjectBenchmark.Definitions().ToArray();

            var gap = allTypes.Count / definitions.Length;

            var index = gap / 2;

            foreach (var definition in definitions)
            {
                allTypes.Insert(index, definition);

                index += gap + 1;
            }

            scope.Registration(allTypes);

            scope.BuildContainer();

            foreach (var type in allTypes.Select(r => r.ExportType))
            {
                scope.Resolve(type);
            }
        }
        #endregion
    }
}
