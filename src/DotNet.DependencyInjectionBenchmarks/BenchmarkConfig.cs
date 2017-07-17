﻿using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using DotNet.DependencyInjectionBenchmarks.Exporters;

namespace DotNet.DependencyInjectionBenchmarks
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(Job.Core, Job.Clr);
            Add(new MemoryDiagnoser());
            Add(new CompositeExporter(CsvExporter.Default, HtmlExporter.Default, MarkdownExporter.Default, new WebSiteExporter()));
        }
    }
}
