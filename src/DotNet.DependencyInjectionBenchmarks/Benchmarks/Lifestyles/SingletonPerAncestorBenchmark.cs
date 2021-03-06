﻿using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Lifestyles
{
	[BenchmarkCategory("Lifestyles")]
	public class SingletonPerAncestorBenchmark : StandardBenchmark
	{
		public static string Description =>
            @"This benchmark registers a small object as Singleton Per Ancestor then resolves it as part of a slightly larger object graph";

	    protected override IEnumerable<RegistrationDefinition> Definitions
	    {
	        get
	        {
	            foreach (var definition in SmallObjectServices.Definitions(RegistrationLifestyle.SingletonPerAncestor, typeof(IImportMultipleSmallObject)))
	            {
	                yield return definition;
	            }

	            yield return new RegistrationDefinition
	            {
	                ExportType = typeof(IImportMultipleSmallObject),
	                ActivationType = typeof(ImportMultipleSmallObject)
	            };
	        }
	    }

        protected override void Warmup(IResolveScope scope)
	    {
	        var instance = scope.Resolve<IImportMultipleSmallObject>();

	        if (!ReferenceEquals(instance.SmallObject1, instance.SmallObject2))
	        {
	            throw new Exception("Not the same instance");
	        }
        }

	    protected override void ExecuteBenchmark(IResolveScope scope)
	    {
	        scope.Resolve<IImportMultipleSmallObject>();
	    }

        #region Benchmarks

        [Benchmark]
		[BenchmarkCategory(nameof(CastleWindsor))]
		public void CastleWindsor()
		{
			ExecuteBenchmark(CastleWindsorContainer);
		}

		[Benchmark]
		[BenchmarkCategory(nameof(Grace))]
		public void Grace()
		{
			ExecuteBenchmark(GraceContainer);
		}
        
		#endregion

	}
}
