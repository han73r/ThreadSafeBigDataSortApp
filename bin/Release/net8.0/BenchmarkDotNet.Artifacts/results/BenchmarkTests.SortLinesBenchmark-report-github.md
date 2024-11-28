```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.4894/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method              | Mean           | Error         | StdDev        | Gen0      | Gen1      | Gen2     | Allocated  |
|-------------------- |---------------:|--------------:|--------------:|----------:|----------:|---------:|-----------:|
| SortLinesNew_Small  |       5.061 μs |     0.0203 μs |     0.0190 μs |         - |         - |        - |          - |
| SortLinesNew_Medium |   2,353.058 μs |    11.5397 μs |     9.0094 μs |         - |         - |        - |       24 B |
| SortLinesNew_Large  |  38,533.271 μs |   768.6236 μs | 2,051.6122 μs |         - |         - |        - |       31 B |
| SortLines_Small     |      39.088 μs |     0.1936 μs |     0.1716 μs |    2.0142 |         - |        - |    16984 B |
| SortLines_Medium    |   7,307.131 μs |    85.2999 μs |    75.6161 μs |  187.5000 |         - |        - |  1673408 B |
| SortLines_Large     | 127,039.811 μs | 2,511.2322 μs | 2,989.4435 μs | 2000.0000 | 1250.0000 | 250.0000 | 16729278 B |
