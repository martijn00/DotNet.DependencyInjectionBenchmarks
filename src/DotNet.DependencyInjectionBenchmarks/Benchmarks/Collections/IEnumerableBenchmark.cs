﻿using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Collections
{
    [BenchmarkCategory("Collections")]
    public class IEnumerableBenchmark : BaseBenchmark
    {
        public static string Description =>
            "This benchmark registers 5 small objects then resolves them as an IEnumerable(T).";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            var warmup = new Action<IResolveScope>[]
            {
                scope => scope.Resolve<IEnumerable<IEnumerableService>>()
            };

            SetupContainerForTest(CreateAutofacContainer(), definitions, warmup);
            SetupContainerForTest(CreateDryIocContainer(), definitions, warmup);
            SetupContainerForTest(CreateGraceContainer(), definitions, warmup);
            SetupContainerForTest(CreateLightInjectContainer(), definitions, warmup);
            SetupContainerForTest(CreateMicrosoftDependencyInjectionContainer(), definitions, warmup);
            //SetupContainerForTest(CreateNInjectContainer(), definitions, warmup);
            SetupContainerForTest(CreateSimpleInjectorContainer(), definitions, warmup);
            SetupContainerForTest(CreateStructureMapContainer(), definitions, warmup);
        }

        public static IEnumerable<RegistrationDefinition> Definitions()
        {
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService1), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService2), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService3), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService4), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService5), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(ISingletonService), ActivationType = typeof(SingletonService) };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService), ActivationType = typeof(TransientService) };
        }

        #region Benchmarks

        [Benchmark]
        [BenchmarkCategory(nameof(Autofac))]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacContainer);
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

        //[Benchmark]
        //[BenchmarkCategory(nameof(NInject))]
        //public void NInject()
        //{
        //    ExecuteBenchmark(NInjectContainer);
        //}

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
            if (scope.Resolve<IEnumerable<IEnumerableService>>().Count() != 5)
            {
                throw new Exception("Count does not equal 5");
            }
        }

        #endregion
    }
}
