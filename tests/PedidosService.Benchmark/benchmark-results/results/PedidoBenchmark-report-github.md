```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3915)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.203
  [Host]     : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX2


```
| Method        | Mean     | Error    | StdDev   | Allocated |
|-------------- |---------:|---------:|---------:|----------:|
| EnviarPedidos | 208.3 ms | 27.49 ms | 77.08 ms |  10.29 KB |
