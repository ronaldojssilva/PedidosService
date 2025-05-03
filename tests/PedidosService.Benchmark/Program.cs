using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;

BenchmarkRunner.Run<PedidoBenchmark>(
    ManualConfig.Create(DefaultConfig.Instance)
        .WithArtifactsPath("benchmark-results")
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
);
