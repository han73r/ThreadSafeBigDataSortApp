```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.4894/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method               | Mean          | Error       | StdDev        | Gen0      | Gen1      | Gen2     | Allocated  |
|--------------------- |--------------:|------------:|--------------:|----------:|----------:|---------:|-----------:|
| SortLinesTree_Small  |      7.771 μs |   0.0711 μs |     0.0665 μs |    1.4038 |    0.0458 |        - |    11816 B |
| SortLinesTree_Medium |  2,341.707 μs |   7.1759 μs |     5.6024 μs |  152.3438 |   89.8438 |  23.4375 |  1219457 B |
| SortLinesTree_Large  | 50,942.168 μs | 883.3705 μs | 1,084.8584 μs | 1363.6364 | 1272.7273 | 272.7273 | 11427876 B |
| SortLines_Small      |      5.120 μs |   0.0111 μs |     0.0098 μs |         - |         - |        - |          - |
| SortLines_Medium     |  2,352.393 μs |  16.6709 μs |    15.5940 μs |         - |         - |        - |        2 B |
| SortLines_Large      | 37,499.404 μs | 739.5930 μs | 1,255.8851 μs |         - |         - |        - |       31 B |
