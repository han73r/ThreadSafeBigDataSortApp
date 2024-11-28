```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.4894/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method              | Mean          | Error        | StdDev       |
|-------------------- |--------------:|-------------:|-------------:|
| SortLinesNew_Small  |      34.67 μs |     0.339 μs |     0.301 μs |
| SortLinesNew_Medium |   9,350.85 μs |   185.430 μs |   173.451 μs |
| SortLinesNew_Large  | 132,906.64 μs | 2,583.568 μs | 3,267.386 μs |
| SortLines_Small     |      36.40 μs |     0.112 μs |     0.099 μs |
| SortLines_Medium    |   7,441.42 μs |    42.940 μs |    33.525 μs |
| SortLines_Large     | 123,374.62 μs | 2,127.773 μs | 1,661.225 μs |
