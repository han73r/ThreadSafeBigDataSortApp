```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.4894/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method              | Mean | Error |
|-------------------- |-----:|------:|
| MergeChunksNewAsync |   NA |    NA |

Benchmarks with issues:
  MergeChunksBenchmark.MergeChunksNewAsync: DefaultJob
