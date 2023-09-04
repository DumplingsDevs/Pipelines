# Benchmarks

In this section, you'll find comprehensive benchmark results that compare Pipelines in different configuration comparing
to other libraries.

```

BenchmarkDotNet v0.13.6, macOS Ventura 13.4.1 (c) (22F770820d) [Darwin 22.5.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK 7.0.200
  [Host]     : .NET 7.0.3 (7.0.323.6910), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 7.0.3 (7.0.323.6910), Arm64 RyuJIT AdvSIMD


```
|                                   Method |      Mean |     Error |    StdDev | Ratio | RatioSD | Rank |   Gen0 |   Gen1 | Allocated | Alloc Ratio |
|----------------------------------------- |----------:|----------:|----------:|------:|--------:|-----:|-------:|-------:|----------:|------------:|
|               WrapperDispatcherGenerator |  5.842 μs | 0.0182 μs | 0.0171 μs |  1.00 |    0.00 |    1 | 2.4567 | 0.0229 |  15.09 KB |        1.00 |
|                                  MediatR |  8.250 μs | 0.0272 μs | 0.0254 μs |  1.41 |    0.01 |    2 | 2.0294 | 0.0153 |  12.47 KB |        0.83 |
|                    MediatRWithBehaviours | 17.349 μs | 0.1814 μs | 0.1416 μs |  2.97 |    0.03 |    3 | 4.4861 | 1.0986 |   27.6 KB |        1.83 |
| WrapperDispatcherGeneratorWithDecorators | 30.408 μs | 0.5932 μs | 0.6347 μs |  5.22 |    0.11 |    4 | 4.6692 | 1.1902 |  28.65 KB |        1.90 |
|                      PipelinesReflection | 33.674 μs | 0.1697 μs | 0.1587 μs |  5.76 |    0.03 |    5 | 5.3101 | 0.0610 |  32.86 KB |        2.18 |
|        PipelinesReflectionWithDecorators | 61.176 μs | 0.2777 μs | 0.2462 μs | 10.47 |    0.05 |    6 | 8.5449 | 2.0752 |   52.6 KB |        3.48 |
