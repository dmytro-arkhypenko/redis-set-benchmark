```

BenchmarkDotNet v0.13.10, Windows 10 (10.0.19045.3448/22H2/2022Update) (VMware)
Intel Xeon Gold 6140 CPU 2.30GHz, 2 CPU, 16 logical and 16 physical cores
.NET SDK 7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  Job-UKRKRL : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2

IterationCount=3  WarmupCount=1  

```
| Method                 | NumberOfSets | Mean       | Error      | StdDev     | Gen0    | Gen1    | Gen2    | Allocated |
|----------------------- |------------- |-----------:|-----------:|-----------:|--------:|--------:|--------:|----------:|
| **MeasureSetIntersection** | **3**            |  **10.512 ms** |  **13.306 ms** |  **0.7293 ms** | **46.8750** | **46.8750** | **46.8750** |  **394999 B** |
| MeasureSetUnion        | 3            |  83.737 ms |  37.316 ms |  2.0454 ms |       - |       - |       - | 4972084 B |
| MeasureSetDifference   | 3            |  20.826 ms |  11.442 ms |  0.6272 ms | 93.7500 | 93.7500 | 93.7500 |  923437 B |
| **MeasureSetIntersection** | **5**            |   **9.019 ms** |   **5.870 ms** |  **0.3217 ms** |       **-** |       **-** |       **-** |   **61944 B** |
| MeasureSetUnion        | 5            | 110.582 ms |  60.679 ms |  3.3260 ms |       - |       - |       - | 5875205 B |
| MeasureSetDifference   | 5            |  15.211 ms |   8.637 ms |  0.4734 ms | 31.2500 | 31.2500 | 31.2500 |  333553 B |
| **MeasureSetIntersection** | **7**            |   **8.595 ms** |   **5.025 ms** |  **0.2755 ms** |       **-** |       **-** |       **-** |    **9570 B** |
| MeasureSetUnion        | 7            | 118.428 ms | 118.588 ms |  6.5002 ms |       - |       - |       - | 6212742 B |
| MeasureSetDifference   | 7            |  13.850 ms |   8.654 ms |  0.4744 ms |       - |       - |       - |  125858 B |
| **MeasureSetIntersection** | **10**           |   **9.058 ms** |  **22.944 ms** |  **1.2576 ms** |       **-** |       **-** |       **-** |    **1202 B** |
| MeasureSetUnion        | 10           | 135.359 ms |  89.747 ms |  4.9193 ms |       - |       - |       - | 6389984 B |
| MeasureSetDifference   | 10           |  12.926 ms |  13.315 ms |  0.7298 ms |       - |       - |       - |   28035 B |
| **MeasureSetIntersection** | **15**           |   **8.878 ms** |   **4.425 ms** |  **0.2425 ms** |       **-** |       **-** |       **-** |     **970 B** |
| MeasureSetUnion        | 15           | 174.921 ms | 167.933 ms |  9.2050 ms |       - |       - |       - | 6436764 B |
| MeasureSetDifference   | 15           |  13.471 ms |   3.086 ms |  0.1691 ms |       - |       - |       - |    3315 B |
| **MeasureSetIntersection** | **20**           |   **8.313 ms** |   **6.138 ms** |  **0.3364 ms** |       **-** |       **-** |       **-** |    **1210 B** |
| MeasureSetUnion        | 20           | 185.442 ms | 210.855 ms | 11.5577 ms |       - |       - |       - | 6442288 B |
| MeasureSetDifference   | 20           |  13.912 ms |   8.327 ms |  0.4564 ms |       - |       - |       - |    1210 B |
